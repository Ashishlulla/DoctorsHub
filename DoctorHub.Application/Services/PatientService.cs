using AutoMapper;
using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.DTOs.Patients;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using DoctorsHub.Domain.Entities;
using System.Net.Http.Headers;

namespace DoctorsHub.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public PatientService(IPatientRepository patientRepository,IMapper mapper) 
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        public async Task CreatePatientAsync(CreatePatientDto createPatientDto)
        {
            var patient = _mapper.Map<Patient>(createPatientDto);

            await _patientRepository.AddAsync(patient);
        }

        public async Task DeletePatientAsync(int id)
        {
            Patient? patient = await _patientRepository.GetPatientByIdAsync(id);

            if (patient == null)
            {
                throw new KeyNotFoundException($"No patient exist with id = {id}");
            }

           await _patientRepository.DeleteAsync(id);
        }

        public async Task<PagedResult<PatientDto>> GetAllPatientAsync(PatientQueryParameters patientQueryParameters)
        {
            var(patients, TotalRecords) = await _patientRepository.GetAllPatientsAsync(patientQueryParameters);
            
            
            return new PagedResult<PatientDto>
            {
                Items = _mapper.Map<List<PatientDto>>(patients),
                TotalRecords = TotalRecords,
                PageNumber = patientQueryParameters.PageNumber,
                PageSize = patientQueryParameters.PageSize
            };
               
        }

        public async Task<PatientDto> GetPatientByIdAsync(int id)
        {
            Patient? patient = await _patientRepository.GetPatientByIdAsync(id);
            if (patient == null)
            {
                throw new KeyNotFoundException($"No patient exist id = {id}");
            }
            return _mapper.Map<PatientDto>(patient);
        }

 
        public async Task UpdatePatientAsync(UpdatePatientDto dto)
        {
            Patient? patient = await _patientRepository.GetPatientByIdAsync(dto.Id);

            if (patient == null)
            {
                throw new KeyNotFoundException($"No patient exist id = {dto.Id}");
            }

            Patient updatedPatient = _mapper.Map( dto, patient);

            await _patientRepository.UpdateAsync(updatedPatient);
        }

        public async Task<UpdatePatientDto> GetPatientForUpdateByIdAsync(int id)
        {
            Patient? patient = await _patientRepository.GetPatientByIdAsync(id);
            if (patient == null)
            {
                throw new KeyNotFoundException($"No patient exist id = {id}");
            }
            return _mapper.Map<UpdatePatientDto>(patient);
        }

        public async Task<List<PatientDto>> GetAllPatientsAsync()
        {
            List<Patient> patients = await _patientRepository.GetPatientsAsync();

            return _mapper.Map<List<PatientDto>>(patients);
        }
    }
}
