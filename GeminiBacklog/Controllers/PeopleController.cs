using System.Text;
using System.Web.Http;
using GeminiBacklog.Controllers.DataAccess;
using System.Linq;
using System.Collections.Generic;

namespace GeminiBacklog.Controllers
{
    public class PeopleController : ApiController
    {
        readonly static IEnumerable<int> _userIds = DevUsers.Users.Where(u => !u.IsGroup).Select(u => u.UserId);
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
