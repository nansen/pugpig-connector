using System.Web.Mvc;
using System.Web.Routing;

namespace EPiPugPigConnector
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("editions.xml", "editions.xml", new { controller = "Editions", action = "Editions" });
            routes.MapRoute("edition.xml", "edition{id}.xml", new {controller = "Editions", action = "Edition"});
        }
    }
}
