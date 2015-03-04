using System.Collections.Generic;
using CsQuery;
using EPiPugPigConnector.HtmlCrawlers.Manifest.Models;

namespace EPiPugPigConnector.HtmlCrawlers.Interfaces
{
    public interface IManifestCrawler
    {
        List<Asset> Crawl(CQ htmlDocument);
    }
}
