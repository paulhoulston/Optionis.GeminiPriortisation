using GeminiBacklog.Controllers.DataAccess;
using System.Collections.Generic;

namespace GeminiBacklog
{
    class DevUsers
    {
        public static readonly IEnumerable<UserIdModel> Users;

        static DevUsers()
        {
            Users =
                new DBWrapper()
                    .Query<UserIdModel>(SqlQueries.GetSql("GeminiBacklog.Queries.GetDevUserIds.sql"));
        }

        public class UserIdModel
        {
            public int UserId { get; set; }
            public bool IsGroup { get; set; }
        }
    }
}