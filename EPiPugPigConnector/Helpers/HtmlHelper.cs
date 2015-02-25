using System;
using System.Linq;
using System.Web;
using EPiPugPigConnector.EPiExtensions;
using EPiServer;
using EPiServer.Core;

namespace EPiPugPigConnector.Helpers
{
    public static class HtmlHelper
    {
        ///// <summary>
        ///// Takes a relative link such as "/" or "/edition1/firstpage/" and returns 
        ///// adds a extension to the url "/start.html" (if its a episerver startpage) or "/edition1/firstpage.html"
        ///// </summary>
        ///// <param name="link"></param>
        ///// <param name="extension"></param>
        ///// <returns></returns>
        //public static string RelativeLinkToHtmlLink(string link, string extension)
        //{
        //    //UrlBuilder urlBuilder = new UrlBuilder(link);

        //    if (link.Equals("/"))
        //    {
        //        //Is startpage, special fix:
        //        link = GetStartPageUrl();
        //    }

        //    if (link.EndsWith("/"))
        //    {
        //        link = link.TrimEnd(new[] {'/'});
        //        link = String.Format("{0}.{1}", link, extension);
        //    }
        //    return link;
        //}

        private static string GetStartPageUrl()
        {
            string url = string.Empty;
            if (System.Web.HttpContext.Current != null)
            {
                url = PageReference.StartPage.GetPage().URLSegment;
            }
            else
            {
                //called from test project
                url = "start";
            }
            
            return String.Format("/{0}/", url);
        }

        /// <summary>
        /// Takes an absolute "http://mysite/edition/page/" or relative link "/edition/firstpage/" and adds an extension to the url.
        /// Special handling if EPiServer startpage. ("http://mysite/" or "/").
        /// </summary>
        /// <param name="inputUrl">An abslute or relative link such as "/" or "/edition/firstpage/"</param>
        /// <param name="extension">For example "html"</param>
        /// <returns>The url with the added extension</returns>
        public static string FriendlyUrlToUrlWithExtension(string inputUrl, string extension)
        {
            string resultUrl = inputUrl;

            UrlBuilder urlBuilder = new UrlBuilder(inputUrl);
            string path = urlBuilder.Path;

            if (path.Equals("/"))
            {
                //Is startpage, special fix:
                urlBuilder.Path = GetStartPageUrl();
                //Update inputurl:
                inputUrl = urlBuilder.ToString();
            }

            if (string.IsNullOrEmpty(urlBuilder.Query))
            {
                resultUrl = AddExtensionToUrlString(inputUrl, extension);
            }
            else
            {
                //has querystring(s)

                //just add .html to the path?
                urlBuilder.Path = AddExtensionToUrlString(path, extension);
                resultUrl = urlBuilder.ToString();
            }

            return resultUrl;
        }

        private static string AddExtensionToUrlString(string inputUrl, string extension)
        {
            if (inputUrl.EndsWith("/"))
            {
                inputUrl = inputUrl.TrimEnd(new[] {'/'});
                inputUrl = String.Format("{0}.{1}", inputUrl, extension);
            }
            return inputUrl;
        }
    }
}