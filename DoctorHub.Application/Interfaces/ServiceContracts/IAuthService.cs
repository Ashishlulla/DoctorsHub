using DoctorsHub.Application.DTOs.Authentication;

namespace DoctorsHub.Application.Interfaces.ServiceContracts
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto registerDto);

        Task<OtpRequiredResponseDto> LoginAsync(LoginDto loginDto);

        Task<LoginResponseDto> VerifyOtpAsync(
            VerifyOtpDto verifyOtpDto);

        Task ChangePasswordAsync(
            string userId,
            ChangePasswordDto changePasswordDto);

        Task<string> ForgotPasswordAsync(
            ForgotPasswordDto forgotPasswordDto);

        Task<string> VerifyForgotPasswordOtpAsync(
            VerifyForgotPasswordOtpDto dto);

        Task ResetPasswordAsync(
            ResetPasswordDto dto);

        Task LogoutAsync();
    }
}