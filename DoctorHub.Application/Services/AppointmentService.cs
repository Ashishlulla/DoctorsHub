using AutoMapper;
using DoctorsHub.Application.DTOs.Appoitments;
using DoctorsHub.Application.DTOs.Billing;
using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.DTOs.common.DoctorsHub.Application.DTOs.Common;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using DoctorsHub.Domain.Entities;
using DoctorsHub.Domain.Enums;


namespace DoctorsHub.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        //private feilds
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IBillingService _billingService;
        private readonly IMapper _mapper;

        //constructor
        public AppointmentService(IAppointmentRepository appointmentRepository, IBillingService billingService, IMapper mapper) 
        {
            _appointmentRepository = appointmentRepository;
            _billingService = billingService;
            _mapper = mapper;
        }

        public async Task CreateAppointmentAsync(CreateAppointmentDto createAppointmentDto)
        {
            //Past date validation
            if (createAppointmentDto.AppointmentDate < DateOnly.FromDateTime(DateTime.Today))
            {
                throw new ArgumentException("Appointment date cannot be in the past.");
            }

            //Validation: Doctor Exists
            if (!await _appointmentRepository.DoctorExistsAsync(createAppointmentDto.DoctorId))
                throw new ArgumentException("Doctor does not exists.");

            //Validation:Patient Exists
            if (!await _appointmentRepository.PatientExistsAsync(createAppointmentDto.PatientId))
                throw new ArgumentException("Patient does not exists.");

             bool doctorBusy = await _appointmentRepository.DoctorHasConflictingAppointmentAsync(createAppointmentDto.DoctorId, createAppointmentDto.AppointmentDate, createAppointmentDto.StartTime, createAppointmentDto.EndTime);
            
            //Validation: Doctor Availablity
            if(doctorBusy)
                throw new InvalidOperationException("Doctor already has an appointment during this time.");

            bool patientBusy =  await _appointmentRepository.PatientHasConflictingAppointmentAsync(createAppointmentDto.PatientId,createAppointmentDto.AppointmentDate, createAppointmentDto.StartTime, createAppointmentDto.EndTime);

            //Validation: Patient Availablity
            if (patientBusy)
            {
                throw new InvalidOperationException("Patient already has an appointment during this time");
            }
            //Validation:Appointment time Availablity
            if (createAppointmentDto.StartTime<TimeSpan.FromHours(9) || createAppointmentDto.EndTime> TimeSpan.FromHours(18))
            {
                throw new InvalidOperationException("Appointments are only allowed between 9:00AM to 6:00PM");
            }

            //Validation: correct time selection
            if (createAppointmentDto.EndTime <= createAppointmentDto.StartTime) 
            {
                Console.WriteLine(createAppointmentDto);
                throw new InvalidOperationException("Appoinment end time must be later than start time");
            }
        

            var appointment = _mapper.Map<Appointment>(createAppointmentDto);

            await _appointmentRepository.AddAsync(appointment);
        }

        public async Task DeleteAppointmentAsync(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
            {
                throw new KeyNotFoundException($"No appointment exist with given id : {id}");
            }

            await _appointmentRepository.DeleteAsync(id);
        }

        public async Task<List<AppointmentDto>> GetAppointmentsAsync()
        {
           //fetching Appointments data from database
            var appointments = await _appointmentRepository.GetAppointmentsAsync();
            
            //Mapping List<Appoinment> to List<AppointmentDto> using Automapper
            return  _mapper.Map<List<AppointmentDto>>(appointments);
        }

        public async Task<(List<AppointmentDto> Appointments, int TotalRecords)> GetAllAppointmentsAsync(AppointmentQueryParameter appointmentQueryParameter)
        {
            var (appointments, TotalRecords) = await _appointmentRepository.GetAllAppointmentsAsync(appointmentQueryParameter);

            return (_mapper.Map<List<AppointmentDto>>(appointments!), TotalRecords);
        }

        public async Task<AppointmentDto> GetAppointmentByIdAsync(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);

            if (appointment == null) 
            {
                throw new KeyNotFoundException($"No appointment existt with id : {id}");
            }

            return _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<AppointmentDetailsDto> GetAppointmentForDetailsByIdAsync(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
            {
                throw new KeyNotFoundException($"No appointment existt with id : {id}");
            }

            return _mapper.Map<AppointmentDetailsDto>(appointment);
        }

        public async Task<UpdateAppointmentDto> GetAppointmentForUpdateByIdAsync(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
            {
                throw new KeyNotFoundException($"No appointment existt with id : {id}");
            }

            return _mapper.Map<UpdateAppointmentDto>(appointment);
        }

        public async Task UpdateAppointmentAsync(UpdateAppointmentDto updateAppointmentDto)
        {
            if (updateAppointmentDto.AppointmentDate < DateOnly.FromDateTime(DateTime.Today))
            {
                throw new ArgumentException("Appointment date cannot be in the past.");
            }

            if (!await _appointmentRepository.DoctorExistsAsync(updateAppointmentDto.DoctorId))
            {
                throw new ArgumentException("Doctor does not exists");
            }

            if (!await _appointmentRepository.PatientExistsAsync(updateAppointmentDto.PatientId))
            {
                throw new ArgumentException("Patient does not exists");
            }

            bool doctorBusy = await _appointmentRepository.DoctorHasConflictingAppointmentAsync(updateAppointmentDto.DoctorId,updateAppointmentDto.AppointmentDate, updateAppointmentDto.StartTime, updateAppointmentDto.EndTime, updateAppointmentDto.Id);

            if (doctorBusy)
            {
                throw new InvalidOperationException("Doctor has an another appointment during this time.");
            }

            bool patientBusy = await _appointmentRepository.PatientHasConflictingAppointmentAsync(updateAppointmentDto.PatientId, updateAppointmentDto.AppointmentDate, updateAppointmentDto.StartTime, updateAppointmentDto.EndTime, updateAppointmentDto.Id);

            if (patientBusy)
            {
                throw new InvalidOperationException("Patient has an another appointment during this time.");
            }

            if (updateAppointmentDto.StartTime<TimeSpan.FromHours(9) || updateAppointmentDto.EndTime>TimeSpan.FromHours(18))
            {
                throw new InvalidOperationException("Please select schedule slot 9:00 Am to 6:00 PM");     
            }

            if (updateAppointmentDto.EndTime <= updateAppointmentDto.StartTime)
            {
                throw new InvalidOperationException("Appointment end time later than start time.");
            }

            var existingAppointment = await _appointmentRepository.GetByIdAsync(updateAppointmentDto.Id);

            if (existingAppointment == null)
            {
                throw new KeyNotFoundException($"No appointment exists with id: {updateAppointmentDto.Id}");
            }

            Appointment updatedAppointment = _mapper.Map(updateAppointmentDto, existingAppointment);

            await _appointmentRepository.UpdateAsync(updatedAppointment);
        }

        public async Task ConfirmedAppointmentAsync(int appointmentId)
        {
            Appointment? appointment = await _appointmentRepository.GetByIdAsync(appointmentId);

            if (appointment == null)
            {
                throw new KeyNotFoundException($"No appointment exist with appointment id : {appointmentId}");
            }

            await _appointmentRepository.ConfirmedAppointmentAync(appointmentId);
        }

        public async Task RescheduleAppointmentAsync(RescheduleAppointmentDto rescheduleAppointmentDto)
        {
            Appointment appointment = await _appointmentRepository.GetByIdAsync(rescheduleAppointmentDto.Id);

            if (appointment == null)
            {
                throw new KeyNotFoundException($"No appointment found id: {rescheduleAppointmentDto.Id}");
            }

            await _appointmentRepository.RescheduleAppointmentAsync(rescheduleAppointmentDto);
        }

        public async Task CancelAppointmentAsync(int appointmentId)
        {
            Appointment appointment = await _appointmentRepository.GetByIdAsync(appointmentId);

            if (appointment == null)
            {
                throw new KeyNotFoundException($"No appointment found  with appointment id: {appointmentId} .");
            }

            if (appointment.Status == AppointmentStatus.Completed)
            {
                throw new InvalidOperationException("Completed appointments cannot be cancelled.");
            }
            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                throw new InvalidOperationException("Apointment is already cancelled");
            }

            await _appointmentRepository.CancelAppointmentAsync(appointmentId);
        }

        public async Task CompletedAppointmentAsync(int appoinmentId)
        {
            Appointment? appointment = await _appointmentRepository.GetByIdAsync(appoinmentId);

            if (appointment == null)
            {
                throw new KeyNotFoundException($"No appointmen found wth id: {appoinmentId}");
            }
            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                throw new InvalidOperationException("Cancelled Appointments cannot be marked as comleted");
            }

            appointment.Status = AppointmentStatus.Completed;

            await _appointmentRepository.CompletedAppointmentAsync(appoinmentId);

            await _billingService.CreateBillAsync(new CreateBillDto 
            {
                AppointmentId  = appoinmentId,
                AdditionalCharges = 0,
                Discount = 0,
            });
        }
    }
}
