using GeminiBacklog.Controllers.DataAccess;
using GeminiBacklog.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace GeminiBacklog.Controllers
{
    public class GetDeploymentHistoryController : ApiController
    {
        static readonly string _sql = SqlQueries.GetSql("GeminiBacklog.Queries.DeploymentHistory.sql");

        [Route("deployments")]
        public dynamic Get()
        {
            var deployments = new DBWrapper().Query<Models.Deployment, DeploymentIssues>(_sql);

            var output = new List<Deployment>();
            foreach(var deployment in deployments.Item1)
            {
                output.Add(new Deployment
                {
                    Application = deployment.Application,
                    Comments = deployment.Comments,
                    DeployedBy = deployment.DeployedBy,
                    DeploymentDate = deployment.DeploymentDate.ToString("dd/MM/yyyy"),
                    DeploymentStatus = deployment.DeploymentStatus,
                    ReleaseTitle = deployment.ReleaseTitle,
                    Issues = deployments.Item2.Where(issue => issue.DeploymentId == deployment.DeploymentId).Select(issue => new Issue
                    {
                      GeminiUri=issue.GeminiUri,
                      IssueDescription=issue.IssueDescription,
                      Summary = issue.Summary
                    })
                });
            }

            return new
            {
                deployments = output
            };
        }

        class Deployment
        {
            public string DeploymentDate { get; set; }
            public string ReleaseTitle { get; set; }
            public string Application { get; set; }
            public string Comments { get; set; }
            public string DeployedBy { get; set; }
            public string DeploymentStatus { get; set; }

            public IEnumerable<Issue> Issues { get; set; }
        }

        class Issue
        {
            public string IssueDescription { get; set; }
            public string Summary { get; set; }
            public string GeminiUri { get; set; }
        }
    }
}