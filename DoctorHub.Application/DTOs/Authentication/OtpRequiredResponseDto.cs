namespace DoctorsHub.Application.DTOs.Authentication
{
    public class OtpRequiredResponseDto
    {
        public bool OtpRequired { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public List<string> Roles { get; set; } = new List<string>();
    }
}