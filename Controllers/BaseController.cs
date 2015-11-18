using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RCTPL_WebProjects.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["LoggedUserId"] == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = 403;
                    filterContext.Result = new JsonResult { Data = "LogOut", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                    filterContext.Result = RedirectToAction("Index", "WEBUSERS");
            }
            else
            {
                //base.Execute(filterContext.RequestContext);
            }
        }




    }
}