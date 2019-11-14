using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution.Sql
{
    public static class CommandExtension
    {
        #region QueryOne
        public static T QueryOne<T>(this ICommand iCommand) where T : class, new()
        {
            return iCommand.QueryOne<T>(null);
        }

        public static T QueryOne<T>(this ICommand iCommand, params DbParameter[] parameters) where T : class, new()
        {
            using (var dbCommand = iCommand.Build(parameters))
            {
                using (var reader = dbCommand.ExecuteReader())
                {
                    return reader.ToEntities<T>(iCommand)?.FirstOrDefault();
                }
            }
        }

        public static T QueryOne<T>(this ICommand iCommand, object parameters) where T : class, new()
        {
            using (var dbCommand = iCommand.Build(parameters))
            {
                using (var reader = dbCommand.ExecuteReader())
                {
                    return reader.ToEntities<T>(iCommand)?.FirstOrDefault();
                }
            }
        }

        //public static T QueryOne<T>(this ICommand iCommand, object parameters, Dictionary<string, dynamic> outputs) where T : class, new()
        //{
        //    using (var dbCommand = iCommand.Build(parameters))
        //    {
        //        T result;
        //        using (var reader = dbCommand.ExecuteReader())
        //        {
        //            result = reader.ToEntities<T>(iCommand)?.FirstOrDefault();
        //        }
        //        SetOutputParameters(dbCommand, outputs);
        //        return result;
        //    }
        //}

        public static async Task<T> QueryOneAsync<T>(this ICommand iCommand) where T : class, new()
        {
            return await iCommand.QueryOneAsync<T>(null);
        }

        public static async Task<T> QueryOneAsync<T>(this ICommand iCommand, params DbParameter[] parameters) where T : class, new()
        {
            using (var dbCommand = iCommand.Build(parameters))
            {
                using (var reader = await dbCommand.ExecuteReaderAsync())
                {
                    return reader.ToEntities<T>(iCommand)?.FirstOrDefault();
                }
            }
        }

        public static async Task<T> QueryOneAsync<T>(this ICommand iCommand, object parameters) where T : class, new()
        {
            using (var dbCommand = iCommand.Build(parameters))
            {
                using (var reader = await dbCommand.ExecuteReaderAsync())
                {
                    return reader.ToEntities<T>(iCommand)?.FirstOrDefault();
                }
            }
        }

        //public static async Task<T> QueryOneAsync<T>(this ICommand iCommand, object parameters, Dictionary<string, dynamic> outputs) where T : class, new()
        //{
        //    using (var dbCommand = iCommand.Build(parameters))
        //    {
        //        T result;
        //        using (var reader = await dbCommand.ExecuteReaderAsync())
        //        {
        //            result = reader.ToEntities<T>(iCommand)?.FirstOrDefault();
        //        }
        //        SetOutputParameters(dbCommand, outputs);
        //        return result;
        //    }
        //}
        #endregion

        #region Query
        public static IEnumerable<T> Query<T>(this ICommand iCommand) where T : class, new()
        {
            return iCommand.Query<T>(null);
        }

        public static IEnumerable<T> Query<T>(this ICommand iCommand, params DbParameter[] parameters) where T : class, new()
        {
            using (var dbCommand = iCommand.Build(parameters))
            {
                using (var reader = dbCommand.ExecuteReader())
                {
                    return reader.ToEntities<T>(iCommand);
                }
            }
        }

        public static IEnumerable<T> Query<T>(this ICommand iCommand, object parameters) where T : class, new()
        {
            using (var dbCommand = iCommand.Build(parameters))
            {
                using (var reader = dbCommand.ExecuteReader())
                {
                    return reader.ToEntities<T>(iCommand);
                }
            }
        }

        //public static IEnumerable<T> Query<T>(this ICommand iCommand, object parameters, Dictionary<string, dynamic> outputs) where T : class, new()
        //{
        //    using (var dbCommand = iCommand.Build(parameters))
        //    {
        //        IEnumerable<T> result;
        //        using (var reader = dbCommand.ExecuteReader())
        //        {
        //            result = reader.ToEntities<T>(iCommand);
        //        }
        //        SetOutputParameters(dbCommand, outputs);
        //        return result;
        //    }
        //}

        public static async Task<IEnumerable<T>> QueryAsync<T>(this ICommand iCommand) where T : class, new()
        {
            return await iCommand.QueryAsync<T>(null);
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(this ICommand iCommand, params DbParameter[] parameters) where T : class, new()
        {
            using (var dbCommand = iCommand.Build(parameters))
            {
                using (var reader = await dbCommand.ExecuteReaderAsync())
                {
                    return reader.ToEntities<T>(iCommand);
                }
            }
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(this ICommand iCommand, object parameters) where T : class, new()
        {
            using (var dbCommand = iCommand.Build(parameters))
            {
                using (var reader = await dbCommand.ExecuteReaderAsync())
                {
                    return reader.ToEntities<T>(iCommand);
                }
            }
        }

        //public static async Task<IEnumerable<T>> QueryAsync<T>(this ICommand iCommand, object parameters, Dictionary<string, dynamic> outputs) where T : class, new()
        //{
        //    using (var dbCommand = iCommand.Build(parameters))
        //    {
        //        IEnumerable<T> result;
        //        using (var reader = await dbCommand.ExecuteReaderAsync())
        //        {
        //            result = reader.ToEntities<T>(iCommand);
        //        }
        //        SetOutputParameters(dbCommand, outputs);
        //        return result;
        //    }
        //}
        #endregion

        #region Execute
        public static int Execute(this ICommand iCommand)
        {
            return iCommand.Execute(null);
        }

        public static int Execute(this ICommand iCommand, params DbParameter[] parameters)
        {
            using (var dbCommand = iCommand.Build(parameters))
            {
                return dbCommand.ExecuteNonQuery();
            }
        }

        public static int Execute(this ICommand iCommand, object parameters)
        {
            using (var dbCommand = iCommand.Build(parameters))
            {
                return dbCommand.ExecuteNonQuery();
            }
        }

        //public static int Execute(this ICommand iCommand, object parameters, Dictionary<string, dynamic> outputs)
        //{
        //    using (var dbCommand = iCommand.Build(parameters))
        //    {
        //        var result = dbCommand.ExecuteNonQuery();
        //        SetOutputParameters(dbCommand, outputs);
        //        return result;
        //    }
        //}

        public static async Task<int> ExecuteAsync(this ICommand iCommand)
        {
            return await iCommand.ExecuteAsync(null);
        }
        public static async Task<int> ExecuteAsync(this ICommand iCommand, params DbParameter[] parameters)
        {
            using (var dbCommand = iCommand.Build(parameters))
            {
                return await dbCommand.ExecuteNonQueryAsync();
            }
        }

        public static async Task<int> ExecuteAsync(this ICommand iCommand, object parameters)
        {
            using (var dbCommand = iCommand.Build(parameters))
            {
                return await dbCommand.ExecuteNonQueryAsync();
            }
        }

        //public static async Task<int> ExecuteAsync(this ICommand iCommand, object parameters, Dictionary<string, dynamic> outputs)
        //{
        //    using (var dbCommand = iCommand.Build(parameters))
        //    {
        //        var result = await dbCommand.ExecuteNonQueryAsync();
        //        SetOutputParameters(dbCommand, outputs);
        //        return result;
        //    }
        //}
        #endregion

        #region ExecuteScalar
        public static object ExecuteScalar(this ICommand iCommand)
        {
            return iCommand.ExecuteScalar(null);
        }

        public static object ExecuteScalar(this ICommand iCommand, params DbParameter[] parameters)
        {
            using (var dbCommand = iCommand.Build(parameters))
            {
                var result = dbCommand.ExecuteScalar();
                return result;
            }
        }

        public static object ExecuteScalar(this ICommand iCommand, object parameters)
        {
            using (var dbCommand = iCommand.Build(parameters))
            {
                var result = dbCommand.ExecuteScalar();
                return result;
            }
        }

        //public static object ExecuteScalar(this ICommand iCommand, object parameters, Dictionary<string, dynamic> outputs)
        //{
        //    using (var dbCommand = iCommand.Build(parameters))
        //    {
        //        var result = dbCommand.ExecuteScalar();
        //        SetOutputParameters(dbCommand, outputs);
        //        return result;
        //    }
        //}

        public static async Task<object> ExecuteScalarAsync(this ICommand iCommand)
        {
            return await ExecuteScalarAsync(null);
        }

        public static async Task<object> ExecuteScalarAsync(this ICommand iCommand, params DbParameter[] parameters)
        {
            using (var dbCommand = iCommand.Build(parameters))
            {
                var result = await dbCommand.ExecuteScalarAsync();
                return result;
            }
        }

        public static async Task<object> ExecuteScalarAsync(this ICommand iCommand, object parameters)
        {
            using (var dbCommand = iCommand.Build(parameters))
            {
                var result = await dbCommand.ExecuteScalarAsync();
                return result;
            }
        }
        //public static async Task<object> ExecuteScalarAsync(this ICommand iCommand, object parameters, Dictionary<string, dynamic> outputs)
        //{
        //    using (var dbCommand = iCommand.Build(parameters))
        //    {
        //        var result = await dbCommand.ExecuteScalarAsync();
        //        SetOutputParameters(dbCommand, outputs);
        //        return result;
        //    }
        //}
        #endregion
        /// <summary>
        /// Indicate store procedure parameters have prefix
        /// </summary>
        /// <param name="iCommand"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        //public static ICommand WithParameterPrefix(this ICommand iCommand, string prefix)
        //{
        //    iCommand.ParameterPrefix = prefix;
        //    return iCommand;
        //}

        /// <summary>
        /// Set parameters explicitly
        /// Parameters of Query, QueryOne, Execute, ExecuteScalar will be Ignored
        /// ParameterPrefix will be Ignored
        /// </summary>
        /// <param name="iCommand"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        //public static ICommand WithParameters(this ICommand iCommand, params DbParameter[] parameters)
        //{
        //    iCommand.ExplicitParameters = parameters;
        //    // if explicit parameters set, disable ParameterPrefix
        //    iCommand.ParameterPrefix = string.Empty;
        //    return iCommand;
        //}

        /// <summary>
        /// Map a provider specific type
        /// </summary>
        /// <param name="iCommand"></param>
        /// <returns></returns>
        public static ICommand WithTypeHandler<TType, THandler>(this ICommand iCommand) where THandler : ITypeHandler
        {
            iCommand.TypeHandlers.Add(typeof(TType), Activator.CreateInstance<THandler>());
            return iCommand;
        }

        /// <summary>
        /// set command timeout in seconds, default 30
        /// </summary>
        /// <param name="iCommand"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static ICommand SetTimeOut(this ICommand iCommand, int timeout)
        {
            iCommand.CommandTimeout = timeout;
            return iCommand;
        }

        /// <summary>
        /// set command transaction
        /// </summary>
        /// <param name="iCommand"></param>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        public static ICommand SetTransaction(this ICommand iCommand, DbTransaction dbTransaction)
        {
            iCommand.Transaction = dbTransaction;
            return iCommand;
        }

        #region private methods
        private static void SetOutputParameters(DbCommand dbCommand, Dictionary<string, dynamic> outPuts)
        {
            if (dbCommand.Parameters == null || dbCommand.Parameters.Count <= 0)
            {
                return;
            }
            foreach (DbParameter parameter in dbCommand.Parameters)
            {
                if (parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Output)
                {
                    outPuts.Add(parameter.ParameterName, parameter.Value);
                }
            }
        }
        #endregion
    }
}
