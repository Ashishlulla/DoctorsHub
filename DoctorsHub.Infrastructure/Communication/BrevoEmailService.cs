using DoctorsHub.Application.DTOs.Communication;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Infrastructure.Communication
{
    public class BrevoEmailService : IEmailService
    {
        public Task SendAsync(EmailMessageDto email, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
