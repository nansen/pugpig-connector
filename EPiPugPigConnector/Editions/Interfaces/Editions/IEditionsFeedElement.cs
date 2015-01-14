using System;

namespace EPiPugPigConnector.Editions.Interfaces.Editions
{
    /// <summary>
    /// The interface for the Editions.xml OPDS feed.
    /// </summary>
    public interface IEditionsFeedElement
    {
        /// <summary>
        /// com.mycompany.editions
        /// </summary>
        string FeedId { get; set; }

        /// <summary>
        /// All Editions
        /// </summary>
        string FeedTitle { get; set; }

        /// <summary>
        /// E.g. "http://epipugpigdemo.azurewebsites.net/editions.xml"
        /// </summary>
        string FeedLinkHref { get; set; }

        /// <summary>
        /// 2011-08-08T15:02:28+00:00
        /// Should be ISO 8601 datetime
        /// http://tools.ietf.org/html/rfc3339 
        /// </summary>
        string FeedUpdated { get; set; }

        /// <summary>
        /// Returns xml formatted datetime value.
        /// </summary>
        /// <param name="dateTimeUpdated"></param>
        /// <returns>2011-08-08T15:02:28+00:00</returns>
        string GetFeedUpdatedFormatted(DateTime dateTimeUpdated);
    }
}
