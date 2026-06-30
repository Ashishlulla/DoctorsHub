using DoctorsHub.Application.DTOs.Patients;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.Interfaces
{
    public interface IPatientService
    {
        Task CreatePatientAsync(CreatePatientDto createPatientDto);

        Task<List<PatientDto>> GetAllPatientAsync();

        Task<PatientDto> GetPatientByIdAsync(int id);
        Task<UpdatePatientDto> GetPatientForUpdateByIdAsync(int id);

        Task UpdatePatientAsync( UpdatePatientDto dto);

        Task DeletePatientAsync(int id);
    }
}
