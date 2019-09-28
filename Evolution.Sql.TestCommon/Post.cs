using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Sql.TestCommon
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
