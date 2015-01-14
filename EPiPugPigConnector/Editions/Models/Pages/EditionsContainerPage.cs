using System;
using System.ComponentModel.DataAnnotations;
using EPiPugPigConnector.Editions.Interfaces.Editions;
using EPiPugPigConnector.Editions.Models.Pages.Helpers;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace EPiPugPigConnector.Editions.Models.Pages
{
    [ContentType(
        GUID = "7135D46A-098A-4E25-AA7B-CC076720AAD4",
        DisplayName = "[PugPig] Editions Container",
        Description = "Root page for the pugpig feed.",
        Order = 30001
    )]
    [AvailableContentTypes(Include = new []{ typeof(EditionPage) })]
    public class EditionsContainerPage : PageData, IEditionsFeedElement
    {
        /// <summary>
        /// Gets or sets a value indicating whether the Changed property should be updated on every page publish.
        /// Default is that changed date only gets updated when the "mark as changed" checkbox is checked. / or the first time the page is published.
        /// </summary>
        public override bool SetChangedOnPublish
        {
            get { return true; }
        }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            this.Property["PageChildOrderRule"].Value = EPiServer.Filters.FilterSortOrder.CreatedAscending;
        }

        [Display(GroupName = SystemTabNames.Content, Order = 110, Name = "Feed id", Description = "E.g. FeedId-1")]
        [CultureSpecific]
        [ScaffoldColumn(false)] //hide in editor mode
        public virtual string FeedId
        {
            get
            {
                string feedId = this.GetPropertyValue(p => p.FeedId);
                string fallbackFeedId = string.Format("FeedId-{0}", this.PageLink.ID);
                // Use feedId, otherwise fall back to fallbackFeedId
                return !string.IsNullOrWhiteSpace(feedId)
                       ? feedId
                       : fallbackFeedId;
            }
            set { }
        }

        [Display(GroupName = SystemTabNames.Content, Order = 120, Name = "Feed title", Description = "E.g. \"All Editions\"")]
        [CultureSpecific]
        [Required]
        public virtual string FeedTitle { get; set; }

        
        #region NonEditorProperties

        [ScaffoldColumn(false)]
        public string FeedLinkHref { 
            get { return EditionsHelper.GetLinkHref(); } 
            set { }
        }

        [ScaffoldColumn(false)]
        public string FeedUpdated
        {
            get { return GetFeedUpdatedFormatted(this.Changed); }
            set { this.SetPropertyValue(p => p.FeedUpdated, value); }
        }
        
        public string GetFeedUpdatedFormatted(DateTime dateTimeUpdated)
        {
            return XmlHelper.GetDateTimeXmlFormatted(dateTimeUpdated);
        }

        #endregion
    }
}
