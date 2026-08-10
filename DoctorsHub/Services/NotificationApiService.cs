using DoctorsHub.Application.DTOs.Notification;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        public async Task<List<NotificationDto>> GetAllNotifications()
        {
            AddToken();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };

            HttpResponseMessage response =
                await _httpClient.GetAsync("/api/Notification/all");

            response.EnsureSuccessStatusCode();

            var rawJson = await response.Content.ReadAsStringAsync();

           

            var notifications =
                JsonSerializer.Deserialize<List<NotificationDto>>(rawJson, options);

            return notifications ?? new List<NotificationDto>();
        }

        public async Task<List<NotificationDto>> GetAllUnreadNotifications()
        {
            AddToken();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };

            HttpResponseMessage response =
                await _httpClient.GetAsync("/api/Notification/All-Unread");

            response.EnsureSuccessStatusCode();

            var rawJson = await response.Content.ReadAsStringAsync();

           

            var notifications =
                JsonSerializer.Deserialize<List<NotificationDto>>(rawJson, options);

            return notifications ?? new List<NotificationDto>();
        }

        public async Task<List<NotificationDto>> GetByUserIdAsync(string userId) 
        {
            AddToken();

            HttpResponseMessage response = await _httpClient.GetAsync($"/api/Notification/User/{userId}");
            response.EnsureSuccessStatusCode();

            List<NotificationDto>? notifications = await response.Content.ReadFromJsonAsync<List<NotificationDto>>();

            return notifications ?? new List<NotificationDto>();
        }
        public async Task<NotificationDto?> CreateNotificationAsync(CreateNotificationDto createNotificationDto)
        {
            AddToken();

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/Notification",createNotificationDto);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<NotificationDto>();
        }

        public async Task<List<NotificationDto>> GetUnreadByUserIdAsync(string userId)
        {
            AddToken();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/Notification/Unread/{userId}");
            response.EnsureSuccessStatusCode();

            List<NotificationDto>? notifications = await response.Content.ReadFromJsonAsync<List<NotificationDto>>(options);

            return notifications ?? new List<NotificationDto>();
        }

        public async Task<NotificationDto?> GetByIdAsync(int id)
        {
            AddToken();

            HttpResponseMessage response = await _httpClient.GetAsync($"/api/Notification/{id}");
            response.EnsureSuccessStatusCode();

            NotificationDto? notification = await response.Content.ReadFromJsonAsync<NotificationDto>();

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
