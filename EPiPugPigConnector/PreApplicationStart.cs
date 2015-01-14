using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiPugPigConnector;
using EPiPugPigConnector.HttpModules.ManifestHandler;
using EPiPugPigConnector.HttpModules.RelativeUrl;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(PreApplicationStart), "Start")]
namespace EPiPugPigConnector
{
    public static class PreApplicationStart
    {
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(RelativeUrlHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(ManifestHttpModule));
        }
    }
}
