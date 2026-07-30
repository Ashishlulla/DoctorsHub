using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.DTOs.Authentication
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;

        public string Email { get; set; }
        
        public List<string> Roles { get; set; } = new List<string>();
    }
}
