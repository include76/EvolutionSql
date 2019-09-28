using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Evolution.Sql
{
    public interface ISqlSession: IDisposable
    {
        IDbConnection Connection { get; set; }

        IEnumerable<TEntity> Query<TEntity>(Dictionary<string, dynamic> parameters) where TEntity : class, new();
        IEnumerable<TEntity> Query<TEntity>(string queryName, Dictionary<string, dynamic> parameters) where TEntity : class, new();

        TEntity QueryOne<TEntity>(Dictionary<string, dynamic> parameters) where TEntity : class, new();
        TEntity QueryOne<TEntity>(string queryName, Dictionary<string, dynamic> parameters) where TEntity : class, new();

        /// <summary>
        /// execute command and return number of rows affected
        /// ex: update record or delete record
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandName"></param>
        /// <returns></returns>
        int Execute<T>(string commandName, T obj);

        /// <summary>
        /// execute command and return first colomn of first row, other are ignored
        /// ex: insert record and return auto generated id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandName"></param>
        /// <returns></returns>
        object ExecuteScalar<T>(string commandName, T obj);

        #region Transaction
        void BeginTransaction(IsolationLevel isolationLevel = System.Data.IsolationLevel.ReadCommitted);
        void Commit();
        void Rollback();
        #endregion
    }
}
