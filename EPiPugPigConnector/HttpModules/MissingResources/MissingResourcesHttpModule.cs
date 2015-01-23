using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiPugPigConnector.HttpModules.MissingResources
{
    /// <summary>
    /// Offline manifest download gets interupted if a referenced file is missing
    /// Therefore if a manifest asset type of file is asked for and 404 (not found) or 500 (server error)
    /// is returned, override this and return 200 (OK) with an empty response.
    /// This avoids problems with manifest downloads.
    /// 
    /// Also if a manifest gets generated for /start.html and another manifest points to the same file
    /// the manifests must be cached to avoid;
    /// "Application Cache Error event: Manifest changed during update" error in the client browser.
    ///
    /// E.g. you shouldnt create two versions of the same manifest file before a page has finished downloading the assets...
    /// (its a cascading manifest downloading problem).
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
            var context = HttpContext.Current;
            string fileExtension = context.Request.CurrentExecutionFilePathExtension;
            if (context.Response.ContentType != "text/html")
            {
                if (IsManifestAssetFileExtension(fileExtension))
                {
                    if (context.Error.Message.Contains("404"))
                    {
                        context.Response.Clear();
                        context.Response.StatusCode = 200;
                        context.Response.AddHeader("OfflineManifestErrorOverride_OriginalErrorMessage", string.Format("Error: {0}", context.Error.Message));
                        context.ClearError();
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
                    context.Response.AddHeader("OfflineManifestErrorOverride_OriginalStatusCode", string.Format("StatusCode: {0}", context.Response.StatusCode));
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