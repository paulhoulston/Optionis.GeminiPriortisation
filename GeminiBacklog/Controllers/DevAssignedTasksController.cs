using GeminiBacklog.Models;
using System.Web.Http;

namespace GeminiBacklog.Controllers
{
    public class DevAssignedTasksController : ApiController
    {
        [Route("people/devissues")]
        public dynamic Get()
        {
            return new { Issues = new BAUTaskModel[] { } };
        }
    }
}