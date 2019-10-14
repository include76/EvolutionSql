using Evolution.Sql.Attribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Sql.TestCommon
{
    [Command(Name = "insert"
        , Text = @"insert into [user](UserId, FirstName, LastName) values(@UserId, @FirstName, @LastName);"
        , CommandType = System.Data.CommandType.Text)]
    [Command(Name = "get" , Text = @"select * from [user] where userid = @UserId", CommandType = System.Data.CommandType.Text)]
    [Command(Name = "getPartialCol", Text = @"select UserId, FirstName, CreatedOn from [user] where userid = @userId", CommandType = System.Data.CommandType.Text)]
    public class User
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
