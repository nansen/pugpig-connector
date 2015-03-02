using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CsQuery;
using EPiPugPigConnector.EPiExtensions;
using EPiPugPigConnector.Helpers;
using EPiPugPigConnector.Utils;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web;
using EPiServer.Web.Routing;

namespace EPiPugPigConnector.HttpModules.CustomHtml
{
    //From: http://patrickdesjardins.com/blog/modify-the-html-output-of-any-of-your-page-before-rendering
    public class CustomHtmlStream : Stream
    {
        private readonly Stream filter;
        private readonly MemoryStream cacheStream = new MemoryStream();
        private System.Uri currentRequestUri;

        public CustomHtmlStream(Stream filter, System.Uri currentRequestUri)
        {
            this.filter = filter;
            this.currentRequestUri = currentRequestUri;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            cacheStream.Write(buffer, 0, count);
        }

        public override void Flush()
        {
            if (cacheStream.Length > 0)
            {
                string htmlDocument = Encoding.UTF8.GetString(cacheStream.ToArray(), 0, (int)cacheStream.Length);

                //Modify html output here.
                htmlDocument = ModifyHtmlOutput(htmlDocument);

                var buffer = Encoding.UTF8.GetBytes(htmlDocument);
                this.filter.Write(buffer, 0, buffer.Length);
                cacheStream.SetLength(0);
            }

            this.filter.Flush();
        }

        private string ModifyHtmlOutput(string htmlDocument)
        {
            StopwatchTimer timer = new StopwatchTimer();

            //adds the .manifest src to the html element, (makes it easier to test the manifest in regular webbrowsers
            // as opposed to only testing via pugpig reader xml. )
            CQ cqDocument = new CQ(htmlDocument);
            var csQueryHtml = cqDocument.Select("html");

            if (csQueryHtml.Length > 0)
            {
                string manifestUrl = GetManifestUrl(currentRequestUri);
                csQueryHtml.Attr("manifest", manifestUrl);
            }

            ModifyHtmlWithRelativeAnchorLinkUrls(csQueryHtml);
            ModifyCssWithRelativeUrls(csQueryHtml);

            string resultHtml = cqDocument.Render();

            var timeElapsed = timer.Stop();

            return resultHtml;
        }

        private void ModifyHtmlWithRelativeAnchorLinkUrls(CQ htmlDom)
        {
            CQ select = htmlDom.Select("a");

            foreach (var item in select)
            {
                if (item.HasAttribute("href"))
                {
                    item.Attributes["href"] = GetHtmlRelativeUrl(item.Attributes["href"]);
                    item.Attributes["href"] = HtmlHelper.FriendlyUrlToUrlWithExtension(item.Attributes["href"], "html");
                }
            }
        }

        private void ModifyCssWithRelativeUrls(CQ htmlDom)
        {
            CQ select = htmlDom.Select("link");

            foreach (var item in select)
            {
                if (item.HasAttribute("href"))
                {
                    item.Attributes["href"] = GetHtmlRelativeUrl(item.Attributes["href"]);
                }
            }
        }

        private void Get(CQ htmlDom)
        {
            CQ select = htmlDom.Select("a");

            foreach (var item in select)
            {
                if (item.HasAttribute("href"))
                {
                    item.Attributes["href"] = GetHtmlRelativeUrl(item.Attributes["href"]);
                    item.Attributes["href"] = HtmlHelper.FriendlyUrlToUrlWithExtension(item.Attributes["href"], "html");
                }
            }
        }

        private string GetHtmlRelativeUrl(string url)
        {
            var abslouteUrl = UrlHelper.GetAbslouteUrl(url);
            var relativeUrl = UrlHelper.GetRelativeUrl(currentRequestUri.ToString(), abslouteUrl);

            return relativeUrl;
        }

        /// <summary>
        /// Return attribute value from element
        /// </summary>
        private List<string> GetUrlsFrom(CQ htmlDom, string elementName, string attributeName)
        {
            var resultList = new List<string>();
            CQ select = htmlDom.Select(elementName);

            foreach (var item in select)
            {
                if (item.HasAttribute(attributeName))
                {
                    resultList.Add(item.GetAttribute(attributeName));
                }
            }

            resultList = resultList.Distinct().ToList(); //remove duplicates
            return resultList;
        }

        private string GetManifestUrl(Uri requestUri)
        {
            string url = string.Empty;

            UrlBuilder urlBuilder = new UrlBuilder(currentRequestUri);

            return string.Format("{0}.manifest", urlBuilder.Path);

            //var pageReference = PageHelper.GetPageReferenceFromExternalUrl(currentRequestUri);
            //if (pageReference != null && PageReference.IsValue(pageReference))
            //{
            //    var page = pageReference.GetPage();
            //    if (page != null)
            //    {
            //        url = PageHelper.GetFriendlyUrlWithExtension(page, ".manifest");
            //    }
            //}
            //else
            //{

            //}
            //return url;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this.filter.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            this.filter.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this.filter.Read(buffer, offset, count);
        }

        public override bool CanRead
        {
            get { return this.filter.CanRead; }
        }

        public override bool CanSeek
        {
            get { return this.filter.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return this.filter.CanWrite; }
        }

        public override long Length
        {
            get { return this.filter.Length; }
        }

        public override long Position
        {
            get { return this.filter.Position; }
            set { this.filter.Position = value; }
        }
    }
}