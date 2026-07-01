using DoctorsHub.Domain.Entities;

namespace DoctorsHub.Application.Interfaces.RepositoryContracts
{
    public interface IAppointmentRepository 
    {
        Task AddAsync(Appointment appointment);
        Task<List<Appointment>> GetAppointmentsAsync();
        Task<Appointment> GetByIdAsync(int id);
        Task UpdateAsync(Appointment appointment);
        Task DeleteAsync(int id);


        Task<bool> DoctorHasConflictingAppointmentAsync(
            int doctorId,
            DateTime appointmentDate,
            TimeSpan startTime,
            TimeSpan endTime,
            int? appointmentId = null);

        Task<bool> PatientHasConflictingAppointmentAsync(
            int patientId,
            DateTime appointmentDate,
            TimeSpan startTime,
            TimeSpan endTime,
            int? appointmentId = null);

        Task<bool> DoctorExistsAsync(int doctorId);

        Task<bool> PatientExistsAsync(int patientId);
    }
}
