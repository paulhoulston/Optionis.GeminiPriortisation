using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Http;
using GeminiBacklog.Controllers.DataAccess;

namespace GeminiBacklog.Controllers
{
    public class PeopleController : ApiController
    {
        readonly static int[] _userIds = ConfigurationManager.AppSettings["GEMINI_USER_IDS"].Split(',').Select(value => int.Parse(value)).ToArray();
        readonly static string _sql = "SELECT userid AS GeminiId, firstname + ' ' + surname AS Name FROM dbo.users WHERE userid in (@userIds)";

        public dynamic Get()
        {
            var userIds = new StringBuilder();
            foreach (var id in _userIds)
                userIds.AppendFormat("'{0}',", id);
            var sql = _sql.Replace("@userIds", userIds.ToString().TrimEnd(','));

            return new { people = new DBWrapper().Query<Person>(sql) };
        }
        
        class Person
        {
            public int GeminiId { get; set; }
            public string Name { get; set; }
        }
    }
}
