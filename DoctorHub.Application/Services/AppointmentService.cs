using AutoMapper;
using DoctorsHub.Application.DTOs.Appoitments;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using DoctorsHub.Domain.Entities;


namespace DoctorsHub.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        //private feilds
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        //constructor
        public AppointmentService(IAppointmentRepository appointmentRepository, IMapper mapper) 
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task CreateAppointmentAsync(CreateAppointmentDto createAppointmentDto)
        {
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

        public async Task<List<AppointmentDto>> GetAllAppointmentsAsync()
        {
           //fetching Appointments data from database
            var appointments = await _appointmentRepository.GetAppointmentsAsync();
            
            //Mapping List<Appoinment> to List<AppointmentDto> using Automapper
            return  _mapper.Map<List<AppointmentDto>>(appointments);
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
            var existingAppointment = await _appointmentRepository.GetByIdAsync(updateAppointmentDto.Id);

            if (existingAppointment == null)
            {
                throw new KeyNotFoundException($"No appointment exists with id: {updateAppointmentDto.Id}");
            }

            Appointment updatedAppointment = _mapper.Map(updateAppointmentDto, existingAppointment);

            await _appointmentRepository.UpdateAsync(updatedAppointment);
        }
    }
}
