using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Sql
{
    internal sealed class CommandFactory
    {
        internal static AbstractCommand Instance(string dbFactoryName)
        {
            switch (dbFactoryName)
            {
                case "SqlClientFactory":
                    return new SqlServerCommand();
                case "MySqlClientFactory":
                    return new MySqlCommand();
                default:
                    throw new Exception($"{dbFactoryName} is not supported.");
            }
        }
    }
}
