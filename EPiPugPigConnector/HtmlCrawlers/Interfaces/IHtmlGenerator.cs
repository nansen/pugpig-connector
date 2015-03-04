namespace EPiPugPigConnector.HtmlCrawlers.Interfaces
{
    /// <summary>
    /// Interface to implement the Subject object of the Observer Pattern
    /// </summary>
    public interface IHtmlGenerator
    {
        IHtmlGenerator AddModifier(IHtmlModifier modifier);
        void RemoveModifier(IHtmlModifier modifier);
        string GenerateHtml(string htmlDocument);
    }
    
}
