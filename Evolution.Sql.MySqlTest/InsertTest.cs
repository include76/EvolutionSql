using Evolution.Sql.MySqlTest.Modal;
using Evolution.Sql.TestCommon.Interface;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Evolution.Sql.MySqlTest
{
    public class InsertTest: IInsertTest
    {
        private string connectionStr = @"server=localhost;user=root;database=blog;port=3306;password=root";
        [SetUp]
        public void Setup()
        {
            DbProviderFactories.RegisterFactory("MySql.Data.MySqlClient", MySqlClientFactory.Instance);
        }

        [Test]
        public void Insert_With_Inline_Sql()
        {
            using (var sqlSession = new SqlSession("MySql.Data.MySqlClient", connectionStr))
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
                var postId = outPuts["Id"];
                Assert.NotNull(postId);
                Assert.Greater(int.Parse(postId.ToString()), 0);
                // just for test cache parameter
                sqlSession.Execute<Blog>("insert", blog, outPuts);
                postId = outPuts["Id"];
                Assert.NotNull(postId);
                Assert.Greater(int.Parse(postId.ToString()), 0);
            }
        }
    }
}
