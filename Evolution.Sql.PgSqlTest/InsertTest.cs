using Evolution.Sql.PgSqlTest.Model;
using Npgsql;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Evolution.Sql.PgSqlTest
{
    public class InsertTest
    {
        private string connectionStr = @"Server=127.0.0.1;Port=5432;Database=blog;User Id=postgres;Password=";

        private string userInsSql = "insert into \"user\"(user_id, first_name, last_name, created_by, created_on)" +
                                    "values(@UserId, @FirstName, @LastName, @CreatedBy, @CreatedOn)";

        [SetUp]
        public void Setup()
        {

        }

        public void Insert_With_Function_And_Return_Auto_Id()
        {

        }

        [Test]
        public void Insert_With_Function_Overloaded()
        {
            using (var connection = new NpgsqlConnection(connectionStr))
            {
                var userId = Guid.NewGuid();
                var user = new
                {
                    UserId = userId,
                    FirstName = "Bruce",
                    LastName = "Lee",
                    CreatedBy = "Locke",
                    CreatedOn = DateTime.Now
                };

                var result = connection.Sql(userInsSql)
                   .Execute(user);
                Assert.Greater(result, 0);

                var parameters = new Npgsql.NpgsqlParameter[]
                {
                    new NpgsqlParameter("p_content", "随便发点什么"),
                    new NpgsqlParameter("p_title", "this is a pgslq blog"),
                    new NpgsqlParameter("p_created_on", DateTime.Now),
                    new NpgsqlParameter("p_created_by", userId),
                    //new NpgsqlParameter("p_noexists", 10),
                    //new NpgsqlParameter("p_blog_id", System.Data.DbType.Int32){ Direction = System.Data.ParameterDirection.Output }
                };
                var blogId1 = connection.Procedure("blog_ins")
                    .ExecuteScalar(parameters);

                //var blogId1 = int.Parse(parameters[4].Value.ToString());
                Assert.Greater(int.Parse(blogId1.ToString()), 0);

                //#get blog just inserted
                var blog1 = connection.Sql("select * from blog where id = @id")
                    .QueryOne<Blog>(new { id = blogId1 });
                Assert.AreEqual(blog1.Content, "随便发点什么");

                // use overloaded stored function that not content p_content parameter
                parameters = new Npgsql.NpgsqlParameter[]
                {
                    new NpgsqlParameter("p_title", "this is a pgslq blog"),
                    new NpgsqlParameter("p_content", "随便发点什么"),
                    new NpgsqlParameter("p_created_on", DateTime.Now),
                    new NpgsqlParameter("p_created_by", userId),
                    new NpgsqlParameter("p_updated_by", userId),
                    new NpgsqlParameter("p_blog_id", System.Data.DbType.Int32){ Direction = System.Data.ParameterDirection.Output }
                 };
                connection.Procedure("blog_ins")
                    //.WithParameters(parameters)
                    .Execute(parameters);
                var blogId2 = int.Parse(parameters[5].Value.ToString());
                Assert.Greater(blogId2, 0);

                //#get blog just inserted
                var blog2 = connection.Sql("select * from blog where id = @id")
                    .QueryOne<Blog>(new { id = blogId2 });
                Assert.AreEqual(blog2.UpdatedBy, userId);
            }
        }

        [Test]
        public async Task Insert_With_Inline_Sql_Auto_Generated_Id()
        {
            using (var connection = new NpgsqlConnection(connectionStr))
            {
                var tag = new Tag
                {
                    Name = "CSharp",
                    Description = "programe language i love"
                };
                var tagId1 = await connection.Sql("insert into tag(\"name\", description) values(@Name, @Description) returning id")
                    .ExecuteScalarAsync(tag);
                Assert.Greater(int.Parse(tagId1.ToString()), 0);
                tag = new Tag
                {
                    Name = "C",
                    Description = "morther langugae"
                };
                var tagId2 = await connection.Sql("insert into tag(\"name\", description) values(@Name, @Description) returning id")
                    .ExecuteScalarAsync(tag);
                Assert.Greater(int.Parse(tagId2.ToString()), 0);
                Assert.AreNotEqual(tagId1, tagId2);
            }
        }
    }
}
