using System;
using System.Collections.Generic;
using System.Web.Http;
using GeminiBacklog.Controllers.DataAccess;

namespace GeminiBacklog.Controllers
{
    public class WeeklyWorkBreakDownController : ApiController
    {
        static readonly string _sql = SqlQueries.GetSql("GeminiBacklog.Queries.WeeklyWorkBreakDownForUser.sql");

        [Route("people/weeklybreakdown/{userId}/{startDate}")]
        public IEnumerable<BreakDown> Get(int userId, DateTime startDate)
        {
            return new DBWrapper().Query<BreakDown>(_sql, new { userId, startDate });
        }

        public class BreakDown
        {
            public string IssueType { get; set; }
            public int CumulativeMinutes { get; set; }
        }
    }
}