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
    
    public partial class LabPayment
    {
        public long ID { get; set; }
        public int Quantity { get; set; }
        public decimal LabCharge { get; set; }
        public decimal Discount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public Nullable<int> Appointment_ID { get; set; }
        public Nullable<long> Doctor_ID { get; set; }
        public Nullable<int> LabCategory_ID { get; set; }
        public Nullable<int> LabTest_ID { get; set; }
    
        public virtual Appointment Appointment { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual LabCategory LabCategory { get; set; }
        public virtual LabTest LabTest { get; set; }
    }
}