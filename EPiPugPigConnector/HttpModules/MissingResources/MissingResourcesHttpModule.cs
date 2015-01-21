using System;
using System.Collections.Generic;
using System.Linq;
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
            string fileExtension = HttpContext.Current.Request.CurrentExecutionFilePathExtension;

            if (HttpContext.Current.Response.ContentType != "text/html")
            {
                if (IsManifestAssetFileExtension(fileExtension))
                {
                    if (HttpContext.Current.Error.Message.Contains("404"))
                    {

                    }
                }
            }
        }

        private bool IsManifestAssetFileExtension(string fileExtension)
        {
            fileExtension = fileExtension.ToLower();
            var validFileExtensions = new List<string>()
            {
                ".png", ".jpg", ".jpeg", "gif", ".css", ".js"
            };

            return validFileExtensions.Any(extension => extension.Equals(fileExtension));
        }

        void context_EndRequest(object sender, EventArgs e)
        {

            //returns 200 instead of errors on files reqeusted by .html pages.
            //To prevent offline manifest creation from failing (one single asset file failing and the offline manifest
            //download proccess is stopped.
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