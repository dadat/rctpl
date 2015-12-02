using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RCTPL_WebProjects.Models;
using System.Web.Http;
using System.Net;

namespace RCTPL_WebProjects.Controllers
{
    public class AdminController : AdminBaseController
    {

        private RCTPLEntities db = new RCTPLEntities();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public IEnumerable<TBL_WEBUSERS> GetAllUsers()
        {
            return db.TBL_WEBUSERS;
        }

        //public IHttpActionResult GetUser(string username)
        //{
        //    var product = db.TBL_WEBUSERS.FirstOrDefault((p) => p.USERNAME == username);
        //    if (product == null)
        //    {
        //        return null;
        //    }
        //    return product.ToString();
        //}

        // 
        // GET: /HelloWorld/Welcome/ 

        public string Welcome(string name, int numTimes = 1)
        {
            return HttpUtility.HtmlEncode("Hello " + name + ", NumTimes is: " + numTimes);
        }
    }
}