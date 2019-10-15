using Evolution.Sql.Attribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Sql.TestCommon
{
    #region sql server commands
    [Command(Name = "Insert"
        , Text = @"insert into [user](UserId, FirstName, LastName) values(@UserId, @FirstName, @LastName);"
        , CommandType = System.Data.CommandType.Text)]
    [Command(Name = "Get" 
        , Text = @"select * from [user] where userid = @UserId"
        , CommandType = System.Data.CommandType.Text)]
    [Command(Name = "GetPartialCol"
        , Text = @"select UserId, FirstName, CreatedOn from [user] where userid = @userId"
        , CommandType = System.Data.CommandType.Text)]
    [Command(Name = "Update"
        , Text = @"update [user] set FirstName=@FirstName, LastName=@LastName, UpdatedOn=@UpdatedOn
                    where UserId = @UserId"
        , CommandType = System.Data.CommandType.Text)]
    [Command(Name = "TestTableParameter"
        , Text = "[dbo].[uspWithTableParameter]")]
    #endregion
    public class User
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
