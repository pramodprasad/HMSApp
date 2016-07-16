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
using HMS.BAL;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Web.Helpers;  

namespace HospitalManagement.Controllers
{
    public class PatientVisitController : Controller
    {
        private HMSDBEntities db = new HMSDBEntities();

        // GET: /PatientVisit/
        public ActionResult Index(long? id)
        {
            var patientvisits = db.PatientVisits.Include(p => p.Appointment).Include(p => p.PaymentMode).Where(p => p.Appointment.PatientDetails_ID == id);
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
            //db.ShiftTypes.Where(x => x.ID == appointment.ShiftType_ID).Select(a => a.ShiftCharge).FirstOrDefault();
            patientVisit.RegistrationAmount = appointment.ShiftType.ShiftCharge;
            patientVisit.PayAmount = appointment.ShiftType.ShiftCharge;
            model.PatientVisit = patientVisit;
            model.Appointment = appointment;
            ViewBag.PaymentMode_ID = new SelectList(db.PaymentModes, "ID", "Mode");
            ViewBag.BranchDetails = new SelectList(db.BranchDetails, "ID", "Name");
            List<DoctorName> doctornamelist = UtilityManager.GetName();            
            ViewBag.Doctors = new SelectList(doctornamelist, "ID", "Name");
            //ViewBag.Doctor = new SelectList(db.Doctors.Include("EmployeeDetail").ToList(), "ID", "EmployeeDetail.FirstName");
            ViewBag.Specialization = new SelectList(db.Specializations, "ID", "Name");
            ViewBag.PatientType = new SelectList(db.PatientTypes, "ID", "Type");
            ViewBag.PatientStatus = new SelectList(db.PatientStatus, "ID", "Details");
            return View(model);
        }

        // POST: /PatientVisit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddVisitViewModel model)
        {
            if (ModelState.ContainsKey("Appointment.PatientDetail.Gender"))
            {
                ModelState.Remove("Appointment.PatientDetail.Gender");
            }

            if (ModelState.IsValid)
            {
                Appointment appointment = db.Appointments.Find(model.PatientVisit.Appointment_ID);
                if(appointment != null)
                {
                    appointment.Doctor_ID = model.Appointment.Doctor_ID;
                    appointment.Specialization_ID = model.Appointment.Specialization_ID;
                    appointment.PatientType_ID = model.Appointment.PatientType_ID;
                    appointment.VisitStatus = 1;//Patient is 1- visited , 0- not visited
                    db.Entry(appointment).State = EntityState.Modified;
                    db.SaveChanges();
                }
                model.PatientVisit.UpdatedDate = DateTime.Now;
                model.PatientVisit.CreatedBy = 1;
                db.PatientVisits.Add(model.PatientVisit);
                db.SaveChanges();
               // return RedirectToAction("VisitedPatient", "Appointments");
                return RedirectToAction("GetTodaysVisitedPatients");
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

        public ActionResult GetTodaysVisitedPatients()
        {
            List<PatientVisit> visitedpatient = new List<PatientVisit>();
            visitedpatient = db.PatientVisits.Include(p => p.Appointment).Include(a => a.Appointment.PatientDetail).Include(a => a.Appointment.Doctor.EmployeeDetail).Include(a => a.Appointment.ShiftType).Where(w => w.Appointment.VisitStatus == 1 && (DbFunctions.TruncateTime(w.VisitedDate) == DbFunctions.TruncateTime(DateTime.Now))).ToList();
            return View(visitedpatient.OrderByDescending(a => a.ID).ToList());
        }

        public ActionResult GetVisitedPatientDetails()
        {
            List<PatientVisit> visitedpatient = new List<PatientVisit>();
            visitedpatient = db.PatientVisits.Include(p => p.Appointment).Include(a => a.Appointment.PatientDetail).Include(a => a.Appointment.Doctor.EmployeeDetail).Include(a => a.Appointment.ShiftType).Where(w => w.Appointment.VisitStatus == 1 ).ToList();
            return View(visitedpatient.OrderByDescending(a => a.ID).ToList());
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

        public ActionResult OutPatientSlip(int id)
        {
            PatientVisit visitedpatient = new PatientVisit();
            visitedpatient = db.PatientVisits.Include(p => p.Appointment).Include(a => a.Appointment.PatientDetail).Include(a => a.Appointment.Doctor.EmployeeDetail.BranchDetail).Include(a => a.Appointment.Doctor.EmployeeDetail).Include(a => a.Appointment.ShiftType).Where(w => w.Appointment.VisitStatus == 1 && w.PayAmount > 0 && w.ID == id).FirstOrDefault();
            return View(visitedpatient);

        }

        public ActionResult DoctorsPriscription(int id)
        {
            PatientVisit visitedpatient = new PatientVisit();
            visitedpatient = db.PatientVisits.Include(p => p.Appointment).Include(a => a.Appointment.PatientDetail).Include(a => a.Appointment.Doctor.EmployeeDetail).Include(a => a.Appointment.ShiftType).Where(w => w.Appointment.VisitStatus == 1 && w.PayAmount > 0 && w.ID == id).FirstOrDefault();
            return View(visitedpatient);

        }

        public FileStreamResult CreateTodayPatientVisitDetails()
        {
            List<PatientVisit> patientvisits = new List<PatientVisit>();
            patientvisits = db.PatientVisits.Include(a => a.Appointment).Include(p => p.PatientStatu).Include(m => m.PaymentMode).ToList();

            //var all = db.sp_GetRegistrationPayment(null, null, null, null, DateTime.Now);
            WebGrid grid = new WebGrid(source: patientvisits, canPage: false, canSort: false);
            string gridHtml = grid.GetHtml(
                        tableStyle: "webgrid-table",
                        headerStyle: "webgrid-header",
                        footerStyle: "webgrid-footer",
                        alternatingRowStyle: "webgrid-alternating-row",
                        selectedRowStyle: "webgrid-selected-row",
                        rowStyle: "webgrid-row-style",
                        mode: WebGridPagerModes.All,
                   columns: grid.Columns(
                            grid.Column("ID", "Visit ID"),
                            grid.Column("VisitedDate", "Patient"),
                            grid.Column("PatientStatusID", "Father/Husband"),
                            grid.Column("UpdatedDate", "Patient Type"),
                            grid.Column("RegistrationAmount", "Reg. Amount"),
                            grid.Column("DiscountAmount", "Dis. Amount"),
                            grid.Column("PayAmount", "Pay Amount"),
                            grid.Column("CreatedBy", "Pay Mode"),
                            grid.Column("Appointment_ID", "Doctor"),
                            grid.Column("PaymentMode_ID", "PaymentMode_ID")
                           )
                    ).ToString();
            string exportData = String.Format("{0}{1}", "", gridHtml);
            var bytes = System.Text.Encoding.UTF8.GetBytes(exportData);
            using (var input = new MemoryStream(bytes))
            {
                var output = new MemoryStream();
                var document = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 50);
                string strDate = "Date :" + DateTime.Now.ToString("yyyy-MM-dd");
                document.AddHeader("Registration Payment", strDate);
              
                var writer = PdfWriter.GetInstance(document, output);
                writer.CloseStream = false;
                document.Open();
                var xmlWorker = iTextSharp.tool.xml.XMLWorkerHelper.GetInstance();
                xmlWorker.ParseXHtml(writer, document, input, System.Text.Encoding.UTF8);
                document.Close();
                output.Position = 0;
                return new FileStreamResult(output, "application/pdf");
            }
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
