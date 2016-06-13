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
    
    public partial class ServicePayment
    {
        public long ID { get; set; }
        public int ServiceUnit { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal Discount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public Nullable<long> Doctor_ID { get; set; }
        public Nullable<long> Service_ID { get; set; }
        public Nullable<long> ServiceSubCategory_ID { get; set; }
        public Nullable<int> Appointment_ID { get; set; }
        public Nullable<long> PaymentStatusID { get; set; }
        public int PaymentModeID { get; set; }
    
        public virtual Appointment Appointment { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual EmployeeDetail EmployeeDetail { get; set; }
        public virtual EmployeeDetail EmployeeDetail1 { get; set; }
        public virtual PaymentMode PaymentMode { get; set; }
        public virtual PaymentStau PaymentStau { get; set; }
        public virtual Service Service { get; set; }
        public virtual ServiceSubCategory ServiceSubCategory { get; set; }
    }
}
