using System.IO;
using System.Reflection;
using System.Web.Http;
using GeminiBacklog.Controllers.DataAccess;
using GeminiBacklog.Models;

namespace GeminiBacklog.Controllers
{
    public class IssuesController : ApiController
    {
        static readonly string _sql;

        static IssuesController()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GeminiBacklog.Queries.AssignedIssues.sql"))
            using (StreamReader reader = new StreamReader(stream))
            {
                _sql = reader.ReadToEnd();
            }
        }

        [Route("issues/{userId}")]
        public dynamic Get(int userId)
        {
            return new { Issues = new DBWrapper().Query<IssueModel>(_sql, new { userId }) };
        }
    }
}