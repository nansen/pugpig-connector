using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EPiPugPigConnector.Editions
{
    public static class UrlHelper
    {
        public static string GetCurrentDomainRootUrl()
        {
            var request = HttpContext.Current.Request;

            string rootUrl = String.Format("{0}{1}{2}{3}",
                request.Url.Scheme,
                Uri.SchemeDelimiter,
                request.Url.Host,
                (request.Url.IsDefaultPort ? "" : ":" + request.Url.Port));

            return rootUrl;
        }
    }
}
