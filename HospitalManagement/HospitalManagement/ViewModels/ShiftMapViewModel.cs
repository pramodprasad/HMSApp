using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using HospitalManagement.Models;
using HMS.Entity;

namespace HospitalManagement.ViewModels
{
    public class ShiftMapViewModel
    {
        public Doctor Doctor { get; set; }
        public List<AssignedDayData> AssignedDayData { get; set; }
        public List<AssignedDateData> AssignedDateData { get; set; }
    }
}