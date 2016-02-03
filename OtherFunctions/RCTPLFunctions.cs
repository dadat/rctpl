using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;


namespace RCTPL_WebProjects.OtherFunctions
{
    public class RCTPLFunctions
    {

        public DateTime GetServerDate()  
        {

            DateTime mydate = new DateTime();

            SqlConnection cn = new SqlConnection("Server=scciltfrb.vbactive.net;Database=RCTPL;User Id=rctpldb;Password=1012rctpl2015;");
            cn.Open();
            SqlCommand cmd = new SqlCommand("select GETDATE()", cn);
            SqlDataReader r = cmd.ExecuteReader();

            if (!r.HasRows) 
            {
                return mydate;
            }
               
            while (r.Read())
            {
                mydate = Convert.ToDateTime(r.GetValue(0));
            }

            cn.Close();
            return mydate;
        }


    }
}