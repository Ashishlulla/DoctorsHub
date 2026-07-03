using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Domain.Entities;
using DoctorsHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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
			Doctor? doctor = await _db.Doctors.FindAsync(id);

			if (doctor != null)
			{
				_db.Doctors.Remove(doctor);
				await _db.SaveChangesAsync();
			}
		}

		public async Task<(List<Doctor> Doctors, int TotalCount)> GetAllDoctorsAsync(
			DoctorQueryParameters doctorQueryParameters)
		{
			IQueryable<Doctor> query = _db.Doctors
				.AsNoTracking()
				.Include(d => d.User)
				.Include(d => d.Specialization);

			// Filtering
			if (!string.IsNullOrWhiteSpace(doctorQueryParameters.searchString))
			{
				switch (doctorQueryParameters.searchBy)
				{
					case nameof(Doctor.FullName):
						query = query.Where(d =>
							EF.Functions.Like(d.FullName, $"%{doctorQueryParameters.searchString}%"));
						break;

					case nameof(Doctor.Qualification):
						query = query.Where(d =>
							EF.Functions.Like(d.Qualification, $"%{doctorQueryParameters.searchString}%"));
						break;

					case nameof(Doctor.Specialization.Name):
						query = query.Where(d =>
							EF.Functions.Like(d.Specialization.Name, $"%{doctorQueryParameters.searchString}%"));
						break;

					case nameof(Doctor.User.Email):
						query = query.Where(d =>
							EF.Functions.Like(d.User.Email!, $"%{doctorQueryParameters.searchString}%"));
						break;

					default:
						query = query.Where(d =>
							EF.Functions.Like(d.FullName, $"%{doctorQueryParameters.searchString}%"));
						break;
				}
			}

			// Sorting
			query = (doctorQueryParameters.sortBy,
					 (doctorQueryParameters.sortOrder ?? "asc").ToLower()) switch
			{
				(nameof(Doctor.FullName), "asc") =>
					query.OrderBy(d => d.FullName),

				(nameof(Doctor.FullName), "desc") =>
					query.OrderByDescending(d => d.FullName),

				(nameof(Doctor.ExperienceYears), "asc") =>
					query.OrderBy(d => d.ExperienceYears),

				(nameof(Doctor.ExperienceYears), "desc") =>
					query.OrderByDescending(d => d.ExperienceYears),

				(nameof(Doctor.ConsultationFee), "asc") =>
					query.OrderBy(d => d.ConsultationFee),

				(nameof(Doctor.ConsultationFee), "desc") =>
					query.OrderByDescending(d => d.ConsultationFee),

				(nameof(Doctor.AverageRating), "asc") =>
					query.OrderBy(d => d.AverageRating),

				(nameof(Doctor.AverageRating), "desc") =>
					query.OrderByDescending(d => d.AverageRating),

				_ => query.OrderBy(d => d.FullName)
			};

			// Total Records
			int totalRecords = await query.CountAsync();

			// Pagination
			List<Doctor> doctors = await query
				.Skip((doctorQueryParameters.PageNumber - 1) * doctorQueryParameters.PageSize)
				.Take(doctorQueryParameters.PageSize)
				.ToListAsync();

			return (doctors, totalRecords);
		}

		public async Task<Doctor?> GetByIdAsync(int id)
		{
			return await _db.Doctors
				.Include(d => d.User)
				.Include(d => d.Specialization)
				.FirstOrDefaultAsync(d => d.Id == id);
		}

		public async Task<Doctor?> GetByUserIdAsync(string userId)
		{
			return await _db.Doctors
				.Include(d => d.User)
				.Include(d => d.Specialization)
				.FirstOrDefaultAsync(d => d.UserId == userId);
		}

		public async Task<List<Doctor>> GetDoctorsAsync()
		{
			return await _db.Doctors
				.Include(d => d.User)
				.Include(d => d.Specialization)
				.ToListAsync();
		}

		public async Task UpdateDoctorAsync(Doctor doctor)
		{
			_db.Doctors.Update(doctor);
			await _db.SaveChangesAsync();
		}
	}
}