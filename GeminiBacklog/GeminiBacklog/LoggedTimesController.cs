using System.Web.Http;

namespace GeminiBacklog
{
    public class LoggedTimesController : ApiController
    {
        [Route("loggedtimes/{userId}")]
        public dynamic Get(int userId)
        {
            return new[]
            {
                userId
            };
        }
    }
}