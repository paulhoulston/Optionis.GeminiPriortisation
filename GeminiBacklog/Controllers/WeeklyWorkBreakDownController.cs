using System;
using System.Linq;
using System.Web.Http;
using GeminiBacklog.Controllers.DataAccess;

namespace GeminiBacklog.Controllers
{
    public class WeeklyWorkBreakDownController : ApiController
    {
        static readonly string _sql = SqlQueries.GetSql("GeminiBacklog.Queries.WeeklyWorkBreakDownForUser.sql");

        [Route("people/weeklybreakdown/{userId}/{startDate}")]
        public dynamic Get(int userId, DateTime startDate)
        {
            return new
            {
                issueTypes = new DBWrapper()
                .Query<BreakDown>(_sql, new { userId, startDate })
                .Select(item => new
                {
                    item.IssueType,
                    total = new WeeklyTotal(item.CumulativeMinutes)
                })
            };
        }

        public class BreakDown
        {
            public string IssueType { get; set; }
            public int CumulativeMinutes { get; set; }
        }
    }
}