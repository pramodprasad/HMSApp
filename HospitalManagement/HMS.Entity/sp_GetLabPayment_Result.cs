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
    
    public partial class sp_GetLabPayment_Result
    {
        public string LabCategory { get; set; }
        public string LabTestName { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public decimal LabCharge { get; set; }
        public int Quantity { get; set; }
        public decimal NetAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
        public string Mode { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public long ContactNo { get; set; }
        public string FatherOrHusbandName { get; set; }
        public string Gender { get; set; }
        public string Doctor { get; set; }
        public long DoctorID { get; set; }
        public string PatientType { get; set; }
        public int PTypeID { get; set; }
        public int AppointmentID { get; set; }
    }
}
