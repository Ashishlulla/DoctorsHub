using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Domain.Entities;
using DoctorsHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        //private feilds
        private readonly ApplicationDbContext _db;

        //constructor
        public AppointmentRepository(ApplicationDbContext db) 
        {
            _db = db;
        }

        public async Task AddAsync(Appointment appointment)
        {
            await _db.AddAsync(appointment);

            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var appointment = await _db.Appointments.FindAsync(id);

             _db.Appointments.Remove(appointment!);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsAsync()
        {
            return await _db.Appointments.ToListAsync();
        }

        public async Task<Appointment> GetByIdAsync(int id)
        {
            Appointment? appointment = await _db.Appointments!.FindAsync(id);
            return appointment!;

        }

        public async Task UpdateAsync(Appointment appointment)
        {
            _db.Appointments.Update(appointment);
            await _db.SaveChangesAsync();
        }
    }
}
