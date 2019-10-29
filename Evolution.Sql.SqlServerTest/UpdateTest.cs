using Evolution.Sql.SqlServerTest.Modal;
using Evolution.Sql.TestCommon;
using Evolution.Sql.TestCommon.Interface;
using NUnit.Framework;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace Evolution.Sql.SqlServerTest
{
    public class UpdateTest: IUpdateTest
    {
        private string connectionStr = @"Data Source =.\sqlexpress; Initial Catalog = Blog; Integrated Security = True";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Update_With_Inline_Sql()
        {
            var userId = Guid.NewGuid();
            using (var connection = new SqlConnection(connectionStr))
            {
                var user = new User
                {
                    UserId = userId,
                    FirstName = "Bruce",
                    LastName = "Lee"
                };
                var result = connection.Sql(@"insert into [user](UserId, FirstName, LastName) 
                                                values(@UserId, @FirstName, @LastName)")
                    .Execute(user);
                Assert.Greater(result, 0);
            }

            using (var connection = new SqlConnection(connectionStr))
            {
                var user = connection.Sql("select * from [user] where userid = @UserId")
                        .Query<User>(new { UserId = userId })?.FirstOrDefault();
                Assert.NotNull(user);
                Assert.AreEqual(userId, user.UserId);

                user.FirstName = "Luice";
                user.LastName = "Tom";
                user.UpdatedOn = DateTime.Now;
                var result = connection.Sql("update [user] set FirstName=@FirstName, LastName=@LastName, UpdatedOn=@UpdatedOn where UserId = @UserId").Execute(user);
                Assert.AreEqual(1, result);
            }

            using (var connection = new SqlConnection(connectionStr))
            {
                var user = connection.Sql("select * from [user] where userid = @UserId").Query<User>(new { UserId = userId })?.FirstOrDefault();
                Assert.NotNull(user);
                Assert.AreEqual(userId, user.UserId);
                Assert.AreEqual("Luice", user.FirstName);
                Assert.AreEqual("Tom", user.LastName);
            }
        }

        [Test]
        public void Update_With_StoredProcedure()
        {
            //throw new System.NotImplementedException();
        }
    }
}