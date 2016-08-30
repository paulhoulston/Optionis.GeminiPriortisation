using System;
using System.Linq;
using System.Web.Http;
using GeminiBacklog.Controllers.DataAccess;
using GeminiBacklog.Models;

namespace GeminiBacklog.Controllers
{
    public class ReopenedIssuesController : ApiController
    {
        public const string URI = "metrics/reopenedissues/{dateFrom}";
        static readonly string _sql = SqlQueries.GetSql("GeminiBacklog.Queries.ReopenedIssues.sql");

        public static string Route(DateTime dateFrom)
        {
            return URI.Replace("{dateFrom}", dateFrom.ToString("yyyy-MM-dd"));
        }

        [Route(URI)]
        public dynamic Get(DateTime dateFrom)
        {
            var reopenedIssues = new DBWrapper().Query<ReopenedIssue>(_sql, new { dateFrom });

            return
                new
                {
                    count = reopenedIssues.Count(),
                    issues = reopenedIssues
                };
        }
    }
}