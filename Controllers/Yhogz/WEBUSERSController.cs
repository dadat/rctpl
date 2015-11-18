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

namespace RCTPL_WebProjects.Controllers.Yhogz
{
    public class WEBUSERSController : Controller
    {
        private RCTPLEntities db = new RCTPLEntities();
        clsYtiruceS.GetSerial Yfunction = new clsYtiruceS.GetSerial();
        string myElement = "ITDept6953069";
        // GET: WEBUSERS
        public async Task<ActionResult> Index()
        {
            //return View(await db.TBL_WEBUSERS.ToListAsync());
            StrViewModel msg = new StrViewModel();
            msg.message = "";
            return View(msg);
        }

        // GET: WEBUSERS
        [HttpPost]
        public async Task<ActionResult> Index(string uname, string password)
        {
            var userSQL = await (from a in db.TBL_WEBUSERS where a.USERNAME == uname select a).ToListAsync();

            //var sql = await (from a in db.TBL_WEBUSERS
            //           where a.USERNAME == uname && a.PASSWORD == password
            //           select a).ToListAsync();

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
                    if (userSQL[0].USER_CODE == password)
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
                StrViewModel vm = new StrViewModel();
                vm.message = "Invalid Credentials";
                return View(vm);

            }
            
            //return View(await db.TBL_WEBUSERS.ToListAsync());
        }

        //LOGOUT PLEASE
        public ActionResult Logout(){
            Session.RemoveAll();
            return RedirectToAction("Index", "WEBUSERS");
        }

