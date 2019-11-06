using Evolution.Sql.SqlServerTest.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Evolution.Sql.SqlServerTest
{
    public class CommandTest
    {
        private string connectionStr = @"Data Source =.\sqlexpress; Initial Catalog = Blog; Integrated Security = True";
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task WithParameters_ParameterPrefix_And_parameters_Should_Be_Ignore_Test()
        {
            using (var connection = new SqlConnection(connectionStr))
            {
                var userId = Guid.NewGuid();
                var user = new User
                {
                    UserId = userId,
                    FirstName = "Bruce",
                    LastName = "Lee",
                    CreatedOn = DateTime.Now
                };
                var result = connection.Sql(@"insert into [user](UserId, FirstName, LastName, CreatedOn) 
                                                values(@UserId, @FirstName, @LastName, @CreatedOn)")
                    .Execute(user);
                Assert.Greater(result, 0);

                var pUserId = new SqlParameter("@UserId", user.UserId);
                var pTotalCount = new SqlParameter("@totalCount", -1);
                pTotalCount.Direction = System.Data.ParameterDirection.InputOutput;
                var userFromDb = await connection.Procedure("uspUserGet")
                    .WithParameters(pUserId, pTotalCount)
                    .QueryOneAsync<User>();
                Assert.NotNull(userFromDb);
                Assert.Greater(int.Parse(pTotalCount.Value.ToString()), 0);

                SqlParameter[] parameters = {
                    new SqlParameter("pUserId",userId),
                    new SqlParameter("pFirstName", "Bob"),
                    new SqlParameter("pLastName", "Lee"),
                    new SqlParameter("pUpdatedBy", "system" ),
                    new SqlParameter("pUpdatedOn", DateTime.Now)
                    };
                var updatedRowCount = connection.Procedure("uspUserUpd")
                    .ParameterPrefix("p")
                    .WithParameters(parameters)
                    .Execute();
                Assert.AreEqual(1, updatedRowCount);

                var updatedUser = await connection.Procedure("uspUserGet")
                    .QueryOneAsync<User>(new { userId = userId, totalCount = 0 });
                Assert.NotNull(updatedUser);
                Assert.AreEqual("Bob", updatedUser.FirstName);
            }
        }
    }
}
