using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.Web.Controllers
{
    
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("Error/Index")]
        public IActionResult Index()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (exceptionFeature?.Error != null)
            {
                _logger.LogError(
                    exceptionFeature.Error,
                    "Unhandled application exception. Path: {Path}",
                    HttpContext.Request.Path);
            }

            return View();
        }

        [Route("Error/{statusCode}")]
        public IActionResult StatusCode(int statusCode)
        {
            _logger.LogWarning(
                "HTTP status code {StatusCode} returned for path {Path}",
                statusCode,
                HttpContext.Request.Path);

            return View("StatusCode", statusCode);
        }
    }
}