using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPiPugPigConnector.ExtensionMethods
{
    public static class StringExtensions
    {
        public static bool IsNotNullOrEmpty(this string stringValue)
        {
            return !string.IsNullOrEmpty(stringValue);
        }
    }
}
