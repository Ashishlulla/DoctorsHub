using AutoMapper;
using DoctorsHub.Application.DTOs.Appoitments;
using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.DTOs.common.DoctorsHub.Application.DTOs.Common;

namespace DoctorsHub.Web.Services
{
    public class AppointmentApiService
    {
        //Private Feilds
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IMapper _mapper;

        //Constructor
        public AppointmentApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
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
        public async Task<List<AppointmentDto>> GetAllAppointmentsAsync() 
        {
            AddToken();
            HttpResponseMessage response = await _httpClient.GetAsync("/api/appointments");
            response.EnsureSuccessStatusCode();

            List<AppointmentDto>? appointments = await  response.Content.ReadFromJsonAsync<List<AppointmentDto>>();

            return appointments ?? new List<AppointmentDto>();
        }

        public async Task<PagedResult<AppointmentDto>> GetAppointmentsAsync(AppointmentQueryParameter appointmentQueryParameter) 
        {
            AddToken();
            string Url = $"/api/appointments?searchBy={appointmentQueryParameter.searchBy}&searchString={appointmentQueryParameter.searchString}&sortBy={appointmentQueryParameter.sortBy}&sortOrder={appointmentQueryParameter.sortOrder}&PageSize={appointmentQueryParameter.PageSize}&PageNumber={appointmentQueryParameter.PageNumber}";


            HttpResponseMessage response = await _httpClient.GetAsync(Url);
            response.EnsureSuccessStatusCode() ;

            PagedResult<AppointmentDto>? appointments = await response.Content.ReadFromJsonAsync<PagedResult<AppointmentDto>>();

            return appointments?? new PagedResult<AppointmentDto>();
        }

        public async Task<AppointmentDto> GetAppointmentByIdAsync(int id) 
        {
            AddToken();
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/appointments/{id}");
            response.EnsureSuccessStatusCode();

            AppointmentDto? appointment = await response.Content.ReadFromJsonAsync<AppointmentDto>();

            return appointment?? new AppointmentDto();
        }

        public async Task<UpdateAppointmentDto> GetAppointmentForUpdateAsync(int id) 
        {
            AddToken();
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/appointments/{id}");
            response.EnsureSuccessStatusCode();

            AppointmentDto? appointment = await response.Content.ReadFromJsonAsync<AppointmentDto>();

            if (appointment == null)
            {
                return new UpdateAppointmentDto();
            }

            return _mapper.Map<UpdateAppointmentDto>(appointment);
        }

        public async Task<AppointmentDetailsDto> GetAppointmentForDetailsAsync(int id)
        {
            AddToken();
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/appointments/{id}");
            response.EnsureSuccessStatusCode();

            AppointmentDto? appointment = await response.Content.ReadFromJsonAsync<AppointmentDto>();

            if (appointment  == null)
            {
                return new AppointmentDetailsDto();
            }

            return _mapper.Map<AppointmentDetailsDto>(appointment);
        }


        public async Task DeleteAppointmentAsync(int id) 
        {
            AddToken();
            HttpResponseMessage response = await _httpClient.DeleteAsync($"/api/appointments/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task CreateAppointmentAsync(CreateAppointmentDto createAppointmentDto) 
        {
            AddToken();
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/appointments", createAppointmentDto);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAppointmentAsync(UpdateAppointmentDto updateAppointmentDto) 
        {
            AddToken();
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"/api/appointments/{updateAppointmentDto.Id}", updateAppointmentDto);
            response.EnsureSuccessStatusCode();
        }

        public async Task ConfirmAppointmentAsync(int id) 
        {
            AddToken();
            HttpResponseMessage response = await _httpClient.PutAsync($"/api/appointments/confirm/{id}", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task CancelAppointmentAsync(int id)
        {
            AddToken();
            HttpResponseMessage response = await _httpClient.PutAsync($"/api/appointments/cancel/{id}", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task CompleteAppointmentAsync(int id)
        {
            AddToken();
            HttpResponseMessage response = await _httpClient.PutAsync($"/api/appointments/complete/{id}", null);
            response.EnsureSuccessStatusCode();
        }
        public async Task RescheduleAppointmentAsync(RescheduleAppointmentDto rescheduleAppointmentDto) 
        {
            AddToken();
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"/api/appointments/reschedule", rescheduleAppointmentDto);
            response.EnsureSuccessStatusCode();
        }
    }
}
