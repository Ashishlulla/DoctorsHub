using AutoMapper;
using DoctorsHub.Application.DTOs.Appoitments;

namespace DoctorsHub.Web.Services
{
    public class AppointmentApiService
    {
        //Private Feilds
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        //Constructor
        public AppointmentApiService(HttpClient httpClient, IMapper mapper) 
        {
            _httpClient = httpClient;
            _mapper = mapper;
        }

        public async Task<List<AppointmentDto>> GetAllAppointmentsAsync() 
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/appointments");
            response.EnsureSuccessStatusCode();

            List<AppointmentDto> appointments = await  response.Content.ReadFromJsonAsync<List<AppointmentDto>>();

            return appointments ?? new List<AppointmentDto>();
        }

        public async Task<AppointmentDto> GetAppointmentByIdAsync(int id) 
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/appointments/{id}");
            response.EnsureSuccessStatusCode();

            AppointmentDto? appointment = await response.Content.ReadFromJsonAsync<AppointmentDto>();

            return appointment?? new AppointmentDto();
        }

        public async Task<UpdateAppointmentDto> GetAppointmentForUpdateAsync(int id) 
        {
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
            HttpResponseMessage response = await _httpClient.DeleteAsync($"/api/appointments/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task CreateAppointmentAsync(CreateAppointmentDto createAppointmentDto) 
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/appointments", createAppointmentDto);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAppointmentAsync(UpdateAppointmentDto updateAppointmentDto) 
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"/api/appointments/{updateAppointmentDto.Id}", updateAppointmentDto);
            response.EnsureSuccessStatusCode();
        }

        public async Task ConfirmAppointmentAsync(int id) 
        {
            HttpResponseMessage response = await _httpClient.PutAsync($"/api/appointments/confirm/{id}", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task CancelAppointmentAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.PutAsync($"/api/appointments/cancel/{id}", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task CompleteAppointmentAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.PutAsync($"/api/appointments/complete/{id}", null);
            response.EnsureSuccessStatusCode();
        }
        public async Task RescheduleAppointmentAsync(RescheduleAppointmentDto rescheduleAppointmentDto) 
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"/api/appointments/reschedule/{rescheduleAppointmentDto.Id}", rescheduleAppointmentDto);
            response.EnsureSuccessStatusCode();
        }
    }
}
