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
    public class PaymentSectionsController : Controller
    {
        private HMSTEntities db = new HMSTEntities();

        // GET: PaymentSections
        public ActionResult Index()
        {
            return View(db.PaymentSections.ToList());
        }

        // GET: PaymentSections/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentSection paymentSection = db.PaymentSections.Find(id);
            if (paymentSection == null)
            {
                return HttpNotFound();
            }
            return View(paymentSection);
        }

        // GET: PaymentSections/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaymentSections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Details")] PaymentSection paymentSection)
        {
            if (ModelState.IsValid)
            {
                db.PaymentSections.Add(paymentSection);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(paymentSection);
        }

        // GET: PaymentSections/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentSection paymentSection = db.PaymentSections.Find(id);
            if (paymentSection == null)
            {
                return HttpNotFound();
            }
            return View(paymentSection);
        }

        // POST: PaymentSections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Details")] PaymentSection paymentSection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymentSection).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(paymentSection);
        }

        // GET: PaymentSections/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentSection paymentSection = db.PaymentSections.Find(id);
            if (paymentSection == null)
            {
                return HttpNotFound();
            }
            return View(paymentSection);
        }

        // POST: PaymentSections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PaymentSection paymentSection = db.PaymentSections.Find(id);
            db.PaymentSections.Remove(paymentSection);
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
