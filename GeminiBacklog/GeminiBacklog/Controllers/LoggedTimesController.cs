using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Web.Http;
using Dapper;

namespace GeminiBacklog.Controllers
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
                    History = sqlConnection.Query<HistoryModel>(_sql, new { UserId = userId, StartDate = firstDayOfWeek })
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
            public string Issue { get; set; }
            public string GeminiRef { get; set; }
            public int Monday { get; set; }
            public int Tuesday { get; set; }
            public int Wednesday { get; set; }
            public int Thursday { get; set; }
            public int Friday { get; set; }
        }
    }
}