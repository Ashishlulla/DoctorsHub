using DoctorsHub.Application.DTOs.Doctors;

namespace DoctorsHub.Web.Services
{
    public class SpecializationApiService
    {
        //private feilds
        private readonly HttpClient _httpClient;

        //Constructor
        public SpecializationApiService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        public async Task<List<SpecializationDTO>> GetAllSpecializationsAsync() 
        {
            HttpResponseMessage response  = await _httpClient.GetAsync("api/specializations");
            response.EnsureSuccessStatusCode();

            List<SpecializationDTO>? specializations  = await response.Content.ReadFromJsonAsync<List<SpecializationDTO>>();

            return  specializations?? new List<SpecializationDTO>();
        }
    }
}
