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
using Microsoft.AspNet.Identity;
using HospitalManagement.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HospitalManagement.Controllers
{
    public class DoctorsController : Controller
    {
        private HMSDBEntities db = new HMSDBEntities();

        // GET: Doctors
        public ActionResult Index()
        {
            var doctors = db.Doctors.Include(d => d.EmployeeDetail).Include(d => d.Qualification).Include(d => d.Specialization);
            return View(doctors.ToList());
        }

        // GET: Doctors/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Include("employeedetail").Where(m => m.ID == id).FirstOrDefault();
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Include("employeedetail").Where(m => m.ID == id).FirstOrDefault();
            if (doctor == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeDetail_ID = new SelectList(db.EmployeeDetails, "ID", "FirstName", doctor.EmployeeDetail_ID);
            ViewBag.Qualification_ID = new SelectList(db.Qualifications, "ID", "Name", doctor.Qualification_ID);
            ViewBag.Specialization_ID = new SelectList(db.Specializations, "ID", "Name", doctor.Specialization_ID);
            ViewBag.CreatedBy = new SelectList(db.EmployeeDetails, "ID", "FirstName", doctor.CreatedBy);
            ViewBag.UpdatedBy = new SelectList(db.EmployeeDetails, "ID", "FirstName", doctor.UpdatedBy);
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,OtherDetails,RegistrationNo,CreatedOn,UpdatedOn,CreatedBy,UpdatedBy,EmployeeDetail_ID,Daywise,Datewise,Qualification_ID,Specialization_ID")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                doctor.UpdatedOn = DateTime.Now;
                var currentUserId = User.Identity.GetUserId();
                if (currentUserId != null)
                {
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var customerId = manager.FindById(currentUserId).HMSEmpID;
                    doctor.UpdatedBy = Convert.ToInt32(customerId);
                }
                db.Entry(doctor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeDetail_ID = new SelectList(db.EmployeeDetails, "ID", "FirstName", doctor.EmployeeDetail_ID);
            ViewBag.Qualification_ID = new SelectList(db.Qualifications, "ID", "Name", doctor.Qualification_ID);
            ViewBag.Specialization_ID = new SelectList(db.Specializations, "ID", "Name", doctor.Specialization_ID);
            ViewBag.CreatedBy = new SelectList(db.EmployeeDetails, "ID", "FirstName", doctor.CreatedBy);
            ViewBag.UpdatedBy = new SelectList(db.EmployeeDetails, "ID", "FirstName", doctor.UpdatedBy);
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Include("employeedetail").Where(m => m.ID == id).FirstOrDefault();
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Doctor doctor = db.Doctors.Find(id);
            db.Doctors.Remove(doctor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ShiftMapping(long id)
        {
            var model = new ShiftMapViewModel();
            Doctor doctor = db.Doctors.Include("employeedetail").Where(m => m.ID == id).FirstOrDefault();
            List<AssignedDayData> assignList = new List<AssignedDayData>();
            List<AssignedDateData> dateList = new List<AssignedDateData>();
            
            if (doctor.Daywise == true)
            {
                var listMappingDetails = db.ShiftDays.Where(m => m.Doctor_ID == id).ToList();

                foreach (var day in db.WeekDays)
                {
                    
                    AssignedDayData assignDay = new AssignedDayData();
                    var item = listMappingDetails.Where(m => m.WeekDays_ID == day.ID).FirstOrDefault();
                    if(item != null)
                    {
                        assignDay.IsSelected = item.Status;
                        assignDay.ShiftType = Convert.ToInt32(item.ShiftType_ID);
                        assignDay.StartTime = item.StartTime;
                        assignDay.EndTime = item.EndTime;

                    }

                    assignDay.Day = day.ID;
                    assignDay.DayName = day.NameOfTheDay;
                    assignList.Add(assignDay);
                }
            }

            dateList.Add(new AssignedDateData ());

            model.Doctor = doctor;
            model.AssignedDateData = dateList;
            model.AssignedDayData = assignList;
            ViewBag.Shifts = new SelectList(db.ShiftTypes, "ID", "Name");
            return View(model);
        }

        [HttpPost]
        public ActionResult ShiftMapping(ShiftMapViewModel model)
        {

            if (ModelState.ContainsKey("AssignedDayData[0].ShiftType"))
            {
                ModelState["AssignedDayData[0].ShiftType"].Errors.Clear();
            }
            if (ModelState.ContainsKey("AssignedDayData[1].ShiftType"))
            {
                ModelState["AssignedDayData[1].ShiftType"].Errors.Clear();
            }
            if (ModelState.ContainsKey("AssignedDayData[2].ShiftType"))
            {
                ModelState["AssignedDayData[2].ShiftType"].Errors.Clear();
            }
            if (ModelState.ContainsKey("AssignedDayData[3].ShiftType"))
            {
                ModelState["AssignedDayData[3].ShiftType"].Errors.Clear();
            }
            if (ModelState.ContainsKey("AssignedDayData[4].ShiftType"))
            {
                ModelState["AssignedDayData[4].ShiftType"].Errors.Clear();
            }
            if (ModelState.ContainsKey("AssignedDayData[5].ShiftType"))
            {
                ModelState["AssignedDayData[5].ShiftType"].Errors.Clear();
            }
            if (ModelState.ContainsKey("AssignedDayData[6].ShiftType"))
            {
                ModelState["AssignedDayData[6].ShiftType"].Errors.Clear();
            }
            if (ModelState.ContainsKey("AssignedDateData.ShiftType"))
            {
                ModelState["AssignedDateData.ShiftType"].Errors.Clear();
            }

            if (ModelState.IsValid)
            {
                if (model.Doctor.Daywise)
                {
                    foreach (var item in model.AssignedDayData)
                    {
                        if (item.IsSelected)
                        {
                            var shiftType = db.ShiftDays.Where(m => m.Doctor_ID == model.Doctor.ID && m.WeekDays_ID == item.Day ).FirstOrDefault();
                            if(shiftType != null)
                            {
                                if(item.ShiftType != 0)
                                {
                                    shiftType.ShiftType_ID = item.ShiftType;
                                }
                                if (item.StartTime != "")
                                {
                                    shiftType.StartTime = item.StartTime;
                                }
                                if (item.EndTime != "")
                                {
                                    shiftType.EndTime = item.EndTime;
                                }
                                shiftType.Status = true;

                                db.Entry(shiftType).State = EntityState.Modified;
                            }
                            else
                            {
                                ShiftDay dayShift = new ShiftDay
                                {
                                    WeekDays_ID = item.Day,
                                    StartTime = item.StartTime,
                                    EndTime = item.EndTime,
                                    ShiftType_ID = item.ShiftType,
                                    Doctor_ID = model.Doctor.ID,
                                    Status = true
                                };

                                db.ShiftDays.Add(dayShift);
                            }

                            db.SaveChanges();
                        }
                        else
                        {
                            var shiftType = db.ShiftDays.Where(m => m.Doctor_ID == model.Doctor.ID && m.WeekDays_ID == item.Day).FirstOrDefault();
                            if(shiftType != null)
                            {
                                shiftType.Status = false;
                                db.Entry(shiftType).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                    }

                    return RedirectToAction("Index", "Doctors");
                }
                else
                {
                    foreach (var item in model.AssignedDateData)
                    {
                        ShiftDate dateShift = new ShiftDate { DayAvailable = item.DateAvailable, StartTime = item.StartTime, EndTime = item.EndTime, Doctor_ID = model.Doctor.ID, ShiftType_ID = item.ShiftType, Status = true };
                        db.ShiftDates.Add(dateShift);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index", "Doctors");
                }

            }

            ViewBag.Shifts = new SelectList(db.ShiftTypes, "ID", "Name");
            return View(model);
        }

        [HttpGet]
        public ActionResult ShiftDetails(long id)
        {
            return View();
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
