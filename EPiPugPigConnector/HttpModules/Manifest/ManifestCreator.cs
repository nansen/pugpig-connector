using System;
using System.Text;

namespace EPiPugPigConnector.HttpModules.Manifest
{
    public class ManifestCreator
    {
        public ManifestCreator()
        {
        }

        /// <summary>
        /// Create a string to be saved in the Manifest file
        /// </summary>
        /// <returns></returns>
        public string MakeManifestFileAsString()
        {
            StringBuilder strb = new StringBuilder();
            strb.Append("CACHE MANIFEST" + Environment.NewLine);
            strb.AppendFormat("# This file was generated at {0}" + Environment.NewLine, DateTime.Now);
            strb.Append("CACHE:" + Environment.NewLine);

            //foreach (string filePath in LocalURLs.Values)
            //{
            //    strb.AppendFormat(String.Format("{0}" + Environment.NewLine, filePath));
            //}

            strb.Append("NETWORK:" + Environment.NewLine);
            strb.Append("*" + Environment.NewLine);

            return strb.ToString();
        }
    }
}