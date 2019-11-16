using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Evolution.Sql
{
    public interface ITypeHandler
    {
        void SetParameter(DbParameter parameter);
        object GetValue(DbDataReader dataReader, int index);
    }
}
