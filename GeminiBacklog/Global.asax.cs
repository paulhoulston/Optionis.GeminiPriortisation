using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace GeminiBacklog
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Formatters.OfType<JsonMediaTypeFormatter>().First().SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}