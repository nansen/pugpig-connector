using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPiPugPigConnector.HttpModules.CustomHtml.Interfaces
{
    /// <summary>
    /// Interface to implement the Subject object of the Observer Pattern
    /// </summary>
    public interface IHtmlStreamProcessor
    {
        IHtmlStreamProcessor AddModifier(IHtmlStreamModifier modifier);
        void RemoveModifier(IHtmlStreamModifier modifier);
        string ProcessHtml(string htmlDocument);
    }
    
}
