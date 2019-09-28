using Evolution.Sql.Attribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Sql.TestCommon
{
    // command with all properties set
    [Command(Text ="uspBlogIns", CommandType = System.Data.CommandType.StoredProcedure)]
    // command only with Text, Name defalut to Text, CommandType default to SP
    [Command(Text ="uspBlogUpd")]
    [Command(Text = "upsBlogGet")]
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public User CreatedUser { get; set; }
        public IList<Tag> Tags { get; set; }
        public IList<Post> Posts { get; set; }
    }
}
