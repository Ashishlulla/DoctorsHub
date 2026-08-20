

namespace DoctorsHub.Application.DTOs.Authentication
{
    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
