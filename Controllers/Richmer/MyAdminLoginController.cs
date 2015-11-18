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

namespace RCTPL_WebProjects.Controllers.Richmer
{
    public class MyAdminLoginController : Controller
    {
        private RCTPLEntities db = new RCTPLEntities();


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string susername, string spassword)
        {

            TBL_SYSTEMUSERS results = await db.TBL_SYSTEMUSERS.Where(r=>r.USERNAME.Equals(susername) && r.PASSWORD.Equals(spassword)).FirstOrDefaultAsync();
            if (results != null)
            { 
            
                if (results.ACTIVE == true)
                {
                    Session["LoggedAdminUserId"] = results.ADMIN_ID;
                    Session["LAdminUsername"] = results.USERNAME;
                    Session["LAdminName"] = results.NAME;
                    return RedirectToAction("Index", "Dashboard");
                
                }
            
            }

            ViewBag.Message = "Invalid Username or Password, or please contact the admin for the activation of your account!";
            return View();
        }


        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "MyAdminLogin");    
        }



   

        // GET: MyAdminLogin/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_SYSTEMUSERS tBL_SYSTEMUSERS = await db.TBL_SYSTEMUSERS.FindAsync(id);
            if (tBL_SYSTEMUSERS == null)
            {
                return HttpNotFound();
            }
            return View(tBL_SYSTEMUSERS);
        }

        // GET: MyAdminLogin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MyAdminLogin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ADMIN_ID,USER_CODE,USERNAME,PASSWORD,E_PASSWORD,EMAIL,NAME,CONTACT_NUMBER,MAILING_ADDRESS,DATE_REGISTERED,REGISTERED_BY,ACTIVE,USER_TYPE")] TBL_SYSTEMUSERS tBL_SYSTEMUSERS)
        {
            if (ModelState.IsValid)
            {
                db.TBL_SYSTEMUSERS.Add(tBL_SYSTEMUSERS);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tBL_SYSTEMUSERS);
        }

        // GET: MyAdminLogin/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_SYSTEMUSERS tBL_SYSTEMUSERS = await db.TBL_SYSTEMUSERS.FindAsync(id);
            if (tBL_SYSTEMUSERS == null)
            {
                return HttpNotFound();
            }
            return View(tBL_SYSTEMUSERS);
        }

        // POST: MyAdminLogin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ADMIN_ID,USER_CODE,USERNAME,PASSWORD,E_PASSWORD,EMAIL,NAME,CONTACT_NUMBER,MAILING_ADDRESS,DATE_REGISTERED,REGISTERED_BY,ACTIVE,USER_TYPE")] TBL_SYSTEMUSERS tBL_SYSTEMUSERS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tBL_SYSTEMUSERS).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tBL_SYSTEMUSERS);
        }

        // GET: MyAdminLogin/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_SYSTEMUSERS tBL_SYSTEMUSERS = await db.TBL_SYSTEMUSERS.FindAsync(id);
            if (tBL_SYSTEMUSERS == null)
            {
                return HttpNotFound();
            }
            return View(tBL_SYSTEMUSERS);
        }

        // POST: MyAdminLogin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            TBL_SYSTEMUSERS tBL_SYSTEMUSERS = await db.TBL_SYSTEMUSERS.FindAsync(id);
            db.TBL_SYSTEMUSERS.Remove(tBL_SYSTEMUSERS);
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
