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
                })
                .OrderBy(o=>o.Count)
                .ToListAsync();
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
                .OrderBy(o=>o.Count)
                .ToListAsync();

        }

        public async Task<List<AppointmentTrendDto>> GetAppointmentTrendAsync()
        {
            var appointments = await _db.Appointments.Select(a => a.AppointmentDate).ToListAsync();

            return   appointments.
                GroupBy(d => d.DayOfWeek).
                Select(g => new AppointmentTrendDto
                {
                    label = g.Key.ToString(),
                    Count = g.Count()
                })
                .OrderBy(o=>o.Count)
                .ToList();
        }

        public async Task<List<AverageRevenueGeneratedByDoctorDto>> GetAverageRevenueGeneratedByDoctors()
        {
            return await _db.Appointments
               .Where(a => a.Status == AppointmentStatus.Completed)
               .GroupBy(d => d.Doctor.FullName)
               .Select(g => new AverageRevenueGeneratedByDoctorDto
               {
                   DoctorName = g.Key,
                    AverageRevenue = g.Average(a => a.Doctor.ConsultationFee)
               })
               .OrderBy(o=>o.AverageRevenue)
               .ToListAsync();
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

        public async Task<List<RevenueByDoctorDto>> GetRevenueByDoctorsAsync()
        {
            return await _db.Appointments
                .Where(a=>a.Status == AppointmentStatus.Completed)
                .GroupBy(d => d.Doctor.FullName)
                .Select(g => new RevenueByDoctorDto
                {
                    DoctorName = g.Key,
                    Revenue = g.Sum(a => a.Doctor.ConsultationFee)
                })
                .OrderBy(o=>o.Revenue)
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
                .OrderBy(o=>o.Revenue)
                .ToListAsync();
        }

        public async Task<List<TopRevenueGeneratingDoctorDto>> GetTopRevenueGeneratingDoctors()
        {
            return await _db.Appointments
                .Where(a => a.Status == AppointmentStatus.Completed)
                .GroupBy(d => d.Doctor.FullName)
                .Select(g => new TopRevenueGeneratingDoctorDto
                {
                    DoctorName = g.Key,
                    RevenueGenerated = g.Sum(a => a.Doctor.ConsultationFee)
                })
                .OrderByDescending(o=>o.RevenueGenerated)
                .Take(5)
                .OrderBy(o=>o.RevenueGenerated)
                .ToListAsync();
        }
    }
}
