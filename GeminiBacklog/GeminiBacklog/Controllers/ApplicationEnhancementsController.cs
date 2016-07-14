using System.IO;
using System.Reflection;
using System.Web.Http;
using GeminiBacklog.Controllers.DataAccess;
using GeminiBacklog.Models;

namespace GeminiBacklog.Controllers
{
    public class ApplicationEnhancementsController : ApiController
    {
        static readonly string _sql;

        static ApplicationEnhancementsController()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GeminiBacklog.Queries.ApplicationEnhancements.sql"))
            using (StreamReader reader = new StreamReader(stream))
            {
                _sql = reader.ReadToEnd();
            }
        }

        // GET api/<controller>
        public dynamic Get()
        {
            return new { Issues = new DBWrapper().Query<IssueModel>(_sql) };
        }
    }

}