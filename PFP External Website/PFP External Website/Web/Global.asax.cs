using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //System.Web.Helpers.AntiForgeryConfig.UniqueClaimTypeIdentifier =
            //System.Security.Claims.ClaimTypes.NameIdentifier;
        }

        void Session_Start(object sender, EventArgs e)
        {
            // place holder to solve endless loop issue in okta (Please don't remoe this function).
        }
    }
}
