using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using HospitalManagement.Models;
using HMS.Entity;

namespace HospitalManagement.Controllers
{
    public class EmployeeDetailsController : Controller
    {
        private HMSTEntities db = new HMSTEntities();

        public EmployeeDetailsController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
            UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager) { AllowOnlyAlphanumericUserNames = false };
            var provider = new DpapiDataProtectionProvider("HospitalManagement");
            UserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser, string>(provider.Create("UserToken")) as IUserTokenProvider<ApplicationUser, string>;
        }

        public EmployeeDetailsController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }
        // GET: EmployeeDetails
        public ActionResult Index()
        {
            var employeeDetails = db.EmployeeDetails.Include(e => e.BranchDetail).Include(e => e.City).Include(e => e.EmployeeType).Include(e => e.Title);
            return View(employeeDetails.ToList());
        }

        // GET: EmployeeDetails/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeDetail employeeDetail = db.EmployeeDetails.Find(id);
            if (employeeDetail == null)
            {
                return HttpNotFound();
            }
            return View(employeeDetail);
        }

        // GET: EmployeeDetails/Create
        public ActionResult Create()
        {
            RegisterViewModel model = new RegisterViewModel();
            EmployeeDetail empDetails = new EmployeeDetail();
            Doctor doctor = new Doctor();
            model.EmpDetails = empDetails;
            model.Doctor = doctor;
            ViewBag.BranchDetail_ID = new SelectList(db.BranchDetails, "ID", "Name");
            ViewBag.City_ID = new SelectList(db.Cities, "ID", "name");
            ViewBag.EmployeeType_ID = new SelectList(db.EmployeeTypes, "ID", "Description");
            ViewBag.Title_ID = new SelectList(db.Titles, "ID", "Name");
            ViewBag.Qualification = new SelectList(db.Qualifications, "ID", "Name");
            ViewBag.Specialization = new SelectList(db.Specializations, "ID", "Name");
            return View(model);
        }

        // POST: EmployeeDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.ContainsKey("EmpDetails.CreatedDate"))
            {
                ModelState["EmpDetails.CreatedDate"].Errors.Clear();
            }
            else if (ModelState.ContainsKey("EmpDetails.Password"))
            {
                ModelState["EmpDetails.Password"].Errors.Clear();
            }
            else if (ModelState.ContainsKey("Doctor.CreatedOn"))
            {
                ModelState["Doctor.CreatedOn"].Errors.Clear();
            }
            else if (ModelState.ContainsKey("Doctor.UpdatedOn"))
            {
                ModelState["Doctor.UpdatedOn"].Errors.Clear();
            }
            else if (ModelState.ContainsKey("Doctor.UpdatedBy"))
            {
                ModelState["Doctor.UpdatedBy"].Errors.Clear();
            }
            else if (ModelState.ContainsKey("Doctor.CreatedBy"))
            {
                ModelState["Doctor.CreatedBy"].Errors.Clear();
            }
            else if (ModelState.ContainsKey("Doctor.Daywise"))
            {
                ModelState["Doctor.Daywise"].Errors.Clear();
            }
            else if (ModelState.ContainsKey("Doctor.Datewise"))
            {
                ModelState["Doctor.Datewise"].Errors.Clear();
            }

            if (ModelState.IsValid)
            {
                model.EmpDetails.CreatedDate = DateTime.Now;
                model.EmpDetails.Password = model.Password;
                model.Doctor.CreatedOn = DateTime.Now;
                model.Doctor.UpdatedOn = DateTime.Now;
                model.Doctor.UpdatedBy = 1;
                model.Doctor.CreatedBy = 1;

                db.EmployeeDetails.Add(model.EmpDetails);
                db.SaveChanges();

                var user = new ApplicationUser { UserName = model.UserName, HMSEmpID = model.EmpDetails.ID, Email=model.EmailAddress};
                var result = await UserManager.CreateAsync(user, model.Password);

                if (model.EmpDetails.EmployeeType_ID == 2)
                {
                    model.EmpDetails.Doctors.Add(model.Doctor);
                    db.SaveChanges();
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BranchDetail_ID = new SelectList(db.BranchDetails, "ID", "Name", model.EmpDetails.BranchDetail_ID);
            ViewBag.City_ID = new SelectList(db.Cities, "ID", "name", model.EmpDetails.City_ID);
            ViewBag.EmployeeType_ID = new SelectList(db.EmployeeTypes, "ID", "Description", model.EmpDetails.EmployeeType_ID);
            ViewBag.Title_ID = new SelectList(db.Titles, "ID", "Name", model.EmpDetails.Title_ID);
            ViewBag.Qualification = new SelectList(db.Qualifications, "ID", "Name", model.Doctor.Specialization_ID);
            ViewBag.Specialization = new SelectList(db.Specializations, "ID", "Name", model.Doctor.Qualification_ID);
            return View(model);
        }

        // GET: EmployeeDetails/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeDetail employeeDetail = db.EmployeeDetails.Find(id);
            if (employeeDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchDetail_ID = new SelectList(db.BranchDetails, "ID", "Name", employeeDetail.BranchDetail_ID);
            ViewBag.City_ID = new SelectList(db.Cities, "ID", "name", employeeDetail.City_ID);
            ViewBag.EmployeeType_ID = new SelectList(db.EmployeeTypes, "ID", "Description", employeeDetail.EmployeeType_ID);
            ViewBag.Title_ID = new SelectList(db.Titles, "ID", "Name", employeeDetail.Title_ID);
            return View(employeeDetail);
        }

        // POST: EmployeeDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,MiddleName,LastName,MobileNo,OtherContactNo,Address1,Address2,AadharNo,VoterIDNo,DriverLicenceNo,CreatedDate,Password,BranchDetail_ID,City_ID,EmployeeType_ID,Title_ID")] EmployeeDetail employeeDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeeDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BranchDetail_ID = new SelectList(db.BranchDetails, "ID", "Name", employeeDetail.BranchDetail_ID);
            ViewBag.City_ID = new SelectList(db.Cities, "ID", "name", employeeDetail.City_ID);
            ViewBag.EmployeeType_ID = new SelectList(db.EmployeeTypes, "ID", "Description", employeeDetail.EmployeeType_ID);
            ViewBag.Title_ID = new SelectList(db.Titles, "ID", "Name", employeeDetail.Title_ID);
            return View(employeeDetail);
        }

        // GET: EmployeeDetails/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeDetail employeeDetail = db.EmployeeDetails.Find(id);
            if (employeeDetail == null)
            {
                return HttpNotFound();
            }
            return View(employeeDetail);
        }

        // POST: EmployeeDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            EmployeeDetail employeeDetail = db.EmployeeDetails.Find(id);
            db.EmployeeDetails.Remove(employeeDetail);
            db.SaveChanges();
            return RedirectToAction("Index");
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
