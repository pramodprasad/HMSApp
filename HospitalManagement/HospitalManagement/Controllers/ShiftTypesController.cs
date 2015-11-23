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
    public class ShiftTypesController : Controller
    {
        private HMSDBEntities db = new HMSDBEntities();

        // GET: ShiftTypes
        public ActionResult Index()
        {
            var shiftTypes = db.ShiftTypes;
            return View(shiftTypes.ToList());
        }

        // GET: ShiftTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShiftType shiftType = db.ShiftTypes.Find(id);
            if (shiftType == null)
            {
                return HttpNotFound();
            }
            return View(shiftType);
        }

        // GET: ShiftTypes/Create
        public ActionResult Create()
        {
            //ViewBag.BranchDetail_ID = new SelectList(db.BranchDetails, "ID", "Name");
            //ViewBag.WeekDays_ID = new SelectList(db.WeekDays, "ID", "NameOfTheDay");
            return View();
        }

        // POST: ShiftTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] ShiftType shiftType)
        {
            if (ModelState.IsValid)
            {
                db.ShiftTypes.Add(shiftType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.BranchDetail_ID = new SelectList(db.BranchDetails, "ID", "Name", shiftType.BranchDetail_ID);
            //ViewBag.WeekDays_ID = new SelectList(db.WeekDays, "ID", "NameOfTheDay", shiftType.WeekDays_ID);
            return View(shiftType);
        }

        // GET: ShiftTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShiftType shiftType = db.ShiftTypes.Find(id);
            if (shiftType == null)
            {
                return HttpNotFound();
            }
            //ViewBag.BranchDetail_ID = new SelectList(db.BranchDetails, "ID", "Name", shiftType.BranchDetail_ID);
            //ViewBag.WeekDays_ID = new SelectList(db.WeekDays, "ID", "NameOfTheDay", shiftType.WeekDays_ID);
            return View(shiftType);
        }

        // POST: ShiftTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,ShiftCharge")] ShiftType shiftType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shiftType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.BranchDetail_ID = new SelectList(db.BranchDetails, "ID", "Name", shiftType.BranchDetail_ID);
            //ViewBag.WeekDays_ID = new SelectList(db.WeekDays, "ID", "NameOfTheDay", shiftType.WeekDays_ID);
            return View(shiftType);
        }

        // GET: ShiftTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShiftType shiftType = db.ShiftTypes.Find(id);
            if (shiftType == null)
            {
                return HttpNotFound();
            }
            return View(shiftType);
        }

        // POST: ShiftTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShiftType shiftType = db.ShiftTypes.Find(id);
            db.ShiftTypes.Remove(shiftType);
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
