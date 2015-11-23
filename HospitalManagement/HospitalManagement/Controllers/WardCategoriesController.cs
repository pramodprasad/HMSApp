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
    public class WardCategoriesController : Controller
    {
        private HMSDBEntities db = new HMSDBEntities();

        // GET: WardCategories
        public ActionResult Index()
        {
            return View(db.WardCategories.ToList());
        }

        // GET: WardCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WardCategory wardCategory = db.WardCategories.Find(id);
            if (wardCategory == null)
            {
                return HttpNotFound();
            }
            return View(wardCategory);
        }

        // GET: WardCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WardCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Status")] WardCategory wardCategory)
        {
            if (ModelState.IsValid)
            {
                db.WardCategories.Add(wardCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(wardCategory);
        }

        // GET: WardCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WardCategory wardCategory = db.WardCategories.Find(id);
            if (wardCategory == null)
            {
                return HttpNotFound();
            }
            return View(wardCategory);
        }

        // POST: WardCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Status")] WardCategory wardCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wardCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(wardCategory);
        }

        // GET: WardCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WardCategory wardCategory = db.WardCategories.Find(id);
            if (wardCategory == null)
            {
                return HttpNotFound();
            }
            return View(wardCategory);
        }

        // POST: WardCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WardCategory wardCategory = db.WardCategories.Find(id);
            db.WardCategories.Remove(wardCategory);
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
