using DoctorsHub.Application.DTOs.BusinessInsigts;
using DoctorsHub.Application.DTOs.BusinessInsigts.AppointmentAnalyticsDto;
using DoctorsHub.Application.DTOs.BusinessInsigts.RevenueAnalyticsDto;
using DoctorsHub.Domain.Enums;
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

        public async Task<BusinessInsightsDto> GetBusinessInsightsAsync(AnalyticsTimeFilter timeFilter = AnalyticsTimeFilter.Month) 
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/BusinessInsights?timeFilter={timeFilter}");
            response.EnsureSuccessStatusCode();

            BusinessInsightsDto? businessInsights = await response.Content.ReadFromJsonAsync<BusinessInsightsDto>();

            return businessInsights ?? new BusinessInsightsDto();
        }
    }
}
