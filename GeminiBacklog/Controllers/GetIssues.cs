using System.Linq;
using GeminiBacklog.Controllers.DataAccess;
using GeminiBacklog.Models;

namespace GeminiBacklog.Controllers
{
    class GetIssues
    {
        readonly string _getIssuesSql = SqlQueries.GetSql("GeminiBacklog.Queries.ApplicationEnhancements.sql");
        static readonly string _getAssignedResourceSql = SqlQueries.GetSql("GeminiBacklog.Queries.GetResouresForIssue.sql");

        public GetIssues(string getIssueSqlKey)
        {
            _getIssuesSql = SqlQueries.GetSql(getIssueSqlKey);
        }

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