using System;

namespace GeminiBacklog.Models
{
    class WorkBreakDown
    {
        public DateTime StartDate { get; set; }
        public string IssueType { get; set; }
        public int CumulativeMinutes { get; set; }
    }
}