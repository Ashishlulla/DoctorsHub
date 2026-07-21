using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Domain.Entities;
using DoctorsHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace DoctorsHub.Infrastructure.Repositories
{
    public class BillingRepository :IBillingRepository
    {
        //Private Feilds
        private readonly ApplicationDbContext _db;

        //Constructor
        public BillingRepository(ApplicationDbContext db) 
        {
            _db = db;
        }

        public async Task AddBillAsync(Bill bill)
        { 
            await _db.Bills.AddAsync(bill);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteBillAsync(Bill bill)
        {
                _db.Bills.Remove(bill);
                await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Bill>> GetAllBillsAsync()
        {
            return await _db.Bills.Include(b => b.Appointment).ThenInclude(d => d.Doctor).Include(a => a.Appointment).ThenInclude(p => p.Patient).ToListAsync();
        }

        public async Task<Bill?> GetBillByAppointmentIdAsync(int appointmentId)
        {
            return await _db.Bills.Include(b=>b.Appointment).ThenInclude(d=>d.Doctor).Include(a=>a.Appointment).ThenInclude(p=>p.Patient).FirstOrDefaultAsync(b=>b.AppointmentId == appointmentId);
        }

        public async Task<Bill?> GetBillByIdAsync(int id)
        {
            Bill? billToFind = await _db.Bills.Include(b => b.Appointment).ThenInclude(d => d.Doctor).Include(a => a.Appointment).ThenInclude(p => p.Patient).FirstOrDefaultAsync(b=>b.Id == id);

            return billToFind;
        }

        public async Task UpdateBillAsync(Bill bill)
        {
            _db.Bills.Update(bill);
            await _db.SaveChangesAsync();
        }
    }
}
