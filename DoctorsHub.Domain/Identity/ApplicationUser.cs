using Microsoft.AspNetCore.Identity;


namespace DoctorsHub.Domain.Identity
{
    public class ApplicationUser :IdentityUser
    {
        public string PersonalEmail { get; set; } = string.Empty;
    }
}
