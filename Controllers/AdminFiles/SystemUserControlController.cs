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
    public class SystemUserControlController : Controller
    {
        private RCTPLEntities3 db = new RCTPLEntities3();

        // GET: SystemUserControl
        public async Task<ActionResult> Index()
        {
            return View(await db.TBL_SYSTEMUSERS.ToListAsync());
        }

        // GET: SystemUserControl/Details/5
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

        [HttpGet]
        public ActionResult MyAction(string search)
        {
            try
            {
                if (search == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TBL_SYSTEMUSERS tblSysUsers = db.TBL_SYSTEMUSERS.SingleOrDefault(u => u.USERNAME == search);
                if (tblSysUsers == null)
                {
                    return HttpNotFound();
                }
                return View(tblSysUsers);
            }
            catch (Exception)
            {
                throw;
            }

        }

        // GET: SystemUserControl/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SystemUserControl/Create
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

        // GET: SystemUserControl/Edit/5
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

        // POST: SystemUserControl/Edit/5
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

        // GET: SystemUserControl/Delete/5
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

        // POST: SystemUserControl/Delete/5
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
