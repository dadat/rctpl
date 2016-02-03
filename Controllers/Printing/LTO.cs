using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RCTPL_WebProjects.Controllers.Printing
{
    public class LTO
    {
        public DateTime? BCHDTE { get; set; }
        public string PALNAME { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string CompanyName { get; set; }
        public char? ByCOMPNAME { get; set; }
        public decimal? BCDPATCOVER { get; set; }
        public int BCDSERIES { get; set; }
        public decimal? SUMINSIRED { get; set; }
        public decimal? BCDTPPCOVER { get; set; }
        public string BCHCHRGNO { get; set; }
        public string MODELYR { get; set; }
        public string COI { get; set; }
        public string AUTHENCODE { get; set; }
        public string BCDSINO { get; set; }
        public string PAADDRESS { get; set; }
        public string PLATENO { get; set; }
        public char? NONLTO { get; set; }
        public string MAKE { get; set; }
        public string SERIALNO { get; set; }
        public string COLOR { get; set; }
        public string UNWEIGHT { get; set; }
        public string COMPID { get; set; }
        public DateTime? INCEPTIONFROM { get; set; }
        public DateTime? INCEPTIONTO { get; set; }
        public string MOTORNO { get; set; }
        public string COLORNUM { get; set; }
        public string SERVICETYPE { get; set; }
        public string SEATINGCAPACITY { get; set; }
        public string MVFILENO { get; set; }
        public string PRNT { get; set; }
        public DateTime? DATEPRINTED { get; set; }
        public string COCSERIES { get; set; }
    }
}