
using DoctorsHub.Domain.Identity;

namespace DoctorsHub.Application.Interfaces.ServiceContracts
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(ApplicationUser user);
    }
}
