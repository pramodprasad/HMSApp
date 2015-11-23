using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HMS.Entity;

namespace HospitalManagement.ViewModels
{
    public class DoctorViewModel
    {
        public Doctor Doctor { get; set; }
        public List<AssignedShift> AssignedShift { get; set; }
    }
}