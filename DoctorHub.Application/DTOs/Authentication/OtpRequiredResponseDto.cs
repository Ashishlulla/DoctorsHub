
namespace DoctorsHub.Application.DTOs.Authentication
{
    public class OtpRequiredResponseDto
    {
        public bool OtpRequired { get; set; }

        public string UserId { get; set; } = string.Empty;
    }
}
