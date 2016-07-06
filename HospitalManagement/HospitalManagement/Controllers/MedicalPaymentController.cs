using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HMS.Entity;
using HospitalManagement.ViewModels;
using HMS.BAL;
using Microsoft.AspNet.Identity;
using HospitalManagement.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using HospitalManagement.ViewModels.Payments;

namespace HospitalManagement.Controllers
{
    public class MedicalPaymentController : Controller
    {
        private HMSTEntities db = new HMSTEntities();

        // GET: MedicalPayment
        public async Task<ActionResult> Index()
        {
            List<MedicalPaymentModel> model = new List<MedicalPaymentModel>();
            var medicalPaymentDetails = db.MedicalPaymentDetails.Include(m => m.Appointment).Include(m => m.Doctor).Include(m => m.PaymentMode).Include(m => m.PaymentSection).Include(m => m.PaymentStau);
            foreach (var item in medicalPaymentDetails.Select(m => m.Appointment_ID).Distinct())
            {
                MedicalPaymentModel medicalPayment = new MedicalPaymentModel();
                var firstVal = medicalPaymentDetails.Where(o => o.Appointment_ID == item.Value).First();
                medicalPayment.AppointmentId = item.Value;
                medicalPayment.PatientName = firstVal.Appointment.PatientDetail.FullName;
                medicalPayment.MedicalCharge = medicalPaymentDetails.Where(o => o.Appointment_ID == item.Value).Sum(o => o.MedicalAmount);
                medicalPayment.Discount = medicalPaymentDetails.Where(o => o.Appointment_ID == item.Value).Sum(o => o.DiscountAmount);
                medicalPayment.Qty = medicalPaymentDetails.Where(o => o.Appointment_ID == item.Value).Sum(o => o.Quantity);
                medicalPayment.NetAmount = medicalPaymentDetails.Where(o => o.Appointment_ID == item.Value).Sum(o => o.NetAmount);
                medicalPayment.PaidAmount = medicalPaymentDetails.Where(o => o.Appointment_ID == item.Value).Sum(o => o.PayAmount);
                medicalPayment.DueAmount = medicalPaymentDetails.Where(o => o.Appointment_ID == item.Value).Sum(o => o.DueAmount);
                medicalPayment.DoctorName = firstVal.Doctor.EmployeeDetail.FirstName + " " + firstVal.Doctor.EmployeeDetail.LastName;
                model.Add(medicalPayment);
            }
            return View(model);
        }

        // GET: MedicalPayment/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var medicalPaymentDetail = await db.MedicalPaymentDetails.Where(p => p.Appointment_ID == id).ToListAsync();
            if (medicalPaymentDetail == null)
            {
                return HttpNotFound();
            }
            return View(medicalPaymentDetail);
        }

        // GET: MedicalPayment/Create
        public ActionResult Create(long? id)
        {
            MedicalPaymentViewModel model = new MedicalPaymentViewModel();
            List<MedicalPaymentDetail> medicalPaymentList = new List<MedicalPaymentDetail>();
            var appointment = db.Appointments.Include(l => l.PatientDetail).Where(a => a.ID == id).FirstOrDefault();
            MedicalPaymentDetail medicalPayment = new MedicalPaymentDetail();
            medicalPayment.Appointment = appointment;
            medicalPayment.Appointment_ID = appointment.ID;
            medicalPayment.Quantity = 1;
            model.MedicalPayment = medicalPayment;
            model.MedicalPayments = medicalPaymentList;
            List<DoctorName> doctornamelist = UtilityManager.GetLabDoctor();
            ViewBag.MedicalPayment_Doctor_ID = new SelectList(doctornamelist, "ID", "Name");
            ViewBag.MedicalPayment_PaymentMode_ID = new SelectList(db.PaymentModes, "ID", "Mode");
            ViewBag.MedicalPayment_PaymentSection_ID = new SelectList(db.PaymentSections, "ID", "Name");
            ViewBag.MedicalPayment_PaymentStatusID = new SelectList(db.PaymentStaus, "ID", "Details");
            ViewBag.MedicalPayment_MedicalEquipmentID = new SelectList(db.MedicalEquipments, "ID", "EquipmentName");
            return View(model);
        }

        // POST: MedicalPayment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MedicalPaymentViewModel model)
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

