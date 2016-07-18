using System.Web.Http;

namespace GeminiBacklog.Controllers
{
    public class SiteVersionController : ApiController
    {
        public class VersionModel
        {
            public string Version { get; set; }
        }

        public VersionModel Get()
        {
            return new VersionModel
            {
                Version = GetType().Assembly.GetName().Version.ToString()
            };
        }
    }
}