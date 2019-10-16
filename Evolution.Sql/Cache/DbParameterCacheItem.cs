using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Evolution.Sql.Cache
{
    internal class DbParameterCacheItem
    {
        public string Name { get; set; }
        public ParameterDirection Direction { get; set; }
        public DbType DbType { get; set; }
    }
}
