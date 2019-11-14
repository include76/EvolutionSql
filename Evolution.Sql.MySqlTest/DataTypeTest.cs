using Evolution.Sql.MySqlTest.Model;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Evolution.Sql.MySqlTest
{
    public class DataTypeTest
    {
        private string connectionStr = @"server=localhost;user=root;database=blog;port=3306;password=root";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void DataType_Test()
        {
            using (var connection = new MySqlConnection(connectionStr))
            {
                var location = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);
                var directory = new FileInfo(location.AbsolutePath).Directory.FullName;
                var bytes = File.ReadAllBytes(Path.Combine(directory, "bridge.jpg"));

                var dataTypeModel = new DataTypeModel
                {
                    ColTinyInt = 127,
                    ColUTinyInt = 225,
                    ColSmallInt = (short)32767,
                    ColMediumInt = 8388607,
                    ColInt = 123456789,
                    ColBigInt = 9223372036854775807,

                    ColDecimal = 1.1234m,
                    ColNumeric = 100000.1234m,

                    ColFloat = 100000000.123456789f,
                    ColDouble = 100000000.123456789,
                    ColReal = 100000000.123456789,

                    ColBit = true,

                    ColDateTime = DateTime.Now,
                    ColDate = DateTime.Now,
                    //ColTime = DateTime.Now,
                    ColTime = new TimeSpan(23, 59, 59),
                    ColTimeStamp = DateTime.Now,// new DateTime(2019, 10, 27, 16, 1, 1),
                    ColYear = 2019,

                    //ColChar = new char[] { 't', 'h', 'i', 's', ' ', 'a', ' ', 'c', 'h', 'a', 'r' },
                    ColChar = "this is a char array",
                    ColVarchar = "this is a varchar string",
                    ColTinyText = "this is a tiny text",
                    ColText = "this is a text",
                    ColMediumText = "this is a medium text",
                    ColLongText = "this is a long text",

                    ColBinary = bytes.Take(255).ToArray(),
                    ColVarBinary = bytes.Take(8000).ToArray(),
                    ColTinyBlob = bytes.Take(255).ToArray(),
                    ColBlob = bytes.Take(8000).ToArray(),
                    ColMediumBlob = bytes,
                    ColLongBlob = bytes,
                    ColJson = "{\"employee\":{\"name\":\"sonoo\",\"salary\":56000,\"married\":true}}",
                    ColBool = true
                };
                var result = connection.Procedure("usp_data_type_ins").Execute(dataTypeModel);
                Assert.Greater(result, 0);
                var dataFromDb = connection.Sql("SELECT * FROM `data_type` ORDER BY ColDateTime").Query<DataTypeModel>(null);
                Assert.NotNull(dataFromDb);
                Assert.Greater(dataFromDb.Count(), 0);
                var newOne = dataFromDb.First();
                Assert.AreEqual(dataTypeModel.ColBigInt, newOne.ColBigInt);
                Assert.AreEqual(dataTypeModel.ColBinary.Take(1000), newOne.ColBinary);
                Assert.AreEqual(dataTypeModel.ColBit, newOne.ColBit);
                Assert.AreEqual(dataTypeModel.ColChar, newOne.ColChar);
                //Assert.AreEqual(dataTypeModel.ColDate, newOne.ColDate);
                //Assert.AreEqual(dataTypeModel.ColDatetime, newOne.ColDatetime);

                Assert.AreEqual(dataTypeModel.ColDecimal, newOne.ColDecimal);
                Assert.AreEqual(dataTypeModel.ColFloat, newOne.ColFloat);

                Assert.AreEqual(dataTypeModel.ColInt, newOne.ColInt);


                Assert.AreEqual(dataTypeModel.ColNumeric, newOne.ColNumeric);

                Assert.AreEqual(dataTypeModel.ColReal, newOne.ColReal);
                //Assert.AreEqual(dataTypeModel.ColSmallDatetime, newOne.ColSmallDatetime);
                Assert.AreEqual(dataTypeModel.ColSmallInt, newOne.ColSmallInt);

                Assert.AreEqual(dataTypeModel.ColText, newOne.ColText);
                //Assert.AreEqual(dataTypeModel.ColTime, newOne.ColTime);
                //Assert.AreEqual(dataTypeModel.ColTimestamp, newOne.ColTimestamp);
                Assert.AreEqual(dataTypeModel.ColTinyInt, newOne.ColTinyInt);
                Assert.AreEqual(dataTypeModel.ColVarBinary, newOne.ColVarBinary);
                Assert.AreEqual(dataTypeModel.ColVarchar, newOne.ColVarchar);
                Assert.AreEqual(dataTypeModel.ColBool, newOne.ColBool);
            }
        }
    }
}
