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
    public class RoomDetailsController : Controller
    {
        private HMSDBEntities db = new HMSDBEntities();

        // GET: RoomDetails
        public ActionResult Index()
        {
            var roomDetails = db.RoomDetails.Include(r => r.Ward);
            return View(roomDetails.ToList());
        }

        // GET: RoomDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomDetail roomDetail = db.RoomDetails.Find(id);
            if (roomDetail == null)
            {
                return HttpNotFound();
            }
            return View(roomDetail);
        }

        // GET: RoomDetails/Create
        public ActionResult Create()
        {
            ViewBag.Ward_ID = new SelectList(db.Wards, "ID", "Name");
            return View();
        }

        // POST: RoomDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoomDetail roomDetail)
        {
            if (ModelState.IsValid)
            {
                db.RoomDetails.Add(roomDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Ward_ID = new SelectList(db.Wards, "ID", "Name", roomDetail.Ward_ID);
            return View(roomDetail);
        }

        // GET: RoomDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomDetail roomDetail = db.RoomDetails.Find(id);
            if (roomDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.Ward_ID = new SelectList(db.Wards, "ID", "Name", roomDetail.Ward_ID);
            return View(roomDetail);
        }

        // POST: RoomDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoomDetail roomDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roomDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Ward_ID = new SelectList(db.Wards, "ID", "Name", roomDetail.Ward_ID);
            return View(roomDetail);
        }

        // GET: RoomDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomDetail roomDetail = db.RoomDetails.Find(id);
            if (roomDetail == null)
            {
                return HttpNotFound();
            }
            return View(roomDetail);
        }

        // POST: RoomDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoomDetail roomDetail = db.RoomDetails.Find(id);
            db.RoomDetails.Remove(roomDetail);
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
