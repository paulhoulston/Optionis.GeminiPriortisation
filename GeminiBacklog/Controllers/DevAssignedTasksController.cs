using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;

namespace GeminiBacklog.Controllers
{
    public class DevAssignedTasksController : ApiController
    {
        static readonly GetIssues _issueGetter = new GetIssues("GeminiBacklog.Queries.DevAssigned.sql");
        static readonly IEnumerable<int> _devUserIds = ConfigurationManager.AppSettings["GEMINI_DEV_USER_IDS"].Split(',').Select(id => int.Parse(id));

        [Route("devissues")]
        public dynamic Get()
        {
            return _issueGetter.Get(new { userIds = _devUserIds });
        }
    }
}