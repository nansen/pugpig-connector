using System;
using CsQuery;
using EPiPugPigConnector.Helpers;
using EPiPugPigConnector.HtmlCrawlers.Interfaces;

namespace EPiPugPigConnector.HtmlCrawlers.CustomHtml.Modifiers
{
    public abstract class UrlModifierBase : IHtmlModifier
    {
        protected readonly Uri CurrentRequestUri;
        protected readonly string Element;
        protected readonly string Attribute;

        public UrlModifierBase(Uri currentRequestUri, string element, string attribute)
        {
            CurrentRequestUri = currentRequestUri;
            Element = element;
            Attribute = attribute;
        }

        public abstract CQ Modify(CQ cqDocument);

        protected string GetHtmlRelativeUrl(string url)
        {
            var abslouteUrl = UrlHelper.GetAbslouteUrl(url);
            var relativeUrl = UrlHelper.GetRelativeUrl(CurrentRequestUri.ToString(), abslouteUrl);

            if (relativeUrl == "./")
                relativeUrl = "../";

            if (relativeUrl == "../")
            {
                int ix1 = abslouteUrl.LastIndexOf('/');
                int ix2 = ix1 > 0 ? abslouteUrl.LastIndexOf('/', ix1 - 1) : -1;
                relativeUrl = string.Format("{0}{1}", relativeUrl, abslouteUrl.Remove(0, ix2 +1));
            }

            return relativeUrl;
        }
    }
}
