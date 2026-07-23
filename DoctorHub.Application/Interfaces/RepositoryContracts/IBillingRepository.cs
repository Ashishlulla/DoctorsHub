using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.DTOs.common.DoctorsHub.Application.DTOs.Common;
using DoctorsHub.Domain.Entities;


namespace DoctorsHub.Application.Interfaces.RepositoryContracts
{
    public interface IBillingRepository
    {
        
        Task<Bill?> GetBillByIdAsync(int id);
        
        Task<Bill?>  GetBillByAppointmentIdAsync(int appointmentId);
        
        Task<IEnumerable<Bill>> GetAllBillsAsync();
        
        Task AddBillAsync(Bill bill);
        
        Task UpdateBillAsync(Bill bill);

        Task DeleteBillAsync(Bill bill);

        Task<(List<Bill> Bills, int totalBills)> GetBillsAsync(BillingQueryParameter billingQueryParameter);
    }
}
