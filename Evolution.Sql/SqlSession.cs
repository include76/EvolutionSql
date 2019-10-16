using Evolution.Sql.Attribute;
using Evolution.Sql.CommandAdapter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Evolution.Sql
{
    public class SqlSession : ISqlSession, IDisposable
    {
        DbConnection _dbConnection;
        DbTransaction _dbTransaction;
        private bool disposed = false;
        public DbConnection Connection
        {
            get { return _dbConnection; }
            set { _dbConnection = value; }
        }

        protected AbstractCommandAdapter _commandAdapter;

        public SqlSession(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
            var factory = DbProviderFactories.GetFactory(dbConnection);
            //var connectionStringBuilder = factory.CreateConnectionStringBuilder();
            //var dbProviderInvariant = connectionStringBuilder["Provider"].ToString();
            _commandAdapter = CommandAdapterFactory.Instance(factory.GetType().Name);
        }

        #region Execute
        public int Execute<T>(string commandName, T obj)
        {
            _dbConnection.TryOpen();
            using (var command = _commandAdapter.Build<T>(_dbConnection, commandName, obj))
            {
                return command.ExecuteNonQuery();
            }
        }

        public int Execute<T>(string commandName, object parameters)
        {
            _dbConnection.TryOpen();
            using (var command = _commandAdapter.Build<T>(_dbConnection, commandName, parameters))
            {
                return command.ExecuteNonQuery();
            }
        }

        public object ExecuteScalar<T>(string commandName, T obj)
        {
            _dbConnection.TryOpen();
            using (var command = _commandAdapter.Build<T>(_dbConnection, commandName, obj))
            {
                return command.ExecuteScalar();
            }
        }

        public object ExecuteScalar<T>(string commandName, object parameters)
        {
            _dbConnection.TryOpen();
            using (var command = _commandAdapter.Build<T>(_dbConnection, commandName, parameters))
            {
                return command.ExecuteScalar();
            }
        }
        #endregion

        #region Query
        //public IEnumerable<T> Query<T>(Dictionary<string, dynamic> parameters) where T : class, new()
        //{
        //    _dbConnection.TryOpen();
        //    using (var command = _commandAdapter.Build<T>(_dbConnection, commandName, obj))
        //    {
        //        using (var dataReader = command.ExecuteReader())
        //        {
        //            return dataReader.ToEntities<T>();
        //        }
        //    }
        //}

        public IEnumerable<T> Query<T>(string commandName, object parameters) where T : class, new()
        {
            return QueryInner<T>(commandName, parameters);
        }

        private IEnumerable<T> QueryInner<T>(string commandName, object parameters) where T : class, new()
        {
            _dbConnection.TryOpen();
            using (var command = _commandAdapter.Build<T>(_dbConnection, commandName, parameters))
            {
                using (var dataReader = command.ExecuteReader())
                {
                    return dataReader.ToEntities<T>();
                }
            }
        }
        #endregion

        #region QueryOne
        //public T QueryOne<T>(Dictionary<string, dynamic> parameters) where T : class, new()
        //{
        //    _dbConnection.TryOpen();
        //    using (var command = _commandAdapter.Build<T>(_dbConnection, commandName, parameters))
        //    {
        //        using (var dataReader = command.ExecuteReader())
        //        {
        //            return dataReader.ToEntity<T>();
        //        }
        //    }
        //}

        public T QueryOne<T>(string commandName, object parameters) where T : class, new()
        {
            return QueryOneInner<T>(commandName, parameters);
        }

        private T QueryOneInner<T>(string commandName, object parameters) where T : class, new()
        {
            _dbConnection.TryOpen();
            using (var command = _commandAdapter.Build<T>(_dbConnection, commandName, parameters))
            {
                using (var dataReader = command.ExecuteReader())
                {
                    return dataReader.ToEntity<T>();
                }
            }
        }
        #endregion

        #region transaction
        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            _dbTransaction = _dbConnection.BeginTransaction();
        }
        public void Commit()
        {
            _dbTransaction.Commit();
        }
        public void Rollback()
        {
            _dbTransaction.Rollback();
        }
        #endregion

        #region private methods

        #endregion

        #region IDisposable
        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    if (_dbTransaction != null)
                    {
                        _dbTransaction.Dispose();
                    }
                    if (_dbConnection != null)
                    {
                        _dbConnection.Dispose();
                    }
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.

                // Note disposing has been done.
                disposed = true;

            }
        }

        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method
        // does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        ~SqlSession()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }
        #endregion

    }
}
