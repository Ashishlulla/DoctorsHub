using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.DTOs.Authentication
{
    public class VerifyOtpDto
    {
        public string UserId { get; set; }
        public string otp { get; set; }
    }
}
