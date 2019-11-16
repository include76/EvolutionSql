using Evolution.Sql.SQLiteTest.Model;
using Evolution.Sql.SQLiteTest.TypeHandler;
using NUnit.Framework;
using System;
using System.Data.SQLite;

namespace Evolution.Sql.SQLiteTest
{
    public class Tests
    {
        static string connStr = "Data Source=blog.sqlite;Version=3;BinaryGUID=False";
        [SetUp]
        public void Setup()
        {
            InitDatabase.Init();
        }

        [Test]
        public void Insert_Test()
        {
            using (var conn = new SQLiteConnection(connStr))
            {
                var userId = Guid.NewGuid();
                var parameters = new
                {
                    user_id = userId,
                    first_name = "Tom",
                    last_name = "Bruce",
                    created_by = "system",
                    created_on = DateTime.Now,
                    updated_by = "system32",
                    updated_on = DateTime.Now
                };
                var result =  conn.Sql(@"INSERT INTO user(user_id
                        , first_name
                        , last_name
                        , created_by
                        , created_on
                        , updated_by
                        , updated_on) 
                        VALUES(
                         @user_id
                        , @first_name
                        , @last_name
                        , @created_by
                        , @created_on
                        , @updated_by
                        , @updated_on 
                        )").Execute(parameters);
                Assert.AreEqual(1, result);

                var user = conn.Sql("SELECT * FROM user WHERE user_id = @user_id")
                //var user = conn.Sql("SELECT * FROM user")
                    .WithTypeHandler<Guid, GuidHandler>()
                    .QueryOne<User>(new { user_id = userId.ToString() });

                Assert.IsNotNull(user);
                Assert.AreEqual(userId, user.UserId);
            }
        }
    }
}