using DoctorsHub.Application.DTOs.Authentication;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.Web.Controllers
{
    [Route("[controller]")]
    [AllowAnonymous]
    public class AuthController : Controller
    {
        //Private Feilds
        private readonly IAuthService _authService;

        //Condtructor
        public AuthController(IAuthService authService) 
        {
            _authService = authService;
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
                await _authService.RegisterAsync(registerDto);
                return RedirectToAction(nameof(Login));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
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
                await _authService.LoginAsync(loginDto);
                return RedirectToAction("Index","DashBoard");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction(nameof(Login));
            }
        }

        [HttpPost]
        [Route("[action]")]

        public async Task<IActionResult> Logout() 
        {
            await _authService.LogoutAsync();

            return RedirectToAction("Login", "Auth");
        }
    }
}
