using Evolution.Sql.Cache;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Evolution.Sql
{
    internal abstract class AbstractCommand : ICommand
    {
        private Regex regex;
        protected static char[] leftQuote = new char[] { '[', '\'', '`', '"' };
        protected static char[] rightQuote = new char[] { ']', '\'', '`', '"' };
        protected string ParameterPattern
        {
            get { return ParameterSymbal + "([^',;=<>\\s\\)]+)"; }
        }

        protected virtual string Sql2GetProcedureParameters
        {
            get { return @"select PARAMETER_NAME, PARAMETER_MODE, DATA_TYPE from INFORMATION_SCHEMA.PARAMETERS where SPECIFIC_SCHEMA= @schema and SPECIFIC_NAME = @name"; }
        }

        protected virtual string DefaultSchema { get; }

        protected virtual string ParameterSymbal => "@";

        public DbConnection Connection { get; set; }
        public CommandType CommandType { get; set; }
        public string CommandText { get; set; }
        public string ParameterPrefix { get; set; }

        DbParameter[] _explicitParameters;
        DbParameter[] ICommand.ExplicitParameters
        {
            get { return _explicitParameters; }
            set { _explicitParameters = value; }
        }

        protected virtual Dictionary<string, DbType> DbDataTypeDbTypeMap { get; set; }

        public AbstractCommand()
        {
            regex = new Regex(ParameterPattern);
        }

        public DbCommand Build(object parameters)
        {
            Connection.TryOpen();
            var dbCommand = Connection.CreateCommand();
            dbCommand.CommandType = CommandType;
            dbCommand.CommandText = CommandText;
            if (_explicitParameters != null && _explicitParameters.Length > 0)
            {
                SetExplicitParameters(dbCommand);
            }
            else
            {
                SetParameters(dbCommand);
                AssignParameterValues(dbCommand, parameters);
            }
            return dbCommand;
        }

        protected void SetExplicitParameters(DbCommand dbCommand)
        {
            foreach (var p in _explicitParameters)
            {
                dbCommand.Parameters.Add(p);
            }
        }

        protected void SetParameters(DbCommand command)
        {
            if (command.CommandType == CommandType.StoredProcedure)
            {
                SetStoredProcedureParameters(command);
            }
            else
            {
                SetSqlParameters(command);
            }
        }

        public virtual void SetStoredProcedureParameters(DbCommand command)
        {
            var cachedParameters = CacheHelper.GetDbParameters($"_{command.CommandText}");
            if (cachedParameters != null)
            {
                foreach (var item in cachedParameters)
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = item.Name;
                    parameter.Direction = item.Direction;
                    parameter.DbType = item.DbType;
                    command.Parameters.Add(parameter);
                }

                return;
            }
            cachedParameters = new List<DbParameterCacheItem>();

            var schemaAndName = command.CommandText.Split('.');
            string schema = string.Empty, name = string.Empty;
            if (schemaAndName.Length == 2)
            {
                schema = schemaAndName[0].TrimStart(leftQuote).TrimEnd(rightQuote);
                name = schemaAndName[1].TrimStart(leftQuote).TrimEnd(rightQuote);
            }
            else if (schemaAndName.Length == 1)
            {
                schema = DefaultSchema;
                name = schemaAndName[0].TrimStart(leftQuote).TrimEnd(rightQuote);
            }
            else
            {
                throw new Exception(string.Format("invalid Stored Procedure Name {0}", command.CommandText));
            }

            using (var cmd = command.Connection.CreateCommand())
            {
                cmd.CommandText = Sql2GetProcedureParameters;
                cmd.CommandType = CommandType.Text;
                var pSchema = cmd.CreateParameter();
                pSchema.ParameterName = "schema";
                //SqlServer: one database can have different schema
                //MySql: schema is database name
                //Oracle: schema is user, but here schema means package name
                pSchema.Value = string.IsNullOrEmpty(schema) ? cmd.Connection.Database : schema;
                cmd.Parameters.Add(pSchema);

                var pName = cmd.CreateParameter();
                pName.ParameterName = "name";
                pName.Value = name;
                cmd.Parameters.Add(pName);

                var parameterName = string.Empty;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        parameterName = reader.GetString(0).TrimStart('@');
                        var parameter = command.CreateParameter();
                        parameter.ParameterName = parameterName;
                        parameter.Direction = GetDirection(reader.GetString(1));
                        //parameter.DbType = GetDbTypeByString(reader.GetString(2).Trim());
                        SetParameterType(parameter, reader.GetString(2).Trim());
                        command.Parameters.Add(parameter);
                        // cache the parameter
                        cachedParameters.Add(new DbParameterCacheItem { Name = parameterName, DbType = parameter.DbType, Direction = parameter.Direction });
                    }
                }
            }
            // cache parameters
            CacheHelper.AddDbParameters($"_{command.CommandText}", cachedParameters);
        }

        public virtual void SetSqlParameters(DbCommand command)
        {
            var results = regex.Matches(command.CommandText);
            var parameterName = string.Empty;
            foreach (var item in results)
            {
                parameterName = item.ToString().TrimStart(ParameterSymbal.ToCharArray());
                var parameter = command.CreateParameter();
                parameter.ParameterName = parameterName;
                //#Note: sql parameter not set DbType, the DbType will infered when assign value
                //#whether set explicitly or not need to be determined
                command.Parameters.Add(parameter);
            }
        }

        public void AssignParameterValues(DbCommand dbCommand, object parameters)
        {
            if (parameters != null)
            {
                var type = parameters.GetType();
                //is a dictionary, not used currently
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    var dic = parameters as Dictionary<string, dynamic>;
                    foreach (DbParameter param in dbCommand.Parameters)
                    {
                        if (param.Direction == ParameterDirection.Output)
                        {
                            continue;
                        }
                        var kv = dic.FirstOrDefault(d => d.Key.ToUpper() == param.ParameterName.ToUpper());
                        if (!string.IsNullOrEmpty(kv.Key))
                        {
                            param.Value = kv.Value;
                        }
                        else if (param.Direction == ParameterDirection.InputOutput)
                        {
                            param.Value = DBNull.Value;
                        }
                    }
                }
                else//is modal or anonymouse type
                {
                    var properties = CacheHelper.GetTypePropertyInfos(type.FullName);
                    if (properties == null)
                    {
                        properties = type.GetProperties();
                        if (!type.IsAnonymousType())
                        {
                            CacheHelper.AddTypePropertyInfos(type.FullName, properties);
                        }
                    }
                    PropertyInfo property;
                    //# remove underscore and parameter prefix from sp parameters,
                    //# so that can match property name
                    //# eg. p_first_name -> FirstName
                    string normalizePrameterName;
                    foreach (DbParameter param in dbCommand.Parameters)
                    {
                        if (param.Direction == ParameterDirection.Output)
                        {
                            continue;
                        }
                        normalizePrameterName = param.ParameterName;
                        if (!string.IsNullOrEmpty(ParameterPrefix))
                        {
                            normalizePrameterName = normalizePrameterName.Substring(ParameterPrefix.Length, normalizePrameterName.Length - ParameterPrefix.Length);
                        }
                        normalizePrameterName = normalizePrameterName.Replace("_", "");
                        property = properties.FirstOrDefault(p => p.Name.ToUpper() == normalizePrameterName.ToUpper());
                        if (property != null)
                        {
                            //param.DbType = GetDbTypeByClrType(property.PropertyType);
                            param.Value = property.GetValue(parameters);
                        }
                        // if it's pure OUT parameter, Value of InputOutput parameter should set to DBNull.Value, or parameter not provided excetpion would be thrown
                        else if (param.Direction == ParameterDirection.InputOutput)
                        {
                            param.Value = DBNull.Value;
                        }
                    }
                }
            }
        }

        private ParameterDirection GetDirection(string direction)
        {
            switch (direction.ToUpper())
            {
                case "IN":
                    return ParameterDirection.Input;
                case "OUT":
                    return ParameterDirection.Output;
                case "INOUT":
                    return ParameterDirection.InputOutput;
                default:
                    return ParameterDirection.ReturnValue;
            }
        }

        private void SetParameterType(DbParameter dbParameter, string dbTypeString)
        {
            dbTypeString = dbTypeString.ToLower();
            if (this.DbDataTypeDbTypeMap.ContainsKey(dbTypeString))
            {
                dbParameter.DbType = DbDataTypeDbTypeMap[dbTypeString];
            }
        }
    }
}
