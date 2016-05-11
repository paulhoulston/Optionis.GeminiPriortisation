using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Web.Http;
using Dapper;

namespace GeminiBacklog
{
    public class LoggedTimesController : ApiController
    {
        static readonly string _connectionString = ConfigurationManager.ConnectionStrings["Gemini"].ConnectionString;
        static readonly string _sql;

        static LoggedTimesController()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GeminiBacklog.Queries.WorkHistory.sql"))
            using (StreamReader reader = new StreamReader(stream))
            {
                _sql = reader.ReadToEnd();
            }
        }

        [Route("loggedtimes/{userId}")]
        public dynamic Get(int userId)
        {
            dynamic results = null;
            var firstDayOfWeek = FirstDayOfWeek();
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                results = new
                {
                    UserId = userId,
                    StartDate = firstDayOfWeek,
                    History = sqlConnection.Query<HistoryModel>(_sql, new { UserId = 81, StartDate = firstDayOfWeek })
                };
                sqlConnection.Close();
            }
            return results;
        }

        DateTime FirstDayOfWeek()
        {
            var today = DateTime.Today;
            return DateTime.Today.AddDays(1 - (int)today.DayOfWeek);
        }

        public class HistoryModel
        {
            public string UserName { get; set; }
            public string Issue { get; set; }
            public string GeminiRef { get; set; }
            public string Monday { get; set; }
            public string Tuesday { get; set; }
            public string Wednesday { get; set; }
            public string Thursday { get; set; }
            public string Friday { get; set; }
        }
    }
}