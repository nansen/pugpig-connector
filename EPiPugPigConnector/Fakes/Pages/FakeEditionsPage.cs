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

namespace EPiPugPigConnector.Fakes.Pages
{
    public class FakeEditionsPage : IEditionsFeedElement
    {
        public FakeEditionsPage()
        {
            this.FeedId = "com.mycompany.myeditions";
            this.FeedLinkHref = "http://epipugpigdemo.azurewebsites.net/editions.xml";
            this.FeedTitle = "All Editions";
            this.FeedUpdated = this.GetFeedUpdatedFormatted(new DateTime(2013, 7, 24, 0, 0, 0, DateTimeKind.Utc));
        }

        public string FeedId { get; set; }
        public string FeedTitle { get; set; }
        public string FeedLinkHref { get; set; }
        public string FeedUpdated { get; set; }
        public string GetFeedUpdatedFormatted(DateTime dateTimeUpdated)
        {
            return XmlHelper.GetDateTimeXmlFormatted(dateTimeUpdated);
        }
    }
}
