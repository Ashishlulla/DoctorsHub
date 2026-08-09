using DoctorsHub.Application.DTOs.Notification;
using DoctorsHub.Domain.Entities;

namespace DoctorsHub.Web.Services
{
    public class NotificationApiService
    {
        //Private Feilds
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        //Constructor
        public NotificationApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor) 
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddToken() 
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["JWT"];

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<Notification>> GetByUserIdAsync(string userId) 
        {
            AddToken();

            HttpResponseMessage response = await _httpClient.GetAsync($"/api/Notification/User/{userId}");
            response.EnsureSuccessStatusCode();

            List<Notification>? notifications = await response.Content.ReadFromJsonAsync<List<Notification>>();

            return notifications ?? new List<Notification>();
        }
        public async Task<Notification?> CreateNotificationAsync(CreateNotificationDto createNotificationDto)
        {
            AddToken();

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/Notification",createNotificationDto);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Notification>();
        }

        public async Task<List<Notification>> GetUnreadByUserIdAsync(string userId)
        {
            AddToken();

            HttpResponseMessage response = await _httpClient.GetAsync($"/api/Notification/Unread/{userId}");
            response.EnsureSuccessStatusCode();

            List<Notification>? notifications = await response.Content.ReadFromJsonAsync<List<Notification>>();

            return notifications ?? new List<Notification>();
        }

        public async Task<Notification?> GetByIdAsync(int id)
        {
            AddToken();

            HttpResponseMessage response = await _httpClient.GetAsync($"/api/Notification/{id}");
            response.EnsureSuccessStatusCode();

            Notification? notification = await response.Content.ReadFromJsonAsync<Notification>();

            return notification;
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            AddToken();

            HttpResponseMessage response = await _httpClient.PutAsync($"/api/Notification/MarkAsRead/{id}", null);
            

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> MarkAllAsReadAsync(string userId)
        {
            AddToken();

            HttpResponseMessage response = await _httpClient.PutAsync($"/api/Notification/MarkAllAsRead/{userId}", null);
            

            return response.IsSuccessStatusCode;
        }
    }
}
