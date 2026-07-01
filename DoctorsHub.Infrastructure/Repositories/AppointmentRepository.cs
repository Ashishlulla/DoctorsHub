using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Domain.Entities;
using DoctorsHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Numerics;
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

        public async Task<bool> DoctorExistsAsync(int doctorId)
        {
            return await _db.Doctors.AnyAsync(p => p.Id == doctorId);
        }

        public async Task<bool> DoctorHasConflictingAppointmentAsync(int doctorId, DateTime appointmentDate, TimeSpan startTime, TimeSpan endTime, int? appointmentId = null)
        {
            bool isConflicting = await _db.Appointments.AnyAsync(a =>
            a.DoctorId == doctorId &&
            a.AppointmentDate == appointmentDate &&
            (!appointmentId.HasValue || a.Id != appointmentId) &&
            a.StartTime < endTime &&
            a.EndTime > startTime
            );
            return isConflicting;
        }

        public async Task<List<Appointment>> GetAppointmentsAsync()
        {
            return await _db.Appointments.Include(p=>p.Patient).Include(d=>d.Doctor).ToListAsync();
        }

        public async Task<Appointment> GetByIdAsync(int id)
        {
            Appointment? appointment = await _db.Appointments.Include(p => p.Patient).Include(d => d.Doctor).FirstOrDefaultAsync(a => a.Id == id);
            return appointment!;

        }

        public  async Task<bool> PatientExistsAsync(int patientId)
        {
            return await _db.Patients.AnyAsync(p=>p.Id ==patientId);

        }

        public async Task<bool> PatientHasConflictingAppointmentAsync(int patientId, DateTime appointmentDate, TimeSpan startTime, TimeSpan endTime, int? appointmentId = null)
        {
            bool isConflicting = await _db.Appointments.AnyAsync(a=>
            a.PatientId == patientId && 
            a.AppointmentDate == appointmentDate &&
            (!appointmentId.HasValue || a.Id !=appointmentId)&&
            a.StartTime < endTime &&
            a.EndTime > startTime

            );
            return isConflicting;
        }

        public async Task UpdateAsync(Appointment appointment)
        {
            _db.Appointments.Update(appointment);
            await _db.SaveChangesAsync();
        }
    }
}
