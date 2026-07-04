using DoctorsHub.Application.DTOs.Appoitments;
using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Domain.Entities;
using DoctorsHub.Domain.Enums;
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

        public async Task<bool> DoctorHasConflictingAppointmentAsync(int doctorId, DateOnly appointmentDate, TimeSpan startTime, TimeSpan endTime, int? appointmentId = null)
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

        public async Task<bool> PatientHasConflictingAppointmentAsync(int patientId, DateOnly appointmentDate, TimeSpan startTime, TimeSpan endTime, int? appointmentId = null)
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

        public async Task<(List<Appointment> Appointments, int TotalRecords)> GetAllAppointmentsAsync(
     AppointmentQueryParameter appointmentQueryParameter)
        {
            IQueryable<Appointment> query = _db.Appointments
                .AsNoTracking()
                .Include(d => d.Doctor)
                .Include(p => p.Patient);

            // Searching
            switch (appointmentQueryParameter.searchBy)
            {
                case "DoctorName":
                    if (!string.IsNullOrWhiteSpace(appointmentQueryParameter.searchString))
                    {
                        query = query.Where(a =>
                            EF.Functions.Like(a.Doctor.FullName, $"%{appointmentQueryParameter.searchString}%"));
                    }
                    break;

                case "PatientName":
                    if (!string.IsNullOrWhiteSpace(appointmentQueryParameter.searchString))
                    {
                        query = query.Where(a =>
                            EF.Functions.Like(a.Patient.FullName, $"%{appointmentQueryParameter.searchString}%"));
                    }
                    break;

                case "AppointmentDate":
                    if (DateOnly.TryParse(appointmentQueryParameter.searchString, out DateOnly appointmentDate))
                    {
                        query = query.Where(a => a.AppointmentDate == appointmentDate);
                    }
                    break;

                case "Status":
                    if (Enum.TryParse<AppointmentStatus>(
                        appointmentQueryParameter.searchString,
                        true,
                        out AppointmentStatus status))
                    {
                        query = query.Where(a => a.Status == status);
                    }
                    break;
            }

            // Sorting
            query = (appointmentQueryParameter.sortBy, appointmentQueryParameter.sortOrder?.ToLower()) switch
            {
                ("DoctorName", "asc") => query.OrderBy(a => a.Doctor.FullName),
                ("DoctorName", "desc") => query.OrderByDescending(a => a.Doctor.FullName),

                ("PatientName", "asc") => query.OrderBy(a => a.Patient.FullName),
                ("PatientName", "desc") => query.OrderByDescending(a => a.Patient.FullName),

                ("AppointmentDate", "asc") => query.OrderBy(a => a.AppointmentDate),
                ("AppointmentDate", "desc") => query.OrderByDescending(a => a.AppointmentDate),

                ("Status", "asc") => query.OrderBy(a => a.Status),
                ("Status", "desc") => query.OrderByDescending(a => a.Status),

                _ => query.OrderBy(a => a.AppointmentDate)
                          .ThenBy(a => a.StartTime)
            };

            // Total Records
            int totalRecords = await query.CountAsync();

            // Pagination
            List<Appointment> appointments = await query
                .Skip((appointmentQueryParameter.PageNumber - 1) * appointmentQueryParameter.PageSize)
                .Take(appointmentQueryParameter.PageSize)
                .ToListAsync();

            return (appointments, totalRecords);
        }

        public async Task ConfirmedAppointmentAync(int appointmentId)
        {
            Appointment? appointment = await _db.Appointments.FindAsync(appointmentId);

            appointment!.Status = AppointmentStatus.Confirmed;

            await _db.SaveChangesAsync();

        }

        public async Task RescheduleAppointmentAsync(RescheduleAppointmentDto rescheduleAppointmentDto)
        {
            Appointment? appointment = await _db.Appointments.FindAsync(rescheduleAppointmentDto.Id);

            appointment.AppointmentDate = rescheduleAppointmentDto.AppointmentDate;
            appointment.StartTime = rescheduleAppointmentDto.StartTime;
            appointment.EndTime = rescheduleAppointmentDto.EndTime;

            appointment.Status = AppointmentStatus.Confirmed;

            await _db.SaveChangesAsync();
        }

        public async Task CancelAppointmentAsync(int appointmentId)
        {
            Appointment? appointment = await _db.Appointments.FindAsync(appointmentId);
            appointment!.Status = AppointmentStatus.Cancelled;

            await _db.SaveChangesAsync();
        }

        public async Task CompletedAppointmentAsync(int appointmentId)
        {
            Appointment? appointment = await _db.Appointments.FindAsync(appointmentId);
            appointment!.Status = AppointmentStatus.Completed;

            await _db.SaveChangesAsync();
        }
    }
}
