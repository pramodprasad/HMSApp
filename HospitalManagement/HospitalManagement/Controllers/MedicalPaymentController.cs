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

namespace HospitalManagement.Controllers
{
    public class MedicalPaymentController : Controller
    {
        private HMSTEntities db = new HMSTEntities();

        // GET: MedicalPayment
        public async Task<ActionResult> Index()
        {
            var medicalPaymentDetails = db.MedicalPaymentDetails.Include(m => m.Appointment).Include(m => m.Doctor).Include(m => m.PaymentMode).Include(m => m.PaymentSection).Include(m => m.PaymentStau);
            return View(await medicalPaymentDetails.ToListAsync());
        }

        // GET: MedicalPayment/Details/5
        public async Task<ActionResult> Details(long? id)
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

        // GET: MedicalPayment/Create
        public ActionResult Create()
        {
            ViewBag.Appointment_ID = new SelectList(db.Appointments, "ID", "ReferalDetails");
            ViewBag.Doctor_ID = new SelectList(db.Doctors, "ID", "OtherDetails");
            ViewBag.PaymentMode_ID = new SelectList(db.PaymentModes, "ID", "Mode");
            ViewBag.PaymentSection_ID = new SelectList(db.PaymentSections, "ID", "Name");
            ViewBag.PaymentStatusID = new SelectList(db.PaymentStaus, "ID", "Details");
            return View();
        }

        // POST: MedicalPayment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,MedicalAmount,PaymentStatusID,PaymentReceivedBy,PaymentReceivedDate,DiscountAmount,DiscountedBy,DiscountDescription,PaymentDetails,Doctor_ID,PaymentMode_ID,PaymentSection_ID,Appointment_ID,Quantity,MedicalEquipmentID,NetAmount,PayAmount,DueAmount")] MedicalPaymentDetail medicalPaymentDetail)
        {
            if (ModelState.IsValid)
            {
                db.MedicalPaymentDetails.Add(medicalPaymentDetail);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Appointment_ID = new SelectList(db.Appointments, "ID", "ReferalDetails", medicalPaymentDetail.Appointment_ID);
            ViewBag.Doctor_ID = new SelectList(db.Doctors, "ID", "OtherDetails", medicalPaymentDetail.Doctor_ID);
            ViewBag.PaymentMode_ID = new SelectList(db.PaymentModes, "ID", "Mode", medicalPaymentDetail.PaymentMode_ID);
            ViewBag.PaymentSection_ID = new SelectList(db.PaymentSections, "ID", "Name", medicalPaymentDetail.PaymentSection_ID);
            ViewBag.PaymentStatusID = new SelectList(db.PaymentStaus, "ID", "Details", medicalPaymentDetail.PaymentStatusID);
            return View(medicalPaymentDetail);
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
            ViewBag.Appointment_ID = new SelectList(db.Appointments, "ID", "ReferalDetails", medicalPaymentDetail.Appointment_ID);
            ViewBag.Doctor_ID = new SelectList(db.Doctors, "ID", "OtherDetails", medicalPaymentDetail.Doctor_ID);
            ViewBag.PaymentMode_ID = new SelectList(db.PaymentModes, "ID", "Mode", medicalPaymentDetail.PaymentMode_ID);
            ViewBag.PaymentSection_ID = new SelectList(db.PaymentSections, "ID", "Name", medicalPaymentDetail.PaymentSection_ID);
            ViewBag.PaymentStatusID = new SelectList(db.PaymentStaus, "ID", "Details", medicalPaymentDetail.PaymentStatusID);
            return View(medicalPaymentDetail);
        }

        // POST: MedicalPayment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,MedicalAmount,PaymentStatusID,PaymentReceivedBy,PaymentReceivedDate,DiscountAmount,DiscountedBy,DiscountDescription,PaymentDetails,Doctor_ID,PaymentMode_ID,PaymentSection_ID,Appointment_ID,Quantity,MedicalEquipmentID,NetAmount,PayAmount,DueAmount")] MedicalPaymentDetail medicalPaymentDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medicalPaymentDetail).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Appointment_ID = new SelectList(db.Appointments, "ID", "ReferalDetails", medicalPaymentDetail.Appointment_ID);
            ViewBag.Doctor_ID = new SelectList(db.Doctors, "ID", "OtherDetails", medicalPaymentDetail.Doctor_ID);
            ViewBag.PaymentMode_ID = new SelectList(db.PaymentModes, "ID", "Mode", medicalPaymentDetail.PaymentMode_ID);
            ViewBag.PaymentSection_ID = new SelectList(db.PaymentSections, "ID", "Name", medicalPaymentDetail.PaymentSection_ID);
            ViewBag.PaymentStatusID = new SelectList(db.PaymentStaus, "ID", "Details", medicalPaymentDetail.PaymentStatusID);
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
