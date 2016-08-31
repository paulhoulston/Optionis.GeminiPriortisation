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
}