using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

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
        /// <param name="request"></param>
        /// <returns></returns>
        public string MakeManifestFileAsString(List<string> offlineUrlsList)
        {
            StringBuilder strb = new StringBuilder();
            strb.Append("CACHE MANIFEST" + Environment.NewLine);
            strb.AppendFormat("# This file was generated at {0}" + Environment.NewLine, DateTime.Now.ToString("o"));
            strb.Append("CACHE:" + Environment.NewLine);

            foreach (string filePath in offlineUrlsList)
            {
                strb.AppendFormat(String.Format("{0}" + Environment.NewLine, filePath));
            }

            strb.Append("NETWORK:" + Environment.NewLine);
            strb.Append("*" + Environment.NewLine);

            return strb.ToString();
        }

        private List<string> GetOfflineUrls()
        {
            //lookup current episerver page / page.

            throw new NotImplementedException();
        }
    }
}