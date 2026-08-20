using DoctorsHub.Application.DTOs.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DoctorsHub.Domain.Identity;

namespace DoctorsHub.Web.Controllers
{
    [Route("[controller]")]
    
    public class AccountController : Controller
    {
        private readonly AuthApiService _authApiService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(AuthApiService authApiService, UserManager<ApplicationUser> userManager)
        {
            _authApiService = authApiService;
            _userManager = userManager;
        }


        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
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
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return View(loginDto);

            try
            {
                OtpRequiredResponseDto? loginResponse =
                    await _authApiService.LoginAsync(loginDto);

                if (loginResponse == null)
                {
                    ModelState.AddModelError(
                        "",
                        "Invalid email or password.");

                    return View(loginDto);
                }
                // MFA disabled → login directly
                if (!loginResponse.OtpRequired)
                {
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
                // MFA enabled → continue to OTP verification
                else
                {
                    return RedirectToAction(
                    "VerifyOtp",
                    new
                    {
                        userId = loginResponse.UserId
                    });
                }
                
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
        [AllowAnonymous]
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
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                IdentityConstants.ApplicationScheme);

            Response.Cookies.Delete("JWT");

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize]
        public async Task<IActionResult> Profile() 
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction(nameof(Login));
            }

            var user = await _userManager.FindByEmailAsync(email);

            if(user == null) 
            {
                return RedirectToAction(nameof(Login));
            }

            var isMfaEnabled = await _userManager.GetTwoFactorEnabledAsync(user);

            ViewBag.isMfaEnabled = isMfaEnabled;


            return View();
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        public async Task<IActionResult> ToggleMfa(bool enabled) 
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email)) 
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return NotFound();

            await _userManager.SetTwoFactorEnabledAsync(user, enabled);

            return RedirectToAction(nameof(Profile));
        }
    }

}