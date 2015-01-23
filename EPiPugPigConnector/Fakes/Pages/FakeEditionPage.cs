using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiPugPigConnector.Editions;
using EPiPugPigConnector.Editions.Interfaces.Editions;
using EPiPugPigConnector.Editions.Models.Pages;
using EPiPugPigConnector.Editions.Models.Pages.Helpers;
using EPiPugPigConnector.Helpers;
using EPiServer;

namespace EPiPugPigConnector.Fakes.Pages
{
    public class FakeEditionPage : IEditionsEntryElement
    {
        public FakeEditionPage()
        {
            var dateTimeIssued = DateTime.Parse("2014-01-01 12:00:00");
            var dateTimeUpdated = DateTime.Parse("2014-01-01 13:00:00");
            this.EntryAuthorName = "John Wayne";
            this.EntryDcTermsIssued = EditionsHelper.EntryDcTermsIssued(dateTimeIssued);
            this.EntryId = EditionsHelper.GetEntryId(null);
            this.EntryLinkCoverImage = "/img/cover.jpg";
            this.EntryLinkEditionXml = EditionsHelper.GetEntryLinkEditionXml(null);
            this.EntrySummaryText = string.Empty;
            this.EntryTitle = "Magazine issue#" + 1;
            this.EntryUpdated = XmlHelper.GetDateTimeXmlFormatted(dateTimeUpdated);
        }        
        
        public FakeEditionPage(DateTime issuedAndUpdated, string authorName, string entryTitle)
        {
            var dateTimeIssued = issuedAndUpdated;
            var dateTimeUpdated = issuedAndUpdated;
            this.EntryAuthorName = authorName;
            this.EntryDcTermsIssued = EditionsHelper.EntryDcTermsIssued(dateTimeIssued);
            this.EntryId = EditionsHelper.GetEntryId(null);
            this.EntryLinkCoverImage = "/img/cover.jpg";
            this.EntryLinkEditionXml = EditionsHelper.GetEntryLinkEditionXml(null);
            this.EntrySummaryText = string.Empty;
            this.EntryTitle = entryTitle;
            this.EntryUpdated = XmlHelper.GetDateTimeXmlFormatted(dateTimeUpdated);
        }

        public string EntryTitle { get; set; }
        public string EntryId { get; set; }
        public string EntryUpdated { get; set; }
        public string GetEntryUpdatedFormatted(DateTime dateTimeUpdated)
        {
            return XmlHelper.GetDateTimeXmlFormatted(dateTimeUpdated);
        }
        public string EntryAuthorName { get; set; }
        public string EntryDcTermsIssued { get; set; }
        public string EntrySummaryText { get; set; }
        //public string EntryLinkCoverImage { get; set; }
        public Url EntryLinkCoverImage { get; set; }
        public string EntryLinkEditionXml { get; set; }
    }
}
