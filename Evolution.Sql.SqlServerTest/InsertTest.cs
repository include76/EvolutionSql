using Evolution.Sql.TestCommon;
using Evolution.Sql.TestCommon.Interface;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Evolution.Sql.SqlServerTest
{
    [TestFixture]
    public class InsertTest: IInsertTest
    {
        private string connectionStr = @"Data Source =.\sqlexpress; Initial Catalog = Blog; Integrated Security = True";
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
                    UserId = Guid.NewGuid(),
                    FirstName = "Bruce",
                    LastName = "Lee"
                };
                var result = sqlSession.Execute<User>("insert", user);
                Assert.Greater(result, 0);
            }
        }

        [Test]
        public void Insert_With_Inline_Sql_Auto_Generated_Id()
        {

        }

        [Test]
        public void Insert_With_StoredProcedure()
        {
            var connection = new SqlConnection(connectionStr);
            using (ISqlSession sqlSession = new SqlSession(connection))
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

                var outPuts = new Dictionary<string, dynamic> ();
                sqlSession.Execute<Blog>("insert", blog, out outPuts);
                var postId = outPuts["Id"];
                Assert.NotNull(postId);
                Assert.Greater(int.Parse(postId.ToString()), 0);
                // just for test cache parameter
                sqlSession.Execute<Blog>("insert", blog, out outPuts);
                postId = outPuts["Id"];
                Assert.NotNull(postId);
                Assert.Greater(int.Parse(postId.ToString()), 0);
            }
        }
    }
}

