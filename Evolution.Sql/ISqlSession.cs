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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<T> Query<T>(string commandName, object parameters) where T : class, new();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandName"></param>
        /// <param name="parameters"></param>
        /// <param name="outPuts"></param>
        /// <returns></returns>
        IEnumerable<T> Query<T>(string commandName, object parameters, Dictionary<string, dynamic> outPuts) where T : class, new();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        T QueryOne<T>(string commandName, object parameters) where T : class, new();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandName"></param>
        /// <param name="parameters"></param>
        /// <param name="outPuts"></param>
        /// <returns></returns>
        T QueryOne<T>(string commandName, object parameters, Dictionary<string, dynamic> outPuts) where T : class, new();

        /// <summary>
        /// execute command and return number of rows affected
        /// ex: update record or delete record
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandName"></param>
        /// <returns></returns>
        int Execute<T>(string commandName, T obj);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandName"></param>
        /// <param name="obj"></param>
        /// <param name="outPuts"></param>
        /// <returns></returns>
        int Execute<T>(string commandName, T obj, Dictionary<string, dynamic> outPuts);

        /// <summary>
        /// execute command and return number of rows affected
        /// ex: update record or delete record
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandName"></param>
        /// <returns></returns>
        int Execute<T>(string commandName, object parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandName"></param>
        /// <param name="parameters"></param>
        /// <param name="outPuts"></param>
        /// <returns></returns>
        int Execute<T>(string commandName, object parameters, Dictionary<string, dynamic> outPuts);

        /// <summary>
        /// execute command and return first colomn of first row, other are ignored
        /// ex: insert record and return auto generated id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandName">commandName</param>
        /// <returns></returns>
        object ExecuteScalar<T>(string commandName, T obj);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandName"></param>
        /// <param name="obj"></param>
        /// <param name="outPuts"></param>
        /// <returns></returns>
        object ExecuteScalar<T>(string commandName, T obj, Dictionary<string, dynamic> outPuts);

        /// <summary>
        /// execute command and return first colomn of first row, other are ignored
        /// ex: insert record and return auto generated id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandName">commandName</param>
        /// <param name="parameters">anonymouse type</param>
        /// <returns></returns>
        object ExecuteScalar<T>(string commandName, object parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandName"></param>
        /// <param name="parameters"></param>
        /// <param name="outPuts"></param>
        /// <returns></returns>
        object ExecuteScalar<T>(string commandName, object parameters, Dictionary<string, dynamic> outPuts);

        #region Transaction
        void BeginTransaction(IsolationLevel isolationLevel = System.Data.IsolationLevel.ReadCommitted);
        void Commit();
        void Rollback();
        #endregion
    }
}