        // GET: WEBUSERS/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_WEBUSERS tBL_WEBUSERS = await db.TBL_WEBUSERS.FindAsync(id);
            if (tBL_WEBUSERS == null)
            {
                return HttpNotFound();
            }
            return View(tBL_WEBUSERS);
        }

        // GET: WEBUSERS/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WEBUSERS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string lname, string fname, string mname, string nostreet, string city, string region, string uname, string password, string cpassword, string email)
        {
            TBL_WEBUSERS tbl = new TBL_WEBUSERS();
            if (ModelState.IsValid)
            {             
                
                tbl.LASTNAME = lname;
                tbl.FIRSTNAME = fname;
                tbl.MIDDLENAME = mname;
                tbl.MAILING_ADDRESS = nostreet;
                tbl.CITY = city;
                tbl.REGION = region;
                tbl.USERNAME = uname;
                //tbl.PASSWORD = password;
                tbl.USER_CODE = Yfunction.generateSerial(9, "ITDept6953069");
               
                if (!string.IsNullOrEmpty(email))
                {
                    tbl.EMAIL = email;
                     //shaPass = Yfunction.generateRandomString(12, myElement);                    
                }
                
                DateTime now = DateTime.Now;
                tbl.DATE_REGISTERED = now;      
                db.TBL_WEBUSERS.Add(tbl);
                await db.SaveChangesAsync();
                string tempname = tbl.FIRSTNAME + " " + tbl.LASTNAME;
                string _bod = string.Format("Dear Sir/Madam {0}, <BR/><BR/> Your account is now registered to RCTPL Web App. Thank you. <BR/> Please click on the link to activate your registration: <a href=\"http://localhost:53822/WEBUSERS/CompletingRegistration/{1}/{2}\">Activate Registration</a> <br/><br/>Username: {3} <br/>Password: {4} <br/><br/> Kindly replace your temporary password as soon as you login. <br/>Thank you!", tempname, tbl.USERNAME, tbl.USER_CODE, tbl.USERNAME, tbl.SHA_PASSWORD);
                sendEmail("RCTPL Web Registration","Registration Confirmation",_bod, email);
               
                //Send Email to new Registration
                //System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                //    new System.Net.Mail.MailAddress("emailaddresskopo@gmail.com", "RCTPL Web Registration"),
                //    new System.Net.Mail.MailAddress(email));
                //m.Subject = "Registration Confirmation";
                //m.Body = string.Format("Dear {0}<BR/><BR/>{1} is now registered to RCTPL Web App. Thank you.<BR/>Please click on the link to activate your registration: <a href=\"/ctest.com/\">Activate Registration</a>", "", "");
                //m.IsBodyHtml = true;
                //System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                //smtp.Credentials = new System.Net.NetworkCredential("emailaddresskopo@gmail.com", "P3o6r1seyl");
                //smtp.EnableSsl = true;
                //smtp.Send(m);

                return View();
            }
            return View();
        }

        bool sendEmail(string _title, string _subject, string _body, string _email){
           
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

        //When link is clicked
        public async Task<ActionResult> CompletingRegistration(string _uname, string _str)
        {
            TBL_WEBUSERS tbl = new TBL_WEBUSERS();
            tbl = (from a in db.TBL_WEBUSERS where a.USERNAME==_uname && a.USER_CODE ==_str select a).Single();
            tbl.ACTIVE = true;
            DateTime now = DateTime.Now;
            tbl.DATE_VERIFIED = now;
            db.Entry(tbl).State = EntityState.Modified;
            await db.SaveChangesAsync();

            sendEmail("Account Activation", "Account Activated", "Username: "+tbl.USERNAME +" has been activated. <br/> You can now use the Username and Temporary Password provided to you. <br/> Thank you!", tbl.EMAIL);

            return View();
        }

        // GET Change password
        //public async Task<ActionResult> ChangePasswd(string uname)
        //{
        //    if (uname == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TBL_WEBUSERS tBL_WEBUSERS = await db.TBL_WEBUSERS.SingleAsync(m => m.USERNAME.Equals(uname));
        //    if (tBL_WEBUSERS == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tBL_WEBUSERS);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePasswd(string _uname, string _currPass, string newPass)
        {
            StrViewModel strTemp = new StrViewModel();
            if (_uname == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                strTemp.title = "Invalid Username";
                strTemp.message = "Username must not be null. Please login first.";
                strTemp.isError = true;               
            }
            try
            {
                TBL_WEBUSERS tbl = await db.TBL_WEBUSERS.SingleAsync(m => m.USERNAME.Equals(_uname) && m.PASSWORD.Equals(_currPass));
                if (tbl == null)
                {
                    //return HttpNotFound();
                    strTemp.title = "Invalid Credentials";
                    strTemp.message = "Verify that your current password input is correct";
                    strTemp.isError = true;                
                }
                else
                {
                    tbl.PASSWORD = newPass;
                    db.Entry(tbl).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    sendEmail("Password Changed", "Credentials Edited", "Dear " + tbl.FIRSTNAME + " " + tbl.LASTNAME + "<p>Your password has changed. Kindly contact your administrator if you never changed your password.</p>", tbl.EMAIL);
                    strTemp.title = "Password has Changed";
                    strTemp.message = "Your new password will take effect the next time you log in. Thank you.";
                    strTemp.isError = false; 
                }

            }
            catch (Exception ex)
            {
                strTemp.title = "Invalid Credentials";
                strTemp.message = "Verify that your current password input is correct";
                strTemp.isError = true;     
                

            }
            
            
            return View(strTemp);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ChangePasswd(string _uname, string _currPass, string newPass)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (_uname == null)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }
        //        TBL_WEBUSERS tbl = await db.TBL_WEBUSERS.SingleAsync(m => m.USERNAME.Equals(_uname) && m.PASSWORD.Equals(_currPass));
        //        if (tbl == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        else
        //        {
        //            tbl.PASSWORD = newPass;
        //            db.Entry(tbl).State = EntityState.Modified;
        //            await db.SaveChangesAsync();
        //        }
        //        return View(tbl);
        //    }

        //    return View();
            
        //}




        // GET: WEBUSERS/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_WEBUSERS tBL_WEBUSERS = await db.TBL_WEBUSERS.FindAsync(id);
            if (tBL_WEBUSERS == null)
            {
                return HttpNotFound();
            }
            return View(tBL_WEBUSERS);
        }

        // POST: WEBUSERS/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "USER_ID,USER_CODE,USERNAME,PASSWORD,E_PASSWORD,SHA_PASSWORD,EMAIL,NAME,CONTACT_NUMBER,MAILING_ADDRESS,DATE_REGISTERED,VERIFICATION_CODE,VERIFICATION_STATUS,DATE_VERIFIED,ACTIVE,REGION")] TBL_WEBUSERS tBL_WEBUSERS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tBL_WEBUSERS).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tBL_WEBUSERS);
        }

        // GET: WEBUSERS/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_WEBUSERS tBL_WEBUSERS = await db.TBL_WEBUSERS.FindAsync(id);
            if (tBL_WEBUSERS == null)
            {
                return HttpNotFound();
            }
            return View(tBL_WEBUSERS);
        }

        // POST: WEBUSERS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            TBL_WEBUSERS tBL_WEBUSERS = await db.TBL_WEBUSERS.FindAsync(id);
            db.TBL_WEBUSERS.Remove(tBL_WEBUSERS);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
