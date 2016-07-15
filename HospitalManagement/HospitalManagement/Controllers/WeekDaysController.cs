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
    public class WeekDaysController : Controller
    {
        private HMSDBEntities db = new HMSDBEntities();

        // GET: /WeekDays/
        public ActionResult Index()
        {
            return View(db.WeekDays.ToList());
        }

        // GET: /WeekDays/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WeekDay weekday = db.WeekDays.Find(id);
            if (weekday == null)
            {
                return HttpNotFound();
            }
            return View(weekday);
        }

        // GET: /WeekDays/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /WeekDays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,NameOfTheDay")] WeekDay weekday)
        {
            if (ModelState.IsValid)
            {
                db.WeekDays.Add(weekday);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(weekday);
        }

        // GET: /WeekDays/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WeekDay weekday = db.WeekDays.Find(id);
            if (weekday == null)
            {
                return HttpNotFound();
            }
            return View(weekday);
        }

        // POST: /WeekDays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,NameOfTheDay")] WeekDay weekday)
        {
            if (ModelState.IsValid)
            {
                db.Entry(weekday).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(weekday);
        }

        // GET: /WeekDays/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WeekDay weekday = db.WeekDays.Find(id);
            if (weekday == null)
            {
                return HttpNotFound();
            }
            return View(weekday);
        }

        // POST: /WeekDays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WeekDay weekday = db.WeekDays.Find(id);
            db.WeekDays.Remove(weekday);
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
