using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Sql.PgSqlTest.Modal
{
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
