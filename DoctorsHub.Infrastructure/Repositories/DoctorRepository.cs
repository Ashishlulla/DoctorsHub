using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub;
using System;
using System.Collections.Generic;
using System.Text;
using DoctorsHub.Domain.Entities;
using DoctorsHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using DoctorsHub.Application.DTOs.Doctors;

namespace DoctorsHub.Infrastructure.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly ApplicationDbContext _db;

        public DoctorRepository(ApplicationDbContext db) 
        {
            _db = db;
        }

        public async Task<Doctor> AddAsync(Doctor doctor)
        {
            await _db.Doctors.AddAsync(doctor);
            await _db.SaveChangesAsync();

            return doctor;
        }

        public async Task DeleteDoctorByIdAsync(int id)
        {
            var doctor = await _db.Doctors.FindAsync(id);
        
            _db.Doctors.Remove(doctor!);
            await _db.SaveChangesAsync();

        }

        public async Task<Doctor?> GetByIdAsync(int id)
        {
            return await _db.Doctors
                .Include(d=>d.User)
                .Include(d => d.Specialization)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Doctor?> GetByUserIdAsync(string userId)
        {
            return await _db.Doctors.
                Include(d=>d.User).
                Include(d => d.Specialization)
                .FirstOrDefaultAsync(d => d.UserId == userId);
        }

        public async Task<List<Doctor>> GetDoctorsAsync()
        {
            return await _db.Doctors.Include(d=>d.User).Include(d=>d.Specialization).ToListAsync();
        }

        public async Task UpdateDoctorAsync(Doctor doctor)
        {
            _db.Doctors.Update(doctor);
            await _db.SaveChangesAsync();
        }
    }
}