                foreach (MedicalPaymentDetail item in model.MedicalPayments)
                {
                    item.DiscountedBy = Convert.ToInt64(customerId);
                    item.PaymentReceivedBy = Convert.ToInt64(customerId);
                    item.PaymentReceivedDate = DateTime.Now;
                    if (item.DueAmount == 0)
                    {
                        item.PaymentStatusID = 1;
                    }
                    else
                    {
                        item.PaymentStatusID = 3;
                    }
                    db.MedicalPaymentDetails.Add(item);
                }
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            List<DoctorName> doctornamelist = UtilityManager.GetLabDoctor();
            ViewBag.MedicalPayment_Doctor_ID = new SelectList(doctornamelist, "ID", "Name", model.MedicalPayments.First().Doctor_ID);
            ViewBag.MedicalPayment_MedicalEquipmentID = new SelectList(db.MedicalEquipments, "ID", "EquipmentName", model.MedicalPayments.First().MedicalEquipmentID);
            ViewBag.MedicalPayment_PaymentMode_ID = new SelectList(db.PaymentModes, "ID", "Mode", model.MedicalPayments.First().PaymentMode_ID);
            ViewBag.MedicalPayment_PaymentSection_ID = new SelectList(db.PaymentSections, "ID", "Name", model.MedicalPayments.First().PaymentSection_ID);
            return View(model);
        }

        // GET: MedicalPayment/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalPaymentDetail medicalPaymentDetail = await db.MedicalPaymentDetails.FindAsync(id);
            if (medicalPaymentDetail == null)
            {
                return HttpNotFound();
            }
            List<DoctorName> doctornamelist = UtilityManager.GetLabDoctor();
            ViewBag.Doctor_ID = new SelectList(doctornamelist, "ID", "Name", medicalPaymentDetail.Doctor_ID);
            ViewBag.PaymentMode_ID = new SelectList(db.PaymentModes, "ID", "Mode", medicalPaymentDetail.PaymentMode_ID);
            ViewBag.PaymentSection_ID = new SelectList(db.PaymentSections, "ID", "Name", medicalPaymentDetail.PaymentSection_ID);
            ViewBag.PaymentStatusID = new SelectList(db.PaymentStaus, "ID", "Details", medicalPaymentDetail.PaymentStatusID);
            ViewBag.MedicalEquipmentID = new SelectList(db.MedicalEquipments, "ID", "EquipmentName", medicalPaymentDetail.MedicalEquipmentID);
            return View(medicalPaymentDetail);
        }

        // POST: MedicalPayment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,MedicalAmount,PaymentStatusID,PaymentReceivedBy,PaymentReceivedDate,DiscountAmount,DiscountedBy,DiscountDescription,PaymentDetails,Doctor_ID,PaymentMode_ID,PaymentSection_ID,Appointment_ID,Quantity,MedicalEquipmentID,NetAmount,PayAmount,DueAmount,OtherDetails")] MedicalPaymentDetail medicalPaymentDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medicalPaymentDetail).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            List<DoctorName> doctornamelist = UtilityManager.GetLabDoctor();
            ViewBag.Doctor_ID = new SelectList(doctornamelist, "ID", "Name", medicalPaymentDetail.Doctor_ID);
            ViewBag.PaymentMode_ID = new SelectList(db.PaymentModes, "ID", "Mode", medicalPaymentDetail.PaymentMode_ID);
            ViewBag.PaymentSection_ID = new SelectList(db.PaymentSections, "ID", "Name", medicalPaymentDetail.PaymentSection_ID);
            ViewBag.PaymentStatusID = new SelectList(db.PaymentStaus, "ID", "Details", medicalPaymentDetail.PaymentStatusID);
            ViewBag.MedicalEquipmentID = new SelectList(db.MedicalEquipments, "ID", "EquipmentName", medicalPaymentDetail.MedicalEquipmentID);
            return View(medicalPaymentDetail);
        }

        // GET: MedicalPayment/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalPaymentDetail medicalPaymentDetail = await db.MedicalPaymentDetails.FindAsync(id);
            if (medicalPaymentDetail == null)
            {
                return HttpNotFound();
            }
            return View(medicalPaymentDetail);
        }

        // POST: MedicalPayment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            MedicalPaymentDetail medicalPaymentDetail = await db.MedicalPaymentDetails.FindAsync(id);
            db.MedicalPaymentDetails.Remove(medicalPaymentDetail);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public JsonResult FillMedicalCharge(int medCatId)
        {
            var serviceCharge = db.MedicalEquipments.Where(s => s.ID == medCatId).FirstOrDefault().UnitPrice;
            if (Convert.ToInt32(serviceCharge) == 0)
            {
                serviceCharge = "0";
            }
            return Json(Convert.ToInt32(serviceCharge), JsonRequestBehavior.AllowGet);
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
