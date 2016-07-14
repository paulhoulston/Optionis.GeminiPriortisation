using System.Web.Http;
using GeminiBacklog.Controllers.DataAccess;
using GeminiBacklog.Models;

namespace GeminiBacklog.Controllers
{
    public class PrioritiesController : ApiController
    {
        static readonly string _sql = SqlQueries.GetSql("GeminiBacklog.Queries.DevPriorities.sql");

        public dynamic Get()
        {
            return new { Issues = new DBWrapper().Query<IssueModel>(_sql) };
        }
    }
}