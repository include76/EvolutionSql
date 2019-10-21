using Evolution.Sql.MySqlTest.Modal;
using Evolution.Sql.TestCommon.Interface;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Evolution.Sql.MySqlTest
{
    public class QueryTest : IQueryTest
    {
        private string connectionStr = @"server=localhost;user=root;database=blog;port=3306;password=root";
        [SetUp]
        public void Setup()
        {
            DbProviderFactories.RegisterFactory("MySql.Data.MySqlClient", MySqlClientFactory.Instance);
        }

        [Test]
        public void QueryOne_With_Inline_Sql()
        {
            using (var sqlSession = new SqlSession("MySql.Data.MySqlClient", connectionStr))
            {
                var userId1 = Guid.NewGuid();
                var user = new User
                {
                    UserId = userId1,
                    FirstName = "Bruce",
                    LastName = "Lee",
                    CreatedOn = DateTime.Now
                };
                var result = sqlSession.Execute<User>("insert", user);
                Assert.Greater(result, 0);

                var bruce = sqlSession.QueryOne<User>("get", new { UserId = userId1 });
                Assert.IsNotNull(bruce);
                Assert.AreEqual(userId1, bruce.UserId);

                var userId2 = Guid.NewGuid();
                user = new User
                {
                    UserId = userId2,
                    FirstName = "Tom",
                    LastName = "Ren",
                    CreatedOn = DateTime.Now
                };

                result = sqlSession.Execute<User>("insert", user);
                Assert.Greater(result, 0);

                var tom = sqlSession.QueryOne<User>("get", new { UserId = userId2 });
                Assert.IsNotNull(tom);
                Assert.AreEqual(userId2, tom.UserId);

                Assert.AreNotEqual(bruce.FirstName, tom.FirstName);
            }
        }

        [Test]
        public void QueryOne_With_StoredProcedure()
        {
            using (var sqlSession = new SqlSession("MySql.Data.MySqlClient", connectionStr))
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
            using (var sqlSession = new SqlSession("MySql.Data.MySqlClient", connectionStr))
            {
                var userId = Guid.NewGuid();
                var user = new User
                {
                    UserId = userId,
                    FirstName = "Bruce",
                    LastName = "Lee"
                };
                sqlSession.Execute<User>("insert", user);

                var blog = new Blog
                {
                    Title = "this is a test post title",
                    Content = "this is a test post content",
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                };

                var outPuts = new Dictionary<string, dynamic>();
                sqlSession.Execute<Blog>("insert", blog, out outPuts);
                var postId = outPuts["Id"];
                Assert.NotNull(postId);
                Assert.Greater(int.Parse(postId.ToString()), 0);
                sqlSession.Execute<Blog>("insert", blog, out outPuts);
                postId = outPuts["Id"];
                Assert.NotNull(postId);
                Assert.Greater(int.Parse(postId.ToString()), 0);

                // query
                var blogs = sqlSession.Query<Blog>("getall", null);
                Assert.NotNull(blogs);
                Assert.Greater(blogs.ToList().Count, 1);
            }
        }

        [Test]
        public void Query_With_StoredProcedure()
        {
            //throw new NotImplementedException();
        }

        [Test]
        public void Get_Null_Value_From_DB_Property_Should_Set_Default_Value()
        {
            using (var sqlSession = new SqlSession("MySql.Data.MySqlClient", connectionStr))
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
