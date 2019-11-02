using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Evolution.Sql
{
    internal class MySqlCommand : AbstractCommand
    {
        public MySqlCommand() : base()
        {
            //https://dev.mysql.com/doc/workbench/en/wb-migration-database-mssql-typemapping.html
            DbDataTypeDbTypeMap = new Dictionary<string, DbType>()
            {
                //Integer
                {"tinyint", DbType.Byte},
                {"smallint", DbType.Int16},
                {"mediumint", DbType.Int16},
                {"int", DbType.Int32 },
                {"bigint", DbType.Int64},
                //
                {"decimal", DbType.Decimal},
                {"numeric", DbType.Decimal},
                //
                {"float", DbType.Single},
                {"double", DbType.Double},
                {"real", DbType.Double},
                //
                {"bit", DbType.Boolean},
                //date type
                {"datetime", DbType.DateTime},
                {"date", DbType.Date},
                {"time", DbType.Time},
                {"timestamp", DbType.Binary},
                {"year", DbType.Int16},
                //string type
                {"char", DbType.StringFixedLength},
                {"varchar", DbType.String},
                {"tinytext", DbType.String},
                {"text", DbType.String },
                {"mediumtext", DbType.String},
                {"longtext", DbType.String},
                //
                {"binary", DbType.Binary},
                {"varbinary", DbType.Binary},
                {"tinyblog", DbType.Binary},
                {"blob",  DbType.Binary},
                {"mediumblob", DbType.Binary},
                {"longblob", DbType.Binary},
                //Json
                {"json", DbType.String},
                // synonym
                {"bool", DbType.Boolean },
                {"boolean", DbType.Boolean }
            };
        }
    }
}
