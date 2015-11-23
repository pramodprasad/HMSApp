using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HMS.Entity;

namespace HospitalManagement.Controllers
{
    public class PatientTypesController : Controller
    {
        private HMSDBEntities db = new HMSDBEntities();

        // GET: PatientTypes
        public ActionResult Index()
        {
            return View(db.PatientTypes.ToList());
        }

        // GET: PatientTypes/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientType patientType = db.PatientTypes.Find(id);
            if (patientType == null)
            {
                return HttpNotFound();
            }
            return View(patientType);
        }

        // GET: PatientTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PatientTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Type,Details")] PatientType patientType)
        {
            if (ModelState.IsValid)
            {
                db.PatientTypes.Add(patientType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(patientType);
        }

        // GET: PatientTypes/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientType patientType = db.PatientTypes.Find(id);
            if (patientType == null)
            {
                return HttpNotFound();
            }
            return View(patientType);
        }

        // POST: PatientTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Type,Details")] PatientType patientType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patientType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(patientType);
        }

        // GET: PatientTypes/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientType patientType = db.PatientTypes.Find(id);
            if (patientType == null)
            {
                return HttpNotFound();
            }
            return View(patientType);
        }

        // POST: PatientTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            PatientType patientType = db.PatientTypes.Find(id);
            db.PatientTypes.Remove(patientType);
            db.SaveChanges();
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
