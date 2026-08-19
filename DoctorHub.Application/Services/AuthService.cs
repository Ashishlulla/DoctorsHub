using DoctorsHub.Application.DTOs.Authentication;
using DoctorsHub.Application.DTOs.Communication;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using DoctorsHub.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DoctorsHub.Application.Services
{
    public class AuthService : IAuthService
    {
        //Private Feilds
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IOtpService _otpService;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService, IEmailService emailService, IOtpService otpService) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _emailService = emailService;
            _otpService = otpService;
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

            var roleResult = await _userManager.AddToRoleAsync(user, registerDto.Role!);

            if (!roleResult.Succeeded)
            {
                throw new Exception(string.Join(", ", roleResult.Errors.Select(e => e.Description)));
            }
        }

        public async Task<OtpRequiredResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                throw new Exception("Invalid Email or Password.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(
                user,
                loginDto.Password,
                false);

            if (!result.Succeeded)
            {
                throw new Exception("Invalid Email or Password.");
            }

            var otpData = await _otpService.GenerateOtpAsync(user.Id);

            //Creating OTP Email for User login
            await _emailService.SendAsync(
            new EmailMessageDto
            {
                To = user.PersonalEmail,
                ToName = user.UserName!,
                Subject = "DoctorsHub Login Verification",

                HtmlBody = $"""
                <h2>DoctorsHub Login Verification</h2>

                <p>
                    Your verification code is:
                </p>

                <h2>{otpData.Otp}</h2>

                <p>
                    This OTP is valid for <strong>90 seconds</strong>.
                </p>

                <p>
                    <strong>Please do not share this code with anyone.</strong>
                </p>
                """,

                PlainTextBody = $"""
                DoctorsHub Login Verification

                Your verification code is: {otpData.Otp}

                This OTP is valid for 90 seconds.

                Please do not share this code with anyone.
                """
            });

            return new OtpRequiredResponseDto
            {
                OtpRequired = true,
                UserId = user.Id,
            };
        }
        
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        
        public async Task<LoginResponseDto> VerifyOtpAsync(VerifyOtpDto verifyotpdto)
        {
            var isValid = await _otpService.ValidateOtpAsync(verifyotpdto.UserId, verifyotpdto.otp);

            if (!isValid)
                throw new Exception("Invalid or Expired otp");

            var user = await _userManager.FindByIdAsync(verifyotpdto.UserId);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var token = await _tokenService.CreateTokenAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

            await _otpService.RemoveOtpAsync(verifyotpdto.UserId);

            return new LoginResponseDto
            {
                Token = token,
                Email = user.Email!,
                Roles = roles.ToList()
            };
        
        }
    }
}
