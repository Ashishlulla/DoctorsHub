using DoctorHub.Application.DTOs.Doctors;
using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.DTOs.common.DoctorsHub.Application.DTOs.Common;

namespace DoctorsHub.Web.Services
{
    public class DoctorApiService
    {
        //private feilds
        private readonly HttpClient _httpClient;

        //Constructor
        public DoctorApiService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        public async Task<PagedResult<DoctorDto>> GetAllDoctorsAsync(DoctorQueryParameters doctorQueryParameters)
        {
            string url = $"api/doctors?PageNumber={doctorQueryParameters.PageNumber}&PageSize={doctorQueryParameters.PageSize}&SearchBy={doctorQueryParameters.searchBy}&searchString={doctorQueryParameters.searchString}&sortBy={doctorQueryParameters.sortBy}&sortOrder={doctorQueryParameters.sortOrder}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            PagedResult<DoctorDto>? Doctors = await response.Content.ReadFromJsonAsync<PagedResult<DoctorDto>>();

            return Doctors ?? new PagedResult<DoctorDto>();
        }

        public async Task<List<DoctorDto>> GetAllDoctorsAsync() 
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/doctors");
            response.EnsureSuccessStatusCode();

            List<DoctorDto>? doctors = await response.Content.ReadFromJsonAsync<List<DoctorDto>>();

            return doctors ?? new List<DoctorDto>();
        }
        

           

        public async Task<DoctorDto> GetDoctorByIdAsync(int id) 
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/doctors/{id}");
            response.EnsureSuccessStatusCode();

            DoctorDto? doctor = await response.Content.ReadFromJsonAsync<DoctorDto>();

            return doctor!;
        }

        public async Task CreateDoctorAsync(CreateDoctorDto createDoctorDto) 
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/doctors", createDoctorDto);
            response.EnsureSuccessStatusCode();
        }
        public async Task UpdateDoctorAsync(UpdateDoctorDto updateDoctorDto)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync("api/doctors", updateDoctorDto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteDoctorAsync(int id) 
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/doctors/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
