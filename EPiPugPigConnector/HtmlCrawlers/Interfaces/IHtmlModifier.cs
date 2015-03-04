using CsQuery;

namespace EPiPugPigConnector.HtmlCrawlers.Interfaces
{
    /// <summary>
    /// Interface to implement the observer object of the Observer Pattern
    /// </summary>
    public interface IHtmlModifier
    {
        CQ Modify(CQ cqDocument);
    }
}
