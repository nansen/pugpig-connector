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
    /// <summary>
    /// Modifies the given attribute, modifies the url to a relative url of the current request url
    /// </summary>
    public class RelativeUrlModifier : UrlModifierBase
    {
        public RelativeUrlModifier(Uri currentRequestUri, string element, string attribute)
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
                }
            }

            return customDocument;
        }
    }
}
