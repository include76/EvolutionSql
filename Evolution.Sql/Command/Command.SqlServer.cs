using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Evolution.Sql.Command
{
    public class SqlServerCommand : AbstractCommand
    {
        public SqlServerCommand() : base()
        {
            //https://msdn.microsoft.com/en-us/library/cc716729(v=vs.110).aspx
            DbDataTypeDbTypeMap = new Dictionary<string, DbType>() {
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
                { "image",              DbType.Binary },
                { "int",                DbType.Int32 },
                { "money",              DbType.Decimal },
                { "nchar",              DbType.StringFixedLength },
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
            };
        }
    }
}
