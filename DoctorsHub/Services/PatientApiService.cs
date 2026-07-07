using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.DTOs.common.DoctorsHub.Application.DTOs.Common;
using DoctorsHub.Application.DTOs.Patients;


namespace DoctorsHub.Web.Services
{
    public class PatientApiService
    {
        //Private Feilds
        private readonly HttpClient _httpClient;

        //Constructor
        public PatientApiService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        public async Task<List<PatientDto>> GetAllPatientsAsync() 
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/patients/all");
            response.EnsureSuccessStatusCode();

            List<PatientDto>?  patients= await response.Content.ReadFromJsonAsync<List<PatientDto>>();

            return patients ?? new List<PatientDto>();
        }

        public async Task<PagedResult<PatientDto>> GetAllPatientsAsync(PatientQueryParameters patientQueryParameters) 
        {
            string url = $"/api/patients?searchBy={patientQueryParameters.searchBy}&searchString={patientQueryParameters.searchString}&sortBy={patientQueryParameters.sortBy}&sortOrder={patientQueryParameters.sortOrder}&PageSize={patientQueryParameters.PageSize}&PageNumber={patientQueryParameters.PageNumber}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            PagedResult<PatientDto>? pagedResult = await response.Content.ReadFromJsonAsync<PagedResult<PatientDto>>();

            return pagedResult ?? new PagedResult<PatientDto>();
        }

        public async Task<PatientDto> GetPatientByIdAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/patients/{id}");
            response.EnsureSuccessStatusCode();

            PatientDto? patients = await response.Content.ReadFromJsonAsync<PatientDto>();

            return patients ?? new PatientDto(); 
        }

        public async Task DeletePatientAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/patients/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task CreatePatientAsync(CreatePatientDto createPatientDto) 
        {
            HttpResponseMessage responseMessage = await _httpClient.PostAsJsonAsync("api/patients", createPatientDto);
            responseMessage.EnsureSuccessStatusCode();
        }

        public async Task UpdatePatientAsync(UpdatePatientDto updatePatientDto)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync("api/patients", updatePatientDto);
            response.EnsureSuccessStatusCode();
        }
    }
}
