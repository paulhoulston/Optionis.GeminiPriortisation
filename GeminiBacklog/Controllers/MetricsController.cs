using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using GeminiBacklog.Controllers.DataAccess;

namespace GeminiBacklog.Controllers
{
    public class MetricsController : ApiController
    {
        public dynamic Get()
        {
            return
                new
                {
                    reopenedIssues = new ReopenedIssuesForLast6Months().Get(),
                    devWorkBreakdown = new WorkBreakdownForDevelopers().Get()
                };
        }

    }

    class ReopenedIssuesForLast6Months
    {
        public dynamic Get()
        {
            var last1Months = DateTime.Today.AddMonths(-1);
            var last3Months = DateTime.Today.AddMonths(-3);
            var last6Months = DateTime.Today.AddMonths(-6);

            return new
            {
                last1Months = new
                {
                    count = new ReopenedIssuesController().Get(last1Months).count,
                    uri = ReopenedIssuesController.Route(last1Months)
                },
                last3Months = new
                {
                    count = new ReopenedIssuesController().Get(last3Months).count,
                    uri = ReopenedIssuesController.Route(last3Months)
                },
                last6Months = new
                {
                    count = new ReopenedIssuesController().Get(last6Months).count,
                    uri = ReopenedIssuesController.Route(last6Months)
                }
            };
        }
    }
    class WorkBreakdownForDevelopers
    {
        static readonly IEnumerable<int> _devUserIds = ConfigurationManager.AppSettings["GEMINI_DEV_USER_IDS"].Split(',').Select(id => int.Parse(id));
        static readonly string _sql = SqlQueries.GetSql("GeminiBacklog.Queries.IssueTypeBreakDownForUsers.sql");

        class BreakDown
        {
            public string IssueType { get; set; }
            public int CumulativeMinutesWeek1 { get; set; }
            public int CumulativeMinutesWeek2 { get; set; }
            public int CumulativeMinutesWeek3 { get; set; }
            public int CumulativeMinutesWeek4 { get; set; }
        }

        public dynamic Get()
        {
            var availableMinutesInWeek = WeeklyTotal.MINUTES_IN_WORKING_WEEK * _devUserIds.Count();
            var startDate = DateTime.Today.AddDays(1 - (int)DateTime.Today.DayOfWeek - 7);
            var devWorkBreakdown = new DBWrapper().Query<BreakDown>(_sql, new { startDate, userIds = _devUserIds });
            return devWorkBreakdown.Select(item => new
            {
                item.IssueType,
                totalWeek1 = new WeeklyTotal(item.CumulativeMinutesWeek1, availableMinutesInWeek),
                totalWeek2 = new WeeklyTotal(item.CumulativeMinutesWeek2, availableMinutesInWeek),
                totalWeek3 = new WeeklyTotal(item.CumulativeMinutesWeek3, availableMinutesInWeek),
                totalWeek4 = new WeeklyTotal(item.CumulativeMinutesWeek4, availableMinutesInWeek)
            });

        }
    }
}