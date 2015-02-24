using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPiPugPigConnector.HttpModules.Manifest
{
    /// <summary>
    /// Scans css files for assests and adds them to a manifest file
    /// </summary>
    public class CssAssetProcessor
    {
        private string _manifestFilePath;

        public CssAssetProcessor(string manifestFilePath, string cssFilePath)
        {
            _manifestFilePath = manifestFilePath;
        }

        public void ProcessCssFile(string cssFile)
        {
            
        }
    }
}
