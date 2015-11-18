using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RCTPL_WebProjects.Controllers.Richmer
{
    public class DashboardController : AdminBaseController
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}