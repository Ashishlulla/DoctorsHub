using AutoMapper;
using DoctorsHub.Application.DTOs.Billing;
using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.DTOs.common.DoctorsHub.Application.DTOs.Common;
using DoctorsHub.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class BillingController : Controller
    {
        private readonly BillingApiService _billingApiService;
        private readonly PdfExportService _pdfExportService;
        private readonly IMapper _mapper;
        private readonly ILogger<BillingController> _logger;

        public BillingController(
            BillingApiService billingApiService,
            PdfExportService pdfExportService,
            IMapper mapper,
            ILogger<BillingController> logger)
        {
            _billingApiService = billingApiService;
            _pdfExportService = pdfExportService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            [FromQuery] BillingQueryParameter billingQueryParameter)
        {
            _logger.LogInformation(
                "Billing index page requested.");

            try
            {
                PagedResult<BillDto> bills =
                    await _billingApiService.GetFilteredBillAsync(
                        billingQueryParameter);

                _logger.LogInformation(
                    "Bills loaded successfully.");

                return View(bills);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading bills.");

                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation(
                "Bill details requested for BillId: {BillId}",
                id);

            try
            {
                BillDto bill =
                    await _billingApiService.GetBillByIdAsync(id);

                _logger.LogInformation(
                    "Bill details loaded successfully for BillId: {BillId}",
                    id);

                return View(bill);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading bill details for BillId: {BillId}",
                    id);

                throw;
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation(
                "Edit bill page requested for BillId: {BillId}",
                id);

            try
            {
                BillDto bill =
                    await _billingApiService.GetBillByIdAsync(id);

                UpdateBillDto updateBillDto =
                    _mapper.Map<UpdateBillDto>(bill);

                ViewBag.BillId = id;

                _logger.LogInformation(
                    "Edit bill page loaded successfully for BillId: {BillId}",
                    id);

                return View(updateBillDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading bill for editing. BillId: {BillId}",
                    id);

                throw;
            }
        }

        [HttpPost]
        [Route("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            UpdateBillDto updateBillDto)
        {
            _logger.LogInformation(
                "Update bill attempt started for BillId: {BillId}",
                id);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning(
                    "Update bill validation failed for BillId: {BillId}",
                    id);

                return View(updateBillDto);
            }

            try
            {
                await _billingApiService.UpdateBillAsync(
                    id,
                    updateBillDto);

                _logger.LogInformation(
                    "Bill updated successfully for BillId: {BillId}",
                    id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while updating bill. BillId: {BillId}",
                    id);

                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return View(updateBillDto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Print(int id)
        {
            _logger.LogInformation(
                "Bill PDF generation requested for BillId: {BillId}",
                id);

            try
            {
                BillDto bill =
                    await _billingApiService.GetBillByIdAsync(id);

                var pdf =
                    _pdfExportService.GenerateBillPdf(bill);

                _logger.LogInformation(
                    "Bill PDF generated successfully for BillId: {BillId}",
                    id);

                return File(
                    pdf,
                    "application/pdf",
                    $"Bill-{bill.Id}.pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while generating bill PDF. BillId: {BillId}",
                    id);

                throw;
            }
        }
    }
}