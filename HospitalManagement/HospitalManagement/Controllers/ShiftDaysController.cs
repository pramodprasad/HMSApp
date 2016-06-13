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
    public class ShiftDaysController : Controller
    {
        private HMSTEntities db = new HMSTEntities();

        // GET: /ShiftDays/
        public ActionResult Index()
        {
            var shiftdays = db.ShiftDays.Include(s => s.Doctor).Include(s => s.ShiftType).Include(s => s.WeekDay);
            return View(shiftdays.ToList());
        }

        // GET: /ShiftDays/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShiftDay shiftday = db.ShiftDays.Find(id);
            if (shiftday == null)
            {
                return HttpNotFound();
            }
            return View(shiftday);
        }

        // GET: /ShiftDays/Create
        public ActionResult Create()
        {
            ViewBag.Doctor_ID = new SelectList(db.Doctors, "ID", "OtherDetails");
            ViewBag.ShiftType_ID = new SelectList(db.ShiftTypes, "ID", "Name");
            ViewBag.WeekDays_ID = new SelectList(db.WeekDays, "ID", "NameOfTheDay");
            return View();
        }

        // POST: /ShiftDays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,StartTime,EndTime,Status,ShiftType_ID,WeekDays_ID,Doctor_ID")] ShiftDay shiftday)
        {
            if (ModelState.IsValid)
            {
                db.ShiftDays.Add(shiftday);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Doctor_ID = new SelectList(db.Doctors, "ID", "OtherDetails", shiftday.Doctor_ID);
            ViewBag.ShiftType_ID = new SelectList(db.ShiftTypes, "ID", "Name", shiftday.ShiftType_ID);
            ViewBag.WeekDays_ID = new SelectList(db.WeekDays, "ID", "NameOfTheDay", shiftday.WeekDays_ID);
            return View(shiftday);
        }

        // GET: /ShiftDays/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShiftDay shiftday = db.ShiftDays.Find(id);
            if (shiftday == null)
            {
                return HttpNotFound();
            }
            ViewBag.Doctor_ID = new SelectList(db.Doctors, "ID", "OtherDetails", shiftday.Doctor_ID);
            ViewBag.ShiftType_ID = new SelectList(db.ShiftTypes, "ID", "Name", shiftday.ShiftType_ID);
            ViewBag.WeekDays_ID = new SelectList(db.WeekDays, "ID", "NameOfTheDay", shiftday.WeekDays_ID);
            return View(shiftday);
        }

        // POST: /ShiftDays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,StartTime,EndTime,Status,ShiftType_ID,WeekDays_ID,Doctor_ID")] ShiftDay shiftday)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shiftday).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Doctor_ID = new SelectList(db.Doctors, "ID", "OtherDetails", shiftday.Doctor_ID);
            ViewBag.ShiftType_ID = new SelectList(db.ShiftTypes, "ID", "Name", shiftday.ShiftType_ID);
            ViewBag.WeekDays_ID = new SelectList(db.WeekDays, "ID", "NameOfTheDay", shiftday.WeekDays_ID);
            return View(shiftday);
        }

        // GET: /ShiftDays/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShiftDay shiftday = db.ShiftDays.Find(id);
            if (shiftday == null)
            {
                return HttpNotFound();
            }
            return View(shiftday);
        }

        // POST: /ShiftDays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShiftDay shiftday = db.ShiftDays.Find(id);
            db.ShiftDays.Remove(shiftday);
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
