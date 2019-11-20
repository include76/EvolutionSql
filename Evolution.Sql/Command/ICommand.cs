using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Evolution.Sql
{
    public interface ICommand
    {
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
    }
}
