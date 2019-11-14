using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Evolution.Sql
{
    internal class SqlServerCommand : AbstractCommand
    {
        protected override string DefaultSchema
        {
            get
            {
                return "dbo";
            }
        }

        public SqlServerCommand() : base()
        {
        }
    }
}
