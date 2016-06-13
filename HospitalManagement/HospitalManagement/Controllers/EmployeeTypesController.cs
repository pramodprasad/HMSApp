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
    public class EmployeeTypesController : Controller
    {
        private HMSTEntities db = new HMSTEntities();

        // GET: EmployeeTypes
        public ActionResult Index()
        {
            return View(db.EmployeeTypes.ToList());
        }

        // GET: EmployeeTypes/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeType employeeType = db.EmployeeTypes.Find(id);
            if (employeeType == null)
            {
                return HttpNotFound();
            }
            return View(employeeType);
        }

        // GET: EmployeeTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmployeeTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Description,Status")] EmployeeType employeeType)
        {
            if (ModelState.IsValid)
            {
                employeeType.CreatedOn = DateTime.Now;
                db.EmployeeTypes.Add(employeeType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employeeType);
        }

        // GET: EmployeeTypes/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeType employeeType = db.EmployeeTypes.Find(id);
            if (employeeType == null)
            {
                return HttpNotFound();
            }
            return View(employeeType);
        }

        // POST: EmployeeTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Description,CreatedOn,Status")] EmployeeType employeeType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeeType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employeeType);
        }

        // GET: EmployeeTypes/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeType employeeType = db.EmployeeTypes.Find(id);
            if (employeeType == null)
            {
                return HttpNotFound();
            }
            return View(employeeType);
        }

        // POST: EmployeeTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            EmployeeType employeeType = db.EmployeeTypes.Find(id);
            db.EmployeeTypes.Remove(employeeType);
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
