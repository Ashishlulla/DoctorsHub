using DoctorsHub.Domain.Entities;


namespace DoctorsHub.Application.Interfaces
{
    public interface IPatientRepository
    {
        Task AddAsync(Patient patient);
        
        Task<List<Patient>> GetAllPatientsAsync();
        
        Task<Patient?> GetPatientByIdAsync(int id);

        Task UpdateAsync(Patient patient);

        Task DeleteAsync(int id);
    }
}
