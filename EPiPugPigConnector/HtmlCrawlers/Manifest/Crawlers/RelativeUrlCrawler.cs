using System;
using System.Collections.Generic;
using System.Linq;
using CsQuery;
using EPiPugPigConnector.Helpers;
using EPiPugPigConnector.HtmlCrawlers.Interfaces;
using EPiPugPigConnector.HtmlCrawlers.Manifest.Models;

namespace EPiPugPigConnector.HtmlCrawlers.Manifest.Crawlers
{
    public class RelativeUrlCrawler : IManifestCrawler
    {
        private readonly Uri _currentRequestUri;
        private readonly string _element;
        private readonly string _attribute;

        public RelativeUrlCrawler(Uri currentRequestUri, string element, string attribute)
        {
            _currentRequestUri = currentRequestUri;
            _element = element;
            _attribute = attribute;
        }

        public virtual List<Asset> Crawl(CQ htmlDocument)
        {
            var urls = GetUrlsFrom(htmlDocument);
            var manifestRelativeUrls = new List<Asset>();

            foreach (var url in urls)
            {
                var abslouteUrl = UrlHelper.GetAbslouteUrl(url);
                manifestRelativeUrls.Add(
                    new Asset
                    {
                        Url = UrlHelper.GetRelativeUrl(_currentRequestUri.ToString(), abslouteUrl),
                        Element = _element
                    }
                );
            }

            return manifestRelativeUrls;
        }

        /// <summary>
        /// Return attribute value from element
        /// </summary>
        protected List<string> GetUrlsFrom(CQ htmlDom)
        {
            var resultList = new List<string>();
            CQ select = htmlDom.Select(_element);

            foreach (var item in select)
            {
                if (item.HasAttribute(_attribute))
                {
                    resultList.Add(item.GetAttribute(_attribute));
                }
            }

            resultList = resultList.Distinct().ToList(); //remove duplicates
            return resultList;
        }
    }
}
