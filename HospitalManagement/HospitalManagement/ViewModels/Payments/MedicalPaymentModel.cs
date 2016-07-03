using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagement.ViewModels.Payments
{
    public class MedicalPaymentModel
    {
        public int AppointmentId { get; set; }
        public string PatientName { get; set; }
        public decimal MedicalCharge { get; set; }
        public int Qty { get; set; }
        public decimal Discount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
        public string DoctorName { get; set; }
    }
}