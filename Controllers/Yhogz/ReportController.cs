using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using RCTPL_WebProjects.Models;
using System.Web.Configuration;
using System.Data;
using System.Data.Entity;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

namespace RCTPL_WebProjects.Controllers.Yhogz
{
    public class ReportController : AdminBaseController
    {
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }
        private RCTPLEntities db = new RCTPLEntities();

        public ActionResult UserStats()
        {
            return View();
        }
        //User Stats = registered user for a given date and per region
        [HttpPost]
        public async Task<ActionResult> UserStats(string _sDate, string _eDate)
        {
            //Declarations of variables
            DateTime _sdt = Convert.ToDateTime(_sDate);
            DateTime _edt = Convert.ToDateTime(_eDate).AddDays(1);
            List<userDTO> _list = new List<userDTO>();
            string _path = "";

            _path = "/CrystalReport/rptUserStats.rpt";
            //string sname = Session["LName"].ToString();

            //Database Query
            var sql =await (from a in db.TBL_WEBUSERS
                       where a.DATE_VERIFIED >= _sdt && a.DATE_VERIFIED <= _edt
                       select a).ToListAsync();

          
                foreach (var r in sql)
                {                   
                    userDTO tbl = new userDTO();
                    tbl._dateV = r.DATE_VERIFIED.ToString();
                    tbl.userName = r.USERNAME;
                    tbl.region = getRegioName(r.REGION);                   
                    _list.Add(tbl);
                }
            

            ReportClass rptH = new ReportClass();
            rptH.FileName = Server.MapPath(_path);
            rptH.Load();           
            rptH.SetDataSource(_list);
            rptH.SetParameterValue("Petsa", _sdt.ToShortDateString() + " - " + _edt.ToShortDateString());
            Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }

        //Get REgion name
        public string getRegioName(string regionCode)
        {
            try
            {
                    var tr = (from a in db.M_REGION
                     where a.CODE == regionCode
                     select a.REGION_NAME).Single(); ;
                    return tr.ToString();
            }
            catch (Exception ex)
            {
                return "No Region Name";
            }           
            
        }

        public ActionResult StatsIncome()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> StatsIncome(string _sDate, string _eDate)
        {
            DateTime _sdt = Convert.ToDateTime(_sDate);
            DateTime _edt = Convert.ToDateTime(_eDate).AddDays(1);
            List<TBChDDTO> _list = new List<TBChDDTO>();
            string _path = "";
            _path = "/CrystalReport/rptStatsIncome.rpt";
            var sql = await (from a in db.T_BILLCHRGD
                              where a.DATE_PRINTED >= _sdt && a.DATE_PRINTED <= _edt
                              select a).ToListAsync();
            foreach (var r in sql)
                {
                    TBChDDTO tbl = new TBChDDTO();
                    tbl._id = r.BILLD_ID.ToString();
                    tbl._total =Convert.ToDecimal(r.BCDPATCOVER);
                    tbl.region = getRegioName(r.REGION);
                    _list.Add(tbl);
                }

                ReportClass rptH = new ReportClass();
                rptH.FileName = Server.MapPath(_path);
                rptH.Load();
                rptH.SetDataSource(_list);
                rptH.SetParameterValue("Petsa", _sdt.ToShortDateString() + " - " + _edt.ToShortDateString());
                Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
                                 
        }

        //Vehicle Stats = registered vehicle per region
        public async Task<ActionResult> VehicleStats(string _sDate, string _eDate)
        {
            DateTime _sdt = Convert.ToDateTime(_sDate);
            DateTime _edt = Convert.ToDateTime(_eDate).AddDays(1);
            var list = await (from a in db.M_PAIP where a.PAREGISTER >= _sdt && a.PAREGISTER <= _edt select a).ToListAsync();
            return null;
        }

        public async Task<ActionResult> PrintOR(string _orNo)
        {
            List<orDTO> _list = new List<orDTO>();
            string _path = "";
            _path = "/CrystalReport/rptOR.rpt";
            var sql = await (from tcol in db.T_COLLECTION from tbilld in db.T_BILLCHRGD
                             from tbillh in db.T_BILLCHRGH from mpai in db.M_PAIP
                             where tbilld.BCDSINO == tcol.COLREFNO &&
                             tbilld.COLORNUM == tcol.COLORNUM &&
                             tcol.COLORNUM == _orNo &&
                             tbilld.BCHCHRGNO == tbillh.BCHCHRGNO &&
                             tbillh.PAPIN == mpai.PAPIN
                             select new { T_COLLECTION = tcol, T_BILLCHRGD = tbilld, M_PAIP = mpai}).ToListAsync();                                                                                          
            foreach (var r in sql)
            {
                orDTO tbl = new orDTO();
                //tbl.colamt = r.T_COLLECTION.COLAMT.ToString();
                //tbl.basic_premium = r.T_BILLCHRGD.BASIC_PREMIUM.ToString();
                //tbl.doc_stamp = r.T_BILLCHRGD.DOC_STAMP.ToString();
                //tbl.comp_fee = r.T_BILLCHRGD.COMP_FEE.ToString();
                //tbl.cert_fee = r.T_BILLCHRGD.CERT_FEE.ToString();
                //tbl.tax_amtd = r.T_BILLCHRGD.TAX_AMT.ToString();
                //tbl.e_vat = r.T_BILLCHRGD.E_VAT.ToString();
                //tbl.lgt = r.T_BILLCHRGD.LGT.ToString();
                //tbl.coi = r.T_BILLCHRGD.COI.ToString();
                //tbl.colordte = r.T_COLLECTION.COLORDTE.ToString();
                //tbl.colpayor = r.T_COLLECTION.COLPAYOR.ToString();
                //tbl.colno = r.T_COLLECTION.COLNO.ToString();
                //tbl.clientcd = r.T_COLLECTION.CLIENTCD.ToString();
                //tbl.colpymtyp = r.T_COLLECTION.COLPYMTYP.ToString();
                //tbl.colornum = r.T_COLLECTION.COLORNUM.ToString();
                //tbl.usrid = r.T_COLLECTION.USRID.ToString();
                //tbl.with_tax = r.T_COLLECTION.WITH_TAX.ToString();
                //tbl.tax_amt = r.T_COLLECTION.TAX_AMT.ToString();
                //tbl.paddress = r.M_PAIP.PAADDRESS.ToString();
                //tbl.plateno = r.M_PAIP.PLATE_NO.ToString();
                //tbl.bcdesc = r.T_BILLCHRGD.BCDESC.ToString();
                //tbl.bcdsp = r.T_BILLCHRGD.BCDSP.ToString();
                //if (Session["UserType"].ToString() == "INTERNAL USER")
                //{
                //    tbl.cashier = Session["LName"].ToString();
                //}
                //else { tbl.cashier = ""; }

                //if (r.M_PAIP.BY_COMPNAME != "1")
                //{
                //    tbl.company = r.M_PAIP.PALNAME + ", " + r.M_PAIP.FIRST_NAME + " " + r.M_PAIP.MIDDLE_NAME;
                //}
                //else
                //{
                //    tbl.company = r.M_PAIP.COMPANY_NAME.ToString();
                //}
                //tbl.firstname = r.M_PAIP.FIRST_NAME.ToString();
                //tbl.middlename = r.M_PAIP.MIDDLE_NAME.ToString();
                //tbl.lastname = r.M_PAIP.PALNAME.ToString();
                //tbl.company = r.M_PAIP.COMPANY_NAME.ToString();

                tbl.papinNo = r.M_PAIP.PAPIN;
                tbl.assTin = r.M_PAIP.ASSURED_TIN;
                tbl.clientName = r.T_COLLECTION.COLPAYOR;
                tbl.clientAddress = r.M_PAIP.PAADDRESS;

                tbl.orDate = r.T_COLLECTION.COLORDTE.ToShortDateString();
                tbl.orNo = r.T_COLLECTION.COLORNUM;
                tbl.paymentType = r.T_COLLECTION.COLPYMTYP;

                tbl.basicPremium = Convert.ToDecimal(r.T_BILLCHRGD.BASIC_PREMIUM);
                tbl.eVat = Convert.ToDecimal(r.T_BILLCHRGD.E_VAT);
                tbl.otherCharges = Convert.ToDecimal(r.T_COLLECTION.COLAMT) +
                    Convert.ToDecimal(r.T_BILLCHRGD.DOC_STAMP) +
                    Convert.ToDecimal(r.T_BILLCHRGD.COMP_FEE) +
                    Convert.ToDecimal(r.T_BILLCHRGD.CERT_FEE) +
                    Convert.ToDecimal(r.T_BILLCHRGD.TAX_AMT) +
                    Convert.ToDecimal(r.T_BILLCHRGD.LGT) +
                    Convert.ToDecimal(r.T_COLLECTION.TAX_AMT) +
                    Convert.ToDecimal(r.T_BILLCHRGD.BCDSP);
                tbl.TotalAmount = tbl.basicPremium + tbl.eVat + tbl.otherCharges;
                tbl.cashierAgent = r.T_COLLECTION.USRID;
                _list.Add(tbl);
            }

            ReportClass rptH = new ReportClass();
            rptH.FileName = Server.MapPath(_path);
            rptH.Load();
            rptH.SetDataSource(_list);
            //rptH.SetParameterValue("Petsa", _sdt.ToShortDateString() + " - " + _edt.ToShortDateString());
            Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");

        }
    }
}