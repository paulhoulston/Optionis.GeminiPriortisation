using System.Web.Http;

namespace GeminiBacklog.Controllers
{
    public class ApplicationEnhancementsController : ApiController
    {
        const string ROUTE = "applicationenhancements";
        static readonly GetIssues _issueGetter = new GetIssues("GeminiBacklog.Queries.ApplicationEnhancements.sql");

        [Route(ROUTE)]
        public dynamic Get(string filter = "")
        {
            return _issueGetter.Get(ROUTE, new { filter });
        }
    }
}