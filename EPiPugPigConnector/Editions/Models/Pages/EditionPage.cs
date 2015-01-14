using System;
using System.ComponentModel.DataAnnotations;
using EPiPugPigConnector.Editions.Interfaces.Edition;
using EPiPugPigConnector.Editions.Interfaces.Editions;
using EPiPugPigConnector.Editions.Models.Pages.Helpers;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace EPiPugPigConnector.Editions.Models.Pages
{
    [ContentType(
        GUID = "3ADE6674-91EF-4182-AA2C-7C83D803B569",
        DisplayName = "[PugPig] Edition",
        Description = "Edition page for the pupig feed, can be several editions for each feed",
        Order = 30002
    )]
    public class EditionPage : PageData, IEditionsEntryElement, IEditionFeedElement
    {
        #region IEditionsEntryElement - Editions.xml feed specific

        /// <summary>
        /// Gets or sets a value indicating whether the Changed property should be updated on every page publish.
        /// Default is that changed date only gets updated when the "mark as changed" checkbox is checked. / or the first time the page is published.
        /// </summary>
        public override bool SetChangedOnPublish
        {
            get { return true; }
        }

        [Display(GroupName = SystemTabNames.Content, Order = 110, Name = "Edition title", Description = "E.g. \"The Company Magazine Issue #4 - 2014\"")]
        [CultureSpecific]
        public virtual string EntryTitle { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 120, Name = "Author name", Description = "E.g. \"Charlie Sheen\"")]
        [CultureSpecific]
        public string EntryAuthorName
        {
            get
            {
                string value = this.GetPropertyValue(p => p.EntryAuthorName);
                string valueFallback = this.CreatedBy;
                // Use value, otherwise fallback
                return !string.IsNullOrWhiteSpace(value)
                       ? value
                       : valueFallback;
            }
            set { this.SetPropertyValue(p => p.EntryAuthorName, value); }
        }

        [Display(GroupName = SystemTabNames.Content, Order = 130, Name = "Edition Summary Text", Description = "Can be empty")]
        [CultureSpecific]
        public string EntrySummaryText { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 140, Name = "Edition Link Cover Image",
            Description = "/img/cover.jpg")]
        [CultureSpecific]
        public string EntryLinkCoverImage
        {
            get { return this.GetPropertyValue(p => p.EntryLinkCoverImage) ?? string.Empty; }
            set { this.SetPropertyValue(p => p.EntryLinkCoverImage, value); }
        }

            #region NonEditorProperties

            [ScaffoldColumn(false)]
            public string EntryId
            {
                get { return EditionsHelper.GetEntryId(this); }
                set { throw new NotImplementedException(); }
            }

            [ScaffoldColumn(false)]
            public string EntryUpdated
            {
                get { return GetEntryUpdatedFormatted(this.Changed); }
                set { this.SetPropertyValue(p => p.EntryUpdated, value); }
            }

            public string GetEntryUpdatedFormatted(DateTime dateTimeUpdated)
            {
                return XmlHelper.GetDateTimeXmlFormatted(dateTimeUpdated);
            }

            [ScaffoldColumn(false)]
            public string EntryDcTermsIssued
            {
                get { return EditionsHelper.EntryDcTermsIssued(this.Changed); }
                set { }
            }

            [ScaffoldColumn(false)]
            public string EntryLinkEditionXml {
                get { return EditionsHelper.GetEntryLinkEditionXml(this); }
                set { } 
            }

            #endregion NonEditorProperties

        #endregion IEditionsEntryElement




        #region IEditionFeedElement - Edition.xml feed specific props.

        [ScaffoldColumn(false)]
        public string FeedId
        {
            get { return this.EntryId; }
            set { throw new NotImplementedException(); }
        }

        [ScaffoldColumn(false)]
        public string FeedLink
        {
            get { return this.EntryLinkEditionXml; }
            set { throw new NotImplementedException(); }
        }

        [ScaffoldColumn(false)]
        public string FeedTitle
        {
            get { return this.EntryTitle; }
            set { throw new NotImplementedException(); }
        }

        [Display(GroupName = SystemTabNames.Content, Order = 125, Name = "Edition subtitle",
            Description = "E.g. \"This is a sample edition with 2 pages")]
        [CultureSpecific]
        public string FeedSubtitle { get; set; }

        [ScaffoldColumn(false)]
        public string FeedAuthorName
        {
            get { return this.EntryAuthorName; }
            set { throw new NotImplementedException(); }
        }

        [ScaffoldColumn(false)]
        public string FeedUpdated
        {
            get { return this.EntryUpdated; }
            set { throw new NotImplementedException(); }
        }

        #endregion
    }
}
