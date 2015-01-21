using System;
using System.Collections.Generic;
using System.Linq;
using CsQuery;
using CsQuery.ExtensionMethods;
using EPiPugPigConnector.EPiExtensions;
using EPiServer.Core;
using EPiServer.XForms.WebControls;

namespace EPiPugPigConnector.HttpModules.Manifest
{
    public class RelativeUrlHtmlProcessor
    {
        private Uri currentUri;

        public RelativeUrlHtmlProcessor(Uri currentUri)
        {
            this.currentUri = currentUri;
        }

        public string ProcessHtml(string htmlDocument)
        {
            var urlsToCache = GetUrlsToCache(htmlDocument);
            ManifestCreator manifestCreator = new ManifestCreator();
            string manifest = manifestCreator.MakeManifestFileAsString(urlsToCache);

            return manifest;
        }

        private List<string> GetUrlsToCache(string htmlDocument)
        {
            /* 
            The cache manifest offline file for pugpig implementation
            requires links to be page relative.      
            The page http://epipugpigdemo.azurewebsites.net/about-us/management/ has this link element:
            
            <link href="/Static/css/bootstrap.css" rel="stylesheet" />
            Which is “website root relative”

            But what we really need is “truly” page relative url such as: (?)
            <link href="../../Static/css/bootstrap.css" rel="stylesheet" />

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
            //TODO: process all of the above type of links.

            //Using csQuery (jQuery like syntax)
            CQ htmlDom = htmlDocument;

            //htmlDom .Select("link")
            //    .Each(item => item.HasAttribute("href"))
            //    .Each(item => item.SetAttribute("href", GetPageRelativeUrl(item.GetAttribute("href"))));

            //process urls
            //using https://github.com/jamietre/CsQuery

            string result = string.Empty;
            var results = new List<string>();

            var cssLinkUrls = GetUrlsFrom(htmlDom, "link", "href");
            var scriptUrls = GetUrlsFrom(htmlDom, "script", "src");
            var anchorLinkUrls = GetAnchorLinkUrls(htmlDom);

            var imgUrls = GetUrlsFrom(htmlDom, "img", "src");

            results.Add(System.Environment.NewLine);
            results.Add("# CSS FILES:");
            results.AddRange(cssLinkUrls);
            
            results.Add(System.Environment.NewLine);
            results.Add("# JS FILES:");
            results.AddRange(scriptUrls);
            
            results.Add(System.Environment.NewLine);
            results.Add("# ANCHOR ELEMENT URLS:");
            results.AddRange(anchorLinkUrls);

            results.Add(System.Environment.NewLine);
            results.Add("# IMAGE FILES:");
            results.AddRange(imgUrls);
            results.Add(System.Environment.NewLine);
            
            return results;

            //return as string
            //return string.Join(System.Environment.NewLine, results.ToArray());
        }

        private List<string> GetAnchorLinkUrls(CQ htmlDom)
        {
            var links = GetUrlsFrom(htmlDom, "a", "href");
            links = links.Distinct().ToList();

            
            for (int index = 0; index < links.Count; index++)
            {
                var link = links[index];
                
                //fix startpage link
                if (link.Equals("/"))
                {
                    link = GetStartPageUrl();
                    links[index] = link;
                }

                //to .html links
                if (link.EndsWith("/"))
                {
                    link = link.TrimEnd(new[] { '/' });
                    link = string.Format("{0}.html", link);
                    links[index] = link;
                }

                //remove login link: /Util/login.aspx?ReturnUrl=/editions-root-page/the-edition-issue-1/startpage/article-page-1/
                if(link.ToLower().StartsWith("/util/login.aspx") ||
                   link.ToLower().EndsWith("/logout.html"))
                {
                    //remove 
                    links.RemoveAt(index);
                    if (index > 0)
                        index--;
                }
            }
            
            //TODO:
            //get .html based links:

            return links;
        }

        private string GetStartPageUrl()
        {
            var url = PageReference.StartPage.GetPage().URLSegment;
            return string.Format("/{0}/", url);
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