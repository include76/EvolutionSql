using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Sql
{
    public static class CommandExtension
    {
        public static IEnumerable<T> Query<T>(this ICommand command, object parameters) where T: class, new()
        {
            var dbCommand = command.Build(parameters);
            using (var reader = dbCommand.ExecuteReader())
            {
                return reader.ToEntities<T>();
            }
        }

        public static int Execute(this ICommand command, object parameters)
        {
            var dbCommand = command.Build(parameters);
            return dbCommand.ExecuteNonQuery();
        }

        public static T ExecuteScalar<T>(this ICommand command, object parameters)
        {
            var dbCommand = command.Build(parameters);
            var reuslt = dbCommand.ExecuteScalar();
            return (T)reuslt;
        }
    }
}
