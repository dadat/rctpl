using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RCTPL_WebProjects.Models.VehicleDB;

namespace RCTPL_WebProjects.Controllers
{
    public class SearchVehicleController : BaseController
    {
        private VehicleQSEntities db = new VehicleQSEntities();

        // GET: SearchVehicle
        public async Task<ActionResult> Index()
        {
            return View(await db.VehicleDetails.ToListAsync());
        }

        // GET: SearchVehicle/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleDetail vehicleDetail = await db.VehicleDetails.FindAsync(id);
            if (vehicleDetail == null)
            {
                return HttpNotFound();
            }
            return View(vehicleDetail);
        }

        // GET: SearchVehicle/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SearchVehicle/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "icCode,CUSTOMER_ID,model,make,plateno,fileno,engineno,chassisno,color,denom,pd,cylinder,fuel,series,type,grosswt,netwt,shipwt,capwt,vclass,trdate,lto,status")] VehicleDetail vehicleDetail)
        {
            if (ModelState.IsValid)
            {
                db.VehicleDetails.Add(vehicleDetail);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(vehicleDetail);
        }

        // GET: SearchVehicle/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleDetail vehicleDetail = await db.VehicleDetails.FindAsync(id);
            if (vehicleDetail == null)
            {
                return HttpNotFound();
            }
            return View(vehicleDetail);
        }

        // POST: SearchVehicle/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "icCode,CUSTOMER_ID,model,make,plateno,fileno,engineno,chassisno,color,denom,pd,cylinder,fuel,series,type,grosswt,netwt,shipwt,capwt,vclass,trdate,lto,status")] VehicleDetail vehicleDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehicleDetail).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(vehicleDetail);
        }

        // GET: SearchVehicle/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleDetail vehicleDetail = await db.VehicleDetails.FindAsync(id);
            if (vehicleDetail == null)
            {
                return HttpNotFound();
            }
            return View(vehicleDetail);
        }

        // POST: SearchVehicle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            VehicleDetail vehicleDetail = await db.VehicleDetails.FindAsync(id);
            db.VehicleDetails.Remove(vehicleDetail);
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
