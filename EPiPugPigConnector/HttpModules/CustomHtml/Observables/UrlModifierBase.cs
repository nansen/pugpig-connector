using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsQuery;
using EPiPugPigConnector.Helpers;
using EPiPugPigConnector.HttpModules.CustomHtml.Interfaces;

namespace EPiPugPigConnector.HttpModules.CustomHtml.Observables
{
    public abstract class UrlModifierBase : IHtmlStreamModifier
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

            return relativeUrl;
        }
    }
}
