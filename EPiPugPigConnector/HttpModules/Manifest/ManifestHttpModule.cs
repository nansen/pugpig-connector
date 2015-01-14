using System;
using System.Web;

namespace EPiPugPigConnector.HttpModules.Manifest
{
    class ManifestHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += context_BeginRequest;
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            var httpApplication = (HttpApplication)sender;

            if (httpApplication.Request.RawUrl.ToLower().EndsWith(".manifest"))
            {
                //todo: genereate manifest for current episerver page.
                // the url without the .manifest ending should point to the episerver page.
                // the url with the .html ending should point to a RelativeUrl processed 
                // (custom logic for manifest urls) episerver page.

                var test = 1;
            }
        }

        public void Dispose()
        {

        }
    }
}
