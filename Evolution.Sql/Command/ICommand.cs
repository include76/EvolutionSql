using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Evolution.Sql
{
    public interface ICommand
    {
        internal DbCommand Build(object parameters);
        internal DbCommand Build(params DbParameter[] parameters);

        string ParameterPrefix { get; set; }

        internal DbParameter[] ExplicitParameters { get; set; }
        internal Dictionary<Type, ITypeHandler> TypeHandlers { get; set; }
        internal int CommandTimeout { get; set; }
        internal DbTransaction Transaction { get; set; }
    }
}
