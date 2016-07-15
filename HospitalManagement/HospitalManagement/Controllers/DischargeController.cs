﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HMS.Entity;

namespace HospitalManagement.Controllers
{
    public class DischargeController : Controller
    {
        private HMSDBEntities db = new HMSDBEntities();

        // GET: Discharge
        public async Task<ActionResult> Index()
        {
            var dischargeSummaries = db.DischargeSummaries.Include(d => d.Appointment).Include(d => d.Doctor).Include(d => d.PatientDetail);
            return View(await dischargeSummaries.ToListAsync());
        }

        // GET: Discharge/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DischargeSummary dischargeSummary = await db.DischargeSummaries.FindAsync(id);
            if (dischargeSummary == null)
            {
                return HttpNotFound();
            }
            return View(dischargeSummary);
        }

        // GET: Discharge/Create
        public ActionResult Create()
        {
            ViewBag.AppointmentID = new SelectList(db.Appointments, "ID", "ReferalDetails");
            ViewBag.DoctorID = new SelectList(db.Doctors, "ID", "OtherDetails");
            ViewBag.PatientDetailsID = new SelectList(db.PatientDetails, "ID", "FullName");
            return View();
        }

        // POST: Discharge/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,AppointmentID,PatientDetailsID,DoctorID,DateOfAdmission,DateOfDischarge,Daignosis,BriffSummary,Investigation,OperationNotes,TreatmentGiven,Advice,ToattendClinicOnAfter,OtherDetails")] DischargeSummary dischargeSummary)
        {
            if (ModelState.IsValid)
            {
                db.DischargeSummaries.Add(dischargeSummary);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AppointmentID = new SelectList(db.Appointments, "ID", "ReferalDetails", dischargeSummary.AppointmentID);
            ViewBag.DoctorID = new SelectList(db.Doctors, "ID", "OtherDetails", dischargeSummary.DoctorID);
            ViewBag.PatientDetailsID = new SelectList(db.PatientDetails, "ID", "FullName", dischargeSummary.PatientDetailsID);
            return View(dischargeSummary);
        }

        // GET: Discharge/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DischargeSummary dischargeSummary = await db.DischargeSummaries.FindAsync(id);
            if (dischargeSummary == null)
            {
                return HttpNotFound();
            }
            ViewBag.AppointmentID = new SelectList(db.Appointments, "ID", "ReferalDetails", dischargeSummary.AppointmentID);
            ViewBag.DoctorID = new SelectList(db.Doctors, "ID", "OtherDetails", dischargeSummary.DoctorID);
            ViewBag.PatientDetailsID = new SelectList(db.PatientDetails, "ID", "FullName", dischargeSummary.PatientDetailsID);
            return View(dischargeSummary);
        }

        // POST: Discharge/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,AppointmentID,PatientDetailsID,DoctorID,DateOfAdmission,DateOfDischarge,Daignosis,BriffSummary,Investigation,OperationNotes,TreatmentGiven,Advice,ToattendClinicOnAfter,OtherDetails")] DischargeSummary dischargeSummary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dischargeSummary).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AppointmentID = new SelectList(db.Appointments, "ID", "ReferalDetails", dischargeSummary.AppointmentID);
            ViewBag.DoctorID = new SelectList(db.Doctors, "ID", "OtherDetails", dischargeSummary.DoctorID);
            ViewBag.PatientDetailsID = new SelectList(db.PatientDetails, "ID", "FullName", dischargeSummary.PatientDetailsID);
            return View(dischargeSummary);
        }

        // GET: Discharge/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DischargeSummary dischargeSummary = await db.DischargeSummaries.FindAsync(id);
            if (dischargeSummary == null)
            {
                return HttpNotFound();
            }
            return View(dischargeSummary);
        }

        // POST: Discharge/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            DischargeSummary dischargeSummary = await db.DischargeSummaries.FindAsync(id);
            db.DischargeSummaries.Remove(dischargeSummary);
            await db.SaveChangesAsync();
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