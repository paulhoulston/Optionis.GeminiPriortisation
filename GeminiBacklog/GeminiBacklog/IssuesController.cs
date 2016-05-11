using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Web.Http;
using Dapper;

namespace GeminiBacklog
{
    public class IssuesController : ApiController
    {
        static readonly string _connectionString = ConfigurationManager.ConnectionStrings["Gemini"].ConnectionString;
        static readonly string _sql;

        static IssuesController()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GeminiBacklog.Queries.Prioritisation.sql"))
            using (StreamReader reader = new StreamReader(stream))
            {
                _sql = reader.ReadToEnd();
            }
        }

        [Route("issues")]
        public dynamic Get()
        {
            dynamic results = null;
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                results = new { Issues = sqlConnection.Query<IssueModel>(_sql) };
                sqlConnection.Close();
            }
            return results;
        }

        public class IssueModel
        {
            public string Issue { get; set; }
            public string Summary { get; set; }
            public string Status { get; set; }
            public string Type { get; set; }
            public string DueDate { get; set; }
            public string Created { get; set; }
            public string Priority { get; set; }
        }
    }
}