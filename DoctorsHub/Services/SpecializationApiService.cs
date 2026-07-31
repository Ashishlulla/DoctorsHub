using DoctorsHub.Application.DTOs.Doctors;
using System.Net;

namespace DoctorsHub.Web.Services
{
    public class SpecializationApiService
    {
        //private feilds
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        //Constructor
        public SpecializationApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor) 
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

        public async Task<List<SpecializationDTO>> GetAllSpecializationsAsync() 
        {
            AddToken();
            HttpResponseMessage response  = await _httpClient.GetAsync("api/specializations");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Clear stored token/session
                // Redirect to Login
            }

            response.EnsureSuccessStatusCode();

            List<SpecializationDTO>? specializations  = await response.Content.ReadFromJsonAsync<List<SpecializationDTO>>();

            return  specializations?? new List<SpecializationDTO>();
        }
    }
}
