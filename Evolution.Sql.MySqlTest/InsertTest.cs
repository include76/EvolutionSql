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
        }

        [Test]
        public void Insert_With_Inline_Sql()
        {
            using (var connection = new MySqlConnection(connectionStr))
            {
                var user = new User
                {
                    UserId = Guid.NewGuid(),
                    FirstName = "Bruce",
                    LastName = "Lee"
                };
                var result = connection.Sql("insert into `user`(UserId, FirstName, LastName) values(@UserId, @FirstName, @LastName);")
                    .Execute(user);
                Assert.Greater(result, 0);
            }
        }

        [Test]
        public async Task Insert_With_Inline_Sql_Auto_Generated_Id()
        {
            using (var connection = new MySqlConnection(connectionStr))
            {
                var tag = new Tag
                {
                    Name = "CSharp",
                    Description = "programe language i love"
                };
                var tagId1 = connection.Sql("INSERT INTO `tag` VALUES(NULL, @Name, @Description); SELECT LAST_INSERT_ID();")
                    .ExecuteScalar(tag);
                Assert.Greater(int.Parse(tagId1.ToString()), 0);
                tag = new Tag
                {
                    Name = "C",
                    Description = "c langugae"
                };
                var tagId2 = await connection.Sql("INSERT INTO `tag` VALUES(NULL, @Name, @Description); SELECT LAST_INSERT_ID();")
                    .ExecuteScalarAsync(tag);
                Assert.Greater(int.Parse(tagId2.ToString()), 0);
                Assert.AreNotEqual(tagId1, tagId2);
            }
        }

        [Test]
        public void Insert_With_StoredProcedure()
        {
            using (var connection = new MySqlConnection(connectionStr))
            {
                var userId = Guid.NewGuid();
                var user = new User
                {
                    UserId = userId,
                    FirstName = "Bruce",
                    LastName = "Lee"
                };
                connection.Sql("insert into `user`(UserId, FirstName, LastName) values(@UserId, @FirstName, @LastName);")
                    .Execute(user);

                var blog = new Blog
                {
                    Title = "this is a test post title",
                    Content = "this is a test post content",
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = null
                };

                var outPuts = new Dictionary<string, dynamic>();
                connection.Procedure("usp_blog_ins").Execute(blog, outPuts);
                var postId = outPuts["BlogId"];
                Assert.NotNull(postId);
                Assert.Greater(int.Parse(postId.ToString()), 0);
                // just for test cache parameter
                outPuts = new Dictionary<string, dynamic>();
                connection.Procedure("usp_blog_ins").Execute(blog, outPuts);
                postId = outPuts["BlogId"];
                Assert.NotNull(postId);
                Assert.Greater(int.Parse(postId.ToString()), 0);
            }
        }
    }
}
