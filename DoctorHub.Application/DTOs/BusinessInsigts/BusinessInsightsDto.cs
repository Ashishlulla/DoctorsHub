
using DoctorsHub.Application.DTOs.BusinessInsigts.AppointmentAnalyticsDto;
using DoctorsHub.Application.DTOs.BusinessInsigts.RevenueAnalyticsDto;

namespace DoctorsHub.Application.DTOs.BusinessInsigts
{
    public class BusinessInsightsDto
    {
        //----------------------------------------------------------------------------------------------------------------
        //                                      Appointment Analytics DTOs
        //----------------------------------------------------------------------------------------------------------------

        public List<AppointmentStatusChartDto> GetAppointmentStatuses { get; set; } = new List<AppointmentStatusChartDto>();
        public List<AppointmentTrendDto> GetAppointmentsTrend { get; set; } = new List<AppointmentTrendDto>();
        public List<AppointmentsByDoctorDto> GetAppointmentsByDoctors { get; set; } = new List<AppointmentsByDoctorDto>();
        public List<PeakAppointmentHoursDto> GetPeakAppointmentsHours { get; set; } = new List<PeakAppointmentHoursDto>();

        //----------------------------------------------------------------------------------------------------------------
        //                                      Revenue Analytics DTOs
        //----------------------------------------------------------------------------------------------------------------

        public List<RevenueTrendDto> GetRevenueTrends { get; set; } = new List<RevenueTrendDto>();
        
        public List<RevenueByDoctorDto> GetRevenueByDoctors { get; set; } = new List<RevenueByDoctorDto>();

        public List<TopRevenueGeneratingDoctorDto> GetTopRevenueGeneratingDoctors { get; set; } = new List<TopRevenueGeneratingDoctorDto>();

        public List<AverageRevenueGeneratedByDoctorDto> GetAverageRevenueGeneratedByDoctors { get; set; } = new List<AverageRevenueGeneratedByDoctorDto>();
    }
}
