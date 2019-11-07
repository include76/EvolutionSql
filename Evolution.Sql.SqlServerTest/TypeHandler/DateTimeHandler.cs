using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace Evolution.Sql.SqlServerTest.TypeHandler
{
    public class DateTimeHandler : ITypeHandler
    {
        public void GetValue(DbDataReader dbDataReader)
        {
            throw new NotImplementedException();
        }

        public void SetDbParameter(DbParameter dbParameter)
        {
            (dbParameter as SqlParameter).SqlDbType = System.Data.SqlDbType.DateTime2;
        }
    }
}
