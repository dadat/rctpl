using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RCTPL_WebProjects.Models
{
    public class TransactionHistoryModels
    {
        public string PLATE_NO { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public Nullable<System.DateTime> BCHDTE { get; set; }
        public string MAKE { get; set; }
        public Nullable<decimal> BCDPATCOVER { get; set; }
        public string COLORNUM { get; set; }
        public Nullable<System.DateTime> DATEPAID { get; set; }
        public string REF_NUM { get; set; }
        public string FULLNAME { get; set; }
    }
}