using DoctorsHub.Application.DTOs.Billing;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        //Private Feilds
        private readonly IBillingService _billingService;


        //Constructor
        public BillingController(IBillingService billingService) 
        {
            _billingService = billingService;
        }

        [HttpGet]

        public async Task<IActionResult> GetAllBillsAsync() 
        {
            IEnumerable<BillDto> bills = await _billingService.GetAllBillsAsync();

            return Ok(bills);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBillByIdAsync(int id) 
        {
            BillDto? bill =  await _billingService.GetBillByIdAsync(id);

            return Ok(bill);
        }

        [HttpGet("appointment/{appointmentId:int}")]
        public async Task<IActionResult> GetBillByAppointmentIdAsync(int appointmentId)
        {
            BillDto? bill = await _billingService.GetBillByAppointmentIdAsync(appointmentId);

            return Ok(bill);
        }

        [HttpPost]

        public async Task<IActionResult> CreateBillAsync([FromBody]CreateBillDto createBillDto) 
        {
            await _billingService.CreateBillAsync(createBillDto);

            return Ok($"Bill created successfully for appointment id = {createBillDto.AppointmentId}.");
        }

        [HttpPut("{id:int}")]

        public async Task<IActionResult> UpdateBillAsync(int id, [FromBody]UpdateBillDto updateBillDto) 
        {
            await _billingService.UpdateBillAsync(id, updateBillDto);

            return Ok($"Successfully update bill with bill id = {id}");
        }

        [HttpDelete("{id:int}")]

        public async Task<IActionResult> DeleteBillAsync(int id)
        {
            await _billingService.DeleteBillAsync(id);

            return Ok($"Successfully deleted bill with bill id = {id}");
        }
    }
}
