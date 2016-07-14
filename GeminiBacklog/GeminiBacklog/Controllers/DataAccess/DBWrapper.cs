using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;

namespace GeminiBacklog.Controllers.DataAccess
{
    class DBWrapper
    {
        static readonly string _connectionString = ConfigurationManager.ConnectionStrings["Gemini"].ConnectionString;

        public IEnumerable<T> Query<T>(string sql, object param = null)
        {
            IEnumerable<T> results = null;
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                results = sqlConnection.Query<T>(sql, param);
                sqlConnection.Close();
            }
            return results;
        }
    }
}