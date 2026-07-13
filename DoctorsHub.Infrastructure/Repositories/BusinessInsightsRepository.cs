using DoctorsHub.Application.DTOs.BusinessInsigts.AppointmentAnalyticsDto;
using DoctorsHub.Application.DTOs.BusinessInsigts.RevenueAnalyticsDto;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Domain.Enums;
using DoctorsHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


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

        public Task<List<RevenueTrendDto>> GetRevenueTrendAsync()
        {
            string[] months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
            
            return _db.Appointments.Where(a => a.Status == AppointmentStatus.Completed)
                .GroupBy(a=> new
                {
                    a.AppointmentDate.Year,a.AppointmentDate.Month
                })
                .Select(g=> new RevenueTrendDto 
                {
                    MonthYear = $"{months[g.Key.Month-1]} {g.Key.Year}",
                    Revenue = g.Sum(a=>a.Doctor.ConsultationFee)
                })
                .ToListAsync();
        }
    }
}
