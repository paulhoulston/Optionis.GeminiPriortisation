using System.Web.Http;
using GeminiBacklog.Controllers.DataAccess;
using GeminiBacklog.Models;

namespace GeminiBacklog.Controllers
{
    public class SearchIssuesController : ApiController
    {
        static readonly string _sql = SqlQueries.GetSql("GeminiBacklog.Queries.SearchIssues.sql");

        public dynamic Get(string searchTerm)
        {
            return new { Issues = new DBWrapper().Query<IssueModel>(_sql, new { searchTerm }) };
        }
    }
}