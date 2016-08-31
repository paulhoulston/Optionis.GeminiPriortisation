using System.Web.Http;
using GeminiBacklog.Controllers.DataAccess;
using GeminiBacklog.Models;

namespace GeminiBacklog.Controllers
{
    public class IssuesController : ApiController
    {
        static readonly string _sql = SqlQueries.GetSql("GeminiBacklog.Queries.AssignedIssues.sql");

        [Route("people/issues/{userId}")]
        public dynamic Get(int userId)
        {
            return new { Issues = new DBWrapper().Query<IssueModel>(_sql, new { userId }) };
        }
    }
}