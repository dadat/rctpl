using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RCTPL_WebProjects.Models
{
    public class SharedModel
    {

        public IEnumerable<TransactionHistoryModels> THM { get; set; }
        public changePasswdViewModel CPM { get; set; }

    }
}