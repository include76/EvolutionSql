using System;
using System.Text;
using System.Data.Common;

namespace Evolution.Sql
{
    public static class EvolutionSql
    {
        public static ICommand Procedure(this DbConnection connection,
            string procedure,
            int commandTimeout = 30,
            DbTransaction transaction = null)
        {
            //var factory = DbProviderFactories.GetFactory(connection);
            //var command = CommandFactory.Instance(factory.GetType().Name);
            var command = CommandFactory.Instance(connection.GetType().Name);
            command.Connection = connection;
            command.CommandText = procedure;
            command.CommandTimeout = commandTimeout;
            command.Transaction = transaction;
            command.CommandType = System.Data.CommandType.StoredProcedure;
            return command;
        }

        public static ICommand Sql(this DbConnection connection,
            string sql,
            int commandTimeout = 30,
            DbTransaction transaction = null)
        {
            //var factory = DbProviderFactories.GetFactory(connection);
            //var command = CommandFactory.Instance(factory.GetType().Name);
            var command = CommandFactory.Instance(connection.GetType().Name);
            command.Connection = connection;
            command.CommandText = sql;
            command.CommandTimeout = commandTimeout;
            command.Transaction = transaction;
            command.CommandType = System.Data.CommandType.Text;
            return command;
        }
    }
}
