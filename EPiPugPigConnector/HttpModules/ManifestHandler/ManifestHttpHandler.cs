using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EPiPugPigConnector.HttpModules.ManifestHandler
{
    public class ManifestHttpHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            if (context.Request.RawUrl.Contains(".cspx"))
            {
                string newUrl = context.Request.RawUrl.Replace(".cspx", ".aspx");
                context.Server.Transfer(newUrl);
            }
        }
    }
}
