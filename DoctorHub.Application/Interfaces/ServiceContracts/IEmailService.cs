

using DoctorsHub.Application.DTOs.Communication;

namespace DoctorsHub.Application.Interfaces.ServiceContracts
{
    public interface IEmailService
    {
        Task SendAsync(EmailMessageDto email, CancellationToken cancellationToken = default);
    }
}
