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
    public class PatientController : Controller
    {
        private HMSDBEntities db = new HMSDBEntities();

        // GET: Patient
        public ActionResult Index()
        {
            return View(db.PatientDetails.Include(c => c.City).Include(c => c.Appointments).ToList());
        }

        // GET: Patient/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientDetail patientDetail = db.PatientDetails.Find(id);
            if (patientDetail == null)
            {
                return HttpNotFound();
            }
            return View(patientDetail);
        }

        // GET: Patient/Create
        public ActionResult Create()
        {
            PatientViewModel model = new PatientViewModel();
            Appointment appointment = new Appointment();
            appointment.AppointmentDate = DateTime.Now;
            model.Appointment = appointment;
            ViewBag.Specialization = new SelectList(db.Specializations.ToList(), "ID", "Name");
            List<DoctorName> doctornamelist = UtilityManager.GetName();
            ViewBag.Doctors = new SelectList(doctornamelist, "ID", "Name");
            //ViewBag.Doctors = new SelectList(db.Doctors.Include("EmployeeDetail").ToList(), "ID", "EmployeeDetail.LastName");
           
            ViewBag.City = new SelectList(db.Cities.ToList(), "ID", "Name");
            ViewBag.ShiftType = new SelectList(db.ShiftTypes, "ID", "Name");
            ViewBag.PatientType = new SelectList(db.PatientTypes, "ID", "Type");
            ViewBag.Branch = new SelectList(db.BranchDetails, "ID", "Name");
            return View(model);
        }

        // POST: Patient/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PatientViewModel patientDetail)
        {

            var alreadyExists = db.PatientDetails.Where(p => p.FullName == patientDetail.PatientDetails.FullName && p.ContactNo == patientDetail.PatientDetails.ContactNo);
            if (alreadyExists.Count() > 0)
            {
                ModelState.AddModelError("", "Patient with same name and contact no already exists in the system");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var currentUserId = User.Identity.GetUserId();
                    long customerId = 1;
                    long branchId = 1;
                    if (currentUserId != null)
                    {
                        var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                        customerId = manager.FindById(currentUserId).HMSEmpID;
                        branchId = manager.FindById(currentUserId).BranchID;
                        patientDetail.PatientDetails.CreatedBy = Convert.ToInt32(customerId);
                    }

                    DateTime today = DateTime.Today;
                    //int age = today.Year - patientDetail.PatientDetails.Age;
                    patientDetail.PatientDetails.DOB = today.AddYears(- (patientDetail.PatientDetails.Age));
                    //if (patientDetail.PatientDetails.DOB.Date > today.AddYears(-age)) age--;

                    //patientDetail.PatientDetails.Age = age;
                    patientDetail.PatientDetails.CreatedOn = DateTime.Now;
                    patientDetail.PatientDetails.DateOfRegistration = DateTime.Now;
                    db.PatientDetails.Add(patientDetail.PatientDetails);
                    db.SaveChanges();
                    patientDetail.Appointment.CreatedDate = DateTime.Now;
                    patientDetail.Appointment.CreatedBy = Convert.ToInt32(customerId);
                    //patientDetail.Appointment.BranchDetails_ID = Convert.ToInt32(branchId);                   
                    patientDetail.PatientDetails.Appointments.Add(patientDetail.Appointment);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Appointments");
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            ViewBag.Specialization = new SelectList(db.Specializations.ToList(), "ID", "Name", patientDetail.Appointment.Specialization_ID);
            ViewBag.Doctors = new SelectList(db.Doctors.Include("EmployeeDetail").ToList(), "ID", "EmployeeDetail.FirstName", patientDetail.Appointment.Doctor_ID);
            ViewBag.City = new SelectList(db.Cities.ToList(), "ID", "Name", patientDetail.PatientDetails.City_ID);
            ViewBag.ShiftType = new SelectList(db.ShiftTypes, "ID", "Name", patientDetail.Appointment.ShiftType_ID);
            ViewBag.PatientType = new SelectList(db.PatientTypes, "ID", "Type", patientDetail.Appointment.PatientType_ID);
            ViewBag.Branch = new SelectList(db.BranchDetails, "ID", "Name", patientDetail.Appointment.BranchDetails_ID);
            return View(patientDetail);
        }

        public ActionResult Search(string searchType, string searchValue, string PatientType)
        {
            var patients = db.PatientDetails.Include("Appointments").AsQueryable();
            int id = Convert.ToInt32(Request["SearchType"]);
            var searchParameter = "Searching ";
            if (!string.IsNullOrEmpty(searchValue) || !string.IsNullOrEmpty(PatientType))
            {
                if (!string.IsNullOrEmpty(searchValue) && !string.IsNullOrEmpty(PatientType))
                {
                    var patientType = Convert.ToInt32(PatientType);
                    switch (id)
                    {
                        case 0:
                            int iQ = int.Parse(searchValue);
                            patients = patients.Where(p => p.ID.Equals(iQ) && p.Appointments.OrderByDescending(a => a.ID).FirstOrDefault().PatientType_ID == patientType);
                            searchParameter += " Id for ' " + searchValue + " '";
                            break;
                        case 1:
                            patients = patients.Where(p => p.FullName.Contains(searchValue) && p.Appointments.OrderByDescending(a => a.ID).FirstOrDefault().PatientType_ID == patientType);
                            searchParameter += " Patient Name for ' " + searchValue + " '";
                            break;
                        case 2:
                            patients = patients.Where(p => p.FatherOrHusbandName.Contains(searchValue) && p.Appointments.OrderByDescending(a => a.ID).FirstOrDefault().PatientType_ID == patientType);
                            searchParameter += " Father/Husband for '" + searchValue + "'";
                            break;
                        case 3:
                            patients = patients.Where(p => p.ContactNo.ToString().Contains(searchValue) && p.Appointments.OrderByDescending(a => a.ID).FirstOrDefault().PatientType_ID == patientType);
                            searchParameter += " Mobile for '" + searchValue + "'";
                            break;
                    }

                }
                else if (!string.IsNullOrEmpty(searchValue))
                {
                    switch (id)
                    {
                        case 0:
                            int iQ = int.Parse(searchValue);
                            patients = patients.Where(p => p.ID.Equals(iQ));
                            searchParameter += " Id for ' " + searchValue + " '";
                            break;
                        case 1:
                            patients = patients.Where(p => p.FullName.Contains(searchValue));
                            searchParameter += " Patient Name for ' " + searchValue + " '";
                            break;
                        case 2:
                            patients = patients.Where(p => p.FatherOrHusbandName.Contains(searchValue));
                            searchParameter += " Father/Husband for '" + searchValue + "'";
                            break;
                        case 3:
                            patients = patients.Where(p => p.ContactNo.ToString().Contains(searchValue));
                            searchParameter += " Mobile for '" + searchValue + "'";
                            break;
                    }
                }
                else
                {
                    var patientType = Convert.ToInt32(PatientType);
                    patients = patients.Where(p => p.Appointments.OrderByDescending(a => a.ID).FirstOrDefault().PatientType_ID == patientType);
                }

            }
            else
            {
                searchParameter += "ALL";
            }
            ViewBag.SearchParameter = searchParameter;
            ViewBag.PatientType = new SelectList(db.PatientTypes, "ID", "Type");
            return View(patients.OrderByDescending(o => o.ID).ToList());
        }

        // GET: Patient/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientDetail patientDetail = db.PatientDetails.Find(id);
            if (patientDetail == null)
            {
                return HttpNotFound();

            }

            ViewBag.City = new SelectList(db.Cities.ToList(), "ID", "Name");
            return View(patientDetail);
        }

        // POST: Patient/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PatientDetail patientDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patientDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.City = new SelectList(db.Cities.ToList(), "ID", "Name");
            return View(patientDetail);
        }

        // GET: Patient/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientDetail patientDetail = db.PatientDetails.Find(id);
            if (patientDetail == null)
            {
                return HttpNotFound();
            }
            return View(patientDetail);
        }

        // POST: Patient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            PatientDetail patientDetail = db.PatientDetails.Find(id);
            db.PatientDetails.Remove(patientDetail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //public JsonResult FillDoctor(int SpecializationId, string appointmentDay, int shiftTypeId)
        //{
        //    var appDate = DateTime.Parse(appointmentDay);
        //    var day = Convert.ToInt32(appDate.DayOfWeek);
        //    var dateDay = appDate.Day;
        //    var doctors = db.Doctors.Include("EmployeeDetail").Include("ShiftDates").Include("ShiftDays").Where(m => m.Specialization_ID == SpecializationId).ToList();
        //    List<SelectListItem> doctorList = new List<SelectListItem>();
        //    if (doctors.Count() > 0)
        //    {
        //        foreach (var item in doctors)
        //        {
        //            var shiftDayCount = item.ShiftDays.Where(a => a.WeekDays_ID == day && a.ShiftType_ID == shiftTypeId).FirstOrDefault();
        //            var shiftDateCount = item.ShiftDates.Where(a => a.DayAvailable.Day == dateDay && a.ShiftType_ID == shiftTypeId).FirstOrDefault();
        //            if (shiftDateCount != null || shiftDayCount != null)
        //            {
        //                doctorList.Add(new SelectListItem { Text = item.EmployeeDetail.FirstName, Value = item.ID.ToString() });
        //            }

        //        }
        //    }
        //    return Json(doctorList, JsonRequestBehavior.AllowGet);
        //}

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
