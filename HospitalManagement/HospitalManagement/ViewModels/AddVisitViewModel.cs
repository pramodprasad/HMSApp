using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HMS.Entity;

namespace HospitalManagement.ViewModels
{
    public class AddVisitViewModel
    {
        public PatientVisit PatientVisit { get; set; }
        public Appointment Appointment { get; set; }
    }
}