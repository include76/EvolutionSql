using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Sql.Attribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute: System.Attribute
    {
        public string ColumnName { get; set; }
    }
}
