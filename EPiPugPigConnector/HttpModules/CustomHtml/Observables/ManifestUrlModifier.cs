using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsQuery;
using EPiPugPigConnector.HttpModules.CustomHtml.Interfaces;
using EPiServer;

namespace EPiPugPigConnector.HttpModules.CustomHtml.Observables
{
    /// <summary>
    /// Adds the manifest url to the html tag.
    /// </summary>
    public class ManifestUrlModifier : IHtmlStreamModifier
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
