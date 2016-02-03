using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RCTPL_WebProjects
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "WEBUSERS", action = "Index", id = UrlParameter.Optional }
            );
            //Comment for checkin
            routes.MapRoute(
                "CompleteRegistration",
                "{controller}/{action}/{_uname}/{_str}",
            new { controller = "WEBUSERS", action = "CompleteRegistration", _uname="", _str="" });
        }
    }
}
