using Evolution.Sql.SqlServerTest.Modal;
using Evolution.Sql.TestCommon;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
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
        public void DataTableParameter_Test()
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

        [Test]
        public void StoredProcedure_Parameter_Direction_Test()
        {
            using (var sqlSession = new SqlSession(new SqlConnection(connectionStr)))
            {
                var outPuts = new Dictionary<string, dynamic>();
                var result = sqlSession.Execute<DummyModel>("uspParamDirection", new { pIn = 2, pInOut = 5 }, outPuts);
            }
        }

        [Test]
        public void DataType_Test()
        {
            using (var sqlSession = new SqlSession(new SqlConnection(connectionStr)))
            {
                var location = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);
                var directory =  new FileInfo(location.AbsolutePath).Directory.FullName;
                var bytes = File.ReadAllBytes(Path.Combine(directory, "bridge.jpg"));

                var dataTypeModel = new DataTypeModal
                {
                    ColBigInt = 9223372036854775807,
                    ColBit = true,
                    ColDecimal = 100000000.123456789m,
                    ColInt = 123456789,
                    ColMoney = 100000000.123456789m,
                    ColNumeric = 100000000.123456789m,
                    ColSmallInt = (short)32767,
                    ColSmallMoney = 1.1m,
                    ColTinyInt = 225,
                    ColFloat = 100000000.123456789,
                    ColReal = 100000000.123456789f,
                    ColDate = DateTime.Now,
                    ColDatetime = DateTime.Now,
                    ColDatetime2 = DateTime.Now,
                    ColDatetimeOffset = DateTimeOffset.Now,
                    ColSmallDatetime = DateTime.Now,
                    ColTime = DateTime.Now,
                    ColChar = new char[] { 'a', 'b', 'c' },
                    ColText = "abcdefg",
                    ColVarchar = new char[] { 'a', 'b', 'c' },
                    ColNChar = new char[] { 'a', 'b', 'c' },
                    ColNText = "abcdefg",
                    ColNVarchar = "abcdefg",
                    ColBinary = bytes,
                    ColImage = bytes,
                    ColVarBinary = bytes,
                    ColXml = @"<note>
                          <to>Tove</to>
                          <from>Jani</from>
                          <heading>Reminder</heading>
                          <body>Don't forget me this weekend!</body>
                        </note>"
                };
                var result = sqlSession.Execute<DataTypeModal>("insert", dataTypeModel);
            }
        }
    }
}
