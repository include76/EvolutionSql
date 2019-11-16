using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace Evolution.Sql.SqlServerTest.TypeHandler
{
    public class DateTimeHandler : ITypeHandler
    {
        public object GetValue(DbDataReader dataReader, int index)
        {
            if (dataReader.GetFieldType(index) == typeof(TimeSpan))
            {
                return new DateTime(dataReader.GetFieldValue<TimeSpan>(index).Ticks);
            }
            return dataReader.GetFieldValue<DateTime>(index);
        }

        public void SetParameter(DbParameter dbParameter)
        {
            (dbParameter as SqlParameter).SqlDbType = System.Data.SqlDbType.DateTime2;
        }
    }
}
