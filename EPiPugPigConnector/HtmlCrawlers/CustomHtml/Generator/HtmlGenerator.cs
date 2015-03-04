using System.Collections.Generic;
using CsQuery;
using EPiPugPigConnector.HtmlCrawlers.Interfaces;

namespace EPiPugPigConnector.HtmlCrawlers.CustomHtml.Generator
{
    public class HtmlGenerator : IHtmlGenerator
    {
        private List<IHtmlModifier> _crawlers;

        public HtmlGenerator()
        {
            _crawlers = new List<IHtmlModifier>();
        }

        public IHtmlGenerator AddModifier(IHtmlModifier modifier)
        {
            _crawlers.Add(modifier);

            return this;
        }

        public void RemoveModifier(IHtmlModifier modifier)
        {
            _crawlers.Remove(modifier);
        }

        public string GenerateHtml(string htmlDocument)
        {
            var cqDocument = new CQ(htmlDocument);
           
            foreach (var customHtmlStreamObserver in _crawlers)
            {
                cqDocument = customHtmlStreamObserver.Modify(cqDocument);
            }

            var resultHtml = cqDocument.Render();

            return resultHtml;
        }
    }
}
