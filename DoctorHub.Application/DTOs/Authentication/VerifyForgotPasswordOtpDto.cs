

namespace DoctorsHub.Application.DTOs.Authentication
{
    public class VerifyForgotPasswordOtpDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;
    }
}
