namespace EPiPugPigConnector.Editions.Interfaces.Edition
{
    public interface IEditionEntryElement
    {
        string EntryId { get; set; }
        string EntryHtmlLink { get; set; }
        string EntryManifestLink { get; set; }
        string EntryTitle { get; set; }
        string EntrySummary { get; set; }
        string EntryUpdated { get; set; }
    }
}