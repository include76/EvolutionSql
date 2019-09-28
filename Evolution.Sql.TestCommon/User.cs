using Evolution.Sql.Attribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Sql.TestCommon
{
    [Command(Name = "insert"
        , Text = @"isnert into user(UserId, FirstName, LastName) values(@FirstName, @LastName);"
        , CommandType = System.Data.CommandType.Text)]
    [Command(Name = "get" , Text = @"select * from user where id = @Id")]
    public class User
    {
        public string Id { get; set; }
        public string FristName { get; set; }
        public string LastName { get; set; }
    }
}
