using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.DTOs.Communication
{
    public class EmailAttachmentDto
    {
        public string FileName { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public byte[] Content { get; set; } = Array.Empty<byte>();
    }
}
