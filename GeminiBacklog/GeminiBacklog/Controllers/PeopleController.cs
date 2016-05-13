using System.Web.Http;

namespace GeminiBacklog.Controllers
{
    public class PeopleController : ApiController
    {
        public dynamic Get()
        {
            return new
            {
                people = new[]
                {
                    new Person { GeminiId = 81, Name = "Paul Houlston" },
                    new Person { GeminiId = 33, Name = "Ross Cooper" },
                    new Person { GeminiId = 54, Name = "Simon Aspinall" },
                }
            };
        }

        class Person
        {
            public int GeminiId { get; set; }
            public string Name { get; set; }
        }
    }
}
