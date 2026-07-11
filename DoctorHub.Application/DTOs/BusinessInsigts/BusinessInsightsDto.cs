
namespace DoctorsHub.Application.DTOs.BusinessInsigts
{
    public class BusinessInsightsDto
    {
        //------------------------------------------------------------------------------------------------------------------
        //                                      Appointment Analytics DTOs
        //------------------------------------------------------------------------------------------------------------------

        public List<AppointmentStatusChartDto> GetAppointmentStatuses { get; set; } = new List<AppointmentStatusChartDto>();
        public List<AppointmentTrendDto> GetAppointmentsTrend { get; set; } = new List<AppointmentTrendDto>();
        public List<AppointmentsByDoctorDto> GetAppointmentsByDoctors { get; set; } = new List<AppointmentsByDoctorDto>();
        public List<PeakAppointmentHoursDto> GetPeakAppointmentsHours { get; set; } = new List<PeakAppointmentHoursDto>();
    }
}
