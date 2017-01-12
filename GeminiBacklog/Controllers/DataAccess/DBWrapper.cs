using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;

namespace GeminiBacklog.Controllers.DataAccess
{
    class DBWrapper
    {
        static readonly string _connectionString = ConfigurationManager.ConnectionStrings["Gemini"].ConnectionString;

        public IEnumerable<T> Query<T>(string sql, object param = null) where T : class
        {
            IEnumerable<T> results = null;
            Query(sqlConnection => results = sqlConnection.Query<T>(sql, param));
            return results;
        }

        public IEnumerable<T> Exec<T>(string sql, object param = null) where T : class
        {
            IEnumerable<T> results = null;
            Query(sqlConnection => results = sqlConnection.Query<T>(sql, param, commandType: System.Data.CommandType.StoredProcedure));
            return results;
        }

        public T Single<T>(string sql, object param = null) where T : class
        {
            T result = null;
            Query(sqlConnection => result = sqlConnection.ExecuteScalar<T>(sql, param));
            return result;
        }

        void Query(Action<SqlConnection> sqlMethod)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                sqlMethod(sqlConnection);
                sqlConnection.Close();
            }
        }
    }
}