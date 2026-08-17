using AutoMapper;
using DoctorsHub.Application.DTOs.Billing;
using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.DTOs.common.DoctorsHub.Application.DTOs.Common;
using DoctorsHub.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace DoctorsHub.Web.Controllers
{
    [Authorize(Roles ="Admin, Receptionist, Doctor")]
    [Route("[controller]/[action]")]
    public class BillingController : Controller
    {
        //Private Fields
        private readonly BillingApiService _billingApiService;
        private readonly PdfExportService _pdfExportService;

        private readonly IMapper _mapper;

        //Constructor
        public BillingController(BillingApiService billingApiService, PdfExportService pdfExportService, IMapper mapper) 
        {
            _billingApiService = billingApiService;
            _pdfExportService = pdfExportService;

            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery]BillingQueryParameter billingQueryParameter)
        {
            PagedResult<BillDto> bills = await  _billingApiService.GetFilteredBillAsync(billingQueryParameter);
            
            return View(bills);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            BillDto bill = await _billingApiService.GetBillByIdAsync(id);

            return View(bill);
        }

        [HttpGet]
        [Route("[action]")]

        public async Task<IActionResult> Edit(int id) 
        {
            BillDto bill = await _billingApiService.GetBillByIdAsync(id);

            UpdateBillDto updateBillDto =  _mapper.Map<UpdateBillDto>(bill);

            ViewBag.BillId = id;

            return View(updateBillDto);
        }

        [HttpPost]
        [Route("[action]")]
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

            var pdf = _pdfExportService.GenerateBillPdf(bill);

            return File(
                pdf,
                "application/pdf",
                $"Bill-{bill.Id}.pdf"
            );
        }

    }
}
