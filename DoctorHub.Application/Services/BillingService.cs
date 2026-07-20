
using AutoMapper;
using DoctorsHub.Application.DTOs.Billing;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using DoctorsHub.Domain.Entities;

namespace DoctorsHub.Application.Services
{
    public class BillingService : IBillingService
    {
        //Private Feilds 
        private readonly IBillingRepository _billingRepository;
        private readonly IMapper _mapper;

        //Constructor
        public BillingService(IBillingRepository billingRepository, IMapper mapper)
        {
            _billingRepository = billingRepository; ;
            _mapper = mapper;
        }

        public async Task CreateBillAsync(CreateBillDto createBillDto)
        {
            await _billingRepository.AddBillAsync(_mapper.Map<Bill>(createBillDto));
        }

        public async Task DeleteBillAsync(int id)
        {
            Bill? bill = await _billingRepository.GetBillByIdAsync(id);
            if (bill == null)
            {
                throw new KeyNotFoundException($"No Bill found with id={id}");
            }

            await _billingRepository.DeleteBillAsync(bill);
        }

        public Task<IEnumerable<BillDto>> GetAllBillsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BillDto?> GetBillByAppointmentIdAsync(int appointmentId)
        {
            throw new NotImplementedException();
        }

        public Task<BillDto?> GetBillByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateBillAsync(int id, UpdateBillDto updateBillDto)
        {
            throw new NotImplementedException();
        }
    }
}
