using DoctorsHub.Application.DTOs.Doctors;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using DoctorsHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.Services
{
    public class SpecializationService : ISpecializationService
    {
        private readonly ISpecializationRepository _specializationRepository;

        public SpecializationService(ISpecializationRepository specializationRepository) 
        {
            _specializationRepository = specializationRepository;
        }

        public async Task<List<SpecializationDTO>> GetAllSpecialization()
        {
            IEnumerable<Specialization> specializations = await _specializationRepository.GetAllSpecializationAsync();

            return specializations
                .Select(s =>  new SpecializationDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                })
                .ToList();
        }
    }
}
