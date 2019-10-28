using Evolution.Sql.SqlServerTest.Modal;
using Evolution.Sql.TestCommon;
using Evolution.Sql.TestCommon.Interface;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

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
            using(var connection = new SqlConnection(connectionStr))
            {
                var user = new User
                {
                    UserId = Guid.NewGuid(),
                    FirstName = "Bruce",
                    LastName = "Lee",
                    CreatedOn = DateTime.Now
                };
                var result = connection.Sql(@"insert into [user](UserId, FirstName, LastName, CreatedOn) 
                                                values(@UserId, @FirstName, @LastName, @CreatedOn)")
                    .Execute(user);
                Assert.Greater(result, 0);
            }
        }

        [Test]
        public async Task Insert_With_Inline_Sql_Auto_Generated_Id()
        {
            using (ISqlSession sqlSession = new SqlSession(new SqlConnection(connectionStr)))
            {
                var tag = new Tag
                {
                    Name = "CSharp",
                    Description = "programe language i love"
                };
                var tagId1 = sqlSession.ExecuteScalar<Tag>("insert", tag);
                Assert.Greater(int.Parse(tagId1.ToString()), 0);
                tag = new Tag
                {
                    Name = "C",
                    Description = "mother langugae"
                };
                var tagId2 = await sqlSession.ExecuteScalarAsync("insert", tag);
                Assert.Greater(int.Parse(tagId2.ToString()), 0);
                Assert.AreNotEqual(tagId1, tagId2);
            }
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
                sqlSession.Execute<Blog>("insert", blog, outPuts);
                var postId = outPuts["BlogId"];
                Assert.NotNull(postId);
                Assert.Greater(int.Parse(postId.ToString()), 0);
                // just for test cache parameter
                outPuts = new Dictionary<string, dynamic>();
                sqlSession.Execute<Blog>("insert", blog, outPuts);
                postId = outPuts["BlogId"];
                Assert.NotNull(postId);
                Assert.Greater(int.Parse(postId.ToString()), 0);
            }
        }
    }
}

