using Evolution.Sql.TestCommon;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Evolution.Sql.TestSqlServer
{
    [TestFixture]
    public class InsertTest
    {
        private string connectionStr = @"";
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Insert_With_Inline_Sql()
        {
            var connection = new SqlConnection(connectionStr);
            using (var sqlSession = new SqlSession(connection))
            {
                var user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    FristName = "Bruce",
                    LastName = "Lee"
                };
                var result = sqlSession.Execute<User>("insert", user);
                Assert.Greater(result, 0);
            }

        }

        [Test]
        public void Insert_With_StoredProcedure()
        {
            var connection = new SqlConnection(connectionStr);
            using (ISqlSession sqlSession = new SqlSession(connection))
            {
                var userId = Guid.NewGuid().ToString();
                var user = new User
                {
                    Id = userId,
                    FristName = "Bruce",
                    LastName = "Lee"
                };
                sqlSession.Execute<User>("insert", user);

                var post = new Post
                {
                    Content = "this is a test post",
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now
                };

                var postId = sqlSession.ExecuteScalar<Post>("uspBlogIns", post);
                Assert.NotNull(postId);
                Assert.Greater((int)postId, 0);
            }
        }
    }
}
