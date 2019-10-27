using Evolution.Sql.Attribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Sql.MySqlTest.Modal
{ 
    [Command(Name = "insert", 
        Text = @"INSERT INTO `tag` VALUES(NULL, @Name, @Description); SELECT LAST_INSERT_ID();", 
        CommandType = System.Data.CommandType.Text)]
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
