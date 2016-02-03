using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RCTPL_WebProjects.Models;
using RCTPL_WebProjects.Models.VehicleDB;
 using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
namespace RCTPL_WebProjects.Models.Rich
{
    public class COCModel
    {

        
        public string papin { get; set; }
        public string name { get; set; }
        public string bchrgno { get; set; }
        public string vehiclename { get; set; }
        public string invoicenum { get; set; }
        public string package_desc { get; set; }
        public string plateNo { get; set; }
        public string chasis { get; set; }
        public string YearCoverage { get; set; }
        public Nullable<decimal> DOC_STAMP { get; set; }
        public Nullable<decimal> E_VAT { get; set; }
        public Nullable<decimal> LGT { get; set; }
        public Nullable<decimal> SUM_INSIRED { get; set; }
        public Nullable<decimal> COMP_FEE { get; set; }
        public Nullable<decimal> CERT_FEE { get; set; }
        public Nullable<decimal> BASIC_PREMIUM { get; set; }
        public string VEHICLE_NUMBER { get; set; }
        public string VEHICLE_CODE { get; set; }
        public Nullable<decimal> TOTAL_AMOUNT { get; set; }

    }

   
}