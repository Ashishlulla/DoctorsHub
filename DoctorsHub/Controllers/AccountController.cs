using DoctorsHub.Application.DTOs.Authentication;
using DoctorsHub.Domain.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DoctorsHub.Web.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly AuthApiService _authApiService;
        private readonly UserManager<ApplicationUser> _userManager;

        //Logger
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            AuthApiService authApiService,
            UserManager<ApplicationUser> userManager, ILogger<AccountController> logger)
        {
            _authApiService = authApiService;
            _userManager = userManager;
            _logger = logger;
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
            _logger.LogInformation(
                "Registration attempt started for user: {Email}",
                registerDto.Email);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning(
                    "Registration validation failed for user: {Email}",
                    registerDto.Email);

                return View(registerDto);
            }

            try
            {
                await _authApiService.RegisterAsync(registerDto);

                _logger.LogInformation(
                    "Registration successful for user: {Email}",
                    registerDto.Email);

                return RedirectToAction(nameof(Login));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred during registration for user: {Email}",
                    registerDto.Email);

                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return View(registerDto);
            }
        }


        [HttpGet]
        [Route("[action]")]
        [Route("/")]
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
            _logger.LogInformation("Login attempt for user: {Email}", loginDto.Email);

            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }

            try
            {
                OtpRequiredResponseDto? loginResponse =
                    await _authApiService.LoginAsync(loginDto);

                if (loginResponse == null)
                {
                    _logger.LogWarning("Login failed for user: {Email}", loginDto.Email);
                    ModelState.AddModelError(
                        string.Empty,
                        "Invalid email or password.");

                    return View(loginDto);
                }

                // MFA disabled → Login directly
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

                // MFA enabled → Verify OTP
                return RedirectToAction(
                    nameof(VerifyOtp),
                    new
                    {
                        userId = loginResponse.UserId
                    });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return View(loginDto);
            }
        }



        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public IActionResult VerifyOtp(string userId)
        {
            _logger.LogInformation(
                "OTP verification page requested for user: {UserId}",
                userId);

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
            _logger.LogInformation(
                "OTP verification attempt started for user: {UserId}",
                userId);

            try
            {
                var verifyOtpDto = new VerifyOtpDto
                {
                    UserId = userId,
                    otp = otp
                };

                LoginResponseDto? loginResponse =
                    await _authApiService.VerifyOtpAsync(
                        verifyOtpDto);

                if (loginResponse == null)
                {
                    _logger.LogWarning(
                        "OTP verification failed for user: {UserId}. OTP may be invalid or expired.",
                        userId);

                    ViewBag.UserId = userId;
                    ViewBag.OtpError =
                        "Invalid or expired OTP.";

                    return View();
                }

                _logger.LogInformation(
                    "OTP verification successful for user: {UserId}, Email: {Email}",
                    userId,
                    loginResponse.Email);

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

                _logger.LogInformation(
                    "User successfully signed in after OTP verification. UserId: {UserId}, Email: {Email}, Roles: {Roles}",
                    userId,
                    loginResponse.Email,
                    string.Join(", ", loginResponse.Roles));

                return RedirectToAction(
                    "Index",
                    "DashBoard");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred during OTP verification for user: {UserId}",
                    userId);

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
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            _logger.LogInformation(
                "Logout attempt started for user: {Email}",
                userEmail);

            try
            {
                await HttpContext.SignOutAsync(
                    IdentityConstants.ApplicationScheme);

                Response.Cookies.Delete("JWT");

                _logger.LogInformation(
                    "User successfully logged out: {Email}",
                    userEmail);

                return RedirectToAction(nameof(Login));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while logging out user: {Email}",
                    userEmail);

                throw;
            }
        }



        [HttpGet]
        [Route("[action]")]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var email =
                User.FindFirst(ClaimTypes.Email)?.Value;

            _logger.LogInformation(
                "Profile page requested for user: {Email}",
                email);

            if (string.IsNullOrEmpty(email))
            {
                _logger.LogWarning(
                    "Profile access denied because authenticated user email claim is missing.");

                return RedirectToAction(nameof(Login));
            }

            try
            {
                var user =
                    await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    _logger.LogWarning(
                        "Profile access failed because user was not found: {Email}",
                        email);

                    return RedirectToAction(nameof(Login));
                }

                var isMfaEnabled =
                    await _userManager.GetTwoFactorEnabledAsync(user);

                _logger.LogInformation(
                    "Profile loaded successfully for user: {Email}. MFA enabled: {MfaEnabled}",
                    email,
                    isMfaEnabled);

                ViewBag.isMfaEnabled = isMfaEnabled;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading profile for user: {Email}",
                    email);

                throw;
            }
        }


        [HttpPost]
        [Route("[action]")]
        [Authorize]
        public async Task<IActionResult> ToggleMfa(bool enabled)
        {
            var email =
                User.FindFirst(ClaimTypes.Email)?.Value;

            _logger.LogInformation(
                "MFA toggle request received for user: {Email}. Requested state: {Enabled}",
                email,
                enabled);

            if (string.IsNullOrEmpty(email))
            {
                _logger.LogWarning(
                    "MFA toggle failed because authenticated user email claim is missing.");

                return Unauthorized();
            }

            try
            {
                var user =
                    await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    _logger.LogWarning(
                        "MFA toggle failed because user was not found: {Email}",
                        email);

                    return NotFound();
                }

                var result =
                    await _userManager.SetTwoFactorEnabledAsync(
                        user,
                        enabled);

                if (!result.Succeeded)
                {
                    _logger.LogWarning(
                        "Failed to update MFA setting for user: {Email}. Requested state: {Enabled}. Errors: {Errors}",
                        email,
                        enabled,
                        string.Join(
                            "; ",
                            result.Errors.Select(error => error.Description)));

                    return View("Error");
                }

                _logger.LogInformation(
                    "MFA setting updated successfully for user: {Email}. MFA enabled: {Enabled}",
                    email,
                    enabled);

                return RedirectToAction(nameof(Profile));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while updating MFA setting for user: {Email}. Requested state: {Enabled}",
                    email,
                    enabled);

                throw;
            }
        }


        [HttpGet]
        [Route("[action]")]
        [Authorize]
        public IActionResult ChangePassword()
        {
            var email =
                User.FindFirst(ClaimTypes.Email)?.Value;

            _logger.LogInformation(
                "Change password page requested for user: {Email}",
                email);

            return View();
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(
            ChangePasswordDto changePasswordDto)
        {
            var email =
                User.FindFirst(ClaimTypes.Email)?.Value;

            _logger.LogInformation(
                "Change password attempt started for user: {Email}",
                email);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning(
                    "Change password validation failed for user: {Email}",
                    email);

                return View(changePasswordDto);
            }

            try
            {
                await _authApiService.ChangePasswordAsync(
                    changePasswordDto);

                _logger.LogInformation(
                    "Password changed successfully for user: {Email}",
                    email);

                return RedirectToAction(nameof(Profile));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while changing password for user: {Email}",
                    email);

                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return View(changePasswordDto);
            }
        }



        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            _logger.LogInformation(
                "Forgot password page requested.");

            return View();
        }

        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(
            ForgotPasswordDto forgotPasswordDto)
        {
            _logger.LogInformation(
                "Forgot password request started for user: {Email}",
                forgotPasswordDto.PersonalEmail);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning(
                    "Forgot password validation failed for user: {Email}",
                    forgotPasswordDto.PersonalEmail);

                return View(forgotPasswordDto);
            }

            try
            {
                var userId =
                    await _authApiService.ForgotPasswordAsync(
                        forgotPasswordDto);

                _logger.LogInformation(
                    "Forgot password request processed successfully for user: {Email}",
                    forgotPasswordDto.PersonalEmail);

                return RedirectToAction(
                    nameof(VerifyForgotPasswordOtp),
                    new
                    {
                        userId
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while processing forgot password request for user: {Email}",
                    forgotPasswordDto.PersonalEmail);

                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return View(forgotPasswordDto);
            }
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public IActionResult VerifyForgotPasswordOtp(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction(
                    nameof(ForgotPassword));
            }

            return View(
                new VerifyForgotPasswordOtpDto
                {
                    UserId = userId
                });
        }

        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyForgotPasswordOtp(VerifyForgotPasswordOtpDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                var resetToken =
                    await _authApiService.VerifyForgotPasswordOtpAsync(dto);

                return RedirectToAction(
                    nameof(ResetPassword),
                    new
                    {
                        userId = dto.UserId,
                        resetToken = resetToken
                    });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return View(dto);
            }
        }


        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public IActionResult ResetPassword(string userId, string resetToken)
        {
            _logger.LogInformation(
                "Reset password page requested for UserId: {UserId}",
                userId);

            if (string.IsNullOrEmpty(userId) ||
                string.IsNullOrEmpty(resetToken))
            {
                _logger.LogWarning(
                    "Reset password request rejected because UserId or reset token is missing. UserId: {UserId}",
                    userId);

                return RedirectToAction(
                    nameof(ForgotPassword));
            }

            _logger.LogInformation(
                "Reset password page loaded successfully for UserId: {UserId}",
                userId);

            return View(
                new ResetPasswordDto
                {
                    UserId = userId,
                    ResetToken = resetToken
                });
        }


        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            _logger.LogInformation(
                "Password reset attempt started for UserId: {UserId}",
                dto.UserId);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning(
                    "Password reset validation failed for UserId: {UserId}",
                    dto.UserId);

                return View(dto);
            }

            try
            {
                await _authApiService.ResetPasswordAsync(dto);

                _logger.LogInformation(
                    "Password reset successful for UserId: {UserId}",
                    dto.UserId);

                TempData["SuccessMessage"] =
                    "Your password has been reset successfully.";

                return RedirectToAction(
                    nameof(Login));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while resetting password for UserId: {UserId}",
                    dto.UserId);

                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return View(dto);
            }
        }


        [HttpGet]
        [Route("[action]")]
        [Authorize]
        public IActionResult Settings()
        {
            var email =
                User.FindFirst(ClaimTypes.Email)?.Value;

            _logger.LogInformation(
                "Settings page requested for user: {Email}",
                email);

            return View();
        }


        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public IActionResult AccessDenied(string? returnUrl = null)
        {
            var email =
                User.FindFirst(ClaimTypes.Email)?.Value;

            _logger.LogWarning(
                "Access denied page requested for user: {Email}, ReturnUrl: {ReturnUrl}",
                email,
                returnUrl);

            ViewBag.ReturnUrl = returnUrl;

            return View();
        }
    }
}