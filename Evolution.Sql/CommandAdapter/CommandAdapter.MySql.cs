/***
 * Author: Locke Duan
 * Date: 2019-10-18
 ***/
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Evolution.Sql.CommandAdapter
{
    public class CommandAdapterMySql : AbstractCommandAdapter
    {
        public CommandAdapterMySql() : base()
        {
            //https://dev.mysql.com/doc/workbench/en/wb-migration-database-mssql-typemapping.html
            DbEgineTypeDbTypeMap = new Dictionary<string, DbType>()
            {
                //Integer
                {"TINYINT", DbType.Byte},
                {"SMALLINT", DbType.Int16},
                {"MEDIUMINT", DbType.Int16},
                {"INT", DbType.Int32 },
                {"BIGINT", DbType.Int64 },
                //
                {"DECIMAL", DbType.Decimal},
                {"NUMERIC", DbType.Decimal},
                //
                {"FLOAT ", DbType.Double},
                {"DOUBLE ", DbType.Double},
                {"REAL", DbType.Single },
                //
                {"BIT ", DbType.Boolean},
                //date type
                {"DATETIME ", DbType.DateTime},
                {"DATE ", DbType.Date },
                {"TIME ", DbType.Time  },
                {"TIMESTAMP ", DbType.Binary },
                {"YEAR ", DbType.Int16 },
                {"YEAR ", DbType.Int16 },
                //string type
                {"CHAR ", DbType.StringFixedLength},
                {"VARCHAR ", DbType.String},
                {"TINYTEXT ", DbType.String},
                {"TEXT ", DbType.String },
                {"MEDIUMTEXT ", DbType.String},
                {"LONGTEXT ",  DbType.String},
                //
                {"BINARY ", DbType.Binary},
                {"VARBINARY ", DbType.Binary},
                {"TINYBLOB ", DbType.Binary},
                {"BLOB ",  DbType.Binary},
                {"MEDIUMBLOB ", DbType.Binary},
                {"LONGBLOB ", DbType.Binary},
                //Json
                {"JSON",  DbType.String}
            };
        }
    }
}
