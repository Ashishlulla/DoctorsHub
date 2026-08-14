using DoctorsHub.Application.DTOs.Communication;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using DoctorsHub.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
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


        public async Task SendAsync(EmailMessageDto email, CancellationToken cancellationToken = default)
        {
            var request = new BrevoEmailRequest 
            {
                Sender = new BrevoSender 
                {
                    Email = _brevoSettings.SenderEmail,
                    Name = _brevoSettings.SenderName,
                },

                To = new List<BrevoRecipient>() 
                {
                    new BrevoRecipient
                    {
                        Name = email.ToName,
                        Email =email.To
                    }
                },

                Subject = email.Subject,
                
                HtmlContent = email.HtmlBody,
                
                TextContent = email.PlainTextBody,

                Attachment = email.Attachments.Any()
                    ? email.Attachments
                        .Select(a => new BrevoAttachment
                        {
                            Name = a.FileName,
                            Content = Convert.ToBase64String(a.Content)
                        })
                        .ToList()
                    : null
            };

            _httpClient.DefaultRequestHeaders.Remove("api-key");
            _httpClient.DefaultRequestHeaders.Add("api-key", _brevoSettings.ApiKey);

            var response = await _httpClient.PostAsJsonAsync(
            "smtp/email",
            request,
            cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Brevo API returned {(int)response.StatusCode}: {responseBody}");
            }
        }
    }
}
