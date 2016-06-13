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
    public class PatientStatusController : Controller
    {
        private HMSTEntities db = new HMSTEntities();

        // GET: /PatientStatus/
        public ActionResult Index()
        {
            return View(db.PatientStatus.ToList());
        }

        // GET: /PatientStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientStatu patientstatu = db.PatientStatus.Find(id);
            if (patientstatu == null)
            {
                return HttpNotFound();
            }
            return View(patientstatu);
        }

        // GET: /PatientStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /PatientStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Details,IsActive")] PatientStatu patientstatu)
        {
            if (ModelState.IsValid)
            {
                db.PatientStatus.Add(patientstatu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(patientstatu);
        }

        // GET: /PatientStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientStatu patientstatu = db.PatientStatus.Find(id);
            if (patientstatu == null)
            {
                return HttpNotFound();
            }
            return View(patientstatu);
        }

        // POST: /PatientStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Details,IsActive")] PatientStatu patientstatu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patientstatu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(patientstatu);
        }

        // GET: /PatientStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientStatu patientstatu = db.PatientStatus.Find(id);
            if (patientstatu == null)
            {
                return HttpNotFound();
            }
            return View(patientstatu);
        }

        // POST: /PatientStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PatientStatu patientstatu = db.PatientStatus.Find(id);
            db.PatientStatus.Remove(patientstatu);
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
