using DoctorsHub.Application.DTOs.BusinessInsigts;
using System.ComponentModel;

namespace DoctorsHub.Web.Services
{
    public class BusinessInsightsApiService
    {
        //Private Feilds
        private readonly HttpClient _httpClient;

        //Constructor
        public BusinessInsightsApiService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        public async Task<List<AppointmentStatusChartDto>> GetAppointmentStatusesAsync() 
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/businessinsights/appointment-status");
            response.EnsureSuccessStatusCode();

            List<AppointmentStatusChartDto>? appointmentStatuses = await response.Content.ReadFromJsonAsync<List<AppointmentStatusChartDto>>();

            return appointmentStatuses?? new List<AppointmentStatusChartDto>();
        }

        public async Task<List<AppointmentTrendDto>> GetAppointmentTrendsAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/businessinsights/appointment-trend");
            response.EnsureSuccessStatusCode();

            List<AppointmentTrendDto>? appointmentTrend = await response.Content.ReadFromJsonAsync<List<AppointmentTrendDto>>();

            return appointmentTrend ?? new List<AppointmentTrendDto>();
        }

        public async Task<List<AppointmentsByDoctorDto>> GetAppointmentByDoctorsAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/BusinessInsights/appointment-by-doctors");
            response.EnsureSuccessStatusCode();

            List<AppointmentsByDoctorDto>? appointmentsByDoctors = await response.Content.ReadFromJsonAsync<List<AppointmentsByDoctorDto>>();

            return appointmentsByDoctors ?? new List<AppointmentsByDoctorDto>();
        }

        public async Task<List<PeakAppointmentHoursDto>> GetPeakAppointmentHoursAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/BusinessInsights/appointment-peakhours");
            response.EnsureSuccessStatusCode();

            List<PeakAppointmentHoursDto>? peakAppointmentHours = await response.Content.ReadFromJsonAsync<List<PeakAppointmentHoursDto>>();

            return peakAppointmentHours ?? new List<PeakAppointmentHoursDto>();
        }
    }
}
