using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Domain.Entities;
using DoctorsHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace DoctorsHub.Infrastructure.Repositories
{
     public class PatientRepository : IPatientRepository
    {
        //Private Feilds
        private readonly ApplicationDbContext _db;
        
        //Constructor
        public PatientRepository(ApplicationDbContext db) 
        {
            _db = db;
        }

        public async Task AddAsync(Patient patient)
        {
            await _db.Patients.AddAsync(patient);

            await _db.SaveChangesAsync();
        }

       

        public async Task<Patient?> GetPatientByIdAsync(int id)
        {
            
            return  await _db.Patients.FindAsync(id);
        }

        public async Task UpdateAsync(Patient patient)
        {
            _db.Patients.Update(patient);
            await _db.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var patient = await _db.Patients.FindAsync(id);

            if (patient != null)
            {
                _db.Patients.Remove(patient!);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<(List<Patient>, int TotalRecord)> GetAllPatientsAsync(PatientQueryParameters patientQueryParameters)
        {
            IQueryable<Patient> query = _db.Patients;


            //Searching + Filtering 
            switch (patientQueryParameters.searchBy) 
            {
                case nameof(Patient.FullName):
                    query = query.Where(p => EF.Functions.Like(p.FullName, $"%{patientQueryParameters.searchString}%"));
                    break;

                case nameof(Patient.Email):
                    query = query.Where(p => EF.Functions.Like(p.Email, $"%{patientQueryParameters.searchString}%"));
                    break;

                case nameof(Patient.PhoneNumber):
                    query = query.Where(p => EF.Functions.Like(p.PhoneNumber, $"%{patientQueryParameters.searchString}%"));
                    break;

                case nameof(Patient.BloodGroup):
                    query = query.Where(p => EF.Functions.Like(p.BloodGroup, $"%{patientQueryParameters.searchString}%"));
                    break;


                default:
                    query = query.Where(p => EF.Functions.Like(p.FullName, $"%{patientQueryParameters.searchString}%"));
                    break;
            }

            //Sorting
            query = (patientQueryParameters.sortBy, patientQueryParameters.sortOrder) switch
            {
                (nameof(Patient.FullName), "asc") => query.OrderBy(d => d.FullName),
                (nameof(Patient.FullName), "desc") => query.OrderByDescending(d => d.FullName),

                (nameof(Patient.BloodGroup), "asc") => query.OrderBy(d => d.BloodGroup),
                (nameof(Patient.BloodGroup), "desc") => query.OrderByDescending(d => d.BloodGroup),

                (nameof(Patient.DateOfBirth), "asc") => query.OrderBy(d => d.DateOfBirth),
                (nameof(Patient.DateOfBirth), "desc") => query.OrderByDescending(d => d.DateOfBirth),
                
                _ => query.OrderBy(d => d.FullName),

            };

            int TotalRecords = await query.CountAsync();

            //Pagination
             var data = await query.Skip((patientQueryParameters.PageNumber - 1) * patientQueryParameters.PageSize).Take(patientQueryParameters.PageSize).ToListAsync();

            return (data, TotalRecords);
               
        }

        public async Task<List<Patient>> GetPatientsAsync()
        {
            return await _db.Patients.ToListAsync();
        }
    }
}
