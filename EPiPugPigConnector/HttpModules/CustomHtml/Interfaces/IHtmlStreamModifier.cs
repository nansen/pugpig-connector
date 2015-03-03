using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsQuery;

namespace EPiPugPigConnector.HttpModules.CustomHtml.Interfaces
{
    /// <summary>
    /// Interface to implement the observer object of the Observer Pattern
    /// </summary>
    public interface IHtmlStreamModifier
    {
        CQ Modify(CQ cqDocument);
    }
}
