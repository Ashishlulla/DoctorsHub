using DoctorsHub.Application.DTOs.Departments;

namespace DoctorsHub.Web.Services
{
    public class DepartmentApiService
    {
        //Private Feilds
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        //Constructor
        public DepartmentApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        //Private Method
        private void AddToken()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["JWT"];

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto createDepartmentDto)
        {
            AddToken();

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/department/create", createDepartmentDto);

            response.EnsureSuccessStatusCode();

            DepartmentDto? department = await response.Content.ReadFromJsonAsync<DepartmentDto>();

            return department ?? new DepartmentDto();

        }

        public async Task<List<DepartmentDto>> GetDepartmentsAsync()
        {
            AddToken();
            HttpResponseMessage response = await _httpClient.GetAsync("api/department/all");
            response.EnsureSuccessStatusCode();

            List<DepartmentDto>? departments = await response.Content.ReadFromJsonAsync<List<DepartmentDto>>();

            return departments ?? new List<DepartmentDto>();

        }

        public async Task<DepartmentDto> GetDepartmentByIdAsync(int id)
        {
            AddToken();

            HttpResponseMessage response = await _httpClient.GetAsync($"api/department/{id}");
            response.EnsureSuccessStatusCode();

            DepartmentDto? department = await response.Content.ReadFromJsonAsync<DepartmentDto>();

            return department ?? new DepartmentDto();
        }

        public async Task<DepartmentDetailsDto> GetDepartmentDetailsAsync(int id)
        {
            AddToken();

            HttpResponseMessage response =
                await _httpClient.GetAsync($"api/department/{id}/details");

            response.EnsureSuccessStatusCode();

            DepartmentDetailsDto? department =
                await response.Content.ReadFromJsonAsync<DepartmentDetailsDto>();

            return department ?? new DepartmentDetailsDto();
        }

        public async Task<DepartmentDto> UpdateDepartmentAsync(UpdateDepartmentDto updateDepartmentDto)
        {
            AddToken();

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync("api/department/update", updateDepartmentDto);
            response.EnsureSuccessStatusCode();

            DepartmentDto? department = await response.Content.ReadFromJsonAsync<DepartmentDto>();

            return department ?? new DepartmentDto();
        }

        public async Task DeleteDepartmentAsync(int id) 
        {
            AddToken();
            HttpResponseMessage respone = await _httpClient.DeleteAsync($"/api/Department/Delete/{id}");
            respone.EnsureSuccessStatusCode();

         

        }
    }
}
