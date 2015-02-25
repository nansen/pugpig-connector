using System;
using System.Web;

namespace EPiPugPigConnector.Helpers
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

        public static string GetAbslouteUrl(string relativeUrl)
        {
            return string.Format("{0}/{1}", GetCurrentDomainRootUrl(), relativeUrl);
        }
    }
}
