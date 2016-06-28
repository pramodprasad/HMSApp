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
    public class LabTestsController : Controller
    {
        private HMSTEntities db = new HMSTEntities();

        // GET: /LabTests/
        public ActionResult Index()
        {
            var labtests = db.LabTests.Include(l => l.LabCategory);
            return View(labtests.ToList());
        }

        // GET: /LabTests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LabTest labtest = db.LabTests.Find(id);
            if (labtest == null)
            {
                return HttpNotFound();
            }
            return View(labtest);
        }

        // GET: /LabTests/Create
        public ActionResult Create()
        {
            ViewBag.LabCategory_ID = new SelectList(db.LabCategories, "ID", "Name");
            return View();
        }

        // POST: /LabTests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Name,LabTestCost,Remarks,Status,LabCategory_ID")] LabTest labtest)
        {
            if (ModelState.IsValid)
            {
                db.LabTests.Add(labtest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LabCategory_ID = new SelectList(db.LabCategories, "ID", "Name", labtest.LabCategory_ID);
            return View(labtest);
        }

        // GET: /LabTests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LabTest labtest = db.LabTests.Find(id);
            if (labtest == null)
            {
                return HttpNotFound();
            }
            ViewBag.LabCategory_ID = new SelectList(db.LabCategories, "ID", "Name", labtest.LabCategory_ID);
            return View(labtest);
        }

        // POST: /LabTests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Name,LabTestCost,Remarks,Status,LabCategory_ID")] LabTest labtest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(labtest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LabCategory_ID = new SelectList(db.LabCategories, "ID", "Name", labtest.LabCategory_ID);
            return View(labtest);
        }

        // GET: /LabTests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LabTest labtest = db.LabTests.Find(id);
            if (labtest == null)
            {
                return HttpNotFound();
            }
            return View(labtest);
        }

        // POST: /LabTests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LabTest labtest = db.LabTests.Find(id);
            db.LabTests.Remove(labtest);
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
