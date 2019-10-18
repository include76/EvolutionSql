using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Sql.CommandAdapter
{
    internal class CommandAdapterFactory
    {
        internal static AbstractCommandAdapter Instance(string dbFactoryName)
        {
            switch (dbFactoryName)
            {
                case "SqlClientFactory":
                    return new CommandAdapterSqlServer();
                case "MySqlClientFactory":
                    return new CommandAdapterMySql();
                default:
                    throw new Exception($"{dbFactoryName} is not supported.");
            }
        }
    }
}
