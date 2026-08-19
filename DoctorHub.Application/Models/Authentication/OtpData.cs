

namespace DoctorsHub.Application.Models.Authentication
{
    public class OtpData
    {
        public string UserId { get; set; } = string.Empty;

        public string Otp { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }

        public bool IsUsed { get; set; }
    }
}
