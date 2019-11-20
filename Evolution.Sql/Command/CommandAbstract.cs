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
            get { return ParameterSymbol + "([^'\",;=<>\\s\\)]+)"; }
        }

        protected virtual string DefaultSchema { get; }

        protected virtual string ParameterSymbol => "@";

        public DbConnection Connection { get; set; }
        public CommandType CommandType { get; set; }
        public string CommandText { get; set; }
        public string ParameterPrefix { get; set; }

        Dictionary<Type, ITypeHandler> _typeHandlers = new Dictionary<Type, ITypeHandler>();
        Dictionary<Type, ITypeHandler> ICommand.TypeHandlers
        {
            get { return _typeHandlers; }
            set { _typeHandlers = value; }
        }

        internal int CommandTimeout { get; set; } = 30;
        internal DbTransaction Transaction { get; set; }

        protected static Dictionary<Type, DbType> ClrTypeDbTypeMap => new Dictionary<Type, DbType>
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

        public AbstractCommand()
        {
            regex = new Regex(ParameterPattern);
        }

        DbCommand ICommand.Build(object obj)
        {
            Connection.TryOpen();
            var dbCommand = Connection.CreateCommand();
            dbCommand.CommandType = CommandType;
            dbCommand.CommandText = CommandText;
            dbCommand.CommandTimeout = CommandTimeout;
            dbCommand.Transaction = Transaction;
            SetParameters(dbCommand, obj);
            return dbCommand;
        }

        DbCommand ICommand.Build(params DbParameter[] parameters)
        {
            Connection.TryOpen();
            var dbCommand = Connection.CreateCommand();
            dbCommand.CommandType = CommandType;
            dbCommand.CommandText = CommandText;
            dbCommand.CommandTimeout = CommandTimeout;
            dbCommand.Transaction = Transaction;
            if (parameters != null)
            {
                dbCommand.Parameters.AddRange(parameters);
            }
            return dbCommand;
        }

        protected void SetParameters(DbCommand dbCommand, object obj)
        {
            if(null == obj)
            {
                return;
            }
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (properties == null)
            {
                return;
            }
            foreach (var property in properties)
            {
                var parameter = dbCommand.CreateParameter();
                parameter.ParameterName = ParameterSymbol + property.Name;
                parameter.Direction = ParameterDirection.Input;

                if (ClrTypeDbTypeMap.ContainsKey(property.PropertyType))
                {
                    parameter.DbType = ClrTypeDbTypeMap[property.PropertyType];
                }

                if (_typeHandlers.Any())
                {
                    if (_typeHandlers.ContainsKey(property.PropertyType))
                    {
                        _typeHandlers[property.PropertyType].SetParameter(parameter);
                    }
                    else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        var genericType = Nullable.GetUnderlyingType(property.PropertyType);
                        if (_typeHandlers.ContainsKey(genericType))
                        {
                            _typeHandlers[genericType].SetParameter(parameter);
                        }
                    }
                }
                parameter.Value = property.GetValue(obj);

                dbCommand.Parameters.Add(parameter);
            }
        }
    }
}
