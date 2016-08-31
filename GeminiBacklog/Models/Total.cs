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
        const int MINUTES_IN_WORKING_WEEK = 2250;

        public WeeklyTotal(int total)
            : base(total)
        {
            PercentageOfWorkingWeek = 100 * total / MINUTES_IN_WORKING_WEEK;
        }

        public int PercentageOfWorkingWeek { get; set; }

    }
}