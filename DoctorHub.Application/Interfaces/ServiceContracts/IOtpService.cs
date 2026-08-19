using DoctorsHub.Application.Models.Authentication;

namespace DoctorsHub.Application.Interfaces.ServiceContracts
{
    public interface IOtpService
    {
        Task<OtpData> GenerateOtpAsync(string userId);

        Task<bool> ValidateOtpAsync(string userId, string otp);

        Task RemoveOtpAsync(string userId);
    }
}
