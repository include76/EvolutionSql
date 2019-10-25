using Evolution.Sql.Attribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Sql.SqlServerTest.Modal
{
    [Command(Name = "insert", Text = "insert into [tag] values(@Name, @Description) select SCOPE_IDENTITY()", CommandType = System.Data.CommandType.Text)]
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
