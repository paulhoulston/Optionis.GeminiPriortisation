using System;

namespace GeminiBacklog.Controllers
{
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

    class WeeklyTotal : Total
    {
        public const int MINUTES_IN_WORKING_WEEK = 2250;

        public WeeklyTotal(int total, int availableMinutesInWeek, DateTime startDate)
            : base(total)
        {
            StartDate = startDate;
            PercentageOfWorkingWeek = 100 * total / availableMinutesInWeek;
        }

        public int PercentageOfWorkingWeek { get; set; }
        public DateTime StartDate { get; set; }
        public string StartDateDisplay { get { return StartDate.ToShortDateString(); } }
    }
}