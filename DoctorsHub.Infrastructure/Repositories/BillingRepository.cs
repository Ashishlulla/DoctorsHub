using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.DTOs.common.DoctorsHub.Application.DTOs.Common;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Domain.Entities;
using DoctorsHub.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;


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
            return await _db.Bills.Include(a=>a.Appointment).ThenInclude(d=>d.Doctor).Include(a=>a.Appointment).ThenInclude(p=>p.Patient).ToListAsync();
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

        public async Task<(List<Bill> Bills, int totalBills)> GetBillsAsync(BillingQueryParameter billingQueryParameter)
        {
            //creating Query 
            IQueryable<Bill> query =  _db.Bills
                .AsNoTracking()
                .Include(a => a.Appointment)
                .ThenInclude(d => d.Doctor)
                .Include(a => a.Appointment)
                .ThenInclude(p => p.Patient);

            //Searching
            if (!string.IsNullOrWhiteSpace(billingQueryParameter.searchString))
            {
                switch (billingQueryParameter.searchBy)
                {
                    case "PatientName":
                        query = query.Where(p => EF.Functions.Like(p.Appointment.Patient.FullName, $"%{billingQueryParameter.searchString}%"));
                        break;

                    case "DoctorName":
                        query = query.Where(d => EF.Functions.Like(d.Appointment.Doctor.FullName, $"%{billingQueryParameter.searchString}%"));
                        break;

                    case "BillDate":
                        if (DateTime.TryParse(billingQueryParameter.searchString, out DateTime billDate)) 
                        {
                            query = query.Where(b => b.BillDate.Date == billDate.Date);
                        }
                        break;


                    case "AppointmentDate":
                        if (DateOnly.TryParse(billingQueryParameter.searchString,out DateOnly appointmentDate))
                        {
                            query = query.Where(b=>b.Appointment.AppointmentDate == appointmentDate);
                        }
                        break;

                    case "BillId":
                        if (int.TryParse(billingQueryParameter.searchString, out int billId))
                        {
                            query = query.Where(b=>b.Id == billId);
                        }
                        break;
                    }
                }


            //Sorting
            query = (billingQueryParameter.sortBy,billingQueryParameter.sortOrder) switch 
            {
                ("PatientName", "asc")=> query.OrderBy(p=>p.Appointment.Patient.FullName),
                ("PatientName", "desc") => query.OrderByDescending(p => p.Appointment.Patient.FullName),

                ("DoctorName", "asc") => query.OrderBy(p => p.Appointment.Doctor.FullName),
                ("DoctorName", "desc") => query.OrderByDescending(p => p.Appointment.Doctor.FullName),

                ("BillDate", "asc") => query.OrderBy(p => p.BillDate),
                ("BillDate", "desc") => query.OrderByDescending(p => p.BillDate),

                ("AppointmentDate", "asc") => query.OrderBy(p => p.Appointment.AppointmentDate),
                ("AppointmentDate", "desc") => query.OrderByDescending(p => p.Appointment.AppointmentDate),

                ("TotalAmount", "asc") => query.OrderBy(p => p.TotalAmount),
                ("TotalAmount", "desc") => query.OrderByDescending(p => p.TotalAmount),

                _ => query.OrderByDescending(b => b.BillDate)
            };

            //TotalRecords
            int totalBills = await query.CountAsync();

            //Pagination
            List<Bill> bills = await query
                .Skip((billingQueryParameter.PageNumber-1)*billingQueryParameter.PageSize)
                .Take(billingQueryParameter.PageSize)
                .ToListAsync();

            return (bills, totalBills);
        }
    }
}
