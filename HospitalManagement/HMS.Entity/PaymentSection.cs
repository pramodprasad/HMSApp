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
    
    public partial class PaymentSection
    {
        public PaymentSection()
        {
            this.DoctorVisitPaymentDetails = new HashSet<DoctorVisitPaymentDetail>();
            this.MedicalPaymentDetails = new HashSet<MedicalPaymentDetail>();
            this.PatientBills = new HashSet<PatientBill>();
            this.RegistrationPaymentDetails = new HashSet<RegistrationPaymentDetail>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
    
        public virtual ICollection<DoctorVisitPaymentDetail> DoctorVisitPaymentDetails { get; set; }
        public virtual ICollection<MedicalPaymentDetail> MedicalPaymentDetails { get; set; }
        public virtual ICollection<PatientBill> PatientBills { get; set; }
        public virtual ICollection<RegistrationPaymentDetail> RegistrationPaymentDetails { get; set; }
    }
}