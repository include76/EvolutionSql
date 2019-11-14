using Evolution.Sql.MySqlTest.Model;
using Evolution.Sql.TestCommon.Interface;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
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

                var pUserId = new MySqlParameter("pUserId", userId);
                var pTotalCount = new MySqlParameter("totalCount", MySqlDbType.Int32) { Direction = ParameterDirection.Output };
                var userFromDb = connection.Procedure("usp_user_get")
                    .QueryOne<User>(pUserId, pTotalCount);
                Assert.NotNull(userFromDb);

                var parameters = new
                {
                    p_user_id = userFromDb.UserId,
                    p_first_name = "Bob",
                    p_last_name = userFromDb.LastName,
                    p_updated_by = "system",
                    p_updated_on = DateTime.Now
                };
                connection.Procedure("usp_user_upd")
                    .Execute(parameters);

                pUserId = new MySqlParameter("pUserId", userId);
                pTotalCount = new MySqlParameter("totalCount", MySqlDbType.Int32) { Direction = ParameterDirection.Output };
                var updatedUser = connection.Procedure("usp_user_get")
                    .Query<User>(pUserId, pTotalCount).FirstOrDefault();
                Assert.NotNull(updatedUser);
                Assert.AreEqual("Bob", updatedUser.FirstName);
            }
        }
    }
}
