using Evolution.Sql.Attribute;
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

        protected virtual Dictionary<string, DbType> DbEgineTypeToDbTypeMap { get; set; }

        public AbstractCommandAdapter()
        {
            regex = new Regex(ParameterPattern);
            //https://msdn.microsoft.com/en-us/library/cc716729(v=vs.110).aspx
            DbEgineTypeToDbTypeMap = new Dictionary<string, DbType>() {
                { "bigint",             DbType.Int64 },
                { "binary",             DbType.Binary },
                { "bit",                DbType.Boolean },
                { "char",               DbType.AnsiStringFixedLength },
                { "date",               DbType.Date },
                { "datetime",           DbType.DateTime },
                { "datetime2",          DbType.DateTime2 },
                { "datetimeoffset",     DbType.DateTimeOffset },
                { "decimal",            DbType.Decimal },
                //{ "FILESTREAM", DbType.Binary },//???
                { "float",              DbType.Double },
                { "image",              DbType.Int64 },
                { "int",                DbType.Int32 },
                { "money",              DbType.Decimal },
                { "nchar",              DbType.AnsiStringFixedLength },
                { "ntext",              DbType.String },
                { "numeric",            DbType.Decimal },
                { "nvarchar",           DbType.String },
                { "real",               DbType.Single },
                { "rowversion",         DbType.Binary },
                { "smalldatetime",      DbType.DateTime },
                { "smallint",           DbType.Int16 },
                { "smallmoney",         DbType.Decimal },
                { "sql_variant",        DbType.Object },
                { "text",               DbType.String },
                { "time",               DbType.Time },
                { "timestamp",          DbType.Binary },
                { "tinyint",            DbType.Byte },
                { "uniqueidentifier",   DbType.Guid },
                { "varbinary",          DbType.Binary },
                { "varchar",            DbType.AnsiStringFixedLength },
                { "xml",                DbType.Xml },
                //{ "table type",         DbType.Object }:::Not works, when it's table type, let clr infer the DbType
                //------MySql Specific

                //------Oracle Specific
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
            SetParameters(dbCommand);
            AssignParameterValues(dbCommand, parameters);
            return dbCommand;
        }

        public void SetParameters(DbCommand command)
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
                    }
                }
            }
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
                command.Parameters.Add(parameter);
            }
        }

        public void AssignParameterValues(DbCommand dbCommand, object parameters)
        {
            if (parameters != null)
            {
                var type = parameters.GetType();
                //is a dictionary
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
                else//is an anonymouse type
                {
                    var propertyInfos = type.GetProperties();
                    PropertyInfo property;
                    foreach (DbParameter param in dbCommand.Parameters)
                    {
                        if (param.Direction == ParameterDirection.Output)
                        {
                            continue;
                        }
                        property = propertyInfos.FirstOrDefault(p => p.Name.ToUpper() == param.ParameterName.ToUpper());
                        if (property != null)
                        {
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
            if (this.DbEgineTypeToDbTypeMap.ContainsKey(dbTypeString))
            {
                return DbEgineTypeToDbTypeMap[dbTypeString];
            }
            else
            {
                throw new Exception(string.Format("DbType {0} not supported.", dbTypeString));
            }
        }

        private void SetParameterType(DbParameter dbParameter, string dbTypeString)
        {
            if (this.DbEgineTypeToDbTypeMap.ContainsKey(dbTypeString))
            {
                dbParameter.DbType = DbEgineTypeToDbTypeMap[dbTypeString];
            }
        }
    }
}
