using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiPugPigConnector;
using EPiPugPigConnector.HttpModules.Manifest;
using EPiPugPigConnector.HttpModules.RelativeUrl;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(PreApplicationStart), "Start")]
namespace EPiPugPigConnector
{
    /// <summary>
    /// Dynamic registration of HttpModules before application start, using WebActivatorEx and Microsoft.Web.Infrastructure
    /// Idea from: http://stackoverflow.com/questions/2521318/dynamic-adding-httpmodules-and-httphandlers?lq=1
    /// </summary>
    public static class PreApplicationStart
    {
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(RelativeUrlHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(ManifestHttpModule));
        }
    }
}
