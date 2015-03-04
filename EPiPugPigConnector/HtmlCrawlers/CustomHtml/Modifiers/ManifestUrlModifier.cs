using System;
using CsQuery;
using EPiPugPigConnector.HtmlCrawlers.Interfaces;
using EPiServer;

namespace EPiPugPigConnector.HtmlCrawlers.CustomHtml.Modifiers
{
    /// <summary>
    /// Adds the manifest url to the html tag.
    /// </summary>
    public class ManifestUrlModifier : IHtmlModifier
    {
        private readonly Uri _currentRequestUri;

        public ManifestUrlModifier(Uri currentRequestUri)
        {
            _currentRequestUri = currentRequestUri;
        }

        public CQ Modify(CQ cqDocument)
        {
            var customDocument = new CQ(cqDocument.Render());
            var csQueryHtml = customDocument.Select("html");

            if (csQueryHtml.Length > 0)
            {
                string manifestUrl = GetManifestUrl();
                csQueryHtml.Attr("manifest", manifestUrl);
            }

            return customDocument;
        }

        private string GetManifestUrl()
        {
            var urlBuilder = new UrlBuilder(_currentRequestUri);

            return string.Format("{0}.manifest", urlBuilder.Path);
        }
    }
}
