using Evolution.Sql.SqlServerTest.Modal;
using Evolution.Sql.TestCommon;
using Evolution.Sql.TestCommon.Interface;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution.Sql.SqlServerTest
{
    [TestFixture]
    public class QueryTest : IQueryTest
    {
        private string connectionStr = @"Data Source =.\sqlexpress; Initial Catalog = Blog; Integrated Security = True";
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task QueryOne_With_Inline_Sql()
        {
            using (var connection = new SqlConnection(connectionStr))
            {
                var userId = Guid.NewGuid();
                var user = new User
                {
                    UserId = userId,
                    FirstName = "Bruce",
                    LastName = "Lee",
                    CreatedOn = DateTime.Now
                };
                var result = connection.Sql(@"insert into [user](UserId, FirstName, LastName, CreatedOn) 
                                                values(@UserId, @FirstName, @LastName, @CreatedOn)")
                    .Execute(user);
                Assert.Greater(result, 0);

                var userFromDb = connection.Sql("select * from [user] where userid = @UserId")
                    .Query<User>(new { UserId = userId })?.FirstOrDefault();
                Assert.IsNotNull(userFromDb);
                Assert.AreEqual(userId, userFromDb.UserId);

                userFromDb = null;
                userFromDb = (await connection.Sql("select * from [user] where userid = @UserId")
                    .QueryAsync<User>(new { UserId = userId }))?.FirstOrDefault();
                Assert.IsNotNull(userFromDb);
                Assert.AreEqual(userId, userFromDb.UserId);
            }
        }

        [Test]
        public void QueryOne_With_StoredProcedure()
        {
            using (var connection = new SqlConnection(connectionStr))
            {
                var userId = Guid.NewGuid();
                var user = new User
                {
                    UserId = userId,
                    FirstName = "Bruce",
                    LastName = "Lee"
                };
                var result = connection.Sql(@"insert into [user](UserId, FirstName, LastName) 
                                                values(@UserId, @FirstName, @LastName)")
                    .Execute(user);
                Assert.Greater(result, 0);

                var outPuts = new Dictionary<string, dynamic>();
                var userFromDb = connection.Procedure("uspUserGet")
                    .Query<User>(new { UserId = userId, totalCount = 0 }, outPuts).FirstOrDefault();
                Assert.IsNotNull(userFromDb);
                Assert.AreEqual(userId, userFromDb.UserId);
                Assert.True(outPuts.ContainsKey("totalCount"));
                Assert.Greater(outPuts["totalCount"], 0);
            }
        }

        [Test]
        public void Query_With_Inline_Sql()
        {
            //throw new NotImplementedException();
        }

        [Test]
        public void Query_With_StoredProcedure()
        {
            //throw new NotImplementedException();
        }

        [Test]
        public void Get_Null_Value_From_DB_Property_Should_Set_Default_Value()
        {
            using (var connection = new SqlConnection(connectionStr))
            {
                var userId = Guid.NewGuid();
                var user = new User
                {
                    UserId = userId,
                    FirstName = "Bruce",
                    LastName = "Lee"
                };
                var result = connection.Sql(@"insert into [user](UserId, FirstName, LastName) 
                                                values(@UserId, @FirstName, @LastName)")
                    .Execute(user);
                Assert.Greater(result, 0);

                var userFromDb = connection.Sql("select UserId, FirstName, CreatedOn from [user] where userid = @userId")
                    .Query<User>(new { UserId = userId })?.FirstOrDefault(); 
                Assert.IsNotNull(userFromDb);
                Assert.AreEqual(userId, userFromDb.UserId);
                Assert.AreEqual(default(DateTime), userFromDb.CreatedOn);
            }
        }

        public Task Query_Column_Name_Contain_UnderScore_Can_Map_To_Property()
        {
            throw new NotImplementedException();
        }
    }
}
