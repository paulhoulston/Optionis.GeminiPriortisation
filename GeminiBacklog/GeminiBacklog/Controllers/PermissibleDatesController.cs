using System;
using System.Web.Http;

namespace GeminiBacklog.Controllers
{
    public class PermissibleDatesController : ApiController
    {
        [Route("permissibledates")]
        public dynamic Get()
        {
            var theDate = FirstDayOfWeek();
            var availableDates = new DateField[25];
            for (var i = 0; i < 25; i++)
            {
                availableDates[i] = new DateField(theDate.AddDays(-7 * i));
            }
            return new { availableDates };
        }

        DateTime FirstDayOfWeek()
        {
            var today = DateTime.Today;
            return DateTime.Today.AddDays(1 - (int)today.DayOfWeek);
        }

        class DateField
        {
            public DateField(DateTime theDate)
            {
                DateText = theDate.ToString("dd/MM/yyyy");
                DateValue = theDate.ToString("dd-MMM-yyyy");

            }
            public string DateText { get; set; }
            public string DateValue { get; set; }
        }
    }
}