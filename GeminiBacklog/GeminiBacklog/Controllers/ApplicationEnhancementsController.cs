using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.Http;
using GeminiBacklog.Controllers.DataAccess;
using GeminiBacklog.Models;

namespace GeminiBacklog.Controllers
{
    public class ApplicationEnhancementsController : ApiController
    {
        static readonly string _sql = SqlQueries.GetSql("GeminiBacklog.Queries.ApplicationEnhancements.sql");

        public dynamic Get()
        {
            return new { Issues = new DBWrapper().Query<IssueModel>(_sql) };
        }
    }
}