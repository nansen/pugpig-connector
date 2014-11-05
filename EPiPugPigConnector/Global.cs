namespace EPiPugPigConnector
{
    public class Global : EPiServer.Global
    {
        protected override void RegisterRoutes(System.Web.Routing.RouteCollection routes)
        {
            base.RegisterRoutes(routes);

            RouteConfig.RegisterRoutes(routes);
        }
    }
}
