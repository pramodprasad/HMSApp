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
    public class VisitReasonsController : Controller
    {
        private HMSDBEntities db = new HMSDBEntities();

        // GET: VisitReasons
        public ActionResult Index()
        {
            return View(db.VisitReasons.ToList());
        }

        // GET: VisitReasons/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VisitReason visitReason = db.VisitReasons.Find(id);
            if (visitReason == null)
            {
                return HttpNotFound();
            }
            return View(visitReason);
        }

        // GET: VisitReasons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VisitReasons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,VisitPurpose,Details")] VisitReason visitReason)
        {
            if (ModelState.IsValid)
            {
                db.VisitReasons.Add(visitReason);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(visitReason);
        }

        // GET: VisitReasons/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VisitReason visitReason = db.VisitReasons.Find(id);
            if (visitReason == null)
            {
                return HttpNotFound();
            }
            return View(visitReason);
        }

        // POST: VisitReasons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,VisitPurpose,Details")] VisitReason visitReason)
        {
            if (ModelState.IsValid)
            {
                db.Entry(visitReason).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(visitReason);
        }

        // GET: VisitReasons/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VisitReason visitReason = db.VisitReasons.Find(id);
            if (visitReason == null)
            {
                return HttpNotFound();
            }
            return View(visitReason);
        }

        // POST: VisitReasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            VisitReason visitReason = db.VisitReasons.Find(id);
            db.VisitReasons.Remove(visitReason);
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
