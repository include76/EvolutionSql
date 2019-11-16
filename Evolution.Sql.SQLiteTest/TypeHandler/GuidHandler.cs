using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Evolution.Sql.SQLiteTest.TypeHandler
{
    public class GuidHandler : ITypeHandler
    {
        public object GetValue(DbDataReader dbDataReader, int index)
        {
            return Guid.Parse(dbDataReader.GetValue(index).ToString());
        }

        public void SetParameter(DbParameter dbParameter)
        {
            dbParameter.DbType = System.Data.DbType.StringFixedLength;
        }
    }
}
