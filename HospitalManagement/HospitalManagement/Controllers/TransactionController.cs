using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using HospitalManagement.Models;
using HospitalManagement.ViewModels;
using HMS.Entity;
namespace HospitalManagement.Controllers
{
    public class TransactionController : Controller
    {
        private HMSDBEntities db = new HMSDBEntities();
        // GET: Transaction
        public ActionResult Service(string searchType, string searchValue)
        {
            ServiceViewModel model = new ServiceViewModel();
            var patients = (from p in db.PatientDetails select p).AsQueryable();
            int id = Convert.ToInt32(Request["SearchType"]);
            var searchParameter = "Searching ";

            if (!string.IsNullOrWhiteSpace(searchValue))
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

                model.PatientDetail = patients.ToList();
            }
            else
            {
                model.PatientDetail = db.PatientDetails.ToList();//db.PatientDetails.Include("PatientStatus").Where(p => p.PatientStatus.ToList().Where(v => v.VisitDate >= DateTime.Now.AddDays(-30)).Count() > 0).ToList();
                searchParameter += "ALL";
            }

            ViewBag.SearchParameter = searchParameter;
            return View(model);
        }

        public ActionResult AddService(int id)
        {
            var patientList = db.PatientVisits.Include(p  => p.Appointment).Include(p => p.Appointment.PatientDetail).Include(p => p.Appointment.Doctor).Include(p => p.Appointment.Doctor.EmployeeDetail).Include(p => p.Appointment.ShiftType).Where(s => s.Appointment.PatientDetails_ID == id).OrderByDescending(p => p.VisitedDate).ToList();
            return View(patientList);
            //return View();
        }

        public ActionResult Lab()
        {
            return View();
        }

        public ActionResult AddLab(int id)
        {
            return View();
        }

        public ActionResult Registration()
        {
            return View();
        }

        public ActionResult AddRegistration(int id)
        {
            return View();
        }

        public ActionResult Other()
        {
            return View();
        }

        public ActionResult AddOther(int id)
        {
            return View();
        }
    }
}