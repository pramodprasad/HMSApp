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
    
    public partial class WeekDay
    {
        public WeekDay()
        {
            this.ShiftDays = new HashSet<ShiftDay>();
        }
    
        public int ID { get; set; }
        public string NameOfTheDay { get; set; }
    
        public virtual ICollection<ShiftDay> ShiftDays { get; set; }
    }
}
