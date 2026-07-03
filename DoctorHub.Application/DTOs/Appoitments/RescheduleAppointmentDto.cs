

namespace DoctorsHub.Application.DTOs.Appoitments
{
    public class RescheduleAppointmentDto
    {
        public int Id { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
