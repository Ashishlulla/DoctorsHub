using System.ComponentModel.DataAnnotations;

namespace DoctorsHub.Application.DTOs.Authentication
{
    public class ResetPasswordDto
    {
        public string UserId { get; set; } = string.Empty;

        public string ResetToken { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}