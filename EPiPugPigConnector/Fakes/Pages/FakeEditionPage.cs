using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiPugPigConnector.Editions.Interfaces.Editions;
using EPiPugPigConnector.Models.Pages;

namespace EPiPugPigConnector.Fakes.Pages
{
    public class FakeEditionPage : IEditionsXmlFeedEntry
    {
        public FakeEditionPage()
        {
            var dateTimeIssued = DateTime.Parse("2014-01-01 12:00:00");
            var dateTimeUpdated = DateTime.Parse("2014-01-01 13:00:00");
            this.EntryAuthorName = "John Wayne";
            this.EntryDcTermsIssued = EditionsPageHelper.EntryDcTermsIssued(dateTimeIssued);
            this.EntryId = EditionsPageHelper.GetEntryId(null);
            this.EntryLinkCoverImage = "/img/cover.jpg";
            this.EntryLinkEditionXml = EditionsPageHelper.GetEntryLinkEditionXml(null);
            this.EntrySummaryText = string.Empty;
            this.EntryTitle = "Magazine issue#" + 1;
            this.EntryUpdated = EditionsPageHelper.GetDateTimeXmlFormatted(dateTimeUpdated);
        }        
        
        public FakeEditionPage(DateTime issuedAndUpdated, string authorName, string entryTitle)
        {
            var dateTimeIssued = issuedAndUpdated;
            var dateTimeUpdated = issuedAndUpdated;
            this.EntryAuthorName = authorName;
            this.EntryDcTermsIssued = EditionsPageHelper.EntryDcTermsIssued(dateTimeIssued);
            this.EntryId = EditionsPageHelper.GetEntryId(null);
            this.EntryLinkCoverImage = "/img/cover.jpg";
            this.EntryLinkEditionXml = EditionsPageHelper.GetEntryLinkEditionXml(null);
            this.EntrySummaryText = string.Empty;
            this.EntryTitle = entryTitle;
            this.EntryUpdated = EditionsPageHelper.GetDateTimeXmlFormatted(dateTimeUpdated);
        }

        public string EntryTitle { get; set; }
        public string EntryId { get; set; }
        public string EntryUpdated { get; set; }
        public string GetEntryUpdatedFormatted(DateTime dateTimeUpdated)
        {
            return EditionsPageHelper.GetDateTimeXmlFormatted(dateTimeUpdated);
        }
        public string EntryAuthorName { get; set; }
        public string EntryDcTermsIssued { get; set; }
        public string EntrySummaryText { get; set; }
        public string EntryLinkCoverImage { get; set; }
        public string EntryLinkEditionXml { get; set; }
    }
}
