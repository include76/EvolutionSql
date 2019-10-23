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
                {"tinyint", DbType.Byte},
                {"smallint", DbType.Int16},
                {"mediumint", DbType.Int16},
                {"int", DbType.Int32 },
                {"bigint", DbType.Int64},
                //
                {"decimal", DbType.Decimal},
                {"numeric", DbType.Decimal},
                //
                {"float", DbType.Double},
                {"double", DbType.Double},
                {"real", DbType.Single},
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
                {"json", DbType.String}
            };
        }
    }
}
