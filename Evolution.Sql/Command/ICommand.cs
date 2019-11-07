using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Evolution.Sql
{
    public interface ICommand
    {
        DbCommand Build(object parameters);

        string ParameterPrefix { get; set; }

        internal DbParameter[] ExplicitParameters { get; set; }
        internal Dictionary<Type, ITypeHandler> TypeHandlers { get; set; }
    }
}
