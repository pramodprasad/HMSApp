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
    public class LabCategoriesController : Controller
    {
        private HMSDBEntities db = new HMSDBEntities();

        // GET: LabCategories
        public ActionResult Index()
        {
            return View(db.LabCategories.ToList());
        }

        // GET: LabCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LabCategory labCategory = db.LabCategories.Find(id);
            if (labCategory == null)
            {
                return HttpNotFound();
            }
            return View(labCategory);
        }

        // GET: LabCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LabCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Status")] LabCategory labCategory)
        {
            if (ModelState.IsValid)
            {
                db.LabCategories.Add(labCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(labCategory);
        }

        // GET: LabCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LabCategory labCategory = db.LabCategories.Find(id);
            if (labCategory == null)
            {
                return HttpNotFound();
            }
            return View(labCategory);
        }

        // POST: LabCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Status")] LabCategory labCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(labCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(labCategory);
        }

        // GET: LabCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LabCategory labCategory = db.LabCategories.Find(id);
            if (labCategory == null)
            {
                return HttpNotFound();
            }
            return View(labCategory);
        }

        // POST: LabCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LabCategory labCategory = db.LabCategories.Find(id);
            db.LabCategories.Remove(labCategory);
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
