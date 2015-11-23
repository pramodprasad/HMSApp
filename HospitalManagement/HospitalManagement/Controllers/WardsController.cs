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
    public class WardsController : Controller
    {
        private HMSDBEntities db = new HMSDBEntities();

        // GET: Wards
        public ActionResult Index()
        {
            var wards = db.Wards.Include(w => w.BranchDetail).Include(w => w.WardCategory);
            return View(wards.ToList());
        }

        // GET: Wards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ward ward = db.Wards.Find(id);
            if (ward == null)
            {
                return HttpNotFound();
            }
            return View(ward);
        }

        // GET: Wards/Create
        public ActionResult Create()
        {
            ViewBag.BranchDetail_ID = new SelectList(db.BranchDetails, "ID", "Name");
            ViewBag.WardCategory_ID = new SelectList(db.WardCategories.Where(w => w.Status == true), "ID", "Name");
            return View();
        }

        // POST: Wards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Category,TotalNoOfBed,OtherDetails,BranchDetail_ID")] Ward ward)
        {
            if (ModelState.IsValid)
            {
                db.Wards.Add(ward);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.WardCategory_ID = new SelectList(db.WardCategories.Where(w => w.Status == true), "ID", "Name");
            ViewBag.BranchDetail_ID = new SelectList(db.BranchDetails, "ID", "Name", ward.BranchDetail_ID);
            return View(ward);
        }

        // GET: Wards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ward ward = db.Wards.Find(id);
            if (ward == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchDetail_ID = new SelectList(db.BranchDetails, "ID", "Name", ward.BranchDetail_ID);
            ViewBag.WardCategory_ID = new SelectList(db.WardCategories.Where(w => w.Status == true), "ID", "Name", ward.WardCategory_ID);
            return View(ward);
        }

        // POST: Wards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Category,TotalNoOfBed,OtherDetails,BranchDetail_ID")] Ward ward)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ward).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BranchDetail_ID = new SelectList(db.BranchDetails, "ID", "Name", ward.BranchDetail_ID);
            ViewBag.WardCategory_ID = new SelectList(db.WardCategories.Where(w => w.Status == true), "ID", "Name", ward.WardCategory_ID);
            return View(ward);
        }

        // GET: Wards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ward ward = db.Wards.Find(id);
            if (ward == null)
            {
                return HttpNotFound();
            }
            return View(ward);
        }

        // POST: Wards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ward ward = db.Wards.Find(id);
            db.Wards.Remove(ward);
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
