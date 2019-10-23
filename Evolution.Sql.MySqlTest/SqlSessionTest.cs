using NUnit.Framework;
using MySql.Data;
using System;
using System.Data.Common;

namespace Evolution.Sql.MySqlTest
{
    public class SqlSessionTest
    {
        private string connectionStr = @"server=localhost;user=root;database=blog;port=3306;password=root";
        [SetUp]
        public void Setup()
        {
            DbProviderFactories.UnregisterFactory("MySql.Data.MySqlClient");
            //DbProviderFactories.RegisterFactory("MySql.Data.MySqlClient", MySqlClientFactory.Instance);
        }

        [Test]
        public void If_DbProvider_Not_Registered_Should_Throw_Exception()
        {
            Assert.Throws<Exception>(()=>new SqlSession("MySql.Data.MySqlClient", connectionStr));
        }
    }
}