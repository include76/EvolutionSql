using Evolution.Sql.MySqlTest.Modal;
using Evolution.Sql.TestCommon.Interface;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Evolution.Sql.MySqlTest
{
    public class InsertTest : IInsertTest
    {
        private string connectionStr = @"server=localhost;user=root;database=blog;port=3306;password=root";
        [SetUp]
        public void Setup()
        {
            // register factory once when you application startup
            DbProviderFactories.RegisterFactory("MySql.Data.MySqlClient", MySqlClientFactory.Instance);
        }

        [Test]
        public void Insert_With_Inline_Sql()
        {
            // ISqlSession should be injected in real application 
            using (ISqlSession sqlSession = new SqlSession("MySql.Data.MySqlClient", connectionStr))
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
        public async Task Insert_With_Inline_Sql_Auto_Generated_Id()
        {
            using (ISqlSession sqlSession = new SqlSession("MySql.Data.MySqlClient", connectionStr))
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
                    Description = "c langugae"
                };
                var tagId2 = await sqlSession.ExecuteScalarAsync("insert", tag);
                Assert.Greater(int.Parse(tagId2.ToString()), 0);
                Assert.AreNotEqual(tagId1, tagId2);
            }
        }

        [Test]
        public void Insert_With_StoredProcedure()
        {
            using (ISqlSession sqlSession = new SqlSession("MySql.Data.MySqlClient", connectionStr))
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
                    UpdatedOn = null
                };

                var outPuts = new Dictionary<string, dynamic>();
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
