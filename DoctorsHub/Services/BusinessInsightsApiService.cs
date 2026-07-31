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
        private readonly IHttpContextAccessor _httpContextAccessor;

        //Constructor
        public BusinessInsightsApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor) 
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddToken()
        {
            var token = _httpContextAccessor
                .HttpContext?
                .Request
                .Cookies["JWT"];

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue(
                        "Bearer",
                        token);
            }
        }

        public async Task<BusinessInsightsDto> GetBusinessInsightsAsync(AnalyticsTimeFilter timeFilter = AnalyticsTimeFilter.Month) 
        {
            AddToken();
            HttpResponseMessage response = await _httpClient.GetAsync($"api/BusinessInsights?timeFilter={timeFilter}");
            response.EnsureSuccessStatusCode();

            BusinessInsightsDto? businessInsights = await response.Content.ReadFromJsonAsync<BusinessInsightsDto>();

            return businessInsights ?? new BusinessInsightsDto();
        }
    }
}
