using AutoMapper;
using DoctorsHub.Application.DTOs.Billing;
using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.DTOs.common.DoctorsHub.Application.DTOs.Common;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using DoctorsHub.Domain.Entities;

namespace DoctorsHub.Application.Services
{
    public class BillingService : IBillingService
    {
        //Private Feilds 
        private readonly IBillingRepository _billingRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        private readonly IMapper _mapper;

        //Constructor
        public BillingService(IBillingRepository billingRepository, IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _billingRepository = billingRepository; 
            _appointmentRepository = appointmentRepository;

            _mapper = mapper;
        }

        public async Task CreateBillAsync(CreateBillDto createBillDto)
        {

            Appointment? appointment = await _appointmentRepository.GetByIdAsync(createBillDto.AppointmentId);
            if (appointment == null)
            {
                throw new KeyNotFoundException($"No Appointment found appointment id = {createBillDto.AppointmentId}");
            }

            Bill? existingBill = await _billingRepository.GetBillByAppointmentIdAsync(createBillDto.AppointmentId);

            if (existingBill != null)
            {
                throw new InvalidOperationException($"A bill already exists for appointment id {createBillDto.AppointmentId}.");
            }


            Bill bill = _mapper.Map<Bill>(createBillDto);

            bill.ConsultationFee = appointment.Doctor.ConsultationFee;
            Console.WriteLine("Doctor Name : ", appointment.Doctor.FullName);
            Console.WriteLine("Doctor fee : ", appointment.Doctor.ConsultationFee);


            bill.TotalAmount = bill.ConsultationFee + createBillDto.AdditionalCharges - createBillDto.Discount;
            bill.BillDate = DateTime.Now;
            bill.PaymentStatus = Domain.Enums.PaymentStatus.Pending;

            await _billingRepository.AddBillAsync(bill);
            
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

        public async Task<IEnumerable<BillDto>> GetAllBillsAsync()
        {
            IEnumerable<Bill> bills = await _billingRepository.GetAllBillsAsync();

            return  bills.Select(b=> new BillDto 
            {
                AdditionalCharges = b.AdditionalCharges,
                ConsultationFee = b.ConsultationFee,
                Discount = b.Discount,
                DoctorName = b.Appointment.Doctor.FullName,
                PatientName= b.Appointment.Patient.FullName,
                TotalAmount = b.TotalAmount,
                PaymentStatus = b.PaymentStatus,
                BillDate = b.BillDate,
                Id = b.Id,
                AppointmentId = b.AppointmentId,

            });
        }

       

        public async Task<BillDto?> GetBillByAppointmentIdAsync(int appointmentId)
        {
            Bill? bill = await _billingRepository.GetBillByAppointmentIdAsync(appointmentId);

            if (bill == null)
            {
                throw new KeyNotFoundException($"No Bill found with appointment id = {appointmentId}");
            }

            return _mapper.Map<BillDto>(bill);
        }

        public async  Task<BillDto?> GetBillByIdAsync(int id)
        {
            Bill? bill = await _billingRepository.GetBillByIdAsync(id);

            if (bill == null)
            {
                throw new KeyNotFoundException($"No Bill found with id = {id} ");
            }

            return _mapper.Map<BillDto>(bill);
        }

        public async Task<PagedResult<BillDto>> GetBillsAsync(BillingQueryParameter billingQueryParameter)
        {
            var (bills, totalBills) = await _billingRepository.GetBillsAsync(billingQueryParameter);

            PagedResult<BillDto> pagedResult = new PagedResult<BillDto>()
            {
                Items = _mapper.Map<List<BillDto>>(bills),
                PageSize = billingQueryParameter.PageSize,
                PageNumber = billingQueryParameter.PageNumber,
                TotalCount = totalBills,
            };

            return pagedResult;
        }

        public async Task UpdateBillAsync(int id, UpdateBillDto updateBillDto)
        {

            Bill? bill = await _billingRepository.GetBillByIdAsync(id);

            if (bill== null)
            {
                throw new KeyNotFoundException($"No bill found with id = {id}.");
            }


             _mapper.Map(updateBillDto, bill);
            bill.ConsultationFee = bill.Appointment.Doctor.ConsultationFee;
            bill.TotalAmount = bill.ConsultationFee + bill.AdditionalCharges - bill.Discount; 
            await _billingRepository.UpdateBillAsync(bill);
        }

        
    }
}
