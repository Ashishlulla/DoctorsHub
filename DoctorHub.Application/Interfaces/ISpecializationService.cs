using DoctorsHub.Application.DTOs.Doctors;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.Interfaces
{
    public interface ISpecializationService
    {
        Task<List<SpecializationDTO>> GetAllSpecialization();
    }
}
