//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RCTPL_WebProjects.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class T_SINO
    {
        public long SINO_ID { get; set; }
        public string TRKID { get; set; }
        public string SINO { get; set; }
        public System.DateTime SIDATE { get; set; }
        public string SIUSER { get; set; }
        public Nullable<byte> SIAGE { get; set; }
        public string SICANCEL { get; set; }
        public string SIREASON { get; set; }
        public Nullable<System.DateTime> SIDTECANCEL { get; set; }
        public string CANUSER { get; set; }
        public Nullable<decimal> AMOUNT { get; set; }
        public string DOCPRINT { get; set; }
        public string DUMPED { get; set; }
        public string COI { get; set; }
        public string COC { get; set; }
        public string UPLOADED_CODE { get; set; }
        public Nullable<System.DateTime> UPLOADED_DATE { get; set; }
    }
}