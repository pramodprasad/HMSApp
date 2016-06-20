using System;
using System.Collections.Generic;
using HMS.Entity;

namespace HospitalManagement.ViewModels
{
    public class ServicePaymentViewModel
    {
        public ServicePayment ServicePayment { get; set; }

        public List<ServicePayment> ServicePaymentList { get; set; }
    }
}