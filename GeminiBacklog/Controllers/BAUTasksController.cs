using System.Web.Http;

namespace GeminiBacklog.Controllers
{
    public class BAUTasksController : ApiController
    {
        static readonly GetIssues _issueGetter = new GetIssues("GeminiBacklog.Queries.BAUTasks.sql");

        public dynamic Get()
        {
            return _issueGetter.Get();
        }
    }
}