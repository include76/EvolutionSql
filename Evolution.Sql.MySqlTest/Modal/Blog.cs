using Evolution.Sql.Attribute;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Evolution.Sql.MySqlTest.Modal
{
    [Command(Name = "Insert", Text = "usp_blog_ins")]
    [Command(Name = "Update", Text = "usp_blog_upd")]
    [Command(Name = "Get", Text = "usp_blog_get")]
    [Command(Name = "GetAll", Text = "select * from `blog`", CommandType = CommandType.Text)]
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public User CreatedUser { get; set; }
        public IList<Tag> Tags { get; set; }
        public IList<Post> Posts { get; set; }
    }
}
