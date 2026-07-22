using AutoMapper;
using DoctorsHub.Application.DTOs.Billing;
using DoctorsHub.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DoctorsHub.Web.Controllers
{
    public class BillingController : Controller
    {
        //Private Fields
        private readonly BillingApiService _billingApiService;
        private readonly IMapper _mapper;

        //Constructor
        public BillingController(BillingApiService billingApiService, IMapper mapper) 
        {
            _billingApiService = billingApiService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<BillDto> bills = await  _billingApiService.GetBillsAsync();
            
            return View(bills);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            BillDto bill = await _billingApiService.GetBillByIdAsync(id);

            return View(bill);
        }

        [HttpGet]

        public async Task<IActionResult> Edit(int id) 
        {
            BillDto bill = await _billingApiService.GetBillByIdAsync(id);

            UpdateBillDto updateBillDto =  _mapper.Map<UpdateBillDto>(bill);

            ViewBag.BillId = id;

            return View(updateBillDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id , UpdateBillDto updateBillDto) 
        {
            if (!ModelState.IsValid)
            {
                
                return View(updateBillDto);
            }

            try
            {
                await _billingApiService.UpdateBillAsync(id, updateBillDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(updateBillDto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Print(int id) 
        {
            BillDto bill = await _billingApiService.GetBillByIdAsync(id);
            return View(bill);
        }

    }
}
