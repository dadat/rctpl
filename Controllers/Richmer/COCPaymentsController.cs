using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RCTPL_WebProjects.Models;
using RCTPL_WebProjects.Models.Rich;


namespace RCTPL_WebProjects.Controllers.Richmer
{
    public class COCPaymentsController : BaseController
    {

        RCTPLEntities mydb = new RCTPLEntities();


        // GET: COCPayments
        public ActionResult Index()
        {


            return View();
        }



        public ActionResult Payments(string papin, string VEHICLE_CLASS)
        {

            //TBL_VEHICLES myvehicle = mydb.TBL_VEHICLES.Where(s => s.VEHICLE_CODE.Equals(VEHICLE_CLASS)).FirstOrDefault();
            //COCModel mycoc = new COCModel();


         
            //if (myvehicle != null)
            //{

            //    mycoc.papin = "test";
            //    mycoc.name = "Richmer";
            //    mycoc.DOC_STAMP = myvehicle.DOC_STAMP;
            //    mycoc.E_VAT = myvehicle.VAT;
            //    mycoc.LGT = myvehicle.LGT;
            //    mycoc.SUM_INSIRED = myvehicle.SUM_INSURED;
            //    mycoc.COMP_FEE = myvehicle.COMP_FEE;
            //    mycoc.CERT_FEE = myvehicle.CERT_FEE;
            //    mycoc.BASIC_PREMIUM = myvehicle.BASIC_PREMIUM;
            //    mycoc.VEHICLE_NUMBER = myvehicle.VEHICLE_NUMBER;
            //    mycoc.TOTAL_AMOUNT = myvehicle.TOTAL_AMOUNT;          
            //}

          

            



            return View("../COCPayments/Index");
        }


    }
}