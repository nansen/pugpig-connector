using System;
using System.Web;

namespace EPiPugPigConnector.HttpModules.MissingResources
{
    /// <summary>
    /// Offline manifest download gets interupted if a referenced file is missing
    /// Therefore even though the file requested for is not not found just server 200 ok
    /// To avoid problems with manifest downloads.
    /// </summary>
    public class MissingResourcesHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += context_PreRequestHandlerExecute;
            context.PostRequestHandlerExecute += context_PostRequestHandlerExecute;

            context.BeginRequest += context_BeginRequest;
            context.EndRequest += context_EndRequest;

            context.Error += context_Error;
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
        }

        void context_Error(object sender, EventArgs e)
        {

        }

        void context_EndRequest(object sender, EventArgs e)
        {
            var httpApplication = (HttpApplication)sender;
            var context = httpApplication.Context;

            if (httpApplication.Request.RawUrl.ToLower().EndsWith(".html"))
            { 
                if (context.Response.StatusCode == 404 || context.Response.StatusCode == 500)
                {
                    context.Response.Clear();
                    context.Response.StatusCode = 200;
                    context.Response.AddHeader("OfflineManifestErrorOverride_originalErrorCode", "404 not found");
                }
            }
        }

        void context_PostRequestHandlerExecute(object sender, EventArgs e)
        {

        }

        private void context_PreRequestHandlerExecute(object sender, EventArgs e)
        {

        }

        public void Dispose()
        {

        }
    }
}