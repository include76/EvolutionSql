using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Sql.PgSqlTest.Modal
{
    public class User
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
