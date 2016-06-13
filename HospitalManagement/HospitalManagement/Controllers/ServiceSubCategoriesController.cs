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
    public class ServiceSubCategoriesController : Controller
    {
        private HMSTEntities db = new HMSTEntities();

        // GET: ServiceSubCategories
        public ActionResult Index(int id)
        {
            var serviceSubCategories = db.ServiceSubCategories.Include(s => s.BranchDetail).Include(s => s.Service).Where(s => s.Service_ID == id);
            return View(serviceSubCategories.ToList());
        }

        // GET: ServiceSubCategories/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceSubCategory serviceSubCategory = db.ServiceSubCategories.Find(id);
            if (serviceSubCategory == null)
            {
                return HttpNotFound();
            }
            return View(serviceSubCategory);
        }

        // GET: ServiceSubCategories/Create
        public ActionResult Create(int id)
        {
            ServiceSubCategory model = new ServiceSubCategory();
            model.Service_ID = id;
            ViewBag.BranchDetail_ID = new SelectList(db.BranchDetails, "ID", "Name");
            //ViewBag.Service_ID = new SelectList(db.Services, "ID", "Name");
            return View(model);
        }

        // POST: ServiceSubCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,ServiceCharges,AverageDuration,BranchDetail_ID,Service_ID")] ServiceSubCategory serviceSubCategory)
        {
            if (ModelState.IsValid)
            {
                db.ServiceSubCategories.Add(serviceSubCategory);
                db.SaveChanges();
                return RedirectToAction("Index","Services");
            }

            ViewBag.BranchDetail_ID = new SelectList(db.BranchDetails, "ID", "Name", serviceSubCategory.BranchDetail_ID);
            //ViewBag.Service_ID = new SelectList(db.Services, "ID", "Name", serviceSubCategory.Service_ID);
            return View(serviceSubCategory);
        }

        // GET: ServiceSubCategories/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceSubCategory serviceSubCategory = db.ServiceSubCategories.Find(id);
            if (serviceSubCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchDetail_ID = new SelectList(db.BranchDetails, "ID", "Name", serviceSubCategory.BranchDetail_ID);
            ViewBag.Service_ID = new SelectList(db.Services, "ID", "Name", serviceSubCategory.Service_ID);
            return View(serviceSubCategory);
        }

        // POST: ServiceSubCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,ServiceCharges,AverageDuration,BranchDetail_ID,Service_ID")] ServiceSubCategory serviceSubCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviceSubCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Services");
            }
            ViewBag.BranchDetail_ID = new SelectList(db.BranchDetails, "ID", "Name", serviceSubCategory.BranchDetail_ID);
            ViewBag.Service_ID = new SelectList(db.Services, "ID", "Name", serviceSubCategory.Service_ID);
            return View(serviceSubCategory);
        }

        // GET: ServiceSubCategories/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceSubCategory serviceSubCategory = db.ServiceSubCategories.Find(id);
            if (serviceSubCategory == null)
            {
                return HttpNotFound();
            }
            return View(serviceSubCategory);
        }

        // POST: ServiceSubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ServiceSubCategory serviceSubCategory = db.ServiceSubCategories.Find(id);
            db.ServiceSubCategories.Remove(serviceSubCategory);
            db.SaveChanges();
            return RedirectToAction("Index", "Services");
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
