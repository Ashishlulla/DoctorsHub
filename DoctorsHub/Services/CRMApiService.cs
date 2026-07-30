using DoctorsHub.Application.DTOs.CRM;

namespace DoctorsHub.Web.Services
{
    public class CRMApiService
    {
        //Private Feilds
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;


        //Constructor
        public CRMApiService(
           HttpClient httpClient,
           IHttpContextAccessor httpContextAccessor)
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

        public async Task<DashBoardDto> GetDashBoardDataAsync()
        {
            AddToken();

            HttpResponseMessage response = await _httpClient.GetAsync("api/dashboard");

            response.EnsureSuccessStatusCode();

            DashBoardDto? dashBoardData =
                await response.Content.ReadFromJsonAsync<DashBoardDto>();

            return dashBoardData ?? new DashBoardDto();
        }
        public async Task<List<RecentAppointmentsDto>> GetRecentAppointmentsAsync() 
        {
            AddToken();
            HttpResponseMessage response = await _httpClient.GetAsync("/api/dashboard/recent");
            response.EnsureSuccessStatusCode();

            List<RecentAppointmentsDto>? recentAppointments = await response.Content.ReadFromJsonAsync<List<RecentAppointmentsDto>>();

            return recentAppointments ?? new List<RecentAppointmentsDto>();
        }

        public async Task<List<UpcomingAppointmentsDto>> GetUpcomingAppointmentsAsync()
        {
            AddToken(); 
            HttpResponseMessage response = await _httpClient.GetAsync("/api/dashboard/upcoming");
            response.EnsureSuccessStatusCode();

            List<UpcomingAppointmentsDto>? upcomingAppointments = await response.Content.ReadFromJsonAsync<List<UpcomingAppointmentsDto>>();


            return  upcomingAppointments?? new List<UpcomingAppointmentsDto>();
        }

        public async Task<List<TodayAppointmentsDto>> GetTodaysAppointmentsAsync()
        {
            AddToken();

            HttpResponseMessage response = await _httpClient.GetAsync("/api/dashboard/today");
            response.EnsureSuccessStatusCode();

            List<TodayAppointmentsDto>? todaysAppointments = await response.Content.ReadFromJsonAsync<List<TodayAppointmentsDto>>();


            return todaysAppointments ?? new List<TodayAppointmentsDto>();
        }

        public async Task<List<ScheduledAppointmentsDto>> GetScheduledAppointmentsAsync()
        {
            AddToken();
            HttpResponseMessage response = await _httpClient.GetAsync("/api/dashboard/schedule");
            response.EnsureSuccessStatusCode();

            List<ScheduledAppointmentsDto>? scheduledAppointments = await response.Content.ReadFromJsonAsync<List<ScheduledAppointmentsDto>>();


            return scheduledAppointments ?? new List<ScheduledAppointmentsDto>();
        }

        
    }
}
