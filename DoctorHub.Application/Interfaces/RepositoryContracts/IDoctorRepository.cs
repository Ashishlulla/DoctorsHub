using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Domain.Entities;

namespace DoctorsHub.Application.Interfaces.RepositoryContracts
{
    public interface IDoctorRepository
    {
        Task<Doctor> AddAsync(Doctor doctor);

        Task<List<Doctor>> GetDoctorsAsync();

        Task<(List<Doctor> Doctors, int TotalCount)> GetAllDoctorsAsync(
            DoctorQueryParameters doctorQueryParameters);

        Task<Doctor?> GetByIdAsync(int id);

        Task<Doctor?> GetByUserIdAsync(string userId);

        Task UpdateDoctorAsync(Doctor doctor);

        Task DeleteDoctorByIdAsync(int id);
    }
}