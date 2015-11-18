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

namespace RCTPL_WebProjects.Controllers.AdminFiles
{
    public class TransactionHistoryController : Controller
    {
        private RCTPLEntities db = new RCTPLEntities();

        // GET: TransactionHistory
        //public async Task<ActionResult> Index()
        //{
        //    return View(await db.TBL_WEBUSERS.ToListAsync());
        //}
        private HttpContextBase Context { get; set; }
        public ActionResult Index()
        {
            try
            {
                var value = HttpContext.Session["UName"];
                if (Session["UName"] == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TBL_WEBUSERS tBL_WEBUSERS = db.TBL_WEBUSERS.SingleOrDefault(u => u.USERNAME == value);
                if (tBL_WEBUSERS == null)
                {
                    return HttpNotFound();
                }
                return View(tBL_WEBUSERS);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: TransactionHistory/Details/5
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

        // GET: TransactionHistory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TransactionHistory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "USER_ID,USER_CODE,USERNAME,PASSWORD,E_PASSWORD,SHA_PASSWORD,EMAIL,MIDDLENAME,FIRSTNAME,LASTNAME,CONTACT_NUMBER,DATE_REGISTERED,VERIFICATION_CODE,VERIFICATION_STATUS,DATE_VERIFIED,ACTIVE,REGION,MAILING_ADDRESS,CITY")] TBL_WEBUSERS tBL_WEBUSERS)
        {
            if (ModelState.IsValid)
            {
                db.TBL_WEBUSERS.Add(tBL_WEBUSERS);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tBL_WEBUSERS);
        }

        // GET: TransactionHistory/Edit/5
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

        // POST: TransactionHistory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "USER_ID,USER_CODE,USERNAME,PASSWORD,E_PASSWORD,SHA_PASSWORD,EMAIL,MIDDLENAME,FIRSTNAME,LASTNAME,CONTACT_NUMBER,DATE_REGISTERED,VERIFICATION_CODE,VERIFICATION_STATUS,DATE_VERIFIED,ACTIVE,REGION,MAILING_ADDRESS,CITY")] TBL_WEBUSERS tBL_WEBUSERS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tBL_WEBUSERS).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tBL_WEBUSERS);
        }

        // GET: TransactionHistory/Delete/5
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

        // POST: TransactionHistory/Delete/5
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
