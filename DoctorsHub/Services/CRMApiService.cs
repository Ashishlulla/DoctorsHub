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

        public async Task<List<UpcomingAppointmentsDto>> GetUpcomingAppointmentsAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/dashboard/upcoming");
            response.EnsureSuccessStatusCode();

            List<UpcomingAppointmentsDto>? upcomingAppointments = await response.Content.ReadFromJsonAsync<List<UpcomingAppointmentsDto>>();


            return  upcomingAppointments?? new List<UpcomingAppointmentsDto>();
        }

        public async Task<List<TodayAppointmentsDto>> GetTodaysAppointmentsAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/dashboard/today");
            response.EnsureSuccessStatusCode();

            List<TodayAppointmentsDto>? todaysAppointments = await response.Content.ReadFromJsonAsync<List<TodayAppointmentsDto>>();


            return todaysAppointments ?? new List<TodayAppointmentsDto>();
        }

        public async Task<List<ScheduledAppointmentsDto>> GetScheduledAppointmentsAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/dashboard/schedule");
            response.EnsureSuccessStatusCode();

            List<ScheduledAppointmentsDto>? scheduledAppointments = await response.Content.ReadFromJsonAsync<List<ScheduledAppointmentsDto>>();


            return scheduledAppointments ?? new List<ScheduledAppointmentsDto>();
        }

        public async Task<List<AppointmentStatusChartDto>> GetAppointmentStatusChartAsync() 
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/dashboard/appointmentstatuschart");
            response.EnsureSuccessStatusCode();

            List<AppointmentStatusChartDto>? appointmentStatusCharts = await response.Content.ReadFromJsonAsync<List<AppointmentStatusChartDto>>();

            return appointmentStatusCharts ?? new List<AppointmentStatusChartDto>();
        }

        public async Task<List<MonthlyAppointmentChartDto>> GetMonthlyAppointmentAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/DashBoard/monthlyappointment");
            response.EnsureSuccessStatusCode();

            List<MonthlyAppointmentChartDto>? monthlyAppointments = await response.Content.ReadFromJsonAsync<List<MonthlyAppointmentChartDto>>();

            return monthlyAppointments ?? new List<MonthlyAppointmentChartDto>();
        }

        public async Task<List<AppointmentsByDoctorDto>> GetAppointmentByDoctorAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/DashBoard/appointmentsbydoctor");
            response.EnsureSuccessStatusCode();

            List<AppointmentsByDoctorDto>? appointmentsByDoctors = await response.Content.ReadFromJsonAsync<List<AppointmentsByDoctorDto>>();

            return  appointmentsByDoctors ?? new List<AppointmentsByDoctorDto>();
        }
    }
}
