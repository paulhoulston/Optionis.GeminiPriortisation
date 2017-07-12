using System.Linq;
using System.Web.Http;

namespace GeminiBacklog.Controllers
{
    public class DevAssignedTasksController : ApiController
    {
        const string ROUTE = "devissues";
        static readonly GetIssues _issueGetter = new GetIssues("GeminiBacklog.Queries.DevAssigned.sql");
        
        [Route(ROUTE)]
        public dynamic Get(string filter = "")
        {
            return DevAssignedTasksController._issueGetter.Get(ROUTE, new { userIds = DevUsers.Users.Select(u => u.UserId), filter });
        }
    }
}