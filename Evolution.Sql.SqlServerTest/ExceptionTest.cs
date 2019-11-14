using Evolution.Sql.SqlServerTest.Model;
using Evolution.Sql.TestCommon;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Evolution.Sql.SqlServerTest
{
    public class ExceptionTest
    {
        private string connectionStr = @"Data Source =.\sqlexpress; Initial Catalog = Blog; Integrated Security = True";
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void StoredProcedure_Miss_Parameter_Test()
        {
            using (var connection = new SqlConnection(connectionStr))
            {
                var userId = Guid.NewGuid();

                var blog = new
                {
                    BlogId = 0,
                    Title = "this is a test post title",
                    //Content = "this comment out on purpose",
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                };

                //var postId = sqlSession.ExecuteScalar<Blog>("insert", blog);
                Assert.Throws<SqlException>(() => connection.Procedure("uspBlogIns").Execute(blog));
            }
        }
    }
}
