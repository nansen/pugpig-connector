namespace EPiPugPigConnector.Editions.Interfaces.Edition
{
    public interface IEditionFeedElement
    {
        string FeedId { get; set; }
        string FeedLink { get; set; }
        string FeedTitle { get; set; }
        string FeedSubtitle { get; set; }
        string FeedAuthorName { get; set; }
        string FeedUpdated { get; set; }
        //XmlDateTimeIso8601 FeedUpdated { get; set; }
    }
}
