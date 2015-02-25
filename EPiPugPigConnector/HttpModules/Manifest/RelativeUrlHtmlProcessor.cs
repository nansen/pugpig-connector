using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using CsQuery;
using CsQuery.ExtensionMethods;
using EPiPugPigConnector.EPiExtensions;
using EPiPugPigConnector.Helpers;
using EPiPugPigConnector.WebClients;
using EPiServer;
using EPiServer.Core;
using EPiServer.Licensing.Services;

namespace EPiPugPigConnector.HttpModules.Manifest
{
    public class RelativeUrlHtmlProcessor
    {
        private IWebClient _webClient;

        public RelativeUrlHtmlProcessor(IWebClient webClient)
        {
            _webClient = webClient;
        }

        public string ProcessHtml(string htmlDocument)
        {
            var urlsToCache = GetAssetUrlsForManifest(htmlDocument);
            ManifestCreator manifestCreator = new ManifestCreator();
            string manifest = manifestCreator.MakeManifestFileAsString(urlsToCache);

            return manifest;
        }

        private List<string> GetAssetUrlsForManifest(string htmlDocument)
        {
            /*
            Use 3rd party components like HtmlAgility pack or CsQuery 
            to filter out a collection of elements that contains link attributes and modify those.
            e.g. a, img, link, script etc
             * 
             * link href
             * script src
             * a href
             * img src
             * form action (?)
            */

            //Processing urls using CsQuery (jQuery like syntax) for parsing html easily - https://github.com/jamietre/CsQuery
            CQ htmlDom = htmlDocument;

            string result = string.Empty;
            var results = new List<string>();

            var cssLinkUrls = GetUrlsFrom(htmlDom, "link", "href");
            var scriptUrls = GetUrlsFrom(htmlDom, "script", "src");
            var anchorLinkUrls = GetAnchorLinkUrls(htmlDom);

            var imgUrls = GetUrlsFrom(htmlDom, "img", "src");
            var cssImageUrls = GetUrlsFromCssFiles(cssLinkUrls);

            results.Add(System.Environment.NewLine);
            results.Add("# CSS FILES:");
            results.AddRange(cssLinkUrls);
            
            results.Add(System.Environment.NewLine);
            results.Add("# JS FILES:");
            results.AddRange(scriptUrls);
            
            //results.Add(System.Environment.NewLine);
            //results.Add("# ANCHOR ELEMENT URLS:");
            //results.AddRange(anchorLinkUrls);

            results.Add(System.Environment.NewLine);
            results.Add("# IMAGE FILES:");
            results.AddRange(imgUrls);
            results.AddRange(cssImageUrls);
            results.Add(System.Environment.NewLine);
            
            return results;
        }

        private List<string> GetAnchorLinkUrls(CQ htmlDom)
        {
            var links = GetUrlsFrom(htmlDom, "a", "href");
            links = links.Distinct().ToList();
            
            for (int index = 0; index < links.Count; index++)
            {
                var link = links[index];
                
                //to .html links
                if (link.EndsWith("/"))
                {
                    link = HtmlHelper.FriendlyUrlToUrlWithExtension(link, "html");
                    links[index] = link;
                }

                ////remove login link: /Util/login.aspx?ReturnUrl=/editions-root-page/the-edition-issue-1/startpage/article-page-1/
                //if(link.ToLower().StartsWith("/util/login.aspx") ||
                //   link.ToLower().EndsWith("/logout.html"))
                //{
                //    //remove 
                //    links.RemoveAt(index);
                //    if (index > 0)
                //        index--;
                //}
                
                //remove .aspx links
                var uriLink = new EPiServer.UrlBuilder(link);
                if (uriLink.Path.ToLower().EndsWith(".aspx"))
                {
                    //remove 
                    links.RemoveAt(index);
                    if (index > 0)
                    {
                        index--;
                    }
                }
            }

            return links;
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


        #region Get accessts from css flies (needs to be refactored to live elsewhere!)
        private List<string> GetUrlsFromCssFiles(List<string> cssFiles)
        {
            var cssAssets = new List<string>();

            var manifestFilePath = UrlHelper.GetAbslouteUrl(""); //currentUri.ToString();
            foreach (var cssFile in cssFiles)
            {
                var abslouteCssFilePath = UrlHelper.GetAbslouteUrl(cssFile);

                var cssAssetProcessor = new CssAssetProcessor(_webClient, manifestFilePath, abslouteCssFilePath);
                var relativeAsset = cssAssetProcessor.ProcessCssFile();

                cssAssets.AddRange(relativeAsset);
            }
            
            return cssAssets;
        }
        #endregion

        //private string GetPageRelativeUrl(string inputUrl)
        //{
        //    /* 
        //    inputUrl could have the following forms:

        //    http://www.test.com/assets/css/main.css?id=test // absolute
        //    ./assets/css/main.css                           // relative with .
        //    ../../../assets/css/main.css                    // relative with ..
        //    /assets/css/main.css?id=test                    // file extension
        //    /assets/css/mainbundle?id=test                  // extensionless
        //    /assets/css/mainbundle/?id=test                 // extensionless with end path char ('/')            
        //    */

        //    string resultUrl = inputUrl;
        //    string prefix = GetCurrentUrlPrefixString();

        //    //todo: do stuff here.
        //    //todo remove beginning "/" if present:
        //    resultUrl = resultUrl.TrimEnd(new char['/']);
        //    resultUrl = string.Format("{0}{1}", prefix, resultUrl);

        //    return resultUrl;
        //}

        //private string GetCurrentUrlPrefixString()
        //{
        //    string resultPrefix = string.Empty;
        //    string prefixPerDepth = "../";
        //    int prefixDepth = currentUri.AbsolutePath.Count(x => x == '/');

        //    //if at webroot level but with no ending "/" count as 1 still.
        //    if (prefixDepth == 0)
        //        prefixDepth = 1;

        //    for (int i = 0; i < prefixDepth; i++)
        //    {
        //        resultPrefix += prefixPerDepth;
        //    }
        //    return resultPrefix;
        //}


    }
}