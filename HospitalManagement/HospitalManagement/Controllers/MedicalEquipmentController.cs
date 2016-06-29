using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HMS.Entity;

namespace HospitalManagement.Controllers
{
    public class MedicalEquipmentController : Controller
    {
        private HMSTEntities db = new HMSTEntities();

        // GET: MedicalEquipment
        public async Task<ActionResult> Index()
        {
            return View(await db.MedicalEquipments.ToListAsync());
        }

        // GET: MedicalEquipment/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalEquipment medicalEquipment = await db.MedicalEquipments.FindAsync(id);
            if (medicalEquipment == null)
            {
                return HttpNotFound();
            }
            return View(medicalEquipment);
        }

        // GET: MedicalEquipment/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MedicalEquipment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,EquipmentName,Details,UnitPrice")] MedicalEquipment medicalEquipment)
        {
            if (ModelState.IsValid)
            {
                db.MedicalEquipments.Add(medicalEquipment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(medicalEquipment);
        }

        // GET: MedicalEquipment/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalEquipment medicalEquipment = await db.MedicalEquipments.FindAsync(id);
            if (medicalEquipment == null)
            {
                return HttpNotFound();
            }
            return View(medicalEquipment);
        }

        // POST: MedicalEquipment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,EquipmentName,Details,UnitPrice")] MedicalEquipment medicalEquipment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medicalEquipment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(medicalEquipment);
        }

        // GET: MedicalEquipment/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalEquipment medicalEquipment = await db.MedicalEquipments.FindAsync(id);
            if (medicalEquipment == null)
            {
                return HttpNotFound();
            }
            return View(medicalEquipment);
        }

        // POST: MedicalEquipment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            MedicalEquipment medicalEquipment = await db.MedicalEquipments.FindAsync(id);
            db.MedicalEquipments.Remove(medicalEquipment);
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
