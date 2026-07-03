using DoctorHub.Application.DTOs.Doctors;
using DoctorsHub.Application.DTOs.common;

namespace DoctorsHub.Application.Interfaces.ServiceContracts
{
    public interface IDoctorService
    {
        Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto createDoctorDto);
        
        Task<List<DoctorDto>> GetAllDoctorsAsync();
        Task<(List<DoctorDto> Data, int TotalCount)> GetAllDoctorsAsync(DoctorQueryParameters doctorQueryParameters);
        Task<DoctorDto?> GetByIdAsync(int id);
       
        Task UpdateDoctorAsync(int id, UpdateDoctorDto updateDoctorDto);
        Task DeleteDoctorAsync(int id);

    }
}
