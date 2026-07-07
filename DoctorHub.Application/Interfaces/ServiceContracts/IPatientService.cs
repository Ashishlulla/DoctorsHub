using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.DTOs.common.DoctorsHub.Application.DTOs.Common;
using DoctorsHub.Application.DTOs.Patients;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.Interfaces.ServiceContracts
{
    public interface IPatientService
    {
        Task CreatePatientAsync(CreatePatientDto createPatientDto);

        Task<List<PatientDto>> GetAllPatientsAsync();

        Task<PagedResult<PatientDto>> GetAllPatientsAsync(PatientQueryParameters patientQueryParameters);
        
        Task<PatientDto> GetPatientByIdAsync(int id);
        
        Task<UpdatePatientDto> GetPatientForUpdateByIdAsync(int id);

        Task UpdatePatientAsync( UpdatePatientDto dto);

        Task DeletePatientAsync(int id);
    }
}
