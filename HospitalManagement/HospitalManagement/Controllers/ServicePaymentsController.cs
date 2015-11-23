using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HMS.Entity;
using HospitalManagement.Models;
using HospitalManagement.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HospitalManagement.Controllers
{
    public class ServicePaymentsController : Controller
    {
        private HMSDBEntities db = new HMSDBEntities();

        // GET: ServicePayments
        public ActionResult Index()
        {
            var servicePayments = db.ServicePayments.Include(s => s.Doctor).Include(s => s.Appointment).Include(s => s.Service).Include(s => s.ServiceSubCategory);
            return View(servicePayments.ToList());
        }

        // GET: ServicePayments/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicePayment servicePayment = db.ServicePayments.Find(id);
            if (servicePayment == null)
            {
                return HttpNotFound();
            }
            return View(servicePayment);
        }

        // GET: ServicePayments/Create
        public ActionResult Create(int? id)
        {
            var appointment = db.Appointments.Where(a => a.ID == id).FirstOrDefault();
            ServicePayment model = new ServicePayment();
            model.ServiceUnit = 1;
            model.Appointment_ID = appointment.ID;
            ViewBag.Doctor_ID = new SelectList(db.Doctors.Include(s => s.EmployeeDetail), "ID", "EmployeeDetail.FirstName");
            ViewBag.Service_ID = new SelectList(db.Services, "ID", "Name");
            ViewBag.ServiceSubCategory_ID = new SelectList(db.ServiceSubCategories, "ID", "Name");
            return View(model);
        }

        // POST: ServicePayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ServiceUnit,ServiceCharge,Discount,NetAmount,PaidAmount,DueAmount,Doctor_ID,Service_ID,ServiceSubCategory_ID")] ServicePayment servicePayment)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = User.Identity.GetUserId();
                long customerId = 1;

                if (currentUserId != null)
                {
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    customerId = manager.FindById(currentUserId).HMSEmpID;
                }

                servicePayment.CreatedDate = DateTime.Now;
                servicePayment.UpdatedDate = DateTime.Now;
                servicePayment.CreatedBy = Convert.ToInt32(customerId);
                servicePayment.UpdatedBy = Convert.ToInt32(customerId);
                db.ServicePayments.Add(servicePayment);
                db.SaveChanges();
                return RedirectToAction("Service", "Transaction");
            }

            ViewBag.Doctor_ID = new SelectList(db.Doctors, "ID", "OtherDetails", servicePayment.Doctor_ID);
            //ViewBag.PatientStatus_ID = new SelectList(db.PatientStatus, "ID", "ID", servicePayment.PatientStatus_ID);
            ViewBag.Service_ID = new SelectList(db.Services, "ID", "Name", servicePayment.Service_ID);
            ViewBag.ServiceSubCategory_ID = new SelectList(db.ServiceSubCategories, "ID", "Name", servicePayment.ServiceSubCategory_ID);
            return View(servicePayment);
        }

        // GET: ServicePayments/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicePayment servicePayment = db.ServicePayments.Find(id);
            if (servicePayment == null)
            {
                return HttpNotFound();
            }
            ViewBag.Doctor_ID = new SelectList(db.Doctors, "ID", "OtherDetails", servicePayment.Doctor_ID);
            //ViewBag.PatientStatus_ID = new SelectList(db.PatientStatus, "ID", "ID", servicePayment.PatientStatus_ID);
            ViewBag.Service_ID = new SelectList(db.Services, "ID", "Name", servicePayment.Service_ID);
            ViewBag.ServiceSubCategory_ID = new SelectList(db.ServiceSubCategories, "ID", "Name", servicePayment.ServiceSubCategory_ID);
            return View(servicePayment);
        }

        // POST: ServicePayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ServiceUnit,ServiceCharge,Discount,NetAmount,PaidAmount,DueAmount,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,Doctor_ID,PatientStatus_ID,Service_ID,ServiceSubCategory_ID")] ServicePayment servicePayment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(servicePayment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Doctor_ID = new SelectList(db.Doctors, "ID", "OtherDetails", servicePayment.Doctor_ID);
            //ViewBag.PatientStatus_ID = new SelectList(db.PatientStatus, "ID", "ID", servicePayment.PatientStatus_ID);
            ViewBag.Service_ID = new SelectList(db.Services, "ID", "Name", servicePayment.Service_ID);
            ViewBag.ServiceSubCategory_ID = new SelectList(db.ServiceSubCategories, "ID", "Name", servicePayment.ServiceSubCategory_ID);
            return View(servicePayment);
        }

        // GET: ServicePayments/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicePayment servicePayment = db.ServicePayments.Find(id);
            if (servicePayment == null)
            {
                return HttpNotFound();
            }
            return View(servicePayment);
        }

        // POST: ServicePayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ServicePayment servicePayment = db.ServicePayments.Find(id);
            db.ServicePayments.Remove(servicePayment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult FillServiceSubCat(int serviceID)
        {
            var serviceSubList = db.ServiceSubCategories.Where(s => s.Service_ID == serviceID).ToList();
            List<SelectListItem> serviceSubCat = new List<SelectListItem>();
            if (serviceSubList.Count() > 0)
            {
                foreach (var item in serviceSubList)
                {
                    serviceSubCat.Add(new SelectListItem { Text = item.Name, Value = item.ID.ToString() });
                }
            }
            return Json(serviceSubCat, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FillServiceCharge(int serviceSubCatId)
        {
            var serviceCharge = db.ServiceSubCategories.Where(s => s.ID == serviceSubCatId).FirstOrDefault().ServiceCharges;
            if(serviceCharge == null)
            {
                serviceCharge = 1.0M;
            }
            return Json(serviceCharge, JsonRequestBehavior.AllowGet);
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
