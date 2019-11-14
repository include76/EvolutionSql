using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Evolution.Sql
{
    public interface ICommand
    {
        internal int CommandTimeout { get; set; }
        internal DbTransaction Transaction { get; set; }
        internal Dictionary<Type, ITypeHandler> TypeHandlers { get; set; }

        internal DbCommand Build(object parameters);
        internal DbCommand Build(params DbParameter[] parameters);
    }
}
