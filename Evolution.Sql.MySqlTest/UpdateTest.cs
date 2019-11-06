using Evolution.Sql.MySqlTest.Model;
using Evolution.Sql.TestCommon.Interface;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evolution.Sql.MySqlTest
{
    public class UpdateTest : IUpdateTest
    {
        private string connectionStr = @"server=localhost;user=root;database=blog;port=3306;password=root";
        [Test]
        public void Update_With_Inline_Sql()
        {
            //throw new NotImplementedException();
        }

        [Test]
        public void Update_With_StoredProcedure()
        {
            using (var connection = new MySqlConnection(connectionStr))
            {
                var userId = Guid.NewGuid();
                var user = new User
                {
                    UserId = userId,
                    FirstName = "Bruce",
                    LastName = "Lee"
                };
                connection.Sql("insert into `user`(user_id, first_name, last_name) values(@UserId, @FirstName, @LastName);")
                    .Execute(user);

                var userFromDb = connection.Procedure("usp_user_get")
                    .Query<User>(new { pUserId = userId }).FirstOrDefault();
                Assert.NotNull(userFromDb);

                userFromDb.FirstName = "Bob";
                userFromDb.UpdatedBy = "system";
                userFromDb.UpdatedOn = DateTime.Now;
                connection.Procedure("usp_user_upd")
                    .ParameterPrefix("p_")
                    .Execute(userFromDb);

                var updatedUser = connection.Procedure("usp_user_get")
                    .Query<User>(new { pUserId = userId }).FirstOrDefault();
                Assert.NotNull(updatedUser);
                Assert.AreEqual("Bob", updatedUser.FirstName);
            }
        }
    }
}
