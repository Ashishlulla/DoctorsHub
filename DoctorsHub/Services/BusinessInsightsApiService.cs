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
    }
}
