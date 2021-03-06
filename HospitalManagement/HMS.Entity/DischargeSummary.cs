//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class DischargeSummary
    {
        public long ID { get; set; }
        public int AppointmentID { get; set; }
        public long PatientDetailsID { get; set; }
        public long DoctorID { get; set; }
        public System.DateTime DateOfAdmission { get; set; }
        public System.DateTime DateOfDischarge { get; set; }
        public string Daignosis { get; set; }
        public string BriffSummary { get; set; }
        public string Investigation { get; set; }
        public string OperationNotes { get; set; }
        public string TreatmentGiven { get; set; }
        public string Advice { get; set; }
        public string ToattendClinicOnAfter { get; set; }
        public string OtherDetails { get; set; }
    
        public virtual Appointment Appointment { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual PatientDetail PatientDetail { get; set; }
    }
}
