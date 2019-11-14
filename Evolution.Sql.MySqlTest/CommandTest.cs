using Evolution.Sql.MySqlTest.Model;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution.Sql.MySqlTest
{
    public class CommandTest
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
                var updatedUser = await connection.Procedure("usp_user_get")
                    .QueryOneAsync<User>(pUserId, pTotalCount);
                Assert.NotNull(updatedUser);
                Assert.AreEqual("Bob", updatedUser.FirstName);
            }
        }

        /// <summary>
        /// if SetParameters, then ParameterPrefix should be ignored
        /// parameters of Query/Execute should be ignored
        /// </summary>
        /// <returns></returns>
        /*
        [Test]
        public async Task WithParameters_ParameterPrefix_And_parameters_Should_Be_Ignore_Test()
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
                var pTotalCount = new MySqlParameter("totalCount", MySqlDbType.Int32);
                pTotalCount.Direction = System.Data.ParameterDirection.InputOutput;
                var userFromDb = connection.Procedure("usp_user_get")
                    //.WithParameters(pUserId, pTotalCount)
                    .QueryOne<User>(pUserId, pTotalCount);//expect: the bad_user_id be ignored
                Assert.NotNull(userFromDb);
                Assert.Greater(int.Parse(pTotalCount.Value.ToString()), 0);

                userFromDb.FirstName = "Name should be ignored";

                MySqlParameter[] parameters = { new MySqlParameter("p_user_id",userId),
                    new MySqlParameter("p_first_name", "Bob"),
                    new MySqlParameter("p_last_name", "Lee"),
                    new MySqlParameter("p_updated_by", "system" ),
                    new MySqlParameter("p_updated_on", DateTime.Now)
                };
                connection.Procedure("usp_user_upd")
                    .Execute(parameters);

                var updatedUser = await connection.Procedure("usp_user_get")
                    .QueryOneAsync<User>(new { pUserId = userId });
                Assert.NotNull(updatedUser);
                Assert.AreEqual("Bob", updatedUser.FirstName);
            }
        }*/
    }
}
