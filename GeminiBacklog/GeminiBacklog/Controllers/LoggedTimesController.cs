using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
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

        [Route("loggedtimes/{userId}/{startDate}")]
        public dynamic Get(int userId, DateTime startDate)
        {
            var historyItems = HistoryItems(userId, startDate);
            return new
                {
                    UserId = userId,
                    StartDate = startDate,
                    History = historyItems,
                    Total = new
                    {
                        Monday = historyItems.Sum(items => items.Monday),
                        Tuesday = historyItems.Sum(items => items.Tuesday),
                        Wednesday = historyItems.Sum(items => items.Wednesday),
                        Thursday = historyItems.Sum(items => items.Thursday),
                        Friday = historyItems.Sum(items => items.Friday)
                    }
                };
            }

        IEnumerable<HistoryModel> HistoryItems(int userId, DateTime startDate)
        {
            IEnumerable<HistoryModel> results = null;
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                results =
                    sqlConnection
                        .Query<HistoryModel>(_sql, new { UserId = userId, StartDate = startDate })
                        .OrderByDescending(items => items.Monday)
                        .ThenByDescending(items => items.Tuesday)
                        .ThenByDescending(items => items.Wednesday)
                        .ThenByDescending(items => items.Thursday)
                        .ThenByDescending(items => items.Friday);
                sqlConnection.Close();
            }
            return results;
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