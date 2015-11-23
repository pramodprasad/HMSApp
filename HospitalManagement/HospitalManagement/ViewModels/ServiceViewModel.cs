using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HMS.Entity;

namespace HospitalManagement.ViewModels
{
    public class ServiceViewModel
    {
        public List<PatientDetail> PatientDetail { get; set; }
        //public ICollection<patientStatu> patientStatu { get; set; }
    }
}