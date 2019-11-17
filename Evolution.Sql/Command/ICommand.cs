using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Evolution.Sql
{
    public interface ICommand
    {
        internal int CommandTimeout { get; set; }
        internal DbTransaction Transaction { get; set; }
        internal Dictionary<Type, ITypeHandler> TypeHandlers { get; set; }

        internal DbCommand Build(object parameters);
        internal DbCommand Build(params DbParameter[] parameters);

        /// <summary>
        /// Map handler for specific type
        /// </summary>
        /// <param name="iCommand"></param>
        /// <returns></returns>
        public ICommand WithTypeHandler<TType, THandler>() where THandler : ITypeHandler
        {
            this.TypeHandlers.Add(typeof(TType), Activator.CreateInstance<THandler>());
            return this;
        }

        /// <summary>
        /// Set command timeout in seconds, default 30
        /// </summary>
        /// <param name="iCommand"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public ICommand SetTimeOut(int timeout)
        {
            this.CommandTimeout = timeout;
            return this; 
        }

        /// <summary>
        /// Set command transaction
        /// </summary>
        /// <param name="iCommand"></param>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        public ICommand SetTransaction(DbTransaction dbTransaction)
        {
            this.Transaction = dbTransaction;
            return this;
        }
    }
}
