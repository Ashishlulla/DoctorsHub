using DoctorsHub.Application.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.Interfaces.ServiceContracts
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto registerDto);
        Task LoginAsync(LoginDto loginDto);
        Task LogoutAsync();
    }
}
