using DoctorsHub.Application.DTOs.Appoitments;
using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Domain.Entities;

namespace DoctorsHub.Application.Interfaces.RepositoryContracts
{
    public interface IAppointmentRepository 
    {
        //Bussiness Logic Function
        Task AddAsync(Appointment appointment);
        Task<List<Appointment>> GetAppointmentsAsync();

        Task<(List<Appointment> Appointments, int TotalRecords)> GetAllAppointmentsAsync(AppointmentQueryParameter appointmentQueryParameter);
        Task<Appointment> GetByIdAsync(int id);
        Task UpdateAsync(Appointment appointment);
        Task DeleteAsync(int id);


        //Scheduling Bussiness Logic Functions
        Task<bool> DoctorHasConflictingAppointmentAsync(
            int doctorId,
            DateOnly appointmentDate,
            TimeSpan startTime,
            TimeSpan endTime,
            int? appointmentId = null);

        Task<bool> PatientHasConflictingAppointmentAsync(
            int patientId,
            DateOnly appointmentDate,
            TimeSpan startTime,
            TimeSpan endTime,
            int? appointmentId = null);

        Task<bool> DoctorExistsAsync(int doctorId);

        Task<bool> PatientExistsAsync(int patientId);

        Task ConfirmedAppointmentAync(int appointmentId);

        Task RescheduleAppointmentAsync(RescheduleAppointmentDto rescheduleAppointmentDto);

        Task CancelAppointmentAsync(int appointmentId);

        Task CompletedAppointmentAsync(int appointmentId);

    }
}
