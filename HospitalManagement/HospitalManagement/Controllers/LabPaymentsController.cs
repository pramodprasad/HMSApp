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
using HMS.BAL;

namespace HospitalManagement.Controllers
{
    public class LabPaymentsController : Controller
    {
        private HMSTEntities db = new HMSTEntities();

        // GET: LabPayments
        public ActionResult Index()
        {
            var labPayments = db.LabPayments.Include(l => l.Appointment).Include(l => l.Doctor).Include(l => l.LabCategory).Include(l => l.LabTest);
            return View(labPayments.ToList());
        }

        // GET: LabPayments/Create
        [OutputCache(Duration = 0, VaryByParam = "*", NoStore = true)]
        public ActionResult Create(int? id)
        {
            LabPaymentViewModel model = new LabPaymentViewModel();
            List<LabPayment> labPaymentList = new List<LabPayment>();
            var appointment = db.Appointments.Include(l => l.PatientDetail).Where(a => a.ID == id).FirstOrDefault();
            LabPayment labPayment = new LabPayment();
            labPayment.Appointment = appointment;
            labPayment.Appointment_ID = appointment.ID;
            labPayment.Quantity = 1;
            model.LabPayment = labPayment;
            List<DoctorName> doctornamelist = UtilityManager.GetLabDoctor();
            ViewBag.LabPayment_Doctor_ID = new SelectList(doctornamelist, "ID", "Name");
            //ViewBag.LabPayment_Doctor_ID = new SelectList(db.Doctors.Include(s => s.EmployeeDetail), "ID", "EmployeeDetail.FirstName");
            ViewBag.LabPayment_LabCategory_ID = new SelectList(db.LabCategories, "ID", "Name");
            ViewBag.LabPayment_LabTest_ID = new SelectList(db.LabTests, "ID", "Name");
            ViewBag.LabPayment_PaymentModeID = new SelectList(db.PaymentModes, "ID", "Mode");
            return View(model);
        }

        // POST: LabPayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [OutputCache(Duration = 0, VaryByParam = "*", NoStore = true)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LabPaymentViewModel model)
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

                foreach (LabPayment item in model.LabPayments)
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
                    db.LabPayments.Add(item);
                }
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                    //return RedirectToAction("Service", "Transaction");
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            ViewBag.Appointment_ID = new SelectList(db.Appointments, "ID", "ReferalDetails", model.LabPayment.Appointment_ID);
            ViewBag.Doctor_ID = new SelectList(db.Doctors, "ID", "OtherDetails", model.LabPayment.Doctor_ID);
            ViewBag.LabCategory_ID = new SelectList(db.LabCategories, "ID", "Name", model.LabPayment.LabCategory_ID);
            ViewBag.LabTest_ID = new SelectList(db.LabTests, "ID", "Name", model.LabPayment.LabTest_ID);
            ViewBag.LabPayment_PaymentModeID = new SelectList(db.PaymentModes, "ID", "Mode", model.LabPayment.PaymentMode);
            return View(model);
        }

        // GET: LabPayments/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LabPayment labPayment = db.LabPayments.Find(id);
            if (labPayment == null)
            {
                return HttpNotFound();
            }
            ViewBag.Appointment_ID = new SelectList(db.Appointments, "ID", "ReferalDetails", labPayment.Appointment_ID);
            ViewBag.Doctor_ID = new SelectList(db.Doctors, "ID", "OtherDetails", labPayment.Doctor_ID);
            ViewBag.LabCategory_ID = new SelectList(db.LabCategories, "ID", "Name", labPayment.LabCategory_ID);
            ViewBag.LabTest_ID = new SelectList(db.LabTests, "ID", "Name", labPayment.LabTest_ID);
            ViewBag.LabPayment_PaymentModeID = new SelectList(db.PaymentModes, "ID", "Mode", labPayment.PaymentMode);
            return View(labPayment);
        }

        // POST: LabPayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Quantity,LabCharge,Discount,NetAmount,PaidAmount,DueAmount,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,Appointment_ID,Doctor_ID,LabCategory_ID,LabTest_ID")] LabPayment labPayment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(labPayment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Appointment_ID = new SelectList(db.Appointments, "ID", "ReferalDetails", labPayment.Appointment_ID);
            ViewBag.Doctor_ID = new SelectList(db.Doctors, "ID", "OtherDetails", labPayment.Doctor_ID);
            ViewBag.LabCategory_ID = new SelectList(db.LabCategories, "ID", "Name", labPayment.LabCategory_ID);
            ViewBag.LabTest_ID = new SelectList(db.LabTests, "ID", "Name", labPayment.LabTest_ID);
            ViewBag.LabPayment_PaymentModeID = new SelectList(db.PaymentModes, "ID", "Mode", labPayment.PaymentMode);
            return View(labPayment);
        }

        // GET: LabPayments/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LabPayment labPayment = db.LabPayments.Find(id);
            if (labPayment == null)
            {
                return HttpNotFound();
            }
            return View(labPayment);
        }

        // POST: LabPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            LabPayment labPayment = db.LabPayments.Find(id);
            db.LabPayments.Remove(labPayment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult FillLabCharge(int labCatId)
        {
            var serviceCharge = db.LabTests.Where(s => s.ID == labCatId).FirstOrDefault().LabTestCost;
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
