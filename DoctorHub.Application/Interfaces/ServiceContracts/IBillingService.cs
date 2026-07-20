using DoctorsHub.Application.DTOs.Billing;


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
    }
}
