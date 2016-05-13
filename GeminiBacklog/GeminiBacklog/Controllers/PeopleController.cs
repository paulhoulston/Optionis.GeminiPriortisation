using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Http;
using Dapper;

namespace GeminiBacklog.Controllers
{
    public class PeopleController : ApiController
    {
        readonly static int[] _userIds = ConfigurationManager.AppSettings["GEMINI_USER_IDS"].Split(',').Select(value => int.Parse(value)).ToArray();
        static readonly string _connectionString = ConfigurationManager.ConnectionStrings["Gemini"].ConnectionString;
        readonly static string _sql = "SELECT userid AS GeminiId, firstname + ' ' + surname AS Name FROM dbo.users WHERE userid in (@userIds)";

        public dynamic Get()
        {
            IEnumerable<Person> people = null;
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                var userIds = new StringBuilder();
                foreach(var id in _userIds)
                    userIds.AppendFormat("'{0}',", id);
                var sql = _sql.Replace("@userIds", userIds.ToString().TrimEnd(','));

                sqlConnection.Open();
                people = sqlConnection.Query<Person>(sql);
                sqlConnection.Close();
            }

            return new { people = people };
        }
        
        class Person
        {
            public int GeminiId { get; set; }
            public string Name { get; set; }
        }
    }
}
