using DoctorsHub.Application.DTOs.Authentication;
using System.Text.Json;

public class AuthApiService
{
    private readonly HttpClient _httpClient;

    public AuthApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task RegisterAsync(RegisterDto registerDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerDto);
        response.EnsureSuccessStatusCode();
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
    {
        try
        {
            Console.WriteLine(_httpClient.BaseAddress);

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