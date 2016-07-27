using System.Web.Http;

namespace GeminiBacklog.Controllers
{
    public class ApplicationEnhancementsController : ApiController
    {
        static readonly GetIssues _issueGetter = new GetIssues("GeminiBacklog.Queries.ApplicationEnhancements.sql");

        public dynamic Get()
        {
            return _issueGetter.Get();
        }
    }
}