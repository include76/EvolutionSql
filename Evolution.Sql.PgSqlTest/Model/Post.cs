using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Sql.PgSqlTest.Model
{
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
