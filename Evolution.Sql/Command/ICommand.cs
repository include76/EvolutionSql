using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Evolution.Sql
{
    public interface ICommand
    {
        DbCommand Build(object parameters);

        string ParameterPrefix { get; set; }

        DbParameter[] ExplicitParameters { get; set; }
    }
}
