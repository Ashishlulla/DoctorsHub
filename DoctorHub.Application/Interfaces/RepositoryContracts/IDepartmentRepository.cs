using DoctorsHub.Domain.Entities;

namespace DoctorsHub.Application.Interfaces.RepositoryContracts
{
    public interface IDepartmentRepository
    {
        Task<List<Department>> GetDepartmentsAsync();
        
        Task<Department?> GetDepartmentByIdAsync(int id);
        
        Task<Department> CreateDepartmentAsync(Department department);
        
        Task<Department> UpdateDepartmentAsync(Department department);
        
        Task<bool> DeleteDepartmentAsync(Department department);
    }
}
