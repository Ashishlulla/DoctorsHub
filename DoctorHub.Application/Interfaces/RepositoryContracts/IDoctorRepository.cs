using DoctorsHub.Domain.Entities;

namespace DoctorsHub.Application.Interfaces.RepositoryContracts
{
    public interface IDoctorRepository
    {
        //Doctors
        Task<Doctor> AddAsync(Doctor doctor);

        Task<List<Doctor>> GetDoctorsAsync();
        Task<Doctor?> GetByIdAsync(int id);
        Task<Doctor?> GetByUserIdAsync(string userId);

        Task UpdateDoctorAsync(Doctor doctor);
        Task DeleteDoctorByIdAsync(int id);
    }
}
