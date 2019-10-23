using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution.Sql
{
    public partial class SqlSession : ISqlSession
    {
        #region Query
        public async Task<IEnumerable<T>> QueryAsync<T>(string commandName, object parameters) where T : class, new()
        {
            _dbConnection.TryOpen();
            using (var command = _commandAdapter.Build<T>(_dbConnection, commandName, parameters))
            {
                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    return dataReader.ToEntities<T>();
                }
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string commandName, object parameters, Dictionary<string, dynamic> outPuts) where T : class, new()
        {
            _dbConnection.TryOpen();
            using (var command = _commandAdapter.Build<T>(_dbConnection, commandName, parameters))
            {
                IEnumerable<T> result;
                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    result = dataReader.ToEntities<T>();
                }
                SetOutputParameters(command, outPuts);
                return result;
            }
        }
        #endregion

        #region Query One
        public async Task<T> QueryOneAsync<T>(string commandName, object parameters) where T : class, new()
        {
            _dbConnection.TryOpen();
            using (var command = _commandAdapter.Build<T>(_dbConnection, commandName, parameters))
            {
                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    return dataReader.ToEntities<T>()?.FirstOrDefault();
                }
            }
        }

        public async Task<T> QueryOneAsync<T>(string commandName, object parameters, Dictionary<string, dynamic> outPuts) where T : class, new()
        {
            using (var command = _commandAdapter.Build<T>(_dbConnection, commandName, parameters))
            {
                T result;
                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    result = dataReader.ToEntities<T>()?.FirstOrDefault();
                }
                // output parameter must get after datareader closed
                SetOutputParameters(command, outPuts);
                return result;
            }
        }
        #endregion

        #region Execute
        public async Task<int> ExecuteAsync<T>(string commandName, T obj)
        {
            _dbConnection.TryOpen();
            using (var command = _commandAdapter.Build<T>(_dbConnection, commandName, obj))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<int> ExecuteAsync<T>(string commandName, T obj, Dictionary<string, dynamic> outPuts)
        {
            _dbConnection.TryOpen();
            using (var command = _commandAdapter.Build<T>(_dbConnection, commandName, obj))
            {
                var result = await command.ExecuteNonQueryAsync();
                SetOutputParameters(command, outPuts);
                return result;
            }
        }

        public async Task<int> ExecuteAsync<T>(string commandName, object parameters)
        {
            _dbConnection.TryOpen();
            using (var command = _commandAdapter.Build<T>(_dbConnection, commandName, parameters))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<int> ExecuteAsync<T>(string commandName, object parameters, Dictionary<string, dynamic> outPuts)
        {
            _dbConnection.TryOpen();
            using (var command = _commandAdapter.Build<T>(_dbConnection, commandName, parameters))
            {
                var result = await command.ExecuteNonQueryAsync();
                SetOutputParameters(command, outPuts);
                return result;
            }
        }
        #endregion

        #region ExecuteScale
        public async Task<object> ExecuteScalarAsync<T>(string commandName, T obj)
        {
            _dbConnection.TryOpen();
            using (var command = _commandAdapter.Build<T>(_dbConnection, commandName, obj))
            {
                return await command.ExecuteScalarAsync();
            }
        }

        public async Task<object> ExecuteScalarAsync<T>(string commandName, T obj, Dictionary<string, dynamic> outPuts)
        {
            _dbConnection.TryOpen();
            using (var command = _commandAdapter.Build<T>(_dbConnection, commandName, obj))
            {
                SetOutputParameters(command, outPuts);
                return await command.ExecuteScalarAsync();
            }
        }

        public async Task<object> ExecuteScalarAsync<T>(string commandName, object parameters)
        {
            _dbConnection.TryOpen();
            using (var command = _commandAdapter.Build<T>(_dbConnection, commandName, parameters))
            {
                return await command.ExecuteScalarAsync();
            }
        }

        public async Task<object> ExecuteScalarAsync<T>(string commandName, object parameters, Dictionary<string, dynamic> outPuts)
        {
            _dbConnection.TryOpen();
            using (var command = _commandAdapter.Build<T>(_dbConnection, commandName, parameters))
            {
                SetOutputParameters(command, outPuts);
                return await command.ExecuteScalarAsync();
            }
        }
        #endregion
    }
}
