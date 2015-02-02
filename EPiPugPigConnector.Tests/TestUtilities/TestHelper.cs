using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPiPugPigConnector.Tests.TestUtilities
{
    public static class TestHelper
    {
        public static string GetStringFromFileInfo(FileInfo fileInfo)
        {
            StringBuilder sb = new StringBuilder();

            // Open the stream and read it back. 
            using (StreamReader sr = File.OpenText(fileInfo.FullName))
            {
                string line = String.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
            }

            return sb.ToString();
        }

        public static DirectoryInfo GetSolutionDir()
        {
            DirectoryInfo baseDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            return baseDir.Parent.Parent.Parent;
        }

        public static DirectoryInfo GetTestProjectDir()
        {
            DirectoryInfo baseDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            return baseDir.Parent.Parent;;
        }
    }
}
