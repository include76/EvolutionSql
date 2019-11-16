using System;
using System.Text;
using System.Data.Common;

namespace Evolution.Sql
{
    public static class EvolutionSql
    {
        public static ICommand Procedure(this DbConnection connection, string procedureName)
        {
            //var factory = DbProviderFactories.GetFactory(connection);
            //var command = CommandFactory.Instance(factory.GetType().Name);
            var command = CommandFactory.Instance(connection.GetType().Name);
            command.Connection = connection;
            command.CommandText = procedureName;
            command.CommandType = System.Data.CommandType.StoredProcedure;
            return command;
        }

        public static ICommand Sql(this DbConnection connection, string sql)
        {
            //var factory = DbProviderFactories.GetFactory(connection);
            //var command = CommandFactory.Instance(factory.GetType().Name);
            var command = CommandFactory.Instance(connection.GetType().Name);
            command.Connection = connection;
            command.CommandText = sql;
            command.CommandType = System.Data.CommandType.Text;
            return command;
        }
    }
}
