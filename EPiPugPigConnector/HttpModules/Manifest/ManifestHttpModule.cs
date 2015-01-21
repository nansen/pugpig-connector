﻿using System;
using System.Web;

namespace EPiPugPigConnector.HttpModules.Manifest
{
    class ManifestHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.EndRequest += context_EndRequest;
            context.PostRequestHandlerExecute += context_PostRequestHandlerExecute;
        }

        void context_PostRequestHandlerExecute(object sender, EventArgs e)
        {
            var httpApplication = (HttpApplication)sender;

            if (httpApplication.Request.RawUrl.ToLower().EndsWith(Constants.MANIFEST_FILE_EXTENSION))
            {
                //Process current html page with CustomHtmlStream:
                httpApplication.Response.Filter = new CustomHtmlStream(httpApplication.Response.Filter, httpApplication.Request.Url);
                
                var context = httpApplication.Context;
                context.Response.StatusCode = 200;
                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                context.Response.AddHeader("Content-Type", "text/cache-manifest");
            }
        }

        void context_EndRequest(object sender, EventArgs e)
        {

            
        }

        public void Dispose()
        {

        }
    }
}
