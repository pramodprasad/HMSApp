using System.Collections.Generic;
using HMS.Entity;
namespace HospitalManagement.ViewModels
{
    public class MedicalPaymentViewModel
    {
        public MedicalPaymentDetail MedicalPayment { get; set; }
        public List<MedicalPaymentDetail> MedicalPayments { get; set; }
    }
}