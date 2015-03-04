using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsQuery;
using EPiPugPigConnector.HtmlCrawlers.Interfaces;
using EPiPugPigConnector.HtmlCrawlers.Manifest.Models;

namespace EPiPugPigConnector.HtmlCrawlers.Manifest.Generator
{
    public class ManifestGenerator : IManifestGenerator
    {
        /* TODO: Validate the manifest. 
            See validator here: http://manifest-validator.com/
            Links:  
            http://www.html5rocks.com/en/tutorials/appcache/beginner/
            https://pugpig.zendesk.com/hc/en-us/articles/200948946-HTML-Manifest-Document-Type
            Debug offline manifest in chrome: look for errors in the console / under resources -> Application Cache.
         */

        private List<IManifestCrawler> _crawlers;

        public ManifestGenerator()
        {
            _crawlers = new List<IManifestCrawler>();
        }

        public IManifestGenerator AddCrawler(IManifestCrawler crawler)
        {
            _crawlers.Add(crawler);

            return this;
        }

        public void RemoveModifier(IManifestCrawler crawler)
        {
            _crawlers.Remove(crawler);
        }

        public string GenerateManifest(string htmlDocument)
        {
            var cqDocument = new CQ(htmlDocument);
            var linksUrls = new List<Asset>();

            foreach (var customHtmlStreamObserver in _crawlers)
            {
                linksUrls.AddRange(customHtmlStreamObserver.Crawl(cqDocument));
            }

            var offlineUrlsList = GenerateManifestAssets(linksUrls);

            var manifestFile = MakeManifestFileAsString(offlineUrlsList);

            return manifestFile;
        }

        private List<string> GenerateManifestAssets(List<Asset> linksUrls)
        {
            var results = new List<string>();

            results.Add(System.Environment.NewLine);
            results.Add("# CSS FILES:");
            results.AddRange(linksUrls.Where(i => i.Element == "link").Select(x => x.Url));

            results.Add(System.Environment.NewLine);
            results.Add("# JS FILES:");
            results.AddRange(linksUrls.Where(i => i.Element == "script").Select(x => x.Url));

            results.Add(System.Environment.NewLine);
            results.Add("# IMAGE FILES:");
            results.AddRange(linksUrls.Where(i => i.Element == "img").Select(x => x.Url));

            results.Add(System.Environment.NewLine);

            return results;
        }

        /// <summary>
        /// Create a string to be saved in the Manifest file
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string MakeManifestFileAsString(List<string> offlineUrlsList)
        {
            StringBuilder strb = new StringBuilder();
            strb.Append("CACHE MANIFEST" + Environment.NewLine);
            strb.AppendFormat("# This file was generated at {0}" + Environment.NewLine, DateTime.Now.ToString("o"));
            strb.Append("#CACHE:" + Environment.NewLine);

            foreach (string filePath in offlineUrlsList)
            {
                strb.AppendFormat("{0}" + Environment.NewLine, filePath);
            }

            strb.Append("#NETWORK:" + Environment.NewLine);
            strb.Append("#*" + Environment.NewLine);

            return strb.ToString();
        }
    }
}
