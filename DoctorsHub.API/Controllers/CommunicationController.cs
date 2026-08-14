using DoctorsHub.Application.DTOs.Communication;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunicationController : ControllerBase
    {
        //Private Feilds
        private readonly IEmailService _emailService;

        //Constructor
        public CommunicationController(IEmailService emailService) 
        {
            _emailService = emailService;
        }

        [HttpPost("test-email")]
        public async Task<IActionResult> SendTestEmail() 
        {
            EmailMessageDto emailMessage = new EmailMessageDto 
            {
                To = "ashishlulla@zohomail.in",
                ToName = "Ashish Lulla",
                Subject = "DoctorsHub Brevo Test",
                HtmlBody = "<h2>Hello from DoctorsHub!</h2><p>This is a test email sent through Brevo.</p>",
                PlainTextBody = "Hello from DoctorsHub! This is a test email sent through Brevo."
            };

            await _emailService.SendAsync(emailMessage);

            return Ok("Test email sent successfully");
        }

        [HttpGet("test-brevo-config")]
        public IActionResult TestBrevoConfig()
        {
            var configuration = HttpContext.RequestServices
                .GetRequiredService<IConfiguration>();

            var apiKey = configuration["Brevo:ApiKey"];

            return Ok(new
            {
                ApiKeyConfigured = !string.IsNullOrWhiteSpace(apiKey),
                ApiKeyLength = apiKey?.Length ?? 0
            });
        }
    }
}
