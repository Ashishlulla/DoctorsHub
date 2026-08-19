using DoctorsHub.Application.DTOs.Authentication;
using DoctorsHub.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DoctorsHub.Web.Controllers
{
    [Route("[controller]")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly AuthApiService _authApiService;

        public AccountController(AuthApiService authApiService)
        {
            _authApiService = authApiService;
        }


        [HttpGet]
        [Route("[action]")]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return View(registerDto);

            try
            {
                await _authApiService.RegisterAsync(registerDto);

                return RedirectToAction(nameof(Login));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(registerDto);
            }
        }


        [HttpGet]
        [Route("[action]")]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [Route("[action]")]
        
        
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return View(loginDto);

            try
            {
                OtpRequiredResponseDto? otpResponse =
                    await _authApiService.LoginAsync(loginDto);

                if (otpResponse == null)
                {
                    ModelState.AddModelError(
                        "",
                        "Invalid email or password.");

                    return View(loginDto);
                }

                return RedirectToAction(
                    "VerifyOtp",
                    new
                    {
                        userId = otpResponse.UserId
                    });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    ex.Message);

                return View(loginDto);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult VerifyOtp(string userId)
        {
            ViewBag.UserId = userId;

            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> VerifyOtp(
    string userId,
    string otp)
        {
            try
            {
                var verifyOtpDto = new VerifyOtpDto
                {
                    UserId = userId,
                    otp = otp
                };

                LoginResponseDto? loginResponse =
                    await _authApiService.VerifyOtpAsync(verifyOtpDto);

                if (loginResponse == null)
                {
                    ViewBag.UserId = userId;
                    ViewBag.OtpError = "Invalid or expired OTP.";

                    return View();
                }

                Response.Cookies.Append(
                    "JWT",
                    loginResponse.Token,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = false,
                        SameSite = SameSiteMode.Lax,
                        Expires = DateTime.UtcNow.AddHours(2)
                    });

                var claims = new List<Claim>
        {
            new Claim(
                ClaimTypes.Email,
                loginResponse.Email)
        };

                foreach (var role in loginResponse.Roles)
                {
                    claims.Add(
                        new Claim(
                            ClaimTypes.Role,
                            role));
                }

                var identity = new ClaimsIdentity(
                    claims,
                    IdentityConstants.ApplicationScheme);

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    IdentityConstants.ApplicationScheme,
                    principal);

                return RedirectToAction(
                    "Index",
                    "DashBoard");
            }
            catch (Exception ex)
            {
                ViewBag.UserId = userId;
                ViewBag.OtpError = ex.Message;

                return View();
            }
        }


        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                IdentityConstants.ApplicationScheme);

            Response.Cookies.Delete("JWT");

            return RedirectToAction(nameof(Login));
        }
    }
}