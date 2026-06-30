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
    }
}
