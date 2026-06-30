using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Domain.Entities;


namespace DoctorsHub.Application.Interfaces.RepositoryContracts
{
    public interface IPatientRepository
    {
        Task AddAsync(Patient patient);
        
        Task<(List<Patient>, int TotalRecord)> GetAllPatientsAsync(PatientQueryParameters patientQueryParameters);
        
        Task<Patient?> GetPatientByIdAsync(int id);

        Task UpdateAsync(Patient patient);

        Task DeleteAsync(int id);
    }
}
