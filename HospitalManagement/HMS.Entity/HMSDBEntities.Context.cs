﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HMS.Entity
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class HMSTEntities : DbContext
    {
        public HMSTEntities()
            : base("name=HMSTEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<BedAllotment> BedAllotments { get; set; }
        public virtual DbSet<BranchDetail> BranchDetails { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<CounterTable> CounterTables { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<DoctorBillDetail> DoctorBillDetails { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<DoctorVisitPaymentDetail> DoctorVisitPaymentDetails { get; set; }
        public virtual DbSet<EmployeeDetail> EmployeeDetails { get; set; }
        public virtual DbSet<EmployeeType> EmployeeTypes { get; set; }
        public virtual DbSet<LabCategory> LabCategories { get; set; }
        public virtual DbSet<LabPayment> LabPayments { get; set; }
        public virtual DbSet<LabTest> LabTests { get; set; }
        public virtual DbSet<MedicalEquipment> MedicalEquipments { get; set; }
        public virtual DbSet<MedicalPaymentDetail> MedicalPaymentDetails { get; set; }
        public virtual DbSet<PatientBill> PatientBills { get; set; }
        public virtual DbSet<PatientDetail> PatientDetails { get; set; }
        public virtual DbSet<PatientStatu> PatientStatus { get; set; }
        public virtual DbSet<PatientType> PatientTypes { get; set; }
        public virtual DbSet<PatientVisit> PatientVisits { get; set; }
        public virtual DbSet<PaymentMode> PaymentModes { get; set; }
        public virtual DbSet<PaymentSection> PaymentSections { get; set; }
        public virtual DbSet<PaymentStau> PaymentStaus { get; set; }
        public virtual DbSet<Qualification> Qualifications { get; set; }
        public virtual DbSet<RegistrationPaymentDetail> RegistrationPaymentDetails { get; set; }
        public virtual DbSet<RoomDetail> RoomDetails { get; set; }
        public virtual DbSet<ServicePayment> ServicePayments { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<ServiceSubCategory> ServiceSubCategories { get; set; }
        public virtual DbSet<ShiftDate> ShiftDates { get; set; }
        public virtual DbSet<ShiftDay> ShiftDays { get; set; }
        public virtual DbSet<ShiftType> ShiftTypes { get; set; }
        public virtual DbSet<Specialization> Specializations { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<Title> Titles { get; set; }
        public virtual DbSet<VisitReason> VisitReasons { get; set; }
        public virtual DbSet<WardCategory> WardCategories { get; set; }
        public virtual DbSet<WardOrRoomChargeDetail> WardOrRoomChargeDetails { get; set; }
        public virtual DbSet<Ward> Wards { get; set; }
        public virtual DbSet<WeekDay> WeekDays { get; set; }
    
        public virtual ObjectResult<sp_BranchDetails_Result> sp_BranchDetails(Nullable<long> branchID)
        {
            var branchIDParameter = branchID.HasValue ?
                new ObjectParameter("BranchID", branchID) :
                new ObjectParameter("BranchID", typeof(long));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_BranchDetails_Result>("sp_BranchDetails", branchIDParameter);
        }
    
        public virtual ObjectResult<sp_GetPrescription_Result> sp_GetPrescription(Nullable<int> patientVisitID)
        {
            var patientVisitIDParameter = patientVisitID.HasValue ?
                new ObjectParameter("PatientVisitID", patientVisitID) :
                new ObjectParameter("PatientVisitID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetPrescription_Result>("sp_GetPrescription", patientVisitIDParameter);
        }
    
        public virtual ObjectResult<sp_GetReceipt_Result> sp_GetReceipt(Nullable<int> patientVisitID)
        {
            var patientVisitIDParameter = patientVisitID.HasValue ?
                new ObjectParameter("PatientVisitID", patientVisitID) :
                new ObjectParameter("PatientVisitID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetReceipt_Result>("sp_GetReceipt", patientVisitIDParameter);
        }
    
        public virtual ObjectResult<sp_GetLabPayment_Result> sp_GetLabPayment(Nullable<int> appointmentID, string paymentMode, Nullable<int> patientType, Nullable<long> doctorID, Nullable<System.DateTime> createdDate)
        {
            var appointmentIDParameter = appointmentID.HasValue ?
                new ObjectParameter("AppointmentID", appointmentID) :
                new ObjectParameter("AppointmentID", typeof(int));
    
            var paymentModeParameter = paymentMode != null ?
                new ObjectParameter("PaymentMode", paymentMode) :
                new ObjectParameter("PaymentMode", typeof(string));
    
            var patientTypeParameter = patientType.HasValue ?
                new ObjectParameter("PatientType", patientType) :
                new ObjectParameter("PatientType", typeof(int));
    
            var doctorIDParameter = doctorID.HasValue ?
                new ObjectParameter("DoctorID", doctorID) :
                new ObjectParameter("DoctorID", typeof(long));
    
            var createdDateParameter = createdDate.HasValue ?
                new ObjectParameter("CreatedDate", createdDate) :
                new ObjectParameter("CreatedDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetLabPayment_Result>("sp_GetLabPayment", appointmentIDParameter, paymentModeParameter, patientTypeParameter, doctorIDParameter, createdDateParameter);
        }
    
        public virtual ObjectResult<sp_GetRegistrationPayment_Result> sp_GetRegistrationPayment(Nullable<int> patientVisitID, string paymentMode, Nullable<int> patientType, string doctor, Nullable<System.DateTime> visitedDate)
        {
            var patientVisitIDParameter = patientVisitID.HasValue ?
                new ObjectParameter("PatientVisitID", patientVisitID) :
                new ObjectParameter("PatientVisitID", typeof(int));
    
            var paymentModeParameter = paymentMode != null ?
                new ObjectParameter("PaymentMode", paymentMode) :
                new ObjectParameter("PaymentMode", typeof(string));
    
            var patientTypeParameter = patientType.HasValue ?
                new ObjectParameter("PatientType", patientType) :
                new ObjectParameter("PatientType", typeof(int));
    
            var doctorParameter = doctor != null ?
                new ObjectParameter("Doctor", doctor) :
                new ObjectParameter("Doctor", typeof(string));
    
            var visitedDateParameter = visitedDate.HasValue ?
                new ObjectParameter("VisitedDate", visitedDate) :
                new ObjectParameter("VisitedDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetRegistrationPayment_Result>("sp_GetRegistrationPayment", patientVisitIDParameter, paymentModeParameter, patientTypeParameter, doctorParameter, visitedDateParameter);
        }
    }
}
