using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub;
using System;
using System.Collections.Generic;
using System.Text;
using DoctorsHub.Domain.Entities;
using DoctorsHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using DoctorsHub.Application.DTOs.Doctors;
using System.Security.Cryptography;

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

        public async Task<(List<Doctor>, int TotalCount)> GetAllDoctorsAsync(string searchBy=nameof(Doctor.FullName), string? searchString="a", string sortBy =nameof(Doctor.FullName), string sortOrder ="asc", int PageSize = 5, int PageNumber=1)
        {
            IQueryable<Doctor> query = _db.Doctors.Include(d=>d.User).Include(d=>d.Specialization);

            //Filtering
            switch (searchBy) 
            {
                case nameof(Doctor.FullName):
                    query = query.Where(d =>EF.Functions.Like(d.FullName, $"%{searchString}%"));
                    break;

                case nameof(Doctor.Qualification):
                    query = query.Where(d => EF.Functions.Like(d.Qualification, $"%{searchString}%"));
                    break;

                case nameof(Doctor.Specialization.Name):
                    query = query.Where(d => EF.Functions.Like(d.Specialization.Name, $"%{searchString}%"));
                    break;

                case nameof(Doctor.User.Email):
                    query = query.Where(d => EF.Functions.Like(d.User.Email, $"%{searchString}%"));
                    break;

                default:
                    query = query.Where(d => EF.Functions.Like(d.FullName, $"%{searchString}%"));
                    break;
            };

            //Sorting
            query = (sortBy, sortOrder.ToLower()) switch
            {
                (nameof(Doctor.FullName), "asc") => query.OrderBy(d => d.FullName),
                (nameof(Doctor.FullName), "desc") => query.OrderByDescending(d => d.FullName),

                (nameof(Doctor.ExperienceYears), "asc") => query.OrderBy(d => d.ExperienceYears),
                (nameof(Doctor.ExperienceYears), "desc") => query.OrderByDescending(d => d.ExperienceYears),

                (nameof(Doctor.ConsultationFee), "asc") => query.OrderBy(d => d.ConsultationFee),
                (nameof(Doctor.ConsultationFee), "desc") => query.OrderByDescending(d => d.ConsultationFee),

                (nameof(Doctor.AverageRating), "asc") => query.OrderBy(d => d.AverageRating),
                (nameof(Doctor.AverageRating), "desc") => query.OrderByDescending(d => d.AverageRating),
            };

            //Total Records Count
            int TotalRecords = query.Count();

            //Pagination
            var data = await query
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return (data, TotalRecords);
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
