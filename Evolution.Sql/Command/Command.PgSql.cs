using Evolution.Sql.Cache;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Evolution.Sql
{
    internal class PgSqlCommand : AbstractCommand
    {
        protected override string DefaultSchema => "public";
        protected override string ParameterSymbol => "@";

        public PgSqlCommand() : base()
        {
        }
    }
}
