using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HMS.Entity;
using HospitalManagement.ViewModels;

namespace HospitalManagement.Controllers
{
    public class PatientVisitController : Controller
    {
        private HMSDBEntities db = new HMSDBEntities();

        // GET: /PatientVisit/
        public ActionResult Index(long? id)
        {
            var patientvisits = db.PatientVisits.Include(p => p.Appointment).Include(p => p.PaymentMode);
            return View(patientvisits.ToList());
        }

        // GET: /PatientVisit/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientVisit patientvisit = db.PatientVisits.Find(id);
            if (patientvisit == null)
            {
                return HttpNotFound();
            }
            return View(patientvisit);
        }

        // GET: /PatientVisit/Create
        public ActionResult Create(long? id)
        {
            AddVisitViewModel model = new AddVisitViewModel();
            PatientVisit patientVisit = new PatientVisit();
            var appointment = db.Appointments.Include(a => a.PatientDetail).Include("ShiftType").Where(a => a.PatientDetails_ID == id).OrderByDescending(a => a.ID).FirstOrDefault();
            patientVisit.Appointment_ID = appointment.ID;
            patientVisit.CreatedBy = 1;
            patientVisit.VisitedDate = DateTime.Now;
            patientVisit.UpdatedDate = DateTime.Now;
            patientVisit.RegistrationAmount = appointment.ShiftType.ShiftCharge;
            patientVisit.PayAmount = appointment.ShiftType.ShiftCharge;
            model.PatientVisit = patientVisit;
            model.Appointment = appointment;
            ViewBag.PaymentMode_ID = new SelectList(db.PaymentModes, "ID", "Mode");
            ViewBag.BranchDetails = new SelectList(db.BranchDetails, "ID", "Name");
            ViewBag.Doctor = new SelectList(db.Doctors.Include("EmployeeDetail").ToList(), "ID", "EmployeeDetail.FirstName");
            ViewBag.Specialization = new SelectList(db.Specializations, "ID", "Name");
            ViewBag.PatientType = new SelectList(db.PatientTypes, "ID", "Type");
            return View(model);
        }

        // POST: /PatientVisit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddVisitViewModel model)
        {
            if (ModelState.IsValid)
            {
                Appointment appointment = db.Appointments.Find(model.PatientVisit.Appointment_ID);
                if(appointment != null)
                {
                    appointment.Doctor_ID = model.Appointment.Doctor_ID;
                    appointment.Specialization_ID = model.Appointment.Specialization_ID;
                    appointment.PatientType_ID = model.Appointment.PatientType_ID;
                    db.Entry(appointment).State = EntityState.Modified;
                    db.SaveChanges();
                }

                db.PatientVisits.Add(model.PatientVisit);
                db.SaveChanges();
                return RedirectToAction("Index", "Patient");
            }

            ViewBag.PaymentMode_ID = new SelectList(db.PaymentModes, "ID", "Mode", model.PatientVisit.PaymentMode_ID);
            ViewBag.Doctor = new SelectList(db.Doctors.Include("EmployeeDetail").ToList(), "ID", "EmployeeDetail.FirstName", model.Appointment.Doctor_ID);
            ViewBag.Specialization = new SelectList(db.Specializations, "ID", "Name", model.Appointment.Specialization_ID);
            ViewBag.PatientType = new SelectList(db.PatientTypes, "ID", "Type", model.Appointment.PatientType_ID);
            return View(model);
        }

        // GET: /PatientVisit/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientVisit patientvisit = db.PatientVisits.Find(id);
            if (patientvisit == null)
            {
                return HttpNotFound();
            }
            ViewBag.Appointment_ID = new SelectList(db.Appointments, "ID", "ReferalDetails", patientvisit.Appointment_ID);
            ViewBag.PaymentMode_ID = new SelectList(db.PaymentModes, "ID", "Mode", patientvisit.PaymentMode_ID);
            return View(patientvisit);
        }

        // POST: /PatientVisit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,VisitedDate,PatientStatus,UpdatedDate,RegistrationAmount,DiscountAmount,PayAmount,CreatedBy,Appointment_ID,PaymentMode_ID")] PatientVisit patientvisit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patientvisit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Appointment_ID = new SelectList(db.Appointments, "ID", "ReferalDetails", patientvisit.Appointment_ID);
            ViewBag.PaymentMode_ID = new SelectList(db.PaymentModes, "ID", "Mode", patientvisit.PaymentMode_ID);
            return View(patientvisit);
        }

        // GET: /PatientVisit/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientVisit patientvisit = db.PatientVisits.Find(id);
            if (patientvisit == null)
            {
                return HttpNotFound();
            }
            return View(patientvisit);
        }

        // POST: /PatientVisit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PatientVisit patientvisit = db.PatientVisits.Find(id);
            db.PatientVisits.Remove(patientvisit);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult FillDoctor(int SpecializationId)
        {

            var doctors = db.Doctors.Include("EmployeeDetail").Include("ShiftDates").Include("ShiftDays").Where(m => m.Specialization_ID == SpecializationId).ToList();
            List<SelectListItem> doctorList = new List<SelectListItem>();
            if (doctors.Count() > 0)
            {
                foreach (var item in doctors)
                {
                        doctorList.Add(new SelectListItem { Text = item.EmployeeDetail.FirstName, Value = item.ID.ToString() });
                }
            }
            return Json(doctorList, JsonRequestBehavior.AllowGet);
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
