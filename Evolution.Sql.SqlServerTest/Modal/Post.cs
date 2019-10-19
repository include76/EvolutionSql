using Evolution.Sql.Attribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Sql.SqlServerTest.Modal
{
    [Command(Name = "Insert", Text = "insert into Post values(@content, @createdBy, @CreatedOn) select SCOPE_IDENTITY()", CommandType = System.Data.CommandType.Text)]
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int PostId { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
