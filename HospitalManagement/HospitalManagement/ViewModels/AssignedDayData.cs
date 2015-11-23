using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace HospitalManagement.ViewModels
{
    public class AssignedDayData
    {

        public int Day { get; set; }
        public string DayName { get; set; }
        public int ShiftType { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool IsSelected { get; set; }
    }

    public class AssignedDateData
    {
        public DateTime DateAvailable { get; set; }
        public int ShiftType { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

    }
}