using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RCTPL_WebProjects.Models;
using RCTPL_WebProjects.Models.VehicleDB;
using System.Threading.Tasks;
using RCTPL_WebProjects.Models.Rich;
using System.Data.Entity.Validation;
using RCTPL_WebProjects.OtherFunctions;



namespace RCTPL_WebProjects.Controllers
{
    public class COCController : BaseController
    {
        private VehicleQSEntities mydb = new VehicleQSEntities();
        private RCTPLEntities rctpl_db = new RCTPLEntities();
        RCTPLFunctions rctpl_function = new RCTPLFunctions();


        MyModel_Class myclass = new MyModel_Class();
        clsYtiruceS.GetSerial Yfunction = new clsYtiruceS.GetSerial();
        string myElement = "ITDept6953069";
     
        // GET: COC
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(string splate, string schasis, string sengineNo)
        {

         VehicleModel data = new VehicleModel();

        data = await SearchMPAIP(splate, schasis, sengineNo);

        if (data != null)
        {
             TempData["mypapin"] = data.papin;
            ViewBag.Success = "Record Found!";
            return View(data);
        }   


         data = await Searchfrom_VehicleDB(splate, schasis, sengineNo);
         if (data != null)
         {
             ViewBag.Success = "Vehicle Record Found!";
             return View(data);
         }   
            ViewBag.Message = "No Record Found!";
            return View();
        }

        public async Task<VehicleModel> SearchMPAIP(string splate, string schasis, string sengineNo)
        {
            M_PAIP vehicle = new M_PAIP();
            VehicleModel vehiclemodel = new VehicleModel();

             vehicle = rctpl_db.M_PAIP.Where(r => r.PLATE_NO.Equals(splate) && (r.SERIAL_NO.Equals(schasis) || r.MOTOR_NO.Equals(sengineNo))).FirstOrDefault();


             if (vehicle != null)
             {
                 vehiclemodel.papin = vehicle.PAPIN;
                 vehiclemodel.plateno = vehicle.PLATE_NO;
                 vehiclemodel.chasisno = vehicle.SERIAL_NO;
                 vehiclemodel.engineno = vehicle.MOTOR_NO;
                 vehiclemodel.yearmodel = vehicle.MODEL_YR;
                 vehiclemodel.make = vehicle.MAKE;
                 vehiclemodel.series = vehicle.SERIES;
                 vehiclemodel.color = vehicle.COLOR;
                 vehiclemodel.unladed_weight = vehicle.UN_WEIGHT;
                 vehiclemodel.mvfile = vehicle.MV_FILENO;
             }
             else {

                 return null;
             }


             return vehiclemodel;
        
        }

        public async Task<VehicleModel> Searchfrom_VehicleDB(string splate, string schasis, string sengineNo)
        {

            VehicleDetail vehicle = mydb.VehicleDetails.Where(r => r.plateno.Equals(splate) && (r.chassisno.Equals(schasis) || r.engineno.Equals(sengineNo))).FirstOrDefault();
            VehicleModel vehiclemodel = new VehicleModel();

            if (vehicle != null)
            {
                vehiclemodel.papin = "NO PAPIN";
                vehiclemodel.plateno = vehicle.plateno;
                vehiclemodel.chasisno = vehicle.chassisno;
                vehiclemodel.engineno = vehicle.engineno;
                vehiclemodel.yearmodel = vehicle.model.ToString();
                vehiclemodel.make = vehicle.make;
                vehiclemodel.series = vehicle.series;
                vehiclemodel.color = vehicle.color;
                vehiclemodel.unladed_weight = vehicle.capwt.ToString();
                vehiclemodel.mvfile = vehicle.fileno;
            }
            else
            {

                return null;
            }


            return vehiclemodel;
        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> COCSubmit(M_PAIP mpaip, string transtype, string papinNo)
        {
            try
            {
                string mypapin;
                string myInvoiceNum = "NV" + Yfunction.generateSerial(7, myElement);
                string chrgno = "BH" + Yfunction.generateSerial(12, myElement);
                mpaip.PAREGISTER = rctpl_function.GetServerDate();//DateTime.Now;


                if (papinNo == "NO PAPIN" || papinNo == "")
                {               
                    mypapin = Yfunction.generateSerial(12, myElement);
                    mpaip.PAPIN = mypapin;
                }
                else {
                    mypapin = papinNo;
                    mpaip.PAPIN = papinNo;                 
                }

                TBL_VEHICLES Svehicle = new TBL_VEHICLES();
                Svehicle = rctpl_db.TBL_VEHICLES.Where(r => r.VEHICLE_CODE.Equals(mpaip.VEHICLE_TYPE)).FirstOrDefault();
                TempData["myvehicle"] = Svehicle;

                if (ModelState.IsValid)
                {
                    if (papinNo == "NO PAPIN" || papinNo == "")
                    {
                        //check if vehicle class is LTO or NON-LTO based on vehicle Class
                        if (mpaip.VEHICLE_CLASS == "PV" || mpaip.VEHICLE_CLASS == "CV" || mpaip.VEHICLE_CLASS == "MC")
                        {
                            mpaip.NON_LTO = "0";
                        }
                        else {
                            mpaip.NON_LTO = "1";
                        }


                    mpaip.COMP_ID="MLY";
                    //mpaip.VEHICLE_CLASS = Svehicle.SERVICE_CODE;
                        
                    mpaip.VEHICLE_TYPE = Svehicle.SERVICE_CODE;
                    mpaip.COVERAGE_YR = Svehicle.YEAR_COVERAGE.ToString();
                    mpaip.TAX_TYPE ="1";
                    mpaip.BLT_FILENO = "-";                     
                    rctpl_db.M_PAIP.Add(mpaip);
                    await rctpl_db.SaveChangesAsync();

                    }


                    Save_TbillChrgH(mypapin, chrgno);              
                    Save_TbillChrgD(chrgno, mpaip.VEHICLE_TYPE, mypapin, mpaip.PALNAME, myInvoiceNum, mpaip.PLACE_ISSUED, mpaip, transtype);
                    

                    return RedirectToAction("ClientPayments", "COC");
                }

            }
            catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("; ", ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage));
                throw new DbEntityValidationException(errorMessages);
            }

            return View();

        }

        private bool Save_TbillChrgH(string papin, string chrgno)
        {
            try
            {
                T_BILLCHRGH tbillchrgH = new T_BILLCHRGH();


                tbillchrgH.BCHCHRGNO = chrgno;
                tbillchrgH.PAPIN = papin;
                tbillchrgH.BCHDTE = rctpl_function.GetServerDate(); //DateTime.Now;
                tbillchrgH.USRID = Session["UName"].ToString();
                tbillchrgH.PKCD = "CTPL01";

                rctpl_db.T_BILLCHRGH.Add(tbillchrgH);
                rctpl_db.SaveChanges();

                return true;
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("; ", ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage));
                throw new DbEntityValidationException(errorMessages);
            }


        }

        private bool Save_TbillChrgD(string chrgno, string vehiclecode, string papin, string name, string invoiceNum, string region, M_PAIP mpaip, string transtype)
        {

            try
            {

                TBL_VEHICLES myvehicle = (TBL_VEHICLES)TempData["myvehicle"];

                T_BILLCHRGD tbillchrgD = new T_BILLCHRGD();
                tbillchrgD.BCHCHRGNO = chrgno;
                tbillchrgD.BCDSERIES = 1;
                tbillchrgD.BCDITMCD = myvehicle.SERVICE_CODE;
                tbillchrgD.BCDESC = myvehicle.SERVICE_TYPE;
                tbillchrgD.BCDQTY = 1; //- quantity , usually 1 lng plge, unless mgadd ng ibng charges     
                //tbillchrgD.BCDSP = not needed
                tbillchrgD.BCDPATCOVER = myvehicle.TOTAL_AMOUNT; // total amount po ito, same xa sa BCDPATBAL, ang difference lng nila ung, BCDPATBAL mg 0 kpg bayad na
                tbillchrgD.BCDPATBAL = myvehicle.TOTAL_AMOUNT;
                tbillchrgD.BCDTPPCOVER = 0; //for AR amount po sana to, kso ang treatment sa AR prang CASH n din, kya 0 value nlng muna
                tbillchrgD.DISCCD = "0"; //discount , if applicable 
                // tbillchrgD.TPCD = 0;TPCD - agent code
                tbillchrgD.BCDTPPTAG = "2"; //2 is for web
                //tbillchrgD.COLORNUM = if paid
                tbillchrgD.BCDSINO = invoiceNum; //invoice Number            
                tbillchrgD.DOC_STAMP = myvehicle.DOC_STAMP;
                tbillchrgD.E_VAT = myvehicle.VAT;
                tbillchrgD.LGT = myvehicle.LGT;
                tbillchrgD.SUM_INSIRED = myvehicle.SUM_INSURED;
                tbillchrgD.COMP_FEE = myvehicle.COMP_FEE;
                tbillchrgD.CERT_FEE = myvehicle.CERT_FEE;
                tbillchrgD.BASIC_PREMIUM = myvehicle.BASIC_PREMIUM;
                //  tbillchrgD.DOC_OR = OR Number
                //  tbillchrgD.COI = COI Number
                tbillchrgD.COMMISSION = 0;
                //tbillchrgD.AUTHEN_CODE
                tbillchrgD.TAX_AMT = 0;
                tbillchrgD.WITH_COM = "1";
                tbillchrgD.W_TAX = 0;
                tbillchrgD.W_TAX_NET = 0;
                //tbillchrgD.NO_DAYS for pro rata
                tbillchrgD.PRO_RATA = "0";
                tbillchrgD.REG_TYPE = "CTPL";
                tbillchrgD.TRANS_TYPE = transtype;
                tbillchrgD.REGION = region;
                tbillchrgD.VEHICLE_NUMBER = myvehicle.VEHICLE_NUMBER;
                tbillchrgD.VEHICLE_CODE = myvehicle.VEHICLE_CODE;
                tbillchrgD.REF_NUM = invoiceNum;



                rctpl_db.T_BILLCHRGD.Add(tbillchrgD);
                rctpl_db.SaveChanges();

                COCModel mycoc = new COCModel();

                mycoc.papin = papin;
                mycoc.name = name;
                mycoc.bchrgno = chrgno;
                mycoc.plateNo = mpaip.PLATE_NO;
                mycoc.chasis = mpaip.SERIAL_NO;
                mycoc.invoicenum = invoiceNum;
                mycoc.YearCoverage = myvehicle.PACKAGE_DESC;
                mycoc.DOC_STAMP = myvehicle.DOC_STAMP;
                mycoc.E_VAT = myvehicle.VAT;
                mycoc.LGT = myvehicle.LGT;
                mycoc.SUM_INSIRED = myvehicle.SUM_INSURED;
                mycoc.COMP_FEE = myvehicle.COMP_FEE;
                mycoc.CERT_FEE = myvehicle.CERT_FEE;
                mycoc.BASIC_PREMIUM = myvehicle.BASIC_PREMIUM;
                mycoc.VEHICLE_NUMBER = myvehicle.VEHICLE_NUMBER;
                mycoc.VEHICLE_CODE = myvehicle.VEHICLE_CODE;
                mycoc.TOTAL_AMOUNT = myvehicle.TOTAL_AMOUNT;
                
                TempData["COCData"] = mycoc;
                
                myclass.myMainCOC = mycoc;
               
                
               //Save data to T_SINO Tables
                Save_TSino(chrgno, invoiceNum, Convert.ToDecimal(myvehicle.TOTAL_AMOUNT));

            

                return true;
            }

            catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("; ", ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage));
                throw new DbEntityValidationException(errorMessages);
            }

        }

        private bool Save_TSino(string chrgno,  string invoiceNum, decimal totalamount)
        {
        
        try
            {
                T_SINO tsino = new T_SINO();

                tsino.TRKID = chrgno;
                tsino.SINO = invoiceNum;
                tsino.SIDATE = rctpl_function.GetServerDate();//DateTime.Now;
                tsino.SIUSER = Session["UName"].ToString();
                tsino.AMOUNT = totalamount;

                rctpl_db.T_SINO.Add(tsino);
                rctpl_db.SaveChanges();

            return true;
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("; ", ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage));
                throw new DbEntityValidationException(errorMessages);
            }


        }

        public ActionResult ClientPayments()
        {

            COCModel mycoc =  (COCModel)TempData["COCData"];
            return View("../COCPayments/Index", mycoc);
        }
 
        //public async Task<ActionResult> Payments(string REF_NUM)
        //{

        //    if (REF_NUM != null)
        //    {
                
        //        COCModel mycoc =  (from tweb in rctpl_db.TBL_WEBUSERS
        //                          join tchargeh in rctpl_db.T_BILLCHRGH on tweb.USERNAME equals tchargeh.USRID
        //                          join tcharged in rctpl_db.T_BILLCHRGD on tchargeh.BCHCHRGNO equals tcharged.BCHCHRGNO
        //                          join tmpaip in rctpl_db.M_PAIP on tchargeh.PAPIN equals tmpaip.PAPIN
        //                          join tvehicle in rctpl_db.TBL_VEHICLES on tcharged.VEHICLE_CODE equals tvehicle.VEHICLE_CODE
        //                           where tcharged.BCHCHRGNO.Equals(REF_NUM)
        //                          orderby tchargeh.BCHDTE
        //                          select new COCModel
        //                          {
        //                              papin = tmpaip.PAPIN,
        //                              name = tmpaip.PALNAME,
        //                              bchrgno = tcharged.BCHCHRGNO,
        //                              vehiclename = tcharged.BCDESC,
        //                              invoicenum = tcharged.REF_NUM,
        //                              package_desc = tvehicle.PACKAGE_DESC,
        //                              plateNo = tmpaip.PLATE_NO,
        //                              chasis = tmpaip.SERIAL_NO, 
        //                              DOC_STAMP = tcharged.DOC_STAMP,
        //                              E_VAT = tcharged.E_VAT,
        //                              LGT = tcharged.LGT,
        //                              SUM_INSIRED = tcharged.SUM_INSIRED,
        //                              COMP_FEE = tcharged.COMP_FEE,
        //                              CERT_FEE = tcharged.CERT_FEE,
        //                              BASIC_PREMIUM = tcharged.BASIC_PREMIUM,
        //                              VEHICLE_NUMBER = tcharged.VEHICLE_NUMBER,
        //                              VEHICLE_CODE = tcharged.VEHICLE_CODE,
        //                              TOTAL_AMOUNT = tcharged.BCDPATCOVER

        //                          }).FirstOrDefault();
        //        TempData["COCData"] = mycoc;
        //        //COCModel mycoc = (COCModel)TempData["COCData"];
        //        return View("../COCPayments/Index",  mycoc);
        //    }

        //    return View();

        //}

        public ActionResult PrintCOC(string REF_NUM)
        {
            if (REF_NUM != null)
            {

                COCModel mycoc = (from tweb in rctpl_db.TBL_WEBUSERS
                                  join tchargeh in rctpl_db.T_BILLCHRGH on tweb.USERNAME equals tchargeh.USRID
                                  join tcharged in rctpl_db.T_BILLCHRGD on tchargeh.BCHCHRGNO equals tcharged.BCHCHRGNO
                                  join tmpaip in rctpl_db.M_PAIP on tchargeh.PAPIN equals tmpaip.PAPIN
                                  join tvehicle in rctpl_db.TBL_VEHICLES on tcharged.VEHICLE_CODE equals tvehicle.VEHICLE_CODE
                                  where tcharged.BCHCHRGNO.Equals(REF_NUM)
                                  orderby tchargeh.BCHDTE
                                  select new COCModel
                                  {
                                      papin = tmpaip.PAPIN,
                                      name = tmpaip.PALNAME,
                                      bchrgno = tcharged.BCHCHRGNO,
                                      vehiclename = tcharged.BCDESC,
                                      invoicenum = tcharged.REF_NUM,
                                      package_desc = tvehicle.PACKAGE_DESC,
                                      plateNo = tmpaip.PLATE_NO,
                                      chasis = tmpaip.SERIAL_NO,
                                      DOC_STAMP = tcharged.DOC_STAMP,
                                      E_VAT = tcharged.E_VAT,
                                      LGT = tcharged.LGT,
                                      SUM_INSIRED = tcharged.SUM_INSIRED,
                                      COMP_FEE = tcharged.COMP_FEE,
                                      CERT_FEE = tcharged.CERT_FEE,
                                      BASIC_PREMIUM = tcharged.BASIC_PREMIUM,
                                      VEHICLE_NUMBER = tcharged.VEHICLE_NUMBER,
                                      VEHICLE_CODE = tcharged.VEHICLE_CODE,
                                      TOTAL_AMOUNT = tcharged.BCDPATCOVER

                                  }).FirstOrDefault();
                TempData["COCData"] = mycoc;
                //COCModel mycoc = (COCModel)TempData["COCData"];
                return View(mycoc);
            }

            return View();
          
        
        }

    }
}
