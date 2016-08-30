using System;
using System.Linq;
using System.Web.Http;
using GeminiBacklog.Controllers.DataAccess;

namespace GeminiBacklog.Controllers
{
    public class ReopenedIssuesController : ApiController
    {
        static readonly string _sql = SqlQueries.GetSql("GeminiBacklog.Queries.ReopenedIssues.sql");

        class ReopenedIssue
        {
            public int IssueId { get; set; }
            public string Issue { get; set; }
            public string Summary { get; set; }
        }

        [Route("issues/reopened/{dateFrom}")]
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