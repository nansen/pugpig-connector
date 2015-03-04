namespace EPiPugPigConnector.HtmlCrawlers.Interfaces
{
    public interface IManifestGenerator
    {
        IManifestGenerator AddCrawler(IManifestCrawler crawler);
        void RemoveModifier(IManifestCrawler crawler);
        string GenerateManifest(string htmlDocument);
    }
}
