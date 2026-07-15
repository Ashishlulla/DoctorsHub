using DoctorsHub.Application.DTOs.Authentication;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using DoctorsHub.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace DoctorsHub.Application.Services
{
    public class AuthService : IAuthService
    {
        //Private Feilds
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task RegisterAsync(RegisterDto registerDto)
        {
            if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
            {
                throw new ArgumentException($"User already exist with email = {registerDto.Email} .");
            }

            ApplicationUser user = new ApplicationUser 
            {
                Email = registerDto.Email,
                UserName = registerDto.Email,
                
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e=>e.Description)));
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "Patient");

            if (!roleResult.Succeeded)
            {
                throw new Exception(string.Join(", ", roleResult.Errors.Select(e => e.Description)));
            }
        }

        public async Task LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new Exception("Invalid Email or Password.");
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                throw new Exception("Invalid Email or Password.");
            }
            await _signInManager.SignInAsync(user, isPersistent: false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
