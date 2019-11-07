using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Evolution.Sql
{
    public interface ITypeHandler
    {
        void SetDbParameter(DbParameter dbParameter);
        object GetValue(DbDataReader dbDataReader, int index);
    }
}
