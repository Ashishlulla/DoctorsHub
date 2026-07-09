using DoctorsHub.Application.DTOs.Appoitments;
using DoctorsHub.Application.DTOs.CRM;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Domain.Entities;
using DoctorsHub.Domain.Enums;
using DoctorsHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Infrastructure.Repositories
{
    public class CRMRepository : ICRMRepository
    {
        //PrivateFeild
        private readonly ApplicationDbContext _db;
         

        //Constructor
        public CRMRepository(ApplicationDbContext db) 
        {
            _db = db;
        }

        public async Task<DashBoardDto> GetDashBoardAsync()
        {
            return new DashBoardDto 
            {
                TotalDoctors = await _db.Doctors.CountAsync(),
                TotalPatients = await _db.Patients.CountAsync(),
                TotalAppointments = await _db.Appointments.CountAsync(),
                TotalRevenue = await _db.Appointments.Where(x=>x.Status == AppointmentStatus.Completed).SumAsync(c=>c.Doctor.ConsultationFee),
                CancelledAppointments = await _db.Appointments.CountAsync(c=>c.Status==AppointmentStatus.Cancelled),
                CompletedAppointments = await _db.Appointments.CountAsync(c=>c.Status == AppointmentStatus.Completed),
                AverageDoctorRating = await _db.Doctors.AverageAsync(d=>d.AverageRating),
                ScheduleAppointments = await _db.Appointments.CountAsync(s=>s.Status ==AppointmentStatus.Scheduled)
            };
        }

        public async Task<List<Appointment>> GetRecentAppointmentsAsync()
        {
            List<Appointment> recentAppointments = await _db.Appointments.Include(p=>p.Patient).Include(d=>d.Doctor).Where(a => a.Status == AppointmentStatus.Completed).Take(5).ToListAsync();

            return recentAppointments;
        }

        public async Task<List<Appointment>> GetTodaysAppointmentsAsync()
        {
            return await _db.Appointments.Include(p => p.Patient).Include(d => d.Doctor).Where(a => a.AppointmentDate == DateOnly.FromDateTime(DateTime.Today)).ToListAsync();
        }

        public async Task<List<Appointment>> GetUpcomingAppointmentsAsync()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today)
                ;
            return await _db.Appointments
                .Include(d => d.Doctor)
                .Include(p => p.Patient)
                .Where(a => a.AppointmentDate >= today 
                && a.Status !=AppointmentStatus.Completed 
                && a.Status !=AppointmentStatus.Cancelled)
                .OrderBy(a=>a.AppointmentDate)
                .ThenBy(a=>a.StartTime)
                .ToListAsync();
        }
    }
}
