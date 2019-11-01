using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Evolution.Sql.MySqlTest.Modal
{ 
    public class User
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
