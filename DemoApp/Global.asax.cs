using System;
using System.Web.Mvc;

namespace DemoApp
{
    public class EPiServerApplication : EPiPugPigConnector.Global
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //Tip: Want to call the EPiServer API on startup? Add an initialization module instead (Add -> New Item.. -> EPiServer -> Initialization Module)
        }
    }
}