using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EPiServer;
using EPiServer.Core;
using EPiServer.Globalization;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using EPiServer.Web;

namespace EPiPugPigConnector.EPiExtensions
{
    public static class PageUrlExtensions
    {
        ////This method is provided for situations where you want a clean external URL, even when in CMS edit mode
        //// e.g. jQuery, Form actions, etc.
        //private static string GetForcedExternalUrl(PageReference pageLink, string lang)
        //{
        //    RequestContext requestContext = new RequestContext();

        //    requestContext.RouteData = new RouteData();
        //    requestContext.RouteData.DataTokens.Add("contextmode", ContextMode.Default);

        //    var routeValueDictionary = new RouteValueDictionary();
        //    var urlResolver = new UrlResolver();

        //    //temporarily set the request context to null
        //    var contextSaved = HttpContext.Current;
        //    HttpContext.Current = null;

        //    var url = urlResolver.GetVirtualPath(new ContentReference(pageLink.ID), lang, routeValueDictionary, requestContext);

        //    //now set it back
        //    HttpContext.Current = contextSaved;

        //    return url.GetUrl();
        //}
    }
}
