using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Domain.Entities
{
    public class ScheduleSlot
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public bool IsBooked { get; set; }

        //Navigation Property
        public Doctor Doctor { get; set; } = null!;
    }
}
