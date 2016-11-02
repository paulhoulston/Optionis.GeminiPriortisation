using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;

namespace GeminiBacklog.Controllers
{
    public class DevAssignedTasksController : ApiController
    {
        const string ROUTE = "devissues";
        static readonly GetIssues _issueGetter = new GetIssues("GeminiBacklog.Queries.DevAssigned.sql");
        static readonly IEnumerable<int> _devUserIds = ConfigurationManager.AppSettings["GEMINI_DEV_USER_IDS"].Split(',').Select(id => int.Parse(id));

        [Route(ROUTE)]
        public dynamic Get(string filter = "")
        {
            return _issueGetter.Get(ROUTE, new { userIds = _devUserIds, filter });
        }
    }
}