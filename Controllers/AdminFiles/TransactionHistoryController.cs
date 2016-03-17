using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RCTPL_WebProjects.Models;

namespace RCTPL_WebProjects.Controllers.AdminFiles
{
    public class TransactionHistoryController : Controller
    {
        private RCTPLEntities db = new RCTPLEntities();
        // GET: TransactionHistory
        public ActionResult Index()
        {
            var user = Session["UName"].ToString();
            //date paid placeholder
            var today = DateTime.Today;
            TransactionHistoryModels tHistModel = new TransactionHistoryModels();
            
            List<TransactionHistoryModels> listTransHist = (from tweb in db.TBL_WEBUSERS
                                       join tchargeh in db.T_BILLCHRGH on tweb.USERNAME equals tchargeh.USRID
                                       join tcharged in db.T_BILLCHRGD on tchargeh.BCHCHRGNO equals tcharged.BCHCHRGNO
                                       join tmpaip in db.M_PAIP on tchargeh.PAPIN equals tmpaip.PAPIN
                                       where tweb.USERNAME.Contains(user)
                                       orderby tchargeh.BCHDTE
                                       select new TransactionHistoryModels
                                       {
                                           //needs transactionReference
                                           PLATE_NO = tmpaip.PLATE_NO,
                                           LASTNAME = tweb.LASTNAME,
                                           FIRSTNAME = tweb.FIRSTNAME,
                                           BCHDTE = tchargeh.BCHDTE,
                                           MAKE = tmpaip.MAKE,
                                           BCDPATCOVER = tcharged.BCDPATCOVER,
                                           COLORNUM = tcharged.COLORNUM,
                                           DATEPAID = today,
                                           BATCH_CODE = tcharged.BATCH_CODE
                                           //needs tcharged.datepaid
                                       }
                                       ).ToList();
            return View(listTransHist);
        }

    }
}