//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1
{
    using System;
    using System.Collections.Generic;
    
    public partial class TimetableTimeslot_MTM
    {
        public int ID { get; set; }
        public int TimetableID { get; set; }
        public int TimeslotID { get; set; }
    
        public virtual Timeslot Timeslot { get; set; }
        public virtual Timetable Timetable { get; set; }
    }
}
