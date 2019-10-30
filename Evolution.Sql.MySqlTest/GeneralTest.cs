using Evolution.Sql.MySqlTest.Modal;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution.Sql.MySqlTest
{
    public class GeneralTest
    {
        private string connectionStr = @"server=localhost;user=root;database=blog;port=3306;password=root";

        [Test]
        public async Task Update_Parameter_With_Under_Score_And_Prefix()
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
                    .QueryOne<User>(new { pUserId = userId });
                Assert.NotNull(userFromDb);

                userFromDb.FirstName = "Bob";
                userFromDb.UpdatedBy = "system";
                userFromDb.UpdatedOn = DateTime.Now;
                connection.Procedure("usp_user_upd")
                    .ParameterPrefix("p_")
                    .Execute(userFromDb);

                var updatedUser = await connection.Procedure("usp_user_get")
                    .QueryOneAsync<User>(new { pUserId = userId });
                Assert.NotNull(updatedUser);
                Assert.AreEqual("Bob", updatedUser.FirstName);
            }
        }
    }
}
