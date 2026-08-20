using DoctorsHub.Application.DTOs.Authentication;

namespace DoctorsHub.Application.Interfaces.ServiceContracts
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto registerDto);
        Task<OtpRequiredResponseDto> LoginAsync(LoginDto loginDto);

        Task<LoginResponseDto> VerifyOtpAsync(VerifyOtpDto verifyotpdto);

        Task ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto);
        Task LogoutAsync();
    }
}
