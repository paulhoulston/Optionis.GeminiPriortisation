using System.Linq;
using System.Web.Http;
using GeminiBacklog.Controllers.DataAccess;
using GeminiBacklog.Models;

namespace GeminiBacklog.Controllers
{
    public class BAUTasksController : ApiController
    {
        static readonly string _getIssuesSql = SqlQueries.GetSql("GeminiBacklog.Queries.BAUTasks.sql");
        static readonly string _getAssignedResourceSql = SqlQueries.GetSql("GeminiBacklog.Queries.GetResouresForIssue.sql");

        public dynamic Get()
        {
            var dbWrapper = new DBWrapper();
            var issues = dbWrapper.Query<BAUTaskModel>(_getIssuesSql);

            foreach (var issue in issues)
            {
                issue.AssignedTo = dbWrapper.Query<AssignedResource>(_getAssignedResourceSql, new { issueId = issue.IssueId }).Select(resource => resource.UserName);
            }

            return new { Issues = issues };
        }
    }
}