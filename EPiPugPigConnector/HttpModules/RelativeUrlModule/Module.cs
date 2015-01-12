using System;
using System.Web;
using StructureMap.Construction;

namespace EPiPugPigConnector.HttpModules.RelativeUrlModule
{
    public class Module : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += context_PreRequestHandlerExecute;
            context.PostRequestHandlerExecute += context_PostRequestHandlerExecute;
        }

        void context_PostRequestHandlerExecute(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.EnableHtmlUrlRewriteModule)
            {
                var httpApplication = (HttpApplication) sender;

                if (httpApplication.Response.ContentType.Equals("text/html"))
                {
                    //Process current html page with CustomHtmlStream:
                    httpApplication.Response.Filter = new CustomHtmlStream(
                        httpApplication.Response.Filter,
                        httpApplication.Request.Url);
                }
            }
        }

        private void context_PreRequestHandlerExecute(object sender, EventArgs e)
        {

        }

        public void Dispose()
        {

        }
    }
}