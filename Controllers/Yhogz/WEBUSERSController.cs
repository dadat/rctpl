using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RCTPL_WebProjects.Models;
using System.Web.Configuration;


namespace RCTPL_WebProjects.Controllers.Yhogz
{
    public class WEBUSERSController : Controller
    {
        private RCTPLEntities db = new RCTPLEntities();
        clsYtiruceS.GetSerial Yfunction = new clsYtiruceS.GetSerial();
        string myElement = "ITDept6953069";

        public ActionResult StartPage()
        {
            return View();
        }

        // GET: WEBUSERS
        public  ActionResult Index()
        {            
            return View();
        }

        // GET: WEBUSERS
        [HttpPost]
        public async Task<ActionResult> Index(string Username, string password)
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(password))
            {
                ViewBag.isError = true;
                ViewBag.title = "Invalid Credentials";
                ViewBag.message = "Username or Password must not be null.";
                return View();
            }
            var userSQL = await (from a in db.TBL_WEBUSERS where (a.USERNAME == Username && a.PASSWORD == password && a.ACTIVE == true) || (a.USERNAME == Username && a.SHA_PASSWORD == password && a.ACTIVE ==true) select a).ToListAsync();

            if (userSQL.Count > 0 )
            {
                if (!string.IsNullOrEmpty(userSQL[0].PASSWORD))
                {
                    if (userSQL[0].PASSWORD == password)
                    {
                        Session["UName"] = userSQL[0].USERNAME;
                        Session["LoggedUserId"] = userSQL[0].USER_ID;
                        Session["LName"] = userSQL[0].FIRSTNAME;
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    if (userSQL[0].SHA_PASSWORD == password)
                    {
                        Session["UName"] = userSQL[0].USERNAME;
                        Session["LoggedUserId"] = userSQL[0].USER_ID;
                        Session["LName"] = userSQL[0].FIRSTNAME;
                        return RedirectToAction("Index", "Home");
                    }
                }
                Session["UName"] = userSQL[0].USERNAME;
                Session["LoggedUserId"] = userSQL[0].USER_ID;
                Session["LName"] = userSQL[0].FIRSTNAME;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.isError = true;
                ViewBag.title = "Invalid Credentials";
                ViewBag.message = "Please try again.";
                //ViewBag.errMessage = "Invalid Credentials";
                return View();

            }
        }

        //LOGOUT PLEASE
        public ActionResult Logout(){
            Session.RemoveAll();
            return RedirectToAction("StartPage", "WEBUSERS");
        }

        [HttpGet]
        public ActionResult ForgotPasswd()
        {
            return View();
        }
        //FORGOT PASSWORD
        [HttpPost]
        public async Task<ActionResult> ForgotPasswd(ForgotPasswdViewModel tblViewModel)
        {
            if (ModelState.IsValid)
            {
                TBL_WEBUSERS tbl = await db.TBL_WEBUSERS.FirstOrDefaultAsync(m => m.USERNAME == tblViewModel.Username && m.EMAIL == tblViewModel.Email);
                if (!(tbl == null))
                {
                    tbl.SHA_PASSWORD = Yfunction.generateRandomString(12, "ITDept6953069");
                    tbl.PASSWORD = string.Empty;
                    db.Entry(tbl).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    //send email
                    string _bod = string.Format("Dear Sir/Madam {0}, <BR/><BR/> Your password has changed. <BR/> Please login using the following credentials. <br/><br/>Username: {3} <br/>Password: {4} <br/><br/> Kindly replace your temporary password as soon as you login. <br/>Thank you!", tbl.FIRSTNAME + " " + tbl.LASTNAME, tbl.USERNAME, tbl.SHA_PASSWORD, tbl.USERNAME, tbl.SHA_PASSWORD);
                    sendEmail("RCTPL Account Settings Change", "Password Change Confirmation", _bod, tbl.EMAIL);
                    //view password change successful
                    return RedirectToAction("SentNewPassword", new { email = tbl.EMAIL });
                }
            }
            
                ViewBag.isError = true;
                ViewBag.title = "Invalid Credentials";
                ViewBag.message = "Verify that the information you have enter is correct.";
                return View(tblViewModel); 
            
        }

        public ActionResult SentNewPassword(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        // GET: WEBUSERS/Create
       
        public ActionResult Create()
        {
            //RegViewModel vModel = new RegViewModel();
            //vModel.isError = 0;
            return View();
        }

        // POST: WEBUSERS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "lastname,firstname,middlename,address,username,contactno,email,bCode,region,city")] RegViewModel tblUser)
            //string lname, string fname, string mname, string nostreet, string city, string region, string uname, string password, string cpassword, string email        
        {           
            RegViewModel vmodel = new RegViewModel();
            TBL_WEBUSERS tbl = new TBL_WEBUSERS();
            if (ModelState.IsValid)
            {
                TBL_WEBUSERS ifExist = await db.TBL_WEBUSERS.FirstOrDefaultAsync(m => m.USERNAME == tblUser.username);               


                if (!(ifExist == null))
                {                    
                    ViewBag.isError = true;
                    ViewBag.title = "Username already taken";
                    ViewBag.message = "Kindly choose different username.";
                    return View(vmodel);

                }

                ifExist = await db.TBL_WEBUSERS.FirstOrDefaultAsync(m => m.EMAIL == tblUser.email);
                if (!(ifExist == null))
                {
                    ViewBag.isError = true;
                    ViewBag.title = "Email already registered";
                    ViewBag.message = "Please choose different email address or use password recovery system.";                  
                    return View(vmodel);
                }
                
                
                    tbl.LASTNAME = tblUser.lastname;
                    tbl.FIRSTNAME = tblUser.firstname;
                    tbl.MIDDLENAME = tblUser.middlename;
                    tbl.MAILING_ADDRESS = tblUser.address;
                    tbl.CITY = tblUser.city;
                    tbl.REGION = tblUser.region;
                    tbl.USERNAME = tblUser.username;
                    tbl.CONTACT_NUMBER = tblUser.contactno;
                    //tbl.PASSWORD = password;
                    tbl.USER_CODE = Yfunction.generateSerial(9, "ITDept6953069");
                    tbl.SHA_PASSWORD = Yfunction.generateRandomString(12, "ITDept6953069");
                    if (!string.IsNullOrEmpty(tblUser.email))
                    {
                        tbl.EMAIL = tblUser.email;
                         //shaPass = Yfunction.generateRandomString(12, myElement);                    
                    }
                
                    DateTime now = DateTime.Now;
                    tbl.DATE_REGISTERED = now;
                    //tbl.BRANCH_CODE = tblUser.bCode;                   
                    db.TBL_WEBUSERS.Add(tbl);
                    await db.SaveChangesAsync();
                    string tempname = tbl.FIRSTNAME + " " + tbl.LASTNAME;
                    //string _bod = string.Format("Dear Sir/Madam {0}, <BR/><BR/> Your account is now registered to RCTPL Web App. Thank you. <BR/> Please click on the link to activate your registration: <a href=\"http://localhost:53822/WEBUSERS/CompletingRegistration/{1}/{2}\">Activate Registration</a> <br/><br/>Username: {3} <br/>Password: {4} <br/><br/> Kindly replace your temporary password as soon as you login. <br/>Thank you!", tempname, tbl.USERNAME, tbl.USER_CODE, tbl.USERNAME, tbl.USER_CODE);
                
                    string _bod = string.Format("Dear Sir/Madam {0}, <BR/><BR/> Your account is now registered to Allied Bankers Online RCTPL. Thank you. <BR/> Please click on the link to activate your registration: <a href=\"http://" + WebConfigurationManager.AppSettings["ServerIP"] + "/WEBUSERS/CompletingRegistration/{1}/{2}\">Activate Registration</a> <br/><br/>Username: {3} <br/>Password: {4} <br/><br/> Kindly replace your temporary password as soon as you login. <br/>Thank you!", tempname, tbl.USERNAME, tbl.SHA_PASSWORD, tbl.USERNAME, tbl.SHA_PASSWORD);
                    sendEmail("RCTPL Web Registration","Registration Confirmation",_bod, tblUser.email);
                    //vmodel.isError = 1;
                    //return View(vmodel);

                    return RedirectToAction("regSuccess", new { email =tbl.EMAIL });
            }
            ViewBag.isError = true;
            ViewBag.title = "Something went wrong";
            ViewBag.message = "Please check details of registration.";              
            return View(vmodel);
        }
               
        public  ActionResult regSuccess(string email)
        {
            ViewBag.email = email;
            return View();
        }

        bool sendEmail(string _title, string _subject, string _body, string _email)
        {
            try
            {
                //Send Email to new Registration
                System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                    new System.Net.Mail.MailAddress("emailaddresskopo@gmail.com", _title),
                    new System.Net.Mail.MailAddress(_email));
                m.Subject = _subject;
                m.Body = string.Format(_body);
                m.IsBodyHtml = true;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                smtp.Credentials = new System.Net.NetworkCredential("emailaddresskopo@gmail.com", "P3o6r1seyl");
                smtp.EnableSsl = true;
                smtp.Send(m);
                return true;
            }
            catch
            {
                System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                   new System.Net.Mail.MailAddress("emailaddresskopo@gmail.com", _title),
                   new System.Net.Mail.MailAddress(_email));
                m.Subject = _subject;
                m.Body = string.Format(_body);
                m.IsBodyHtml = true;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                smtp.Credentials = new System.Net.NetworkCredential("emailaddresskopo@gmail.com", "P3o6r1seyl");
                smtp.EnableSsl = true;
                smtp.Send(m);
                return true;
            }
               
        }

        //When link is clicked
        public async Task<ActionResult> CompletingRegistration(string _uname, string _str)
        {
            TBL_WEBUSERS tbl = new TBL_WEBUSERS();
            tbl = (from a in db.TBL_WEBUSERS where a.USERNAME==_uname && a.SHA_PASSWORD ==_str select a).Single();
            tbl.ACTIVE = true;
            DateTime now = DateTime.Now;
            tbl.DATE_VERIFIED = now;
            db.Entry(tbl).State = EntityState.Modified;
            await db.SaveChangesAsync();

            sendEmail("Account Activation", "Account Activated", "Username: "+tbl.USERNAME +" has been activated. <br/> You can now use the Username and Temporary Password provided to you. <br/> Thank you!", tbl.EMAIL);

            return View();
        }

       
       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
