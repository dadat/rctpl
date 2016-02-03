using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using RCTPL_WebProjects.Models;
using System.Data;
using System.Data.Entity;

namespace RCTPL_WebProjects.Controllers.Printing
{
    public class PrintPolicyController : Controller
    {

        private RCTPLEntities db = new RCTPLEntities();

        public ActionResult PrintLTO(string tBCDSINO)
        {
            var ListLTO = (from chrgd in db.T_BILLCHRGD
                           from mpaip in db.M_PAIP
                           from chrgh in db.T_BILLCHRGH
                           where chrgd.BCHCHRGNO == chrgh.BCHCHRGNO
                           && mpaip.PAPIN == chrgh.PAPIN
                           && chrgd.COLORNUM != null
                           && chrgd.BCDSINO == tBCDSINO
                           && mpaip.NON_LTO == "0"
                           select new
                           { chrgh.BCHDTE,
                           mpaip.PALNAME,
                           mpaip.FIRST_NAME,
                           mpaip.MIDDLE_NAME,
                           mpaip.COMPANY_NAME,
                           mpaip.BY_COMPNAME,
                           chrgd.BCDPATCOVER,
                           chrgd.BCDSERIES,
                           chrgd.SUM_INSIRED,
                           chrgd.BCDTPPCOVER,
                           chrgd.BCHCHRGNO,
                           mpaip.MODEL_YR,
                           chrgd.COI,
                           chrgd.AUTHEN_CODE,
                           chrgd.BCDSINO,
                           mpaip.PAADDRESS,
                           mpaip.PLATE_NO,
                           mpaip.NON_LTO,
                           mpaip.MAKE,
                           mpaip.SERIAL_NO,
                           mpaip.COLOR,
                           mpaip.UN_WEIGHT,
                           mpaip.COMP_ID,
                           chrgd.INCEPTION_FROM,
                           chrgd.INCEPTION_TO,
                           mpaip.MOTOR_NO,
                           chrgd.COLORNUM,
                           mpaip.SERVICE_TYPE,
                           mpaip.SEATING_CAPACITY,
                           mpaip.MV_FILENO,
                           chrgd.PRNT,
                           chrgd.DATE_PRINTED,
                           chrgd.COC_SERIES}).FirstOrDefault();

            LTO oLTO = new LTO();

            oLTO.AUTHENCODE = ListLTO.AUTHEN_CODE;
            oLTO.BCDPATCOVER = ListLTO.BCDPATCOVER;
            oLTO.BCDSERIES = ListLTO.BCDSERIES;
            oLTO.BCDSINO = ListLTO.BCDSINO;
            oLTO.BCDTPPCOVER = ListLTO.BCDTPPCOVER;
            oLTO.BCHCHRGNO = ListLTO.BCHCHRGNO;
            oLTO.BCHDTE = ListLTO.BCHDTE;
            oLTO.ByCOMPNAME = Convert.ToChar(ListLTO.BY_COMPNAME);
            oLTO.COCSERIES = ListLTO.COC_SERIES;
            oLTO.COI = ListLTO.COI;
            oLTO.COLOR = ListLTO.COLOR;
            oLTO.COLORNUM = ListLTO.COLORNUM;
            oLTO.CompanyName = ListLTO.COMPANY_NAME;
            oLTO.COMPID = ListLTO.COMP_ID;
            oLTO.DATEPRINTED = ListLTO.DATE_PRINTED;
            oLTO.FirstName = ListLTO.FIRST_NAME;
            oLTO.INCEPTIONFROM = ListLTO.INCEPTION_FROM;
            oLTO.INCEPTIONTO = ListLTO.INCEPTION_TO;
            oLTO.MAKE = ListLTO.MAKE;
            oLTO.MiddleName = ListLTO.MIDDLE_NAME;
            oLTO.MODELYR = ListLTO.MODEL_YR;
            oLTO.MOTORNO = ListLTO.MOTOR_NO;
            oLTO.MVFILENO = ListLTO.MV_FILENO;
            oLTO.NONLTO = Convert.ToChar(ListLTO.NON_LTO);
            oLTO.PAADDRESS = ListLTO.PAADDRESS;
            oLTO.PALNAME = ListLTO.PALNAME;
            oLTO.PLATENO = ListLTO.PLATE_NO;
            oLTO.PRNT = ListLTO.PRNT;
            oLTO.SEATINGCAPACITY = ListLTO.SEATING_CAPACITY;
            oLTO.SERIALNO = ListLTO.SERIAL_NO;
            oLTO.SERVICETYPE = ListLTO.SERVICE_TYPE;
            oLTO.SUMINSIRED = ListLTO.SUM_INSIRED;
            oLTO.UNWEIGHT = ListLTO.UN_WEIGHT;

            return View(oLTO);
        }

        public ActionResult PrintNONLTO(string tBCDSINO)
        {
            var ListLTO = (from chrgd in db.T_BILLCHRGD
                           from mpaip in db.M_PAIP
                           from chrgh in db.T_BILLCHRGH
                           where chrgd.BCHCHRGNO == chrgh.BCHCHRGNO
                           && mpaip.PAPIN == chrgh.PAPIN
                           && chrgd.COLORNUM != null
                           && chrgd.BCDSINO == tBCDSINO
                           && mpaip.NON_LTO == "1"
                           select new
                           {
                               chrgh.BCHDTE,
                               mpaip.PALNAME,
                               mpaip.FIRST_NAME,
                               mpaip.MIDDLE_NAME,
                               mpaip.COMPANY_NAME,
                               mpaip.BY_COMPNAME,
                               chrgd.BCDPATCOVER,
                               chrgd.BCDSERIES,
                               chrgd.SUM_INSIRED,
                               chrgd.BCDTPPCOVER,
                               chrgd.BCHCHRGNO,
                               mpaip.MODEL_YR,
                               chrgd.COI,
                               chrgd.AUTHEN_CODE,
                               chrgd.BCDSINO,
                               mpaip.PAADDRESS,
                               mpaip.PLATE_NO,
                               mpaip.NON_LTO,
                               mpaip.MAKE,
                               mpaip.SERIAL_NO,
                               mpaip.COLOR,
                               mpaip.UN_WEIGHT,
                               mpaip.COMP_ID,
                               chrgd.INCEPTION_FROM,
                               chrgd.INCEPTION_TO,
                               mpaip.MOTOR_NO,
                               chrgd.COLORNUM,
                               mpaip.SERVICE_TYPE,
                               mpaip.SEATING_CAPACITY,
                               mpaip.MV_FILENO,
                               chrgd.PRNT,
                               chrgd.DATE_PRINTED,
                               chrgd.COC_SERIES
                           }).FirstOrDefault();

            LTO oLTO = new LTO();

            oLTO.AUTHENCODE = ListLTO.AUTHEN_CODE;
            oLTO.BCDPATCOVER = ListLTO.BCDPATCOVER;
            oLTO.BCDSERIES = ListLTO.BCDSERIES;
            oLTO.BCDSINO = ListLTO.BCDSINO;
            oLTO.BCDTPPCOVER = ListLTO.BCDTPPCOVER;
            oLTO.BCHCHRGNO = ListLTO.BCHCHRGNO;
            oLTO.BCHDTE = ListLTO.BCHDTE;
            oLTO.ByCOMPNAME = Convert.ToChar(ListLTO.BY_COMPNAME);
            oLTO.COCSERIES = ListLTO.COC_SERIES;
            oLTO.COI = ListLTO.COI;
            oLTO.COLOR = ListLTO.COLOR;
            oLTO.COLORNUM = ListLTO.COLORNUM;
            oLTO.CompanyName = ListLTO.COMPANY_NAME;
            oLTO.COMPID = ListLTO.COMP_ID;
            oLTO.DATEPRINTED = ListLTO.DATE_PRINTED;
            oLTO.FirstName = ListLTO.FIRST_NAME;
            oLTO.INCEPTIONFROM = ListLTO.INCEPTION_FROM;
            oLTO.INCEPTIONTO = ListLTO.INCEPTION_TO;
            oLTO.MAKE = ListLTO.MAKE;
            oLTO.MiddleName = ListLTO.MIDDLE_NAME;
            oLTO.MODELYR = ListLTO.MODEL_YR;
            oLTO.MOTORNO = ListLTO.MOTOR_NO;
            oLTO.MVFILENO = ListLTO.MV_FILENO;
            oLTO.NONLTO = Convert.ToChar(ListLTO.NON_LTO);
            oLTO.PAADDRESS = ListLTO.PAADDRESS;
            oLTO.PALNAME = ListLTO.PALNAME;
            oLTO.PLATENO = ListLTO.PLATE_NO;
            oLTO.PRNT = ListLTO.PRNT;
            oLTO.SEATINGCAPACITY = ListLTO.SEATING_CAPACITY;
            oLTO.SERIALNO = ListLTO.SERIAL_NO;
            oLTO.SERVICETYPE = ListLTO.SERVICE_TYPE;
            oLTO.SUMINSIRED = ListLTO.SUM_INSIRED;
            oLTO.UNWEIGHT = ListLTO.UN_WEIGHT;

            return View(oLTO);
        }

    }
}