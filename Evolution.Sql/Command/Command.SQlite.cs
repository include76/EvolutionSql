using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Evolution.Sql
{
    public class SQLiteCommand : AbstractCommand
    {
        public SQLiteCommand()
        {
            DbDataTypeDbTypeMap = new Dictionary<string, DbType>()
            {
            };
        }
    }
}
