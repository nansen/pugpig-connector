using System.Web.Mvc;
using EPiPugPigConnector.HttpModules.RelativeUrlModule;

namespace EPiPugPigConnector.HttpModules
{
    public class ToRelativeUrlActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //pugpig todo: the action migt not be allowed on all pages - gets error in episerver Alloy MVC demo page 
            //using a module for now instead. also makes its reusable for web form based sites as well.

            //var response = filterContext.HttpContext.Response;
            //if (response.ContentType == "text/html")
            //{
            //    response.Filter = new CustomHtmlStream(filterContext.HttpContext.Response.Filter, filterContext.RequestContext.HttpContext.Request.Url);
            //}
        }
    }
}