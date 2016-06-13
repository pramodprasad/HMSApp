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
    public class SpecializationController : Controller
    {
        private HMSTEntities db = new HMSTEntities();

        // GET: /Specialization/
        public ActionResult Index()
        {
            return View(db.Specializations.ToList());
        }

        // GET: /Specialization/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Specialization specialization = db.Specializations.Find(id);
            if (specialization == null)
            {
                return HttpNotFound();
            }
            return View(specialization);
        }

        // GET: /Specialization/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Specialization/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Name,Status")] Specialization specialization)
        {
            if (ModelState.IsValid)
            {
                db.Specializations.Add(specialization);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(specialization);
        }

        // GET: /Specialization/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Specialization specialization = db.Specializations.Find(id);
            if (specialization == null)
            {
                return HttpNotFound();
            }
            return View(specialization);
        }

        // POST: /Specialization/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Name,Status")] Specialization specialization)
        {
            if (ModelState.IsValid)
            {
                db.Entry(specialization).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(specialization);
        }

        // GET: /Specialization/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Specialization specialization = db.Specializations.Find(id);
            if (specialization == null)
            {
                return HttpNotFound();
            }
            return View(specialization);
        }

        // POST: /Specialization/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Specialization specialization = db.Specializations.Find(id);
            db.Specializations.Remove(specialization);
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
