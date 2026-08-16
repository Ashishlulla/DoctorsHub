using AutoMapper;
using DoctorsHub.Application.DTOs.Billing;
using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.DTOs.common.DoctorsHub.Application.DTOs.Common;
using DoctorsHub.Application.DTOs.Communication;
using DoctorsHub.Application.DTOs.Notification;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using DoctorsHub.Domain.Entities;
using DoctorsHub.Domain.Enums;
using System.Security.Cryptography;

namespace DoctorsHub.Application.Services
{
    public class BillingService : IBillingService
    {
        //Private Feilds 
        private readonly IBillingRepository _billingRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;

        private readonly IMapper _mapper;

        //Constructor
        public BillingService(IBillingRepository billingRepository, IAppointmentRepository appointmentRepository, INotificationService notificationService, IEmailService emailService, IMapper mapper)
        {
            _billingRepository = billingRepository; 
            _appointmentRepository = appointmentRepository;
            _notificationService = notificationService;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task<BillDto> CreateBillAsync(CreateBillDto createBillDto)
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


            var bill = _mapper.Map<Bill>(createBillDto);

            bill.ConsultationFee = appointment.Doctor.ConsultationFee;
           


            bill.TotalAmount = bill.ConsultationFee + createBillDto.AdditionalCharges - createBillDto.Discount;
            bill.BillDate = DateTime.Now;
            bill.PaymentStatus = PaymentStatus.Pending;

            var generatedBill = await _billingRepository.AddBillAsync(bill);

            BillDto billDto = _mapper.Map<BillDto>(generatedBill);

            //Creating Bill Generated In-App Notification
            await _notificationService.CreateAsync(
                new CreateNotificationDto
                {
                    UserId = bill.Appointment.Doctor.UserId,
                    BillId = bill.Id,
                    AppointmentId = bill.AppointmentId,
                    Title = "Bill Generated",
                    Message = $"A bill of ₹{bill.TotalAmount} has been generated for {bill.Appointment.Patient.FullName}.",
                    Type = NotificationType.BillGenerated
                });

            //Creating Email for Patient Commmunication
            await _emailService.SendAsync(
                new EmailMessageDto 
                {
                    To = "Lullaashish2807@gmail.com",
                    ToName = "Ashish Lulla",
                    Subject = $"Bill Generated  Bill #{bill.Id}",
                    HtmlBody = $"""
                    <h2>Bill Generated</h2>
                    <p>Hello {appointment.Patient.FullName},</p>
                    <p>Your bill has been generated successfully.</p>

                    <p><strong>Bill ID: </strong>{bill.Id}</p>
                    <p><strong>Doctor Name: </strong>{bill.Appointment.Doctor.FullName}</p>
                    <p><strong>Bill Date: </strong>{bill.BillDate}</p>
                    <p><strong>Consultation Fee: </strong>₹{bill.Appointment.Doctor.ConsultationFee}</p>

                    <p><strong>Please Note: </strong>The amount shown above includes consultation charges only. Additional charges and applicable discounts, if any, will be considered at reception.</p>

                    <p>Thank you for using DoctorsHub.</p>
                    """,
                    PlainTextBody = $"""
                    Bill Generated
                    Hello {appointment.Patient.FullName},
                    Your bill has been generated successfully.

                    Bill ID: {bill.Id}
                    Doctor Name: {bill.Appointment.Doctor.FullName}
                    Bill Date: {bill.BillDate}
                    Consultation Fee: ₹{bill.Appointment.Doctor.ConsultationFee}

                    Please Note: The amount shown above includes consultation charges only. Additional charges and applicable discounts, if any, will be considered at reception.

                    Thank you for using DoctorsHub.
                    """
                });

            return billDto;

            
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

            if (updateBillDto.PaymentStatus == PaymentStatus.Paid)
            {
                await _notificationService.CreateAsync(
                new CreateNotificationDto
                {
                    UserId = bill.Appointment.Doctor.UserId,
                    BillId = bill.Id,
                    AppointmentId = bill.AppointmentId,
                    Title = "Bill Paid",
                    Message = $"A bill of ₹{bill.TotalAmount} has been paid successfully for {bill.Appointment.Patient.FullName}.",
                    Type = NotificationType.BillPaid
                });
            }

            if (updateBillDto.PaymentStatus == PaymentStatus.Cancelled)
            {
                await _notificationService.CreateAsync(
                    new CreateNotificationDto
                    {
                        UserId = bill.Appointment.Doctor.UserId,
                        BillId = bill.Id,
                        AppointmentId = bill.AppointmentId,
                        Title = "Bill Cancelled",
                        Message = $"The bill of ₹{bill.TotalAmount} for {bill.Appointment.Patient.FullName} has been cancelled.",
                        Type = NotificationType.BillCancelled
                    });
            }

            await _billingRepository.UpdateBillAsync(bill);
        }

        
    }
}
