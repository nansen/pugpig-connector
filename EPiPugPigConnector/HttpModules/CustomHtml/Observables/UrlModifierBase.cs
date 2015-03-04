using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Lifestyle;
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
