using Evolution.Sql.PgSqlTest.Model;
using Evolution.Sql.TestCommon.Interface;
using Npgsql;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution.Sql.PgSqlTest
{
    public class QueryTest
    {
        private string connectionStr = @"Server=127.0.0.1;Port=5432;Database=blog;User Id=postgres;Password=";

        private string userInsSql = "insert into \"user\"(user_id, first_name, last_name, created_by, created_on)" +
            "                        values(@UserId, @FirstName, @LastName, @CreatedBy, @CreatedOn)";
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task QueryOne_With_Inline_Sql()
        {
            using (var connection = new NpgsqlConnection(connectionStr))
            {
                var userId1 = Guid.NewGuid();
                var user = new
                {
                    UserId = userId1,
                    FirstName = "Bruce",
                    LastName = "Lee",
                    CreatedBy = "Locke",
                    CreatedOn = DateTime.Now
                };

                var result = connection.Sql(userInsSql)
                   .Execute(user);
                Assert.Greater(result, 0);

                var bruce = connection.Sql("select * from \"user\" where user_id = @UserId")
                    .Query<User>(new { UserId = userId1 })?.FirstOrDefault();
                Assert.IsNotNull(bruce);
                Assert.AreEqual(userId1, bruce.UserId);

                var userId2 = Guid.NewGuid();
                var parameters = new
                {
                    UserId = userId2,
                    FirstName = "Tom",
                    LastName = "Ren",
                    CreatedBy = "Locke",
                    CreatedOn = DateTime.Now
                };

                result = await connection.Sql("insert into \"user\"(user_id, first_name, last_name) values(@UserId, @FirstName, @LastName);")
                    .ExecuteAsync(parameters);
                Assert.Greater(result, 0);

                var tom = await connection.Sql("select * from \"user\" where user_id = @UserId").QueryOneAsync<User>(new { UserId = userId2 });
                Assert.IsNotNull(tom);
                Assert.AreEqual(userId2, tom.UserId);

                Assert.AreNotEqual(bruce.FirstName, tom.FirstName);
            }
        }

        /// <summary>
        /// pgsql use function to return recordset/value
        /// </summary>
        [Test]
        public void QueryOne_With_Function_RefCursor()
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
                    CreatedOn = DateTime.UtcNow
                };
                var result = connection.Sql(userInsSql).Execute(user);
                Assert.AreEqual(result, 1);
                var trans = connection.BeginTransaction();
                var cursor = connection.Procedure("user_get_with_refcursor")
                    .ExecuteScalar(new { p_user_id = userId });

                var userFromDb = connection.Sql($"fetch all in \"{cursor.ToString()}\"")
                    .QueryOne<User>();

                trans.Commit();
                Assert.IsNotNull(userFromDb);
                Assert.AreEqual(userId, userFromDb.UserId);
            }
        }

        [Test]
        public void QueryOne_With_Function_Return_Table()
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
                    CreatedOn = DateTime.UtcNow
                };
                var result = connection.Sql(userInsSql).Execute(user);
                Assert.AreEqual(result, 1);

                var userFromDb = connection.Procedure("user_get_with_table")
                    .QueryOne<User>(new { p_user_id = userId });
                //var userFromDb = connection.Sql("select * from user_get_with_table(@p_user_id)")
                //    .QueryOne<User>(new { p_user_id = userId, p_total_count = 0 });

                Assert.IsNotNull(userFromDb);
                Assert.AreEqual(userId, userFromDb.UserId);
            }
        }

        [Test]
        public void QueryOne_With_Function_Return_Next()
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
                    CreatedOn = DateTime.UtcNow
                };
                var result = connection.Sql(userInsSql).Execute(user);
                Assert.AreEqual(result, 1);

                var userFromDb = connection.Procedure("user_get_with_table_loop_next")
                    .QueryOne<User>(new { p_user_id = userId });
                //var userFromDb = connection.Sql("select * from user_get_with_table_loop_next(@p_user_id)")
                //    .QueryOne<User>(new { p_user_id = userId, p_total_count = 0 });

                Assert.IsNotNull(userFromDb);
                Assert.AreEqual(userId, userFromDb.UserId);
            }
        }

        [Test]
        public void Query_With_Inline_Sql()
        {
            using (var connection = new NpgsqlConnection(connectionStr))
            {
                var userId = Guid.NewGuid();
                var user = new
                {
                    UserId = userId,
                    FirstName = "Bruce",
                    LastName = "Lee",
                    CreatedBy = "system",
                    CreatedOn = DateTime.Now
                };
                var result = connection.Sql(userInsSql)
                   .Execute(user);
                Assert.Greater(result, 0);

                var blog = new
                {
                    p_title = "this is a test post title",
                    p_content = "this is a test post content",
                    p_created_by = userId,
                    p_created_on = DateTime.Now
                };

                var postId = connection.Procedure("blog_ins")
                    .ExecuteScalar(blog);
                Assert.NotNull(postId);
                Assert.Greater(int.Parse(postId.ToString()), 0);
                postId = connection.Procedure("blog_ins")
                    .ExecuteScalar(blog);
                Assert.NotNull(postId);
                Assert.Greater(int.Parse(postId.ToString()), 0);

                // query
                var blogs = connection.Sql("select * from blog")
                    .Query<Blog>();
                Assert.NotNull(blogs);
                Assert.Greater(blogs.ToList().Count, 1);
            }
        }

        [Test]
        public void Query_With_StoredProcedure()
        {
            //throw new NotImplementedException();
        }

        [Test]
        public void Get_Null_Value_From_DB_Property_Should_Set_Default_Value()
        {
            /*
            using (var connection = new NpgsqlConnection(connectionStr))
            {
                var userId = Guid.NewGuid();
                var user = new User
                {
                    UserId = userId,
                    FirstName = "Bruce",
                    LastName = "Lee"
                };
                var result = sqlSession.Execute<User>("insert", user);
                Assert.Greater(result, 0);

                var userFromDb = sqlSession.QueryOne<User>("getPartialCol", new { UserId = userId });
                Assert.IsNotNull(userFromDb);
                Assert.AreEqual(userId, userFromDb.UserId);
                Assert.AreEqual(default(DateTime), userFromDb.CreatedOn);
            }*/
        }

        public Task Query_Column_Name_Contain_UnderScore_Can_Map_To_Property()
        {
            throw new NotImplementedException();
        }
    }

    public class RefCursorModel
    {
        public string RefCursor { get; set; }
    }
}
