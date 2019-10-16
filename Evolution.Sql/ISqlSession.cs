using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Evolution.Sql
{
    public interface ISqlSession: IDisposable
    {
        DbConnection Connection { get; set; }

        //IEnumerable<TEntity> Query<TEntity>(Dictionary<string, dynamic> parameters) where TEntity : class, new();
        IEnumerable<T> Query<T>(string commandName, object parameters) where T : class, new();

        //TEntity QueryOne<TEntity>(Dictionary<string, dynamic> parameters) where TEntity : class, new();
        T QueryOne<T>(string commandName, object parameters) where T : class, new();

        /// <summary>
        /// execute command and return number of rows affected
        /// ex: update record or delete record
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandName"></param>
        /// <returns></returns>
        int Execute<T>(string commandName, T obj);
        /// <summary>
        /// execute command and return number of rows affected
        /// ex: update record or delete record
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandName"></param>
        /// <returns></returns>
        int Execute<T>(string commandName, object parameters);

        /// <summary>
        /// execute command and return first colomn of first row, other are ignored
        /// ex: insert record and return auto generated id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandName"></param>
        /// <returns></returns>
        object ExecuteScalar<T>(string commandName, T obj);

        object ExecuteScalar<T>(string commandName, object parameters);

        #region Transaction
        void BeginTransaction(IsolationLevel isolationLevel = System.Data.IsolationLevel.ReadCommitted);
        void Commit();
        void Rollback();
        #endregion
    }
}
