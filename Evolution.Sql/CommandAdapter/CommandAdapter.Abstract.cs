using Evolution.Sql.Attribute;
using Evolution.Sql.Cache;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Evolution.Sql.CommandAdapter
{
    public abstract class AbstractCommandAdapter
    {
        private Regex regex;
        protected static char[] leftQuote = new char[] { '[', '\'', '`', '"' };
        protected static char[] rightQuote = new char[] { ']', '\'', '`', '"' };
        protected string ParameterPattern
        {
            get { return ParameterPrefix + "([^',;=<>\\s\\)]+)"; }
        }

        protected virtual string Sql2GetProcedureParameters
        {
            get { return @"select PARAMETER_NAME, PARAMETER_MODE, DATA_TYPE from INFORMATION_SCHEMA.PARAMETERS where SPECIFIC_SCHEMA= @schema and SPECIFIC_NAME = @name"; }
        }

        protected virtual string DefaultSchema { get; }

        protected virtual string ParameterPrefix => "@";

        protected virtual Dictionary<string, DbType> DbDataTypeDbTypeMap { get; set; }

        protected virtual Dictionary<Type, DbType> ClrTypeDbTypeMap { get; set; }

        public AbstractCommandAdapter()
        {
            regex = new Regex(ParameterPattern);
            ClrTypeDbTypeMap = new Dictionary<Type, DbType>
            {
                [typeof(byte)] = DbType.Byte,
                [typeof(sbyte)] = DbType.SByte,
                [typeof(short)] = DbType.Int16,
                [typeof(ushort)] = DbType.UInt16,
                [typeof(int)] = DbType.Int32,
                [typeof(uint)] = DbType.UInt32,
                [typeof(long)] = DbType.Int64,
                [typeof(ulong)] = DbType.UInt64,
                [typeof(float)] = DbType.Single,
                [typeof(double)] = DbType.Double,
                [typeof(decimal)] = DbType.Decimal,
                [typeof(bool)] = DbType.Boolean,
                [typeof(string)] = DbType.String,
                [typeof(char)] = DbType.StringFixedLength,
                [typeof(Guid)] = DbType.Guid,
                [typeof(DateTime)] = DbType.DateTime,
                [typeof(DateTimeOffset)] = DbType.DateTimeOffset,
                [typeof(TimeSpan)] = DbType.Time,
                [typeof(byte[])] = DbType.Binary,
                [typeof(byte?)] = DbType.Byte,
                [typeof(sbyte?)] = DbType.SByte,
                [typeof(short?)] = DbType.Int16,
                [typeof(ushort?)] = DbType.UInt16,
                [typeof(int?)] = DbType.Int32,
                [typeof(uint?)] = DbType.UInt32,
                [typeof(long?)] = DbType.Int64,
                [typeof(ulong?)] = DbType.UInt64,
                [typeof(float?)] = DbType.Single,
                [typeof(double?)] = DbType.Double,
                [typeof(decimal?)] = DbType.Decimal,
                [typeof(bool?)] = DbType.Boolean,
                [typeof(char?)] = DbType.StringFixedLength,
                [typeof(Guid?)] = DbType.Guid,
                [typeof(DateTime?)] = DbType.DateTime,
                [typeof(DateTimeOffset?)] = DbType.DateTimeOffset,
                [typeof(TimeSpan?)] = DbType.Time,
                [typeof(object)] = DbType.Object
            };
        }

        public DbCommand Build<T>(DbConnection connection, string commandName, object parameters)
        {
            var dbCommand = connection.CreateCommand();
            var type = typeof(T);
            //TODO: cache command object
            var attr = type.GetCustomAttributes<CommandAttribute>(false)?.FirstOrDefault(x => x.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase));
            if (attr == null)
            {
                throw new Exception(@$"Command {commandName} not found, {type.FullName}");
            }
            if (string.IsNullOrEmpty(attr.Text))
            {
                throw new Exception(@$"Command Text could not be empty, {type.FullName}");
            }
            dbCommand.CommandType = attr.CommandType;
            dbCommand.CommandText = attr.Text;
            SetParameters(dbCommand, type.FullName);
            AssignParameterValues(dbCommand, parameters);
            return dbCommand;
        }

        protected void SetParameters(DbCommand command, string typeFullName)
        {
            if (command.CommandType == CommandType.StoredProcedure)
            {
                SetStoredProcedureParameters(command, typeFullName);
            }
            else
            {
                SetSqlParameters(command);
            }
        }

        public virtual void SetStoredProcedureParameters(DbCommand command, string typeFullName)
        {
            var cachedParameters = CacheHelper.GetDbParameters($"{typeFullName}_{command.CommandText}");
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
            CacheHelper.AddDbParameters($"{typeFullName}_{command.CommandText}", cachedParameters);
        }

        public virtual void SetSqlParameters(DbCommand command)
        {
            var results = regex.Matches(command.CommandText);
            var parameterName = string.Empty;
            foreach (var item in results)
            {
                parameterName = item.ToString().TrimStart(ParameterPrefix.ToCharArray());
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
                    foreach (DbParameter param in dbCommand.Parameters)
                    {
                        if (param.Direction == ParameterDirection.Output)
                        {
                            continue;
                        }
                        property = properties.FirstOrDefault(p => p.Name.ToUpper() == param.ParameterName.ToUpper());
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

        private DbType GetDbTypeByString(string dbTypeString)
        {
            if (this.DbDataTypeDbTypeMap.ContainsKey(dbTypeString))
            {
                return DbDataTypeDbTypeMap[dbTypeString];
            }
            else
            {
                throw new Exception(string.Format("DbType {0} not supported.", dbTypeString));
            }
        }

        private DbType GetDbTypeByClrType(Type clrType)
        {
            if (this.ClrTypeDbTypeMap.ContainsKey(clrType))
            {
                return ClrTypeDbTypeMap[clrType];
            }
            else
            {
                throw new Exception(string.Format($"{clrType.Name} not supported"));
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
