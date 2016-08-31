using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using GeminiBacklog.Controllers.DataAccess;
using GeminiBacklog.Models;

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
                    devWork = new WorkBreakdownForDevelopers().Get()
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
        static readonly string _getBreakdownSql = SqlQueries.GetSql("GeminiBacklog.Queries.WeeklyWorkBreakDownForMultipleUsers.sql");
        static readonly string _getUserSql = SqlQueries.GetSql("GeminiBacklog.Queries.GetUsers.sql");
        readonly DateTime _startDate;
        readonly DBWrapper _dBWrapper;

        public WorkBreakdownForDevelopers()
        {
            _startDate = DateTime.Today.AddDays(1 - (int)DateTime.Today.DayOfWeek);
            _dBWrapper = new DBWrapper();
        }

        public dynamic Get()
        {
            return new
            {
                developers = GetUsers(),
                breakdown = GetWorkBreakdown()
            };
        }

        IEnumerable<DevWorkBreakdown> GetWorkBreakdown()
        {
            var results = new Dictionary<string, DevWorkBreakdown>();

            AddTotalsForWeek(results, _startDate.AddDays(-7), (itemType, weeklyTotal) => results[itemType].CumulativeMinutesWeek1 = weeklyTotal);
            AddTotalsForWeek(results, _startDate.AddDays(-14), (itemType, weeklyTotal) => results[itemType].CumulativeMinutesWeek2 = weeklyTotal);
            AddTotalsForWeek(results, _startDate.AddDays(-21), (itemType, weeklyTotal) => results[itemType].CumulativeMinutesWeek3 = weeklyTotal);
            AddTotalsForWeek(results, _startDate.AddDays(-28), (itemType, weeklyTotal) => results[itemType].CumulativeMinutesWeek4 = weeklyTotal);

            return results.Values;
        }

        private IEnumerable<User> GetUsers()
        {
            return _dBWrapper.Query<User>(_getUserSql, new { userIds = _devUserIds });
        }

        void AddTotalsForWeek(IDictionary<string, DevWorkBreakdown> results, DateTime startDate, Action<string, WeeklyTotal> assignWeeklyTotal)
        {
            var devWorkBreakdown = _dBWrapper.Query<WorkBreakDown>(_getBreakdownSql, new { startDate = startDate, userIds = _devUserIds });

            var availableMinutesInWeek = WeeklyTotal.MINUTES_IN_WORKING_WEEK * _devUserIds.Count();

            foreach (var item in devWorkBreakdown)
            {
                if (!results.ContainsKey(item.IssueType))
                    results.Add(item.IssueType, new DevWorkBreakdown
                    {
                        IssueType = item.IssueType
                    });
                assignWeeklyTotal(item.IssueType, new WeeklyTotal(item.CumulativeMinutes, availableMinutesInWeek, item.StartDate));
            }
        }

        class User
        {
            public int Userid { get; set; }
            public string Username { get; set; }
            public string Firstname { get; set; }
            public string Surname { get; set; }
        }

        class DevWorkBreakdown
        {
            public string IssueType { get; set; }
            public WeeklyTotal CumulativeMinutesWeek1 { get; set; }
            public WeeklyTotal CumulativeMinutesWeek2 { get; set; }
            public WeeklyTotal CumulativeMinutesWeek3 { get; set; }
            public WeeklyTotal CumulativeMinutesWeek4 { get; set; }
        }
    }
}