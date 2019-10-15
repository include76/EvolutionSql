using Evolution.Sql.Attribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Sql.TestCommon
{
    // command with all properties set
    [Command(Name = "Insert", Text ="uspBlogIns", CommandType = System.Data.CommandType.StoredProcedure)]
    //CommandType default to SP
    [Command(Name = "Update", Text ="uspBlogUpd")]
    [Command(Name = "Get", Text = "uspBlogGet")]
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public User CreatedUser { get; set; }
        public IList<Tag> Tags { get; set; }
        public IList<Post> Posts { get; set; }
    }
}
