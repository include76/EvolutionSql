using Evolution.Sql.TestCommon;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Evolution.Sql.SqlServerTest
{
    [TestFixture]
    public class GetTest
    {
        private string connectionStr = @"Data Source =.\sqlexpress; Initial Catalog = Blog; Integrated Security = True";
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void GetOne_With_Inline_Sql()
        {
            var connection = new SqlConnection(connectionStr);
            using (var sqlSession = new SqlSession(connection))
            {
                var userId = Guid.NewGuid().ToString();
                var user = new User
                {
                    UserId = userId,
                    FirstName = "Bruce",
                    LastName = "Lee"
                };
                var result = sqlSession.Execute<User>("insert", user);
                Assert.Greater(result, 0);

                var userFromDb = sqlSession.QueryOne<User>("get", new  { UserId = userId });
                Assert.IsNotNull(userFromDb);
                Assert.AreEqual(userId, userFromDb.UserId);

                userFromDb = null;
                userFromDb = sqlSession.QueryOne<User>("get", new Dictionary<string, dynamic> { { "UserId", userId } });
                Assert.IsNotNull(userFromDb);
                Assert.AreEqual(userId, userFromDb.UserId);
            }
        }

        [Test]
        public void Get_Null_Value_From_DB_Property_Should_Set_Default_Value()
        {
            var connection = new SqlConnection(connectionStr);
            using (var sqlSession = new SqlSession(connection))
            {
                var userId = Guid.NewGuid().ToString();
                var user = new User
                {
                    UserId = userId,
                    FirstName = "Bruce",
                    LastName = "Lee"
                };
                var result = sqlSession.Execute<User>("insert", user);
                Assert.Greater(result, 0);

                var userFromDb = sqlSession.QueryOne<User>("getPartialCol", new { UserId = userId });
                Assert.IsNotNull(userFromDb);
                Assert.AreEqual(userId, userFromDb.UserId);
            }
        }
    }
}
