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

    public async Task<OtpRequiredResponseDto?> LoginAsync(LoginDto loginDto)
    {
        try
        {
            AddToken();

            HttpResponseMessage response =
                await _httpClient.PostAsJsonAsync(
                    "api/auth/login",
                    loginDto);

            var json = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"LOGIN API RESPONSE: {json}");

            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<OtpRequiredResponseDto>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"LOGIN ERROR: {ex.Message}");
            return null;
        }
    }
    public async Task<LoginResponseDto?> VerifyOtpAsync( VerifyOtpDto verifyOtpDto) 
    {
        try
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/auth/verify-otp", verifyOtpDto);
            response.EnsureSuccessStatusCode();

            LoginResponseDto? loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

            return loginResponse;
        }
        catch (Exception ex) 
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task ChangePasswordAsync(ChangePasswordDto changePasswordDto) 
    {
        AddToken();

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/auth/change-password", changePasswordDto);

        response.EnsureSuccessStatusCode();

        
    }

    public async Task<string> ForgotPasswordAsync(
    ForgotPasswordDto forgotPasswordDto)
    {
        var response =
            await _httpClient.PostAsJsonAsync(
                "api/auth/forgot-password",
                forgotPasswordDto);

        response.EnsureSuccessStatusCode();

        var result =
            await response.Content
                .ReadFromJsonAsync<ForgotPasswordResponseDto>();

        if (result == null || string.IsNullOrWhiteSpace(result.UserId))
        {
            throw new Exception("Invalid password reset response.");
        }

        return result.UserId;
    }

    public async Task<string> VerifyForgotPasswordOtpAsync(VerifyForgotPasswordOtpDto dto)
    {
        HttpResponseMessage response =
            await _httpClient.PostAsJsonAsync(
                "api/auth/verify-forgot-password-otp",
                dto);

        var content =
            await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        return content.Trim().Trim('"');
    }
    public async Task ResetPasswordAsync(ResetPasswordDto dto)
    {
        HttpResponseMessage response =
            await _httpClient.PostAsJsonAsync(
                "api/auth/reset-password",
                dto);

        response.EnsureSuccessStatusCode();
    }
}