using System;
using CsQuery;
using EPiPugPigConnector.Helpers;

namespace EPiPugPigConnector.HtmlCrawlers.CustomHtml.Modifiers
{

    public class RelativeHrefUrlModifier : UrlModifierBase
    {
        public RelativeHrefUrlModifier(Uri currentRequestUri, string element, string attribute)
            : base(currentRequestUri, element, attribute)
        {
        }

        public override CQ Modify(CQ cqDocument)
        {
            var customDocument = new CQ(cqDocument.Render());
            var htmlDom = customDocument.Select("html");

            CQ select = htmlDom.Select(Element);

            foreach (var item in select)
            {
                if (item.HasAttribute(Attribute))
                {
                    item.Attributes[Attribute] = GetHtmlRelativeUrl(item.Attributes[Attribute]);
                    item.Attributes[Attribute] = HtmlHelper.FriendlyUrlToUrlWithExtension(item.Attributes[Attribute], "html");
                }
            }

            return customDocument;
        }
    }
}
