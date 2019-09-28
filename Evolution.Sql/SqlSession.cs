using Evolution.Sql.Attribute;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Evolution.Sql
{
    public class SqlSession : ISqlSession, IDisposable
    {
        IDbConnection _dbConnection;
        IDbTransaction _dbTransaction;
        private bool disposed = false;
        public IDbConnection Connection
        {
            get { return _dbConnection; }
            set { _dbConnection = value; }
        }

        public SqlSession()
        {

        }

        public SqlSession(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public int Execute<T>(string commandName, T obj)
        {
            _dbConnection.TryOpen();
            using (var command = _dbConnection.CreateCommand())
            {
                BuildCommand<T>(command, commandName);
                return command.ExecuteNonQuery();
            }
        }

        public object ExecuteScalar<T>(string commandName, T obj)
        {
            _dbConnection.TryOpen();
            using (var command = _dbConnection.CreateCommand())
            {
                BuildCommand<T>(command, commandName);
                return command.ExecuteScalar();
            }
        }

        public IEnumerable<TEntity> Query<TEntity>(Dictionary<string, dynamic> parameters) where TEntity : class, new()
        {
            _dbConnection.TryOpen();
            using (var command = _dbConnection.CreateCommand())
            {
                using (var dataReader = command.ExecuteReader())
                {
                    return dataReader.ToEntities<TEntity>();
                }
            }
        }

        public IEnumerable<TEntity> Query<TEntity>(string queryName, Dictionary<string, dynamic> parameters) where TEntity : class, new()
        {
            _dbConnection.TryOpen();
            using (var command = _dbConnection.CreateCommand())
            {
                using (var dataReader = command.ExecuteReader())
                {
                    return dataReader.ToEntities<TEntity>();
                }
            }
        }

        public TEntity QueryOne<TEntity>(Dictionary<string, dynamic> parameters) where TEntity : class, new()
        {
            _dbConnection.TryOpen();
            using (var command = _dbConnection.CreateCommand())
            {
                using (var dataReader = command.ExecuteReader())
                {
                    return dataReader.ToEntity<TEntity>();
                }
            }
        }

        public TEntity QueryOne<TEntity>(string queryName, Dictionary<string, dynamic> parameters) where TEntity : class, new()
        {
            _dbConnection.TryOpen();
            using (var command = _dbConnection.CreateCommand())
            {
                using (var dataReader = command.ExecuteReader())
                {
                    return dataReader.ToEntity<TEntity>();
                }
            }
        }

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
        private void BuildCommand<T>(IDbCommand dbCommand, string commandName)
        {
            var type = typeof(T);
            foreach (var item in type.GetCustomAttributes(typeof(CommandAttribute), false))
            {
                var attr = item as CommandAttribute;
                dbCommand.CommandType = attr.CommandType;
                // if attribute.Command not set, we suppose commandName is sp name, and use as commandText
                dbCommand.CommandText = string.IsNullOrEmpty(attr.Text) ? commandName : attr.Text;
            }
        }
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
