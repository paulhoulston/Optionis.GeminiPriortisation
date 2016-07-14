using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Web.Http;
using Dapper;
using GeminiBacklog.Models;

namespace GeminiBacklog.Controllers
{
    public class ApplicationEnhancementsController : ApiController
    {
        static readonly string _connectionString = ConfigurationManager.ConnectionStrings["Gemini"].ConnectionString;
        static readonly string _sql;

        static ApplicationEnhancementsController()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GeminiBacklog.Queries.ApplicationEnhancements.sql"))
            using (StreamReader reader = new StreamReader(stream))
            {
                _sql = reader.ReadToEnd();
            }
        }

        // GET api/<controller>
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
    }

    public class BAUTasksController : ApiController
    {
        static readonly string _connectionString = ConfigurationManager.ConnectionStrings["Gemini"].ConnectionString;
        static readonly string _sql;

        static BAUTasksController()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GeminiBacklog.Queries.BAUTasks.sql"))
            using (StreamReader reader = new StreamReader(stream))
            {
                _sql = reader.ReadToEnd();
            }
        }

        // GET api/<controller>
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
    }
}