using Evolution.Sql.SqlServerTest.Model;
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
            using (var connection = new SqlConnection(connectionStr))
            {
                DataTable myDataTable = new DataTable("MyDataType");
                myDataTable.Columns.Add("Id", typeof(Guid));
                myDataTable.Columns.Add("Name", typeof(string));

                myDataTable.Rows.Add(Guid.NewGuid(), "XYZ");
                myDataTable.Rows.Add(Guid.NewGuid(), "ABC");

                var users = connection.Procedure("uspWithTableParameter").Query<User>(new { myData = myDataTable });

                Assert.NotNull(users);
                Assert.AreEqual("XYZ", users.First().FirstName);
            }
        }

        [Test]
        public void StoredProcedure_Parameter_Direction_Test()
        {
            using (var connection = new SqlConnection(connectionStr))
            {
                var outPuts = new Dictionary<string, dynamic>();
                var result = connection.Procedure("uspParamDirection").Execute(new { pIn = 2, pInOut = 5 }, outPuts); 
            }
        }

        [Test]
        public void DataType_Test()
        {
            using (var connection = new SqlConnection(connectionStr))
            {
                var location = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);
                var directory = new FileInfo(location.AbsolutePath).Directory.FullName;
                var bytes = File.ReadAllBytes(Path.Combine(directory, "bridge.jpg"));

                var dataTypeModel = new DataTypeModel
                {
                    ColBigInt = 9223372036854775807,
                    ColBit = true,
                    ColDecimal = 100000000.1234m,
                    ColInt = 123456789,
                    ColMoney = 100000000.1234m,//money only has 4 numbers at right
                    ColNumeric = 100000000.1234m,
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
                    //ColTime = new TimeSpan(23, 59, 59),
                    ColTime = DateTime.Now,
                    ColChar = new char[] { 't', 'h', 'i', 's', ' ', 'a', ' ', 'c', 'h', 'a', 'r' },
                    ColText = "this is a text",
                    ColVarchar = new char[] { 't', 'h', 'i', 's', ' ', 'a', ' ', 'v', 'a', 'r', ' ', 'c', 'h', 'a', 'r' },
                    ColNChar = new char[] { 'a', 'b', 'c' },
                    ColNText = "这是一段文字",
                    ColNVarchar = new char[] { '这', '也', '是', '一', '段', '文', '字' },
                    ColBinary = bytes,
                    ColImage = bytes,
                    ColVarBinary = bytes,
                    ColXml = @"<note><to>Tove</to><from>Jani</from><heading>Reminder</heading><body>Don't forget me this weekend!</body></note>"
                };
                var result = connection.Procedure("uspDataTypeIns").Execute(dataTypeModel);
                Assert.Greater(result, 0);
                var dataFromDb = connection.Sql("SELECT * FROM [DataTypeTable] ORDER BY ColDateTime DESC").Query<DataTypeModel>(null);
                Assert.NotNull(dataFromDb);
                Assert.Greater(dataFromDb.Count(), 0);
                var newOne = dataFromDb.First();
                Assert.AreEqual(dataTypeModel.ColBigInt, newOne.ColBigInt);
                Assert.AreEqual(dataTypeModel.ColBinary.Take(1000), newOne.ColBinary);
                Assert.AreEqual(dataTypeModel.ColBit, newOne.ColBit);
                Assert.AreEqual(dataTypeModel.ColChar, newOne.ColChar.Take(11));
                //Assert.AreEqual(dataTypeModel.ColDate, newOne.ColDate);
                //Assert.AreEqual(dataTypeModel.ColDatetime, newOne.ColDatetime);
                Assert.AreEqual(dataTypeModel.ColDatetime2, newOne.ColDatetime2);
                Assert.AreEqual(dataTypeModel.ColDatetimeOffset, newOne.ColDatetimeOffset);
                Assert.AreEqual(dataTypeModel.ColDecimal, newOne.ColDecimal);
                Assert.AreEqual(dataTypeModel.ColFloat, newOne.ColFloat);
                Assert.AreEqual(dataTypeModel.ColImage, newOne.ColImage);
                Assert.AreEqual(dataTypeModel.ColInt, newOne.ColInt);
                Assert.AreEqual(dataTypeModel.ColMoney, newOne.ColMoney);
                Assert.AreEqual(dataTypeModel.ColNChar, newOne.ColNChar.Take(3));
                Assert.AreEqual(dataTypeModel.ColNText, newOne.ColNText);
                Assert.AreEqual(dataTypeModel.ColNumeric, newOne.ColNumeric);
                Assert.AreEqual(dataTypeModel.ColNVarchar, newOne.ColNVarchar);
                Assert.AreEqual(dataTypeModel.ColReal, newOne.ColReal);
                //Assert.AreEqual(dataTypeModel.ColSmallDatetime, newOne.ColSmallDatetime);
                Assert.AreEqual(dataTypeModel.ColSmallInt, newOne.ColSmallInt);
                Assert.AreEqual(dataTypeModel.ColSmallMoney, newOne.ColSmallMoney);
                Assert.AreEqual(dataTypeModel.ColText, newOne.ColText);
                //Assert.AreEqual(dataTypeModel.ColTime, newOne.ColTime);
                //Assert.AreEqual(dataTypeModel.ColTimestamp, newOne.ColTimestamp);
                Assert.AreEqual(dataTypeModel.ColTinyInt, newOne.ColTinyInt);
                Assert.AreEqual(dataTypeModel.ColVarBinary, newOne.ColVarBinary);
                Assert.AreEqual(dataTypeModel.ColVarchar, newOne.ColVarchar);
                Assert.AreEqual(dataTypeModel.ColXml, newOne.ColXml);
            }
        }
    }
}
