using System;
using System.Configuration;

namespace GeminiBacklog.Models
{
    public class IssueModel
    {
        static readonly string _viewIssueUri = ConfigurationManager.AppSettings["GEMINI_VIEW_ISSUE_URI"];

        public string IssueId { get; set; }
        public string Issue { get; set; }
        public string Summary { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? Created { get; set; }
        public string Priority { get; set; }
        public string Project { get; set; }
        public string GeminiUri { get { return string.Format(_viewIssueUri, IssueId, Project); } }
    }

}