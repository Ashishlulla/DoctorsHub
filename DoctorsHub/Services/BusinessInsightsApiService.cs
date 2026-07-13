using DoctorsHub.Application.DTOs.BusinessInsigts.AppointmentAnalyticsDto;
using DoctorsHub.Application.DTOs.BusinessInsigts.RevenueAnalyticsDto;
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

        public async Task<List<RevenueTrendDto>> GetRevenueTrendAsync() 
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/BusinessInsights/revenue-trend");
            response.EnsureSuccessStatusCode();

            List<RevenueTrendDto>? revenueTrends = await response.Content.ReadFromJsonAsync<List<RevenueTrendDto>>();

            return revenueTrends ?? new List<RevenueTrendDto>();
        }

        public async Task<List<RevenueByDoctorDto>> GetRevenueByDoctorsAsync() 
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/BusinessInsights/revenue-by-doctors");
            response.EnsureSuccessStatusCode();

            List<RevenueByDoctorDto>? revenueByDoctor = await response.Content.ReadFromJsonAsync<List<RevenueByDoctorDto>>();

            return revenueByDoctor ?? new List<RevenueByDoctorDto>();
        }

        public async Task<List<TopRevenueGeneratingDoctorDto>> GetTopRevenueGeneratingsAsync() 
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/BusinessInsights/top-revenue-generating-doctors");
            response.EnsureSuccessStatusCode();

            List<TopRevenueGeneratingDoctorDto>? topRevenueGeneratingDoctor = await response.Content.ReadFromJsonAsync<List<TopRevenueGeneratingDoctorDto>>();

            return topRevenueGeneratingDoctor ?? new List<TopRevenueGeneratingDoctorDto>();
        }

        public async Task<List<AverageRevenueGeneratedByDoctorDto>> GetAverageRevenueGeneratedByDoctorsAsync() 
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/BusinessInsights/average-revenue-by-doctors");
            response.EnsureSuccessStatusCode();

            List<AverageRevenueGeneratedByDoctorDto>? averageRevenueGeneratedByDoctors = await response.Content.ReadFromJsonAsync<List<AverageRevenueGeneratedByDoctorDto>>();

            return averageRevenueGeneratedByDoctors ?? new List<AverageRevenueGeneratedByDoctorDto>();
        }
    }
}
