using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using GeminiBacklog.Controllers.DataAccess;

namespace GeminiBacklog.Controllers
{
    public class LoggedTimesController : ApiController
    {
        static readonly string _sql = SqlQueries.GetSql("GeminiBacklog.Queries.WorkHistory.sql");
        
        [Route("people/loggedtimes/{userId}/{startDate}")]
        public dynamic Get(int userId, DateTime startDate)
        {
            var historyItems = HistoryItems(userId, startDate);
            return new
            {
                userId,
                startDate,
                dates = Enumerable.Range(0, 5).Select(i => startDate.AddDays(i)).Select(date => DetailsForDate(date, historyItems.Where(item => item.WorkDate == date)))
            };
        }

        static dynamic DetailsForDate(DateTime date, IEnumerable<HistoryModel> timeTrackingItemsForDate)
        {
            return new
            {
                date,
                dayOfWeek = date.DayOfWeek.ToString(),
                total = new Total(timeTrackingItemsForDate.Sum(item => item.WorkTimeInMinutes)),
                issues = timeTrackingItemsForDate.Select(item => new { item.Issue, item.GeminiRef }).Distinct().Select(item => new
                {
                    item.GeminiRef,
                    item.Issue,
                    total = new Total(timeTrackingItemsForDate.Where(row => row.GeminiRef.Equals(item.GeminiRef)).Sum(row => row.WorkTimeInMinutes)),
                    tasks = timeTrackingItemsForDate.Where(row => row.GeminiRef.Equals(item.GeminiRef)).Select(row => new { row.Comments, total = new Total(row.WorkTimeInMinutes) })
                })
            };
        }

        static IEnumerable<HistoryModel> HistoryItems(int userId, DateTime startDate)
        {
            return new DBWrapper().Query<HistoryModel>(_sql, new { UserId = userId, StartDate = startDate });
        }

        class Total
        {
            public Total(int time)
            {
                Hours = time / 60;
                Minutes = time - (60 * Hours);
            }

            public int Hours { get; set; }
            public int Minutes { get; set; }
        }

    public class HistoryModel
        {
            public DateTime WorkDate { get; set; }
            public string GeminiRef { get; set; }
            public string Issue { get; set; }
            public int WorkTimeInMinutes { get; set; }
            public string Comments { get; set; }
        }
    }
}