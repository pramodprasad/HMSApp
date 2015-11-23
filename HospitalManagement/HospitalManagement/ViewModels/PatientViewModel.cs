using System;
using System.Collections.Generic;
using HMS.Entity;
namespace HospitalManagement.ViewModels
{
    public class PatientViewModel
    {
        public PatientDetail PatientDetails { get; set; }
        public Appointment Appointment { get; set; }
    }
}