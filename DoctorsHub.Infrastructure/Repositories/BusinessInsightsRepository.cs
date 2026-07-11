using DoctorsHub.Application.DTOs.BusinessInsigts;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Infrastructure.Repositories
{
    public class BusinessInsightsRepository : IBusinessInsightsRepository
    {
        //Private Feilds 
        private readonly ApplicationDbContext _db;

        //Constructor
        public BusinessInsightsRepository(ApplicationDbContext db) 
        {
            _db = db;
        }

        public async Task<List<AppointmentsByDoctorDto>> GetAppointmentsByDoctorsAsync()
        {
            return await _db.Appointments
                .Include(d => d.Doctor)
                .GroupBy(d => d.Doctor.FullName)
                .Select(g => new AppointmentsByDoctorDto 
                {
                    DoctorName = g.Key,
                    Count = g.Count()
                }).ToListAsync();
        }

        public async Task<List<AppointmentStatusChartDto>> GetAppointmentStatusChartAsync()
        {
            return await _db.Appointments
                .GroupBy(a => a.Status)
                .Select(g => new AppointmentStatusChartDto
                    {
                        Status = g.Key,
                        Count = g.Count()
                    }
                )
                .ToListAsync();

        }

        public async Task<List<AppointmentTrendDto>> GetAppointmentTrendAsync()
        {
            var appointments = await _db.Appointments.Select(a => a.AppointmentDate).ToListAsync();

            return  appointments.
                GroupBy(d => d.DayOfWeek).
                Select(g => new AppointmentTrendDto
                {
                    label = g.Key.ToString(),
                    Count = g.Count()
                }).ToList();
        }

        public async Task<List<PeakAppointmentHoursDto>> GetPeakAppointmentHoursAsync()
        {
            return await _db.Appointments
                .GroupBy(a => a.StartTime.Hours)
                .Select(g => new PeakAppointmentHoursDto
                {
                    Hour = g.Key,
                    Count = g.Count()

                })
                .ToListAsync();
        }
    }
}
