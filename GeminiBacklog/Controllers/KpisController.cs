using GeminiBacklog.Controllers.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace GeminiBacklog.Controllers
{
    public class KpisController : ApiController
    {
        const string RELEASES_SQL = "kpi.releases_per_month";
        const string ISSUE_THROUGHPUT_SQL = "kpi.enhancements_vs_bau_monthly_issue_state";
        const string TIME_LOGGED_SQL = "kpi.time_logged_per_month";

        readonly DBWrapper _dbWrapper = new DBWrapper();

        [Route("kpis/{startMonth}/{monthsToView}")]
        public dynamic Get(DateTime startMonth, int monthsToView)
        {
            var param = new { StartMonth = startMonth, MonthsToView = monthsToView };

            return new
            {
                Releases = GetReleases(param),
                IssueThroughput = GetIssueThroughput(param),
                LoggedTime = GetLoggedTime(param)
            };
        }

        dynamic GetLoggedTime(object param)
        {
            return _dbWrapper
                .Exec<LoggedTime>(TIME_LOGGED_SQL, param)
                .Select(i => new
                {
                    Month = i.Month.ToString("MMM-yyyy"),
                    bau = new
                    {
                        time = new Total(i.LoggedTimeForBAU),
                        percentage = (100 * i.LoggedTimeForBAU / (float)(i.LoggedTimeForBAU + i.LoggedTimeForEnhancements)).ToString("N2")
                    },
                    enhancements = new
                    {
                        time = new Total(i.LoggedTimeForEnhancements),
                        percentage = (100 * i.LoggedTimeForEnhancements / (float)(i.LoggedTimeForBAU + i.LoggedTimeForEnhancements)).ToString("N2")
                    }
                });
        }

        dynamic GetReleases(object param)
        {
            return _dbWrapper.Exec<Releases>(RELEASES_SQL, param).Select(i => new
            {
                Month = i.Month.ToString("MMM-yyyy"),
                i.Pending,
                i.Success,
                i.Failed,
                i.Aborted
            });
        }

        dynamic GetIssueThroughput(object param)
        {
            var issueThroughput = _dbWrapper.Exec<IssueThroughput>(ISSUE_THROUGHPUT_SQL, param);
            return new
            {
                bau = Filter(issueThroughput, "BAU"),
                enhancements = Filter(issueThroughput, "Enhancement")
            };
        }

        static dynamic Filter(IEnumerable<IssueThroughput> items, string issueType)
        {
            return items.Where(i => i.IssueType.Equals(issueType)).Select(i => new
            {
                Month = i.Month.ToString("MMM-yyyy"),
                i.CreatedInMonth,
                i.ClosedInMonth,
                i.OpenIssuesAtMonthStart
            });
        }

        class LoggedTime
        {
            public DateTime Month { get; set; }
            public int LoggedTimeForBAU { get; set; }
            public int LoggedTimeForEnhancements { get; set; }
        }

        class IssueThroughput
        {
            public DateTime Month { get; set; }
            public string IssueType { get; set; }
            public int CreatedInMonth { get; set; }
            public int ClosedInMonth { get; set; }
            public int OpenIssuesAtMonthStart { get; set; }
        }

        class Releases
        {
            public DateTime Month { get; set; }
            public int Pending { get; set; }
            public int Success { get; set; }
            public int Failed { get; set; }
            public int Aborted { get; set; }
        }
    }
}