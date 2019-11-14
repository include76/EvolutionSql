using Evolution.Sql.SqlServerTest.Model;
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
                var user = new
                {
                    UserId = Guid.NewGuid(),
                    FirstName = "Bruce",
                    LastName = "Lee",
                    CreatedOn = DateTime.Now
                };
                var result = connection.Sql(@"insert into [user](UserId, FirstName, LastName, CreatedOn) 
                                                values(@UserId, @FirstName, @LastName, @CreatedOn)")
                    .SetTimeOut(3)
                    .Execute(user);
                Assert.Greater(result, 0);
            }
        }

        [Test]
        public async Task Insert_With_Inline_Sql_Auto_Generated_Id()
        {
            using (var connection = new SqlConnection(connectionStr))
            {
                var tag = new Tag
                {
                    Name = "CSharp",
                    Description = "programe language i love"
                };
                var tagId1 = await connection.Sql("insert into [tag] values(@Name, @Description) select SCOPE_IDENTITY()")
                    .ExecuteScalarAsync(tag);
                Assert.Greater(int.Parse(tagId1.ToString()), 0);
                tag = new Tag
                {
                    Name = "C",
                    Description = "mother langugae"
                };
                var tagId2 = await connection.Sql("insert into [tag] values(@Name, @Description) select SCOPE_IDENTITY()")
                    .ExecuteScalarAsync(tag);
                Assert.Greater(int.Parse(tagId2.ToString()), 0);
                Assert.AreNotEqual(tagId1, tagId2);
            }
        }

        [Test]
        public void Insert_With_StoredProcedure()
        {
            using (var connection = new SqlConnection(connectionStr))
            {
                var userId = Guid.NewGuid();
                var user = new
                {
                    UserId = userId,
                    FirstName = "Bruce",
                    LastName = "Lee",
                    CreatedOn = DateTime.Now// DateTime.MinValue
                };
                connection.Sql(@"insert into [user](UserId, FirstName, LastName, CreatedOn) 
                                                values(@UserId, @FirstName, @LastName, @CreatedOn)")
                    .Execute(user);

                var blog = new
                {
                    Title = "this is a test post title",
                    Content = "this is a test post content",
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                };

                var postId = connection.Procedure("uspBlogIns").ExecuteScalar(blog);
                Assert.NotNull(postId);
                Assert.Greater(int.Parse(postId.ToString()), 0);
                // just for test cache parameter
                postId = connection.Procedure("uspBlogIns").ExecuteScalar(blog);
                Assert.NotNull(postId);
                Assert.Greater(int.Parse(postId.ToString()), 0);
            }
        }
    }
}

