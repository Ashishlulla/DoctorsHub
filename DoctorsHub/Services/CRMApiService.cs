using DoctorsHub.Application.DTOs.CRM;

namespace DoctorsHub.Web.Services
{
    public class CRMApiService
    {
        //Private Feilds
        private readonly HttpClient _httpClient;

        //Constructor
        public CRMApiService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        public async Task<DashBoardDto> GetDashBoardDataAsync()        
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/dashboard");
            response.EnsureSuccessStatusCode();

            DashBoardDto? dashBoardData = await response.Content.ReadFromJsonAsync<DashBoardDto>();

            return dashBoardData ?? new DashBoardDto();
        }
        public async Task<List<RecentAppointmentsDto>> GetRecentAppointmentsAsync() 
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/dashboard/recent");
            response.EnsureSuccessStatusCode();

            List<RecentAppointmentsDto>? recentAppointments = await response.Content.ReadFromJsonAsync<List<RecentAppointmentsDto>>();

            return recentAppointments ?? new List<RecentAppointmentsDto>();
        }
    }
}
