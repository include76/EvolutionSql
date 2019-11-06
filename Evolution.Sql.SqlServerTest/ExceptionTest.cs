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
                    Title = "this is a test post title",
                    //Content = "this is a test post content",
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                };

                //var postId = sqlSession.ExecuteScalar<Blog>("insert", blog);
                var outPuts = new Dictionary<string, dynamic> { { "Id", 0 } };
                Assert.Throws<SqlException>(() => connection.Procedure("uspBlogIns").Execute(blog, outPuts));
            }
        }
    }
}
