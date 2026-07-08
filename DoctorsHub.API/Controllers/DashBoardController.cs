using DoctorsHub.Application.DTOs.CRM;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        //Private Feilds 
        private readonly ICRMService _cRMService;

        //Constructor
        public DashBoardController(ICRMService cRMService) 
        {
            _cRMService = cRMService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashBoardAsync() 
        {
            DashBoardDto dashBoardData = await _cRMService.GetDashBoardAsync();

            return Ok(dashBoardData);
        }
    }
}
