using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Common.Utility
{
    public class DatabaseHelper
    {
        private IDbConnection connection;
        public DatabaseHelper(IDbConnection _connection)
        {
            connection = _connection;
        }
        public IList<TOut> ExecuteStoredProcedure<TOut>(string procedureName, object parameters)
        {
            IList<TOut> result = null;

            if (connection.State != ConnectionState.Open)
                connection.Open();

            result = connection.Query<TOut>(procedureName,
                            parameters, commandType: CommandType.StoredProcedure) as List<TOut>;
            return result;
        }

        public TOut ExecuteStoredProcedure_OneResultSet<TOut>(string procedureName, object parameters)
        {
            TOut result = default(TOut);

            if (connection.State != ConnectionState.Open)
                connection.Open();

            result = connection.QueryFirst<TOut>(procedureName,
                            parameters, commandType: CommandType.StoredProcedure);

            return result;
        }

        public async Task ExecuteStoredProcedure_NoResultAsync(string procedureName, object parameters)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();

           await connection.ExecuteAsync(procedureName, param: parameters, commandType: CommandType.Text);
        }

        public async Task<int> ExecuteStoredProcedure_ScalarAsync(string procedureName, object parameters)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();

           return await connection.ExecuteScalarAsync<int>(procedureName, param: parameters, commandType: CommandType.Text);
        }
    }
}
