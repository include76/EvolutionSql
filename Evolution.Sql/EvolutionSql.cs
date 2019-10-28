using Evolution.Sql.Command;
using Evolution.Sql.CommandAdapter;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Evolution.Sql
{
    public static class EvolutionSql
    {
        public static ICommand Procedure(this DbConnection connection, string procedureName)
        {
            var factory = DbProviderFactories.GetFactory(connection);
            var command = CommandFactory.Instance(factory.GetType().Name);
            command.Connection = connection;
            command.CommandText = procedureName;
            command.CommandType = System.Data.CommandType.StoredProcedure;
            //command.Parameters = parameters;
            return command;
        }

        public static ICommand Sql(this DbConnection connection, string sql)
        {
            var factory = DbProviderFactories.GetFactory(connection);
            var command = CommandFactory.Instance(factory.GetType().Name);
            command.Connection = connection;
            command.CommandText = sql;
            command.CommandType = System.Data.CommandType.Text;
            //command.Parameters = parameters;
            return command;
        }
    }
}
