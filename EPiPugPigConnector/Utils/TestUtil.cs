using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPiPugPigConnector.Utils
{
    public static class TestUtil
    {
        public static bool IsCalledFromTest()
        {
            if (System.AppDomain.CurrentDomain.FriendlyName.StartsWith("UnitTestAdapter:"))
                return true;
            return false;
        }
    }
}
