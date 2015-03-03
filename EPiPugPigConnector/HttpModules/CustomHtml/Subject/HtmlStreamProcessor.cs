using System.Collections.Generic;
using CsQuery;
using EPiPugPigConnector.HttpModules.CustomHtml.Interfaces;

namespace EPiPugPigConnector.HttpModules.CustomHtml.Subject
{
    public class HtmlStreamProcessor : IHtmlStreamProcessor
    {
        private List<IHtmlStreamModifier> _observerList;

        public HtmlStreamProcessor()
        {
            _observerList = new List<IHtmlStreamModifier>();
        }

        public IHtmlStreamProcessor AddModifier(IHtmlStreamModifier modifier)
        {
            _observerList.Add(modifier);

            return this;
        }

        public void RemoveModifier(IHtmlStreamModifier modifier)
        {
            _observerList.Remove(modifier);
        }

        public string ProcessHtml(string htmlDocument)
        {
            var cqDocument = new CQ(htmlDocument);
           
            foreach (var customHtmlStreamObserver in _observerList)
            {
                cqDocument = customHtmlStreamObserver.Modify(cqDocument);
            }

            var resultHtml = cqDocument.Render();

            return resultHtml;
        }
    }
}
