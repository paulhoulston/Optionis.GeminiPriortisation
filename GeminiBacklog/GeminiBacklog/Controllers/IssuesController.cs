using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Web.Http;
using Dapper;
using GeminiBacklog.Models;

namespace GeminiBacklog.Controllers
{
    public class IssuesController : ApiController
    {
        static readonly string _connectionString = ConfigurationManager.ConnectionStrings["Gemini"].ConnectionString;
        static readonly string _sql;

        static IssuesController()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GeminiBacklog.Queries.AssignedIssues.sql"))
            using (StreamReader reader = new StreamReader(stream))
            {
                _sql = reader.ReadToEnd();
            }
        }

        [Route("issues/{userId}")]
        public dynamic Get(int userId)
        {
            dynamic results = null;
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                results = new { Issues = sqlConnection.Query<IssueModel>(_sql, new { userId }) };
                sqlConnection.Close();
            }
            return results;
        }
    }
}