using DoctorsHub.Domain.Entities;

namespace DoctorsHub.Application.Interfaces.RepositoryContracts
{
    public interface IDoctorRepository
    {
        //Doctors
        Task<Doctor> AddAsync(Doctor doctor);

        Task<List<Doctor>> GetDoctorsAsync();

        Task<(List<Doctor>, int TotalCount)> GetAllDoctorsAsync( string? searchBy, string? searchString, string? sortBy, string? sortOrder, int PageSize, int PageNumber);
        Task<Doctor?> GetByIdAsync(int id);
        Task<Doctor?> GetByUserIdAsync(string userId);

        Task UpdateDoctorAsync(Doctor doctor);
        Task DeleteDoctorByIdAsync(int id);
    }
}
