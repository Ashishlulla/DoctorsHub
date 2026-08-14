using DoctorsHub.Application.DTOs.Communication;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using DoctorsHub.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Infrastructure.Communication.Brevo
{
    public class BrevoEmailService : IEmailService
    {

        //Private Feilds
        private readonly BrevoSettings _brevoSettings;
        private readonly HttpClient _httpClient;

        //Constructor
        public BrevoEmailService(IOptions<BrevoSettings> brevoSettings, HttpClient httpClient) 
        {
            _brevoSettings = brevoSettings.Value;
            _httpClient = httpClient;
        }


        public Task SendAsync(EmailMessageDto email, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
