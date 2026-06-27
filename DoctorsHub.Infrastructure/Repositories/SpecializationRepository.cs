using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Domain.Entities;
using DoctorsHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Infrastructure.Repositories
{
    public class SpecializationRepository : ISpecializationRepository
    {
        private readonly ApplicationDbContext _db;

        public SpecializationRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Specialization>> GetAllSpecializationAsync()
        {
            return await _db.Specializations.ToListAsync();
        }
    }
}
