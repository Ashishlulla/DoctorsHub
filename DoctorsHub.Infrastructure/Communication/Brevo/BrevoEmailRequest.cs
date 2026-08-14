
namespace DoctorsHub.Infrastructure.Communication.Brevo
{
    public class BrevoEmailRequest
    {
        public BrevoSender Sender { get; set; } = new();

        public List<BrevoRecipient> To { get; set; } = new();

        public string Subject { get; set; } = string.Empty;

        public string? HtmlContent { get; set; }

        public string? TextContent { get; set; }

        public List<BrevoAttachment>? Attachment { get; set; }
    }

    public class BrevoSender
    {
        public string Email { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }

    public class BrevoRecipient
    {
        public string Email { get; set; } = string.Empty;

        public string? Name { get; set; }
    }

    public class BrevoAttachment
    {
        public string Name { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
    }
}
