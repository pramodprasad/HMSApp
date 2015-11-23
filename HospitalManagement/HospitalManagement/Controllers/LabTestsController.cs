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
        private HMSDBEntities db = new HMSDBEntities();

        // GET: LabTests
        public ActionResult Index(int id)
        {
            var labTests = db.LabTests.Include(l => l.LabCategory).Where(l => l.LabCategory_ID == id);
            return View(labTests.ToList());
        }

        // GET: LabTests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LabTest labTest = db.LabTests.Find(id);
            if (labTest == null)
            {
                return HttpNotFound();
            }
            return View(labTest);
        }

        // GET: LabTests/Create
        public ActionResult Create(int id)
        {
            LabTest model = new LabTest();
            model.LabCategory_ID = id;
            //ViewBag.LabCategory_ID = new SelectList(db.LabCategories, "ID", "Name");
            return View(model);
        }

        // POST: LabTests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,LabTestCost,Remarks,Status,LabCategory_ID")] LabTest labTest)
        {
            if (ModelState.IsValid)
            {
                db.LabTests.Add(labTest);
                db.SaveChanges();
                return RedirectToAction("Index", "LabCategories");
            }

            //ViewBag.LabCategory_ID = new SelectList(db.LabCategories, "ID", "Name", labTest.LabCategory_ID);
            return View(labTest);
        }

        // GET: LabTests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LabTest labTest = db.LabTests.Find(id);
            if (labTest == null)
            {
                return HttpNotFound();
            }
            ViewBag.LabCategory_ID = new SelectList(db.LabCategories, "ID", "Name", labTest.LabCategory_ID);
            return View(labTest);
        }

        // POST: LabTests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,LabTestCost,Remarks,Status,LabCategory_ID")] LabTest labTest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(labTest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LabCategory_ID = new SelectList(db.LabCategories, "ID", "Name", labTest.LabCategory_ID);
            return View(labTest);
        }

        // GET: LabTests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LabTest labTest = db.LabTests.Find(id);
            if (labTest == null)
            {
                return HttpNotFound();
            }
            return View(labTest);
        }

        public JsonResult FillLabTest(int LabTypeId)
        {
            var LabTests = db.LabTests.Where(l => l.LabCategory_ID == LabTypeId).ToList();
            List<SelectListItem> LabTestList = new List<SelectListItem>();
            foreach(var item in LabTests)
            {
                LabTestList.Add(new SelectListItem { Text = item.Name, Value = item.ID.ToString()});
            }

            return Json(LabTestList, JsonRequestBehavior.AllowGet);
        }

        // POST: LabTests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LabTest labTest = db.LabTests.Find(id);
            db.LabTests.Remove(labTest);
            db.SaveChanges();
            return RedirectToAction("Index", "LabCategories");
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
