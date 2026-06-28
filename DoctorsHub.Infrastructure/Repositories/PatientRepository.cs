using DoctorsHub.Application.Interfaces;
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

        public async Task<List<Patient>> GetAllPatientsAsync()
        {
            return await _db.Patients.ToListAsync();
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
    }
}
