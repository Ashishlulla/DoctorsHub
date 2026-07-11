
namespace DoctorsHub.Application.DTOs.BusinessInsigts
{
    public class BusinessInsightsDto
    {
        //-----------------------------
        //Appointment Analytics DTOs
        //-----------------------------

        public List<AppointmentStatusChartDto> GetAppointmentStatuses { get; set; } = new List<AppointmentStatusChartDto>();
    }
}
