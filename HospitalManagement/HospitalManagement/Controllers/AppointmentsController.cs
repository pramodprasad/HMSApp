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
    //Controller for appointment module
    public class AppointmentsController : Controller
    {
        private HMSDBEntities db = new HMSDBEntities();

        // GET: Appointments
        public ActionResult Index()
        {
            List<Appointment> appointmentList = new List<Appointment>();
            var appointments = db.Appointments.Include(a => a.BranchDetail).Include(a => a.Doctor).Include(a => a.PatientDetail).Include(a => a.Specialization).Include(a => a.PatientType).Where(a => a.VisitStatus == 0 ).ToList();
            foreach (var item in appointments)
            {
                if (DateTime.Now.Year == item.AppointmentDate.Year && DateTime.Now.Month == item.AppointmentDate.Month && DateTime.Now.Day == item.AppointmentDate.Day)
                {
                    appointmentList.Add(item);
                }
            }
            return View(appointmentList.OrderByDescending(a => a.CreatedDate).ToList());
         }

        // GET: Appointments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // GET: Appointments/Create
        public ActionResult Create(int id)
        {
            Appointment appointment = new Appointment();
            appointment.AppointmentDate = DateTime.Now;
            var patient = db.PatientDetails.Where(p => p.ID == id).FirstOrDefault();
            //var patientPrevVisit = db.PatientStatus.Where(p => p.PatientDetails_ID == patient.ID).FirstOrDefault();
            if(patient != null)
            {
                appointment.PatientDetails_ID = patient.ID;
                appointment.CreatedDate = DateTime.Now;
                appointment.CreatedBy = patient.CreatedBy;
                //appointment.VisitedDate = DateTime.Now;
            }

            //if(patientPrevVisit != null)
            //{
            //    appointment.Doctor_ID = patientPrevVisit.Doctor_ID;
            //}

            ViewBag.BranchDetails_ID = new SelectList(db.BranchDetails, "ID", "Name");
            ViewBag.Doctor_ID = new SelectList(db.Doctors.Include("EmployeeDetail").ToList(), "ID", "EmployeeDetail.FirstName");
            ViewBag.Specialization_ID = new SelectList(db.Specializations, "ID", "Name");
            ViewBag.ShiftType = new SelectList(db.ShiftTypes, "ID", "Name");
            ViewBag.PatientType = new SelectList(db.PatientTypes, "ID", "Type");          
                
            return View(appointment);
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Appointment appointment)
        {
            var checkIfxists = db.Appointments.Where(a => a.PatientDetails_ID == appointment.PatientDetails_ID && (DbFunctions.TruncateTime(a.CreatedDate) == DbFunctions.TruncateTime(DateTime.Now) || DbFunctions.TruncateTime(a.AppointmentDate) == DbFunctions.TruncateTime(appointment.AppointmentDate))).FirstOrDefault();
            if(checkIfxists != null)
            {
                ModelState.AddModelError("", "Appointment already created for this patient.");
            }

            if (ModelState.IsValid)
            {
                appointment.CreatedDate = DateTime.Now;
                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BranchDetails_ID = new SelectList(db.BranchDetails, "ID", "Name", appointment.BranchDetails_ID);
            ViewBag.Doctor_ID = new SelectList(db.Doctors.Include("EmployeeDetail").ToList(), "ID", "EmployeeDetail.FirstName", appointment.Doctor_ID);
            ViewBag.Specialization_ID = new SelectList(db.Specializations, "ID", "Name", appointment.Specialization_ID);
            ViewBag.PatientType = new SelectList(db.PatientTypes, "ID", "Type", appointment.PatientType_ID);
            ViewBag.ShiftType = new SelectList(db.ShiftTypes, "ID", "Name", appointment.ShiftType_ID);          
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchDetails_ID = new SelectList(db.BranchDetails, "ID", "Name", appointment.BranchDetails_ID);
            ViewBag.Doctor_ID = new SelectList(db.Doctors, "ID", "OtherDetails", appointment.Doctor_ID);
            ViewBag.PatientDetails_ID = new SelectList(db.PatientDetails, "ID", "FullName", appointment.PatientDetails_ID);
            ViewBag.Specialization_ID = new SelectList(db.Specializations, "ID", "Name", appointment.Specialization_ID);
            ViewBag.PatientType = new SelectList(db.PatientTypes, "ID", "Type", appointment.PatientType_ID);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CreatedDate,CreatedBy,Doctor_ID,Specialization_ID,AppointmentDate,VisitedDate,IsVisited,Comments,BranchDetails_ID,PatientDetails_ID")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BranchDetails_ID = new SelectList(db.BranchDetails, "ID", "Name", appointment.BranchDetails_ID);
            ViewBag.Doctor_ID = new SelectList(db.Doctors, "ID", "OtherDetails", appointment.Doctor_ID);
            ViewBag.PatientDetails_ID = new SelectList(db.PatientDetails, "ID", "FullName", appointment.PatientDetails_ID);
            ViewBag.Specialization_ID = new SelectList(db.Specializations, "ID", "Name", appointment.Specialization_ID);
            ViewBag.PatientType = new SelectList(db.PatientTypes, "ID", "Type", appointment.PatientType_ID);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            db.Appointments.Remove(appointment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult VisitedPatient()
        {
            List<Appointment> appointment = new List<Appointment>();
            appointment = db.Appointments.Include(a => a.PatientDetail).Include(a => a.PatientType).Include(a => a.Doctor).ToList();
            return View(appointment);
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
