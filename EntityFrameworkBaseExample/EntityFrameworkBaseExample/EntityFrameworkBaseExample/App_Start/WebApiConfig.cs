using EntityFrameworkBaseExample.Filter;
using System.Web.Http;

namespace EntityFrameworkBaseExample.App_Start
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new ExecutionTimeFilter());
        }
    }
}