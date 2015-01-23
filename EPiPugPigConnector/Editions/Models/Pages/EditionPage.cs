using System;
using System.ComponentModel.DataAnnotations;
using EPiPugPigConnector.Editions.Interfaces.Edition;
using EPiPugPigConnector.Editions.Interfaces.Editions;
using EPiPugPigConnector.Editions.Models.Pages.Helpers;
using EPiPugPigConnector.Helpers;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;

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

        [Display(GroupName = SystemTabNames.Content, Order = 105, Name = "Edition Link Cover Image", Description = "/img/cover.jpg")]
        [DefaultDragAndDropTarget]
        [UIHint(UIHint.Image)]
        [CultureSpecific]
        public virtual Url EntryLinkCoverImage
        {
            get
            {
                return this.GetPropertyValue(page => page.EntryLinkCoverImage);
                //Url imageUrl = this.GetPropertyValue(page => page.EntryLinkCoverImage);
                //if (imageUrl == null || imageUrl.IsEmpty())
                //{
                //    return new Url("/Static/gfx/logotype.png");
                //}
                //else
                //{
                //    return imageUrl;
                //}
            }
            set { this.SetPropertyValue(page => page.EntryLinkCoverImage, value); }
        }

        [Display(GroupName = SystemTabNames.Content, Order = 110, Name = "Edition title", 
            Description = "E.g. \"The Company Magazine Issue #4 - 2014\", if not set fallbacks to Page name.")]
        [CultureSpecific]
        public virtual string EntryTitle
        {
            get
            {
                string value = this.GetPropertyValue(p => p.EntryTitle);
                string valueFallback = this.PageName;
                return !string.IsNullOrWhiteSpace(value) ? value : valueFallback;
            }
            set { this.SetPropertyValue(p => p.EntryTitle, value); }
        }

        [Display(GroupName = SystemTabNames.Content, Order = 120, Name = "Author name", Description = "E.g. \"Charlie Sheen\", if not set fallbacks to Episerver Created by property")]
        [CultureSpecific]
        public virtual string EntryAuthorName
        {
            get
            {
                string value = this.GetPropertyValue(p => p.EntryAuthorName);
                string valueFallback = this.CreatedBy;
                return !string.IsNullOrWhiteSpace(value) ? value : valueFallback;
            }
            set { this.SetPropertyValue(p => p.EntryAuthorName, value); }
        }

        [Display(GroupName = SystemTabNames.Content, Order = 130, Name = "Edition Summary Text", Description = "Can be empty")]
        [CultureSpecific]
        public virtual string EntrySummaryText { get; set; }

        #region NonEditorProperties

            [ScaffoldColumn(false)]
            public virtual string EntryId
            {
                get { return EditionsHelper.GetEntryId(this); }
                set { throw new NotImplementedException(); }
            }

            [ScaffoldColumn(false)]
            public virtual string EntryUpdated
            {
                get { return GetEntryUpdatedFormatted(this.Changed); }
                set { this.SetPropertyValue(p => p.EntryUpdated, value); }
            }

            public virtual string GetEntryUpdatedFormatted(DateTime dateTimeUpdated)
            {
                return XmlHelper.GetDateTimeXmlFormatted(dateTimeUpdated);
            }

            [ScaffoldColumn(false)]
            public virtual string EntryDcTermsIssued
            {
                get { return EditionsHelper.EntryDcTermsIssued(this.Changed); }
                set { }
            }

            [ScaffoldColumn(false)]
            public virtual string EntryLinkEditionXml {
                get { return EditionsHelper.GetEntryLinkEditionXml(this); }
                set { } 
            }

            #endregion NonEditorProperties

        #endregion IEditionsEntryElement
        

        #region IEditionFeedElement - Edition.xml feed specific props.

        [ScaffoldColumn(false)]
        public virtual string FeedId
        {
            get { return this.EntryId; }
            set { throw new NotImplementedException(); }
        }

        [ScaffoldColumn(false)]
        public virtual string FeedLink
        {
            get { return this.EntryLinkEditionXml; }
            set { throw new NotImplementedException(); }
        }

        [ScaffoldColumn(false)]
        public virtual string FeedTitle
        {
            get { return this.EntryTitle; }
            set { throw new NotImplementedException(); }
        }

        [Display(GroupName = SystemTabNames.Content, Order = 125, Name = "Edition subtitle",
            Description = "E.g. \"This is a sample edition with 2 pages")]
        [CultureSpecific]
        public virtual string FeedSubtitle { get; set; }

        [ScaffoldColumn(false)]
        public virtual string FeedAuthorName
        {
            get { return this.EntryAuthorName; }
            set { throw new NotImplementedException(); }
        }

        [ScaffoldColumn(false)]
        public virtual string FeedUpdated
        {
            get { return this.EntryUpdated; }
            set { throw new NotImplementedException(); }
        }

        #endregion
    }
}
