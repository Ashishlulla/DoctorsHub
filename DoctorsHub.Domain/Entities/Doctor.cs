using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DoctorsHub.Domain.Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        //public int userId { get; set; } yet to Add
        public string FullName { get; set; } = string.Empty;
        public string Qualification { get; set; } = string.Empty;
        public string About { get; set; } = string.Empty;
        public int ExperienceYears { get; set; }
        public decimal ConsultationFee { get; set; }
        public string VisitDays { get; set; } = string.Empty;
        public double AverageRating { get; private set; }
        public int SpecializationId { get; set; }
        
        //Navigation Properties
        public Specialization Specialization { get; set; } = null!;
        public List<Appointment> Appointments { get; set; } = new();
        public List<Review> Reviews { get; set; } = new();
        public List<ScheduleSlot> ScheduleSlots { get; set; } = new();

    }
}
