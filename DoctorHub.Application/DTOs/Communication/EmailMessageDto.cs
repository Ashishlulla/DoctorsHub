using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.DTOs.Communication
{
    public class EmailMessageDto
    {
        
        public string To { get; set; }
        
        public string? ToName { get; set; }
        
        public string Subject { get; set; } = string.Empty;
        
        public string HtmlBody { get; set; } = string.Empty;

        public string PlainTextBody { get; set; } = string.Empty;

        public List<EmailAttachmentDto> Attachments { get; set; } = new();
    }
}
