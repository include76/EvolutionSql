using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Sql
{
    internal sealed class CommandFactory
    {
        /*internal static AbstractCommand Instance(string dbFactoryName)
        {
            switch (dbFactoryName)
            {
                case "SqlClientFactory":
                    return new SqlServerCommand();
                case "MySqlClientFactory":
                    return new MySqlCommand();
                case "NpgsqlFactory":
                    return new PgSqlCommand();
                case "SQLiteFactory":
                    return new SQLiteCommand();
                default:
                    throw new Exception($"{dbFactoryName} is not supported.");
            }
            
        }*/
        internal static AbstractCommand Instance(string connectionTypeName)
        {
            switch (connectionTypeName)
            {
                case "SqlConnection":
                    return new SqlServerCommand();
                case "MySqlConnection":
                    return new MySqlCommand();
                case "NpgsqlConnection":
                    return new PgSqlCommand();
                case "SQLiteConnection":
                    return new SQLiteCommand();
                default:
                    throw new Exception($"{connectionTypeName} is not supported.");
            }
        }
    }
}
