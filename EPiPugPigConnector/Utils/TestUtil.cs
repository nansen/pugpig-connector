using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPiPugPigConnector.Utils
{
    public static class TestUtil
    {
        public static bool IsCalledFromTest()
        {
            if (AppDomain.CurrentDomain.FriendlyName.StartsWith("UnitTestAdapter:"))
                return true;
            return false;
        }
    }
}
