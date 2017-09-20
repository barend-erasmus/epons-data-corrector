using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using static Dapper.SqlMapper;

namespace EPONSDataCorrector
{
    public class DbExecutor
    {
        private IDbConnection _connection { get; set; }
        private string _connectionString { get; set; }

        public DbExecutor(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new SqlConnection(_connectionString);

        }

        public IList<T> QueryProc<T>(string name, object parameters)
        {
            IList<T> results = _connection.Query<T>(name, param: parameters, commandType: CommandType.StoredProcedure).AsList<T>();

            return results;
        }

        public GridReader QueryMultiTableProc(string name, object parameters)
        {

            return _connection.QueryMultiple(name, param: parameters, commandType: CommandType.StoredProcedure);

        }

        public T QueryOneProc<T>(string name, object parameters)
        {
            IList<T> results = _connection.Query<T>(name, param: parameters, commandType: CommandType.StoredProcedure).AsList<T>();

            return results.FirstOrDefault();
        }

        public IList<T> Query<T>(string name, object parameters)
        {
            IList<T> results = _connection.Query<T>(name, parameters).AsList<T>();

            return results;
        }

        public T QueryOne<T>(string name, object parameters)
        {
            IList<T> results = _connection.Query<T>(name, parameters).AsList<T>();

            return results.FirstOrDefault();
        }
    }
}
