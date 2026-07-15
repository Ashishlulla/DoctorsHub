using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.DTOs.Authentication
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
