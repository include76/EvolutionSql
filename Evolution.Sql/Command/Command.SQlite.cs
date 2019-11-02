using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Evolution.Sql
{
    internal class SQLiteCommand : AbstractCommand
    {
        public SQLiteCommand()
        {
            DbDataTypeDbTypeMap = new Dictionary<string, DbType>()
            {
            };
        }
    }
}
