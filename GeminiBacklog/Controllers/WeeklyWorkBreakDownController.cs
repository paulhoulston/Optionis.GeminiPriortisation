using System;
using System.Linq;
using System.Web.Http;
using GeminiBacklog.Controllers.DataAccess;
using GeminiBacklog.Models;

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
                .Query<WorkBreakDown>(_sql, new { userId, startDate })
                .Select(item => new
                {
                    item.IssueType,
                    total = new WeeklyTotal(item.CumulativeMinutes, WeeklyTotal.MINUTES_IN_WORKING_WEEK, startDate)
                })
            };
        }
    }
}