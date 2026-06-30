using AutoMapper;
using DoctorsHub.Application.DTOs.Patients;
using DoctorsHub.Application.Interfaces;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

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

        public async Task<List<PatientDto>> GetAllPatientAsync()
        {
            List<Patient> patients = await _patientRepository.GetAllPatientsAsync();
            return _mapper.Map<List<PatientDto>>(patients);
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

      
    }
}
