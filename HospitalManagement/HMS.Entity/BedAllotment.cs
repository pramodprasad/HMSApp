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
    
    public partial class BedAllotment
    {
        public long ID { get; set; }
        public System.DateTime DateOfAllotment { get; set; }
        public int NoOfDays { get; set; }
        public bool IsOccupied { get; set; }
        public Nullable<int> RoomDetails_ID { get; set; }
        public Nullable<int> Appointment_ID { get; set; }
    
        public virtual Appointment Appointment { get; set; }
        public virtual RoomDetail RoomDetail { get; set; }
    }
}
