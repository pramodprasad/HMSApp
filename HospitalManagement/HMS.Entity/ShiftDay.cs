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
    using System.Collections.Generic;
    
    public partial class ShiftDay
    {
        public int ID { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool Status { get; set; }
        public Nullable<int> ShiftType_ID { get; set; }
        public Nullable<int> WeekDays_ID { get; set; }
        public Nullable<long> Doctor_ID { get; set; }
    
        public virtual Doctor Doctor { get; set; }
        public virtual ShiftType ShiftType { get; set; }
        public virtual WeekDay WeekDay { get; set; }
    }
}
