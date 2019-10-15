using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Sql.CommandAdapter
{
    public class CommandAdapterSqlServer: AbstractCommandAdapter
    {
        protected override string DefaultSchema
        {
            get
            {
                return "dbo";
            }
        }

        protected override string ParameterPrefix => "@";
    }
}
