using Newtonsoft.Json.Converters;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace ClientWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
