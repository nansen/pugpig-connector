using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiPugPigConnector.Editions.Interfaces.Editions;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace EPiPugPigConnector.Models.Pages
{
    [ContentType(
        GUID = "3ADE6674-91EF-4182-AA2C-7C83D803B569",
        DisplayName = "[PugPig] Edition",
        Description = "Edition page for the pupig feed, can be several editions for each feed",
        Order = 30002
    )]
    public class EditionPage : PageData, IEditionsXmlFeedEntry
    {
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
            get { return EditionsPageHelper.GetEntryId(this); }
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
            return EditionsPageHelper.GetDateTimeXmlFormatted(dateTimeUpdated);
        }

        [ScaffoldColumn(false)]
        public string EntryDcTermsIssued
        {
            get { return EditionsPageHelper.EntryDcTermsIssued(this.Changed); }
            set { }
        }

        [ScaffoldColumn(false)]
        public string EntryLinkEditionXml {
            get { return EditionsPageHelper.GetEntryLinkEditionXml(this); }
            set { } 
        }

        #endregion
    }
}
