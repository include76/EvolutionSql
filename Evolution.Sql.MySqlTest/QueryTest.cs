using Evolution.Sql.MySqlTest.Modal;
using Evolution.Sql.TestCommon.Interface;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Sql.MySqlTest
{
    public class QueryTest : IQueryTest
    {
        private string connectionStr = @"server=localhost;user=root;database=blog;port=3306;password=root";
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void QueryOne_With_Inline_Sql()
        {
            var connection = new MySqlConnection(connectionStr);
            using (var sqlSession = new SqlSession(connection))
            {
                var userId = Guid.NewGuid();
                var user = new User
                {
                    UserId = userId,
                    FirstName = "Bruce",
                    LastName = "Lee",
                    CreatedOn = DateTime.Now
                };
                var result = sqlSession.Execute<User>("insert", user);
                Assert.Greater(result, 0);

                var userFromDb = sqlSession.QueryOne<User>("get", new { UserId = userId });
                Assert.IsNotNull(userFromDb);
                Assert.AreEqual(userId, userFromDb.UserId);

                userFromDb = null;
                userFromDb = sqlSession.QueryOne<User>("get", new { UserId = userId });
                Assert.IsNotNull(userFromDb);
                Assert.AreEqual(userId, userFromDb.UserId);
            }
        }

        [Test]
        public void QueryOne_With_StoredProcedure()
        {
            var connection = new MySqlConnection(connectionStr);
            using (var sqlSession = new SqlSession(connection))
            {
                var userId = Guid.NewGuid();
                var user = new User
                {
                    UserId = userId,
                    FirstName = "Bruce",
                    LastName = "Lee"
                };
                var result = sqlSession.Execute<User>("insert", user);
                Assert.Greater(result, 0);

                var outPuts = new Dictionary<string, dynamic>();
                var userFromDb = sqlSession.QueryOne<User>("usp_user_get", new { pUserId = userId }, out outPuts);
                Assert.IsNotNull(userFromDb);
                Assert.AreEqual(userId, userFromDb.UserId);
                Assert.True(outPuts.ContainsKey("totalCount"));
                Assert.Greater(outPuts["totalCount"], 0);
            }
        }

        [Test]
        public void Query_With_Inline_Sql()
        {
            //throw new NotImplementedException();
        }

        [Test]
        public void Query_With_StoredProcedure()
        {
            //throw new NotImplementedException();
        }

        [Test]
        public void Get_Null_Value_From_DB_Property_Should_Set_Default_Value()
        {
            var connection = new MySqlConnection(connectionStr);
            using (var sqlSession = new SqlSession(connection))
            {
                var userId = Guid.NewGuid();
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
                Assert.AreEqual(default(DateTime), userFromDb.CreatedOn);
            }
        }

    }
}
