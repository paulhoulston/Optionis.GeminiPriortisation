using GeminiBacklog.Controllers.DataAccess;
using GeminiBacklog.Models;
using System.Web.Http;

namespace GeminiBacklog.Controllers
{
    public class UatItemsController : ApiController
    {
        readonly DBWrapper _dbWrapper = new DBWrapper();

        static readonly string _sql = SqlQueries.GetSql("GeminiBacklog.Queries.ItemsInUAT.sql");

        public dynamic Get()
        {
            return new { Issues = new DBWrapper().Query<IssueModel>(_sql) };
        }
    }
}