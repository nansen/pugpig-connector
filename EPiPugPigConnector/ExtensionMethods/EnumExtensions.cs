using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPiPugPigConnector.ExtensionMethods
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Return the integer value for an enum.
        /// 
        /// </summary>
        /// <param name="value">The enum value
        ///              </param>
        /// <returns>
        /// An integer.
        /// 
        /// </returns>
        public static int GetValue(this Enum value)
        {
            return Convert.ToInt32((object)value);
        }

        /// <summary>
        /// Return the integer value cast as a string for an enum
        /// 
        /// </summary>
        /// <param name="value"/>
        /// <returns/>
        public static string GetValueAsString(this Enum value)
        {
            return GetValue(value).ToString();
        }
    }
}
