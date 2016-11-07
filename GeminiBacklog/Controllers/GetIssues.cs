using System.Linq;
using GeminiBacklog.Controllers.DataAccess;
using GeminiBacklog.Models;
using System.Collections.Generic;

namespace GeminiBacklog.Controllers
{
    class GetIssues
    {
        readonly string _getIssuesSql;
        static readonly string _getAssignedResourceSql = SqlQueries.GetSql("GeminiBacklog.Queries.GetResouresForIssue.sql");

        public GetIssues(string getIssueSqlKey)
        {
            _getIssuesSql = SqlQueries.GetSql(getIssueSqlKey);
        }

        public dynamic Get(string uri, object parameters = null)
        {
            var dbWrapper = new DBWrapper();
            var dbIssues = dbWrapper.Query<BAUTaskModel>(_getIssuesSql, parameters);
            var issuesToReturn = new List<BAUTaskModel>();

            foreach (var issueId in dbIssues.Select(i => i.IssueId).Distinct())
            {
                var issue = dbIssues.First(i => i.IssueId.Equals(issueId));
                issue.AssignedTo = dbWrapper.Query<AssignedResource>(_getAssignedResourceSql, new { issueId = issue.IssueId }).Select(resource => resource.UserName);
                issuesToReturn.Add(issue);
            }

            return new
            {
                Self = uri,
                Issues = issuesToReturn
            };
        }
    }
}