using DoctorsHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.Interfaces.RepositoryContracts
{
    public interface ISpecializationRepository
    {
        Task<List<Specialization>> GetAllSpecializationAsync();
    }
}
