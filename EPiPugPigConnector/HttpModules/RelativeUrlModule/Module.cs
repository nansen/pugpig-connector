using System;
using System.Web;

namespace EPiPugPigConnector.HttpModules.RelativeUrlModule
{
    public class Module : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += context_PreRequestHandlerExecute;
        }

        private void context_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            var httpApplication = (HttpApplication)sender;
            if (httpApplication.Response.ContentType != "text/html")
                return; //exit early if file type should not be modified. TODO: add that only *.xml endings should be processed.

            httpApplication.Response.Clear();
            httpApplication.Response.Filter = new CustomHtmlStream(httpApplication.Response.Filter, httpApplication.Request.Url);
        }

        public void Dispose()
        {

        }
    }
}