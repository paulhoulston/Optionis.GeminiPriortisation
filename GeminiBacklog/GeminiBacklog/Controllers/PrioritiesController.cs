using System.IO;
using System.Reflection;
using System.Web.Http;
using GeminiBacklog.Controllers.DataAccess;
using GeminiBacklog.Models;

namespace GeminiBacklog.Controllers
{
    public class PrioritiesController : ApiController
    {
        static readonly string _sql;

        static PrioritiesController()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GeminiBacklog.Queries.DevPriorities.sql"))
            using (StreamReader reader = new StreamReader(stream))
            {
                _sql = reader.ReadToEnd();
            }
        }

        public dynamic Get()
        {
            return new { Issues = new DBWrapper().Query<IssueModel>(_sql) };
        }
    }
}