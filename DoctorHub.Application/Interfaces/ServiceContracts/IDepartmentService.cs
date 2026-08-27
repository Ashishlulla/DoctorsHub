

using DoctorsHub.Application.DTOs.Departments;

namespace DoctorsHub.Application.Interfaces.ServiceContracts
{
    public interface IDepartmentService
    {
        
        Task<DepartmentDto> AddDepartmentAsync(CreateDepartmentDto createDepartmentDto);
        
        Task<DepartmentDto> UpdateDepartmentAsync(UpdateDepartmentDto updateDepartmentDto);
        
        Task DeleteDepartmentAsync(int? id);

        Task<DepartmentDto> GetDepartmentByIdAsync(int? id);

        Task<List<DepartmentDto>> GetDepartmentsAsync();

        Task<DepartmentDetailsDto> GetDepartmentDetailsAsync(int id);

    }
}
