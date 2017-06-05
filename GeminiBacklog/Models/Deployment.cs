using System;
using System.Configuration;

namespace GeminiBacklog.Models
{
    public class Deployment
    {
        public int DeploymentId { get; set; }
        public DateTime DeploymentDate { get; set; }
        public string ReleaseTitle { get; set; }
        public string Application { get; set; }
        public string Comments { get; set; }
        public string DeployedBy { get; set; }
        public string DeploymentStatus { get; set; }
    }

    public class DeploymentIssues
    {
        static readonly string _viewIssueUri = ConfigurationManager.AppSettings["GEMINI_VIEW_ISSUE_URI"];

        public int DeploymentId { get; set; }
        public string ProjectId { get; set; }
        public string IssueId { get; set; }
        public string IssueDescription { get; set; }
        public string Summary { get; set; }

        public string GeminiUri { get { return string.Format(_viewIssueUri, IssueId, ProjectId); } }

    }
}