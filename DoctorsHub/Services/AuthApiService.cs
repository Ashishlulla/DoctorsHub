using DoctorsHub.Application.DTOs.Authentication;
using System.Text.Json;

public class AuthApiService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AuthApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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

    public async Task RegisterAsync(RegisterDto registerDto)
    {
        AddToken();
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerDto);
        response.EnsureSuccessStatusCode();
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
    {
        try
        {
            
            AddToken();

            var response = await _httpClient.PostAsJsonAsync(
                "api/auth/login",
                loginDto);

            var json = await response.Content.ReadAsStringAsync();

            Console.WriteLine(json);

            response.EnsureSuccessStatusCode();

            return System.Text.Json.JsonSerializer.Deserialize<LoginResponseDto>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}