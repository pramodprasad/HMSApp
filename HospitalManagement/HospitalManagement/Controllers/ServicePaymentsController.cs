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
using HospitalManagement.ViewModels.Payments;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using HMS.BAL;

namespace HospitalManagement.Controllers
{
    public class ServicePaymentsController : Controller
    {
        private HMSTEntities db = new HMSTEntities();

        // GET: ServicePayments
        public ActionResult Index()
        {
            List<ServicePaymentModel> model = new List<ServicePaymentModel>();
            var servicePayments = db.ServicePayments.Include(s => s.Doctor).Include(s => s.Appointment).Include(s => s.Service).Include(s => s.ServiceSubCategory);

            foreach(var item in servicePayments.Select(m => m.Appointment_ID).Distinct())
            {
                ServicePaymentModel service = new ServicePaymentModel();
                var firstVal = servicePayments.Where(o => o.Appointment_ID == item.Value).First();
                service.AppointmentId = item.Value;
                service.PatientName = firstVal.Appointment.PatientDetail.FullName;
                service.ServiceCharge = servicePayments.Where(o => o.Appointment_ID == item.Value).Sum(o => o.ServiceCharge);
                service.Discount = servicePayments.Where(o => o.Appointment_ID == item.Value).Sum(o => o.Discount);
                service.Qty = servicePayments.Where(o => o.Appointment_ID == item.Value).Sum(o => o.ServiceUnit);
                service.NetAmount = servicePayments.Where(o => o.Appointment_ID == item.Value).Sum(o => o.NetAmount);
                service.PaidAmount = servicePayments.Where(o => o.Appointment_ID == item.Value).Sum(o => o.PaidAmount);
                service.DueAmount = servicePayments.Where(o => o.Appointment_ID == item.Value).Sum(o => o.DueAmount);
                service.DoctorName = firstVal.Doctor.EmployeeDetail.FirstName + " " + firstVal.Doctor.EmployeeDetail.LastName;
                model.Add(service);
            }
            return View(model);
        }

        // GET: ServicePayments/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var servicePaymentList = db.ServicePayments.Include(s => s.Doctor).Include(s => s.Appointment).Include(s => s.Service).Include(s => s.ServiceSubCategory).Where(o => o.Appointment_ID == id.Value);
            if (servicePaymentList == null)
            {
                return HttpNotFound();
            }
            return View(servicePaymentList);
        }

        // GET: ServicePayments/Create
        public ActionResult Create(int? id)
        {
            ServicePaymentViewModel model = new ServicePaymentViewModel();
            model.ServicePaymentList = new List<ServicePayment>();
            var appointment = db.Appointments.Include(a => a.PatientDetail).Where(a => a.ID == id).OrderByDescending(a => a.AppointmentDate).FirstOrDefault();
            ServicePayment servicePayment = new ServicePayment();
            servicePayment.Appointment = appointment;
            servicePayment.ServiceUnit = 1;
            servicePayment.Appointment_ID = appointment.ID;
            model.ServicePayment = servicePayment;
            model.ServicePayment.PaymentModeID = 0;
            List<DoctorName> doctornamelist = UtilityManager.GetRadiologyDoctor();
            ViewBag.ServicePayment_Doctor_ID = new SelectList(doctornamelist, "ID", "Name");
            //ViewBag.ServicePayment_Doctor_ID = new SelectList(db.Doctors.Include(s => s.EmployeeDetail), "ID", "EmployeeDetail.FirstName");
            ViewBag.ServicePayment_Service_ID = new SelectList(db.Services, "ID", "Name");
            ViewBag.ServicePayment_ServiceSubCategory_ID = new SelectList(db.ServiceSubCategories, "ID", "Name");
            ViewBag.ServicePayment_PaymentModeID = new SelectList(db.PaymentModes, "ID", "Mode");

            return View(model);
        }

        // POST: ServicePayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServicePaymentViewModel model)
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

                foreach (ServicePayment item in model.ServicePaymentList)
                {
                    item.CreatedDate = DateTime.Now;
                    item.UpdatedDate = DateTime.Now;
                    item.CreatedBy = Convert.ToInt32(customerId);
                    item.UpdatedBy = Convert.ToInt32(customerId);
                    item.PaymentModeID = item.PaymentModeID;
                    if (item.DueAmount == 0)
                    {
                        item.PaymentStatusID = 1;
                    }
                    else
                    {
                        item.PaymentStatusID = 3;
                    }
                    db.ServicePayments.Add(item);
                }

                db.SaveChanges();
                return RedirectToAction("Index", "ServicePayments");
            }

            List<DoctorName> doctornamelist = UtilityManager.GetRadiologyDoctor();
            ViewBag.ServicePayment_Doctor_ID = new SelectList(doctornamelist, "ID", "Name", model.ServicePayment.Doctor_ID);
            //ViewBag.PatientStatus_ID = new SelectList(db.PatientStatus, "ID", "ID", servicePayment.PatientStatus_ID);
            ViewBag.ServicePayment_Service_ID = new SelectList(db.Services, "ID", "Name", model.ServicePayment.Service_ID);
            ViewBag.ServicePayment_ServiceSubCategory_ID = new SelectList(db.ServiceSubCategories, "ID", "Name", model.ServicePayment.ServiceSubCategory_ID);
            ViewBag.ServicePayment_PaymentModeID = new SelectList(db.PaymentModes, "ID", "Mode", model.ServicePayment.PaymentModeID);
            return View(model);
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
            List<DoctorName> doctornamelist = UtilityManager.GetRadiologyDoctor();
            ViewBag.Doctor_ID = new SelectList(doctornamelist, "ID", "Name", servicePayment.Doctor_ID);
            //ViewBag.PatientStatus_ID = new SelectList(db.PatientStatus, "ID", "ID", servicePayment.PatientStatus_ID);
            ViewBag.Service_ID = new SelectList(db.Services, "ID", "Name", servicePayment.Service_ID);
            ViewBag.ServiceSubCategory_ID = new SelectList(db.ServiceSubCategories, "ID", "Name", servicePayment.ServiceSubCategory_ID);
            ViewBag.PaymentModeID = new SelectList(db.PaymentModes, "ID", "Mode", servicePayment.PaymentModeID);
            return View(servicePayment);
        }

        // POST: ServicePayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ServicePayment servicePayment)
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
                servicePayment.UpdatedDate = DateTime.Now;
                servicePayment.UpdatedBy = customerId;
                db.Entry(servicePayment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<DoctorName> doctornamelist = UtilityManager.GetRadiologyDoctor();
            ViewBag.Doctor_ID = new SelectList(doctornamelist, "ID", "Name", servicePayment.Doctor_ID);
            //ViewBag.PatientStatus_ID = new SelectList(db.PatientStatus, "ID", "ID", servicePayment.PatientStatus_ID);
            ViewBag.Service_ID = new SelectList(db.Services, "ID", "Name", servicePayment.Service_ID);
            ViewBag.ServiceSubCategory_ID = new SelectList(db.ServiceSubCategories, "ID", "Name", servicePayment.ServiceSubCategory_ID);
            ViewBag.PaymentModeID = new SelectList(db.PaymentModes, "ID", "Mode", servicePayment.PaymentModeID);
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
            if (serviceCharge == 0)
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
