using System;
using System.Web.Http;

namespace GeminiBacklog.Controllers
{
    public class MetricsController : ApiController
    {
        public dynamic Get()
        {
            var last1Months = DateTime.Today.AddMonths(-1);
            var last3Months = DateTime.Today.AddMonths(-3);
            var last6Months = DateTime.Today.AddMonths(-6);
            return
                new
                {
                    reopenedIssues = new
                    {
                        last1Months = new
                        {
                            count = new ReopenedIssuesController().Get(last1Months).count,
                            uri = ReopenedIssuesController.Route(last1Months)
                        },
                        last3Months = new
                        {
                            count = new ReopenedIssuesController().Get(last3Months).count,
                            uri = ReopenedIssuesController.Route(last3Months)
                        },
                        last6Months = new
                        {
                            count = new ReopenedIssuesController().Get(last6Months).count,
                            uri = ReopenedIssuesController.Route(last6Months)
                        }
                    }
                };
        }
    }
}