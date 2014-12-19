using CsQuery;
using System;
using System.Linq;

namespace EPiPugPigConnector.HttpModules.RelativeUrlModule
{
    public class UrlToRelativeUrlHtmlProcessor
    {
        private Uri currentUri;

        public UrlToRelativeUrlHtmlProcessor(Uri currentUri)
        {
            this.currentUri = currentUri;
        }

        public string ProcessHtml(string htmlDocument)
        {
            /* 
            The cache manifest offline file for pugpig implementation
            requires links to be page relative.      
            The page http://epipugpigdemo.azurewebsites.net/about-us/management/ has this link element:
            
            <link href="/Static/css/bootstrap.css" rel="stylesheet" />
            Which is “website root relative”

            But what we really need is “truly” page relative url such as: (?)
            <link href="../../Static/css/bootstrap.css" rel="stylesheet" />
            
            Todo:
            Example html urls to modify            
            <link href="/bundles/css?v=neAKKJhkN8unX8oaCEJV_AOx1CDqZRrjvSvYRkaYF-U1" rel="stylesheet"/>
            <script src="/bundles/js?v=5x_bEnqiEO2lzoxxfpj__tyxcOY5c0JQS8e_OpUn4XQ1"></script>
            <a href="/en/" title="Alloy - collaboration, communication and project management online">
            <img src="/contentassets/61acc0883e6c47f7bd0a81c6fd36cfbc/logotype.png"/>
            <form action="/en/search/" method="get">           
             
            The above urls needs to be page relative e.g. depending on where the html page "lives" relative to webroot
            in order to make the offline manifest work.

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

            //process urls
            //using https://github.com/jamietre/CsQuery

            htmlDom
                .Select("link")
                .Each(item => item.HasAttribute("href"))
                .Each(item => item.SetAttribute("href", GetPageRelativeUrl(item.GetAttribute("href"))));
            
            //return processed html
            return htmlDom.Render();
        }

        private string GetPageRelativeUrl(string inputUrl)
        {
            /* 
            inputUrl could have the following forms:

            http://www.test.com/assets/css/main.css?id=test // absolute
            ./assets/css/main.css                           // relative with .
            ../../../assets/css/main.css                    // relative with ..
            /assets/css/main.css?id=test                    // file extension
            /assets/css/mainbundle?id=test                  // extensionless
            /assets/css/mainbundle/?id=test                 // extensionless with end path char ('/')            
            */

            string resultUrl = inputUrl;
            string prefix = GetCurrentUrlPrefixString();

            //todo: do stuff here.
            //todo remove beginning "/" if present:
            resultUrl = resultUrl.TrimEnd(new char['/']);
            resultUrl = string.Format("{0}{1}", prefix, resultUrl);

            return resultUrl;
        }

        private string GetCurrentUrlPrefixString()
        {
            string resultPrefix = string.Empty;
            string prefixPerDepth = "../";
            int prefixDepth = currentUri.AbsolutePath.Count(x => x == '/');

            //if at webroot level but with no ending "/" count as 1 still.
            if (prefixDepth == 0)
                prefixDepth = 1;

            for (int i = 0; i < prefixDepth; i++)
            {
                resultPrefix += prefixPerDepth;
            }
            return resultPrefix;
        }
    }
}