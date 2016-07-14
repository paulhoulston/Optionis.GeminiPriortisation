using System.IO;
using System.Reflection;

namespace GeminiBacklog.Controllers.DataAccess
{
    class SqlQueries
    {
        public static string GetSql(string resourceName)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}