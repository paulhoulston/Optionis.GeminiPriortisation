using System.Web.Http;

namespace GeminiBacklog.Controllers
{
    public class BAUTasksController : ApiController
    {
        const string ROUTE = "bautasks";
        static readonly GetIssues _issueGetter = new GetIssues("GeminiBacklog.Queries.BAUTasks.sql");

        [Route(ROUTE)]
        public dynamic Get(string filter = "")
        {
            return _issueGetter.Get(ROUTE, new { filter });
        }
    }
}