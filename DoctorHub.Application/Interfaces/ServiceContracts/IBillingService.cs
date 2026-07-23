using DoctorsHub.Application.DTOs.Billing;
using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.DTOs.common.DoctorsHub.Application.DTOs.Common;


namespace DoctorsHub.Application.Interfaces.ServiceContracts
{
    public interface IBillingService
    {
        Task CreateBillAsync(CreateBillDto createBillDto);

        Task UpdateBillAsync(int id, UpdateBillDto updateBillDto);

        Task DeleteBillAsync(int id);

        Task<IEnumerable<BillDto>> GetAllBillsAsync();

        Task<BillDto?> GetBillByIdAsync(int id);
        
        Task<BillDto?> GetBillByAppointmentIdAsync(int appointmentId);

        Task<(PagedResult<List<BillDto>> Bills, int totalBills)> GetBillsAsync(BillingQueryParameter billingQueryParameter);
    }
}
