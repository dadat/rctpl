using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using RCTPL_WebProjects.Models;
using Mindscape.LightSpeed.Web.Mvc;
using Mindscape.LightSpeed;

namespace RCTPL_WebProjects
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //yhogz
           

            //LightSpeedEntityModelBinder.Register(typeof(RCTPLModelUnitOfWork).Assembly);
            //ModelValidatorProviders.Providers.Clear();
            //ModelValidatorProviders.Providers.Add(new LightSpeedMvcValidatorProvider());
        }

        
        //public static LightSpeedContext<RCTPLModelUnitOfWork> LsConn = new LightSpeedContext<RCTPLModelUnitOfWork>()
        //{
        //    ConnectionString = "Data Source=localhost;Initial Catalog=testing;Persist Security Info=True;User ID=sa;Password=12345;Pooling=False",
        //    IdentityMethod = IdentityMethod.IdentityColumn,
        //    DataProvider = DataProvider.SqlServer2008
        //};
    }
}
