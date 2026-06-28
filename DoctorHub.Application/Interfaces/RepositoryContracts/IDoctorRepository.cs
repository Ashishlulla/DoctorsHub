using DoctorsHub.Domain.Entities;

namespace DoctorsHub.Application.Interfaces.RepositoryContracts
{
    public interface IDoctorRepository
    {
        //Doctors
        Task<Doctor> AddAsync(Doctor doctor);

        Task<List<Doctor>> GetDoctorsAsync();

        Task<(List<Doctor>, int TotalCount)> GetAllDoctorsAsync(string searchBy = nameof(Doctor.FullName), string? searchString="a", string sortBy = nameof(Doctor.FullName), string sortOrder = "asc", int PageSize = 5, int PageNumber = 1);
        Task<Doctor?> GetByIdAsync(int id);
        Task<Doctor?> GetByUserIdAsync(string userId);

        Task UpdateDoctorAsync(Doctor doctor);
        Task DeleteDoctorByIdAsync(int id);
    }
}
