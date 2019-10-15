using Evolution.Sql.TestCommon;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Evolution.Sql.SqlServerTest
{
    [TestFixture]
    public class DataTypeTest
    {
        private string connectionStr = @"Data Source =.\sqlexpress; Initial Catalog = Blog; Integrated Security = True";
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TestDataTableParameter()
        {
            var connection = new SqlConnection(connectionStr);
            using (var sqlSession = new SqlSession(connection))
            {
                DataTable myDataTable = new DataTable("MyDataType");
                myDataTable.Columns.Add("Id", typeof(Guid));
                myDataTable.Columns.Add("Name", typeof(string));

                myDataTable.Rows.Add(Guid.NewGuid(), "XYZ");
                myDataTable.Rows.Add(Guid.NewGuid(), "ABC");

                var users = sqlSession.Query<User>("testTableParameter", new { myData = myDataTable });

                Assert.NotNull(users);
                Assert.AreEqual("XYZ", users.First().FirstName);
            }
        }
    }
}
