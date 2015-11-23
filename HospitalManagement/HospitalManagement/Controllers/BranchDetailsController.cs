using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HMS.Entity;
using Microsoft.AspNet.Identity;
using HospitalManagement.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HospitalManagement.Controllers
{
    public class BranchDetailsController : Controller
    {
        private HMSDBEntities db = new HMSDBEntities();

        // GET: BranchDetails
        public ActionResult Index()
        {
            var branchDetails = db.BranchDetails.Include(b => b.City);
            return View(branchDetails.ToList());
        }

        // GET: BranchDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BranchDetail branchDetail = db.BranchDetails.Find(id);
            if (branchDetail == null)
            {
                return HttpNotFound();
            }
            return View(branchDetail);
        }

        // GET: BranchDetails/Create
        public ActionResult Create()
        {
            ViewBag.City_ID = new SelectList(db.Cities, "ID", "name");
            var currentUserId = User.Identity.GetUserId();

            BranchDetail model = new BranchDetail();
            if (currentUserId != null)
            {
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var customerId = manager.FindById(currentUserId).HMSEmpID;
                model.CreatedBy = customerId;
            }
            else
            {
                model.CreatedBy = 0;
            }

            return View(model);
        }

        // POST: BranchDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Address,PhoneNoPri,PhoneNoSec,MobileNo,FaxNo,EmailInfo,EmailCustomerCare,WebAddress,Status,CreatedBy,City_ID")] BranchDetail branchDetail)
        {
            if (ModelState.IsValid)
            {
                branchDetail.CreatedOn = DateTime.Now;
                db.BranchDetails.Add(branchDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.City_ID = new SelectList(db.Cities, "ID", "name", branchDetail.City_ID);
            return View(branchDetail);
        }

        // GET: BranchDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BranchDetail branchDetail = db.BranchDetails.Find(id);
            if (branchDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.City_ID = new SelectList(db.Cities, "ID", "name", branchDetail.City_ID);
            return View(branchDetail);
        }

        // POST: BranchDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Address,PhoneNoPri,PhoneNoSec,MobileNo,FaxNo,EmailInfo,EmailCustomerCare,WebAddress,Status,CreatedOn,CreatedBy,City_ID")] BranchDetail branchDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(branchDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.City_ID = new SelectList(db.Cities, "ID", "name", branchDetail.City_ID);
            return View(branchDetail);
        }

        // GET: BranchDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BranchDetail branchDetail = db.BranchDetails.Find(id);
            if (branchDetail == null)
            {
                return HttpNotFound();
            }
            return View(branchDetail);
        }

        // POST: BranchDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BranchDetail branchDetail = db.BranchDetails.Find(id);
            db.BranchDetails.Remove(branchDetail);
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
