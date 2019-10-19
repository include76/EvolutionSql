using Evolution.Sql.SqlServerTest.Modal;
using Evolution.Sql.TestCommon;
using Evolution.Sql.TestCommon.Interface;
using NUnit.Framework;
using System;
using System.Data.SqlClient;

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
            var connection = new SqlConnection(connectionStr);
            var userId = Guid.NewGuid();
            using (var sqlSession = new SqlSession(connection))
            {
                var user = new User
                {
                    UserId = userId,
                    FirstName = "Bruce",
                    LastName = "Lee"
                };
                var result = sqlSession.Execute<User>("insert", user);
                Assert.Greater(result, 0);
            }

            using (var sqlSession = new SqlSession(new SqlConnection(connectionStr)))
            {
                var user = sqlSession.QueryOne<User>("get", new { UserId = userId });
                Assert.NotNull(user);
                Assert.AreEqual(userId, user.UserId);

                user.FirstName = "Luice";
                user.LastName = "Tom";
                user.UpdatedOn = DateTime.Now;
                var result = sqlSession.Execute<User>("Update", user);
                Assert.AreEqual(1, result);
            }

            using (var sqlSession = new SqlSession(new SqlConnection(connectionStr)))
            {
                var user = sqlSession.QueryOne<User>("get", new { UserId = userId });
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