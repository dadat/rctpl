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

//Wala
namespace RCTPL_WebProjects.Controllers.Yhogz
{
    public class WEBUSERSController : Controller
    {
        private RCTPLEntities db = new RCTPLEntities();
        clsYtiruceS.GetSerial Yfunction = new clsYtiruceS.GetSerial();
        clsYtiruceS.clsSecurity myEncrypt = new clsYtiruceS.clsSecurity();
        
        string myElement = "ITDept6953069";
        // GET: WEBUSERS
        public async Task<ActionResult> Index()
        {
            RegViewModel vModel = new RegViewModel();
            
            var branchSql = await (from a in db.M_BRANCH select a).ToListAsync();
            var regionSql = await (from b in db.M_REGION select b).ToListAsync();
            
            //vModel._list = new Dictionary<string, string>();
            //vModel._listRegion = new Dictionary<string, string>();
            GlobalVar.BranchList = new Dictionary<string, string>();
            GlobalVar.RegionList = new Dictionary<string, string>();
            foreach (var r in branchSql)
            {
                GlobalVar.BranchList.Add(r.BRANCH_CODE, r.BRANCH_NAME);                 
            }

            foreach (var rw in regionSql)
            {
                GlobalVar.RegionList.Add(rw.CODE, rw.REGION_NAME);                
            }
            vModel._list = GlobalVar.BranchList;
            vModel._listRegion = GlobalVar.RegionList;
            vModel.isError = 0;
            return View(vModel);
        }

        // GET: WEBUSERS      
        [HttpPost]
        public async Task<ActionResult> Index(string uname, string password)
        {
            if (string.IsNullOrEmpty(uname) || string.IsNullOrEmpty(password))
            {
                ViewBag.errMessage = "Invalid Credentials";
                return View();
            }

            myEncrypt.lscryptoKey = "ITDept6953069";
            string tempPass = myEncrypt.psEncrypt(password, myElement).ToString();
            var userSQL = await (from a in db.TBL_WEBUSERS where (a.USERNAME == uname && a.PASSWORD == tempPass && a.ACTIVE == true && a.COMP == "MLY") || (a.USERNAME == uname && a.SHA_PASSWORD == password && a.ACTIVE == true && a.COMP == "MLY") select a).ToListAsync();

            if (userSQL.Count > 0)
            {
                if (!string.IsNullOrEmpty(userSQL[0].PASSWORD))
                {
                    if (userSQL[0].PASSWORD == password)
                    {
                        Session["UName"] = userSQL[0].USERNAME;
                        Session["LoggedUserId"] = userSQL[0].USER_ID;
                        Session["LName"] = userSQL[0].FIRSTNAME;
                        Session["LoggedRegion"] = userSQL[0].REGION;
                        Session["BranchCode"] = userSQL[0].BRANCH_CODE;
                        Session["UserType"] = userSQL[0].USER_TYPE;
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
                        Session["LoggedRegion"] = userSQL[0].REGION;
                        Session["BranchCode"] = userSQL[0].BRANCH_CODE;
                        Session["UserType"] = userSQL[0].USER_TYPE;
                        return RedirectToAction("Index", "Home");
                    }
                }
                
                Session["UName"] = userSQL[0].USERNAME;
                Session["LoggedUserId"] = userSQL[0].USER_ID;
                Session["LName"] = userSQL[0].FIRSTNAME;
                Session["LoggedRegion"] = userSQL[0].REGION;
                Session["BranchCode"] = userSQL[0].BRANCH_CODE;
                Session["UserType"] = userSQL[0].USER_TYPE;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                RegViewModel vmod = new RegViewModel();             

              
                vmod._list = GlobalVar.BranchList;
                vmod._listRegion = GlobalVar.RegionList;
                vmod.isError = 2;
                vmod.errMessage = "Invalid Credentials";
                ViewBag.errMessage = "Invalid Credentials";
                return View(vmod);
            }
        }

        //LOGOUT PLEASE
        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "WEBUSERS");
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
            TBL_WEBUSERS tbl = await db.TBL_WEBUSERS.FirstOrDefaultAsync(m => (m.USERNAME.Equals(tblViewModel.Username) && m.EMAIL.Equals(tblViewModel.Email)));
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
                ViewBag.Status = 1;
                ViewBag.Title = "Your Password has changed";
                ViewBag.Message = "Kindly check your email for new Password. Thank you!";
                return View();
                //return RedirectToAction("", "");
            }
            ViewBag.Status = 2;
            ViewBag.Title = "Warning!";
            ViewBag.Message = "Invalid Credentials";
            return View();
        }

       

        // GET: Registration of new user
        public ActionResult Create()
        {
            RegViewModel vModel = new RegViewModel();
            vModel._list = GlobalVar.BranchList;
            vModel._listRegion = GlobalVar.RegionList;
            vModel.isError = 0;
            return View(vModel);
        }

        // POST: WEBUSERS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegViewModel tblUser) //[Bind(Include = "lastname,firstname,middlename,address,username,contactno,email,region,city")
        //string lname, string fname, string mname, string nostreet, string city, string region, string uname, string password, string cpassword, string email        
        {
            RegViewModel vmodel = new RegViewModel();
            TBL_WEBUSERS tbl = new TBL_WEBUSERS();
            if (ModelState.IsValid)
            {
                TBL_WEBUSERS ifExist = await db.TBL_WEBUSERS.FirstOrDefaultAsync(m => m.USERNAME == tblUser.username);



                if (!(ifExist == null))
                {
                    vmodel._list = GlobalVar.BranchList;
                    vmodel._listRegion = GlobalVar.RegionList;
                    vmodel.isError = 2;
                    vmodel.errMessage = "Username already taken. Kindly choose different username.";
                    return View(vmodel);

                }

                ifExist = await db.TBL_WEBUSERS.FirstOrDefaultAsync(m => m.EMAIL == tblUser.email);
                if (!(ifExist == null))
                {
                    vmodel._list = GlobalVar.BranchList;
                    vmodel._listRegion = GlobalVar.RegionList;
                    vmodel.isError = 2;
                    vmodel.errMessage = "Email already registered. Please choose different email address or use password recovery system.";
                    return View(vmodel);
                }


                tbl.LASTNAME = tblUser.lastname;
                tbl.FIRSTNAME = tblUser.firstname;
                tbl.MIDDLENAME = tblUser.middlename;
                tbl.MAILING_ADDRESS = tblUser.address;
                tbl.CITY = tblUser.city;
                tbl.COMP = "MLY";
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
                tbl.USER_TYPE = "CLIENT";
                tbl.BRANCH_CODE = "HardCoded Branch Code";
                db.TBL_WEBUSERS.Add(tbl);
                await db.SaveChangesAsync();
                string tempname = tbl.FIRSTNAME + " " + tbl.LASTNAME;               
                string _bod = string.Format("Dear Sir/Madam {0}, <BR/><BR/> Your account is now registered to RCTPL Web App. Thank you. <BR/> Please click on the link to activate your registration: <a href=\"https://" + WebConfigurationManager.AppSettings["ServerIP"] + "/WEBUSERS/CompletingRegistration/{1}/{2}\">Activate Registration</a> <br/><br/>Username: {3} <br/>Password: {4} <br/><br/> Kindly replace your temporary password as soon as you login. <br/>Thank you!", tempname, tbl.USERNAME, tbl.SHA_PASSWORD, tbl.USERNAME, tbl.SHA_PASSWORD);
                sendEmail("RCTPL Web Registration", "Registration Confirmation", _bod, tblUser.email);
                vmodel._list = GlobalVar.BranchList;
                vmodel._listRegion = GlobalVar.RegionList;
                vmodel.isError = 1;
                return View(vmodel);
            }
            vmodel._list = GlobalVar.BranchList;
            vmodel._listRegion = GlobalVar.RegionList;
            vmodel.isError = 2;
            vmodel.errMessage = "Something went wrong. Please click Register again for details.";
            return View(vmodel);
        }

        //send email to user
        bool sendEmail(string _title, string _subject, string _body, string _email)
        {
            try
            {
                
                System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                    new System.Net.Mail.MailAddress("malayan.rctpl@gmail.com", _title),
                    new System.Net.Mail.MailAddress(_email));
                m.Subject = _subject;
                m.Body = string.Format(_body);
                m.IsBodyHtml = true;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                smtp.Credentials = new System.Net.NetworkCredential("malayan.rctpl@gmail.com", "m2a2l2ayan");
                smtp.EnableSsl = true;
                smtp.Send(m);
                return true;
            }
            catch
            {
                try
                {   
                    System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                       new System.Net.Mail.MailAddress("malayan.rctpl@gmail.com", _title),
                       new System.Net.Mail.MailAddress(_email));
                    m.Subject = _subject;
                    m.Body = string.Format(_body);
                    m.IsBodyHtml = true;
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                    smtp.Credentials = new System.Net.NetworkCredential("malayan.rctpl@gmail.com", "m2a2l2ayan");
                    smtp.EnableSsl = true;
                    smtp.Send(m);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                
            }

        }

        //When activation link is clicked
        public async Task<ActionResult> CompletingRegistration(string _uname, string _str)
        {
            TBL_WEBUSERS tbl = new TBL_WEBUSERS();
            tbl = (from a in db.TBL_WEBUSERS where a.USERNAME == _uname && a.SHA_PASSWORD == _str select a).Single();
            tbl.ACTIVE = true;
            DateTime now = DateTime.Now;
            tbl.DATE_VERIFIED = now;
            db.Entry(tbl).State = EntityState.Modified;
            await db.SaveChangesAsync();

            sendEmail("Account Activation", "Account Activated", "Username: " + tbl.USERNAME + " has been activated. <br/> You can now use the Username and Temporary Password provided to you. <br/> Thank you!", tbl.EMAIL);

            return View();
        }

        //User change password setting
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePasswd(Models.changePasswdViewModel model)
        {            
            if (ModelState.IsValid)
            {
                if (model._uname == null)
                {
                    //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    ViewBag.title = "Invalid Username";
                    ViewBag.message = "Username must not be null. Please login first.";
                    ViewBag.isError = true;
                }
                try
                {
                    //TBL_WEBUSERS tbl = await db.TBL_WEBUSERS.SingleAsync(m => m.USERNAME.Equals(_uname) && m.PASSWORD.Equals(_currPass));
                    myEncrypt.lscryptoKey = myElement;
                    string tempPass = myEncrypt.psEncrypt(model.OldPassword, myElement);
                    TBL_WEBUSERS tbl = await db.TBL_WEBUSERS.Where(m => (m.USERNAME.Equals(model._uname) && m.PASSWORD.Equals(tempPass)) || (m.USERNAME.Equals(model._uname) && m.SHA_PASSWORD.Equals(model.OldPassword))).SingleAsync();//(from t in db.TBL_WEBUSERS where (t.USERNAME == _uname && t.PASSWORD == _currPass) || (t.USERNAME == _uname && t.SHA_PASSWORD == _currPass) select t).SingleAsync();

                    if (tbl == null)
                    {
                        //return HttpNotFound();
                        ViewBag.title = "Invalid Credentials";
                        ViewBag.message = "Verify that your current password input is correct";
                        ViewBag.isError = true;
                    }
                    else
                    {
                        tbl.PASSWORD = myEncrypt.psEncrypt(model.NewPassword, myElement);
                        tbl.SHA_PASSWORD = string.Empty;
                        db.Entry(tbl).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        //sendEmail("Password Changed", "Credentials Edited", "Dear " + tbl.FIRSTNAME + " " + tbl.LASTNAME + "<p>Your password has changed. Kindly contact your administrator if you never changed your password.</p>", tbl.EMAIL);
                        ViewBag.title = "Password has Changed";
                        ViewBag.message = "Your new password will take effect the next time you log in. Thank you.";
                        ViewBag.isError = false;
                    }

                }
                catch
                {
                    ViewBag.title = "Invalid Credentials";
                    ViewBag.message = "Verify that your current password is correct";
                    ViewBag.isError = true;
                }

            }
            else
            {
                ViewBag.title = "Invalid Credentials";
                ViewBag.message = "Verify that your current password is correct and your new password is the same as confirm new password";
                ViewBag.isError = true;
            }

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
