using DoctorsHub.Application.DTOs.BusinessInsigts;
using DoctorsHub.Application.DTOs.BusinessInsigts.AppointmentAnalyticsDto;
using DoctorsHub.Application.DTOs.BusinessInsigts.RevenueAnalyticsDto;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Domain.Entities;
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

        private IQueryable<Appointment> GetFilteredAppointments(AnalyticsTimeFilter timeFilter)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            return timeFilter switch
            {
                AnalyticsTimeFilter.Today => _db.Appointments.Where(a => a.AppointmentDate == today),
                AnalyticsTimeFilter.Week => _db.Appointments.Where(a => a.AppointmentDate >= today.AddDays(-7)),
                AnalyticsTimeFilter.Month => _db.Appointments.Where(a => a.AppointmentDate == today.AddMonths(-1)),
                AnalyticsTimeFilter.Quarter => _db.Appointments.Where(a => a.AppointmentDate == today.AddMonths(-3)),
                AnalyticsTimeFilter.SixMonths => _db.Appointments.Where(a => a.AppointmentDate == today.AddMonths(-6)),

                AnalyticsTimeFilter.Year => _db.Appointments.Where(a => a.AppointmentDate == today.AddYears(-1)),

                _ => _db.Appointments
            };
        }

        public async Task<List<AppointmentsByDoctorDto>> GetAppointmentsByDoctorsAsync(IQueryable<Appointment> appointments)
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

        public async Task<List<AppointmentStatusChartDto>> GetAppointmentStatusChartAsync(IQueryable<Appointment> appointments)
        {

            
            return  appointments
                .GroupBy(a => a.Status)
                .Select(g => new AppointmentStatusChartDto
                    {
                        Status = g.Key,
                        Count = g.Count()
                    }
                )
                .OrderBy(o=>o.Count)
                .ToList();

        }

        public async Task<List<AppointmentTrendDto>> GetAppointmentTrendAsync(IQueryable<Appointment> appointments)
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

        public async Task<List<AverageRevenueGeneratedByDoctorDto>> GetAverageRevenueGeneratedByDoctors(IQueryable<Appointment> appointments)
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

        

        public async Task<List<PeakAppointmentHoursDto>> GetPeakAppointmentHoursAsync(IQueryable<Appointment> appointments)
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

        public async Task<List<RevenueByDoctorDto>> GetRevenueByDoctorsAsync(IQueryable<Appointment> appointments)
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

        public Task<List<RevenueTrendDto>> GetRevenueTrendAsync(IQueryable<Appointment> appointments)
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

        public async Task<List<TopRevenueGeneratingDoctorDto>> GetTopRevenueGeneratingDoctors(IQueryable<Appointment> appointments)
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

        

        public async Task<BusinessInsightsDto> GetbusinessInsightsAsync(AnalyticsTimeFilter timeFilter)
        {
            IQueryable<Appointment> appointments = GetFilteredAppointments(timeFilter);

            // Appointment Analytics methods call

            var appointmentStatusTask = GetAppointmentStatusChartAsync(appointments);
            var appointmentsTrendTask = GetAppointmentTrendAsync(appointments);
            var appointmentByDoctorTask = GetAppointmentsByDoctorsAsync(appointments);
            var appointmentPeakHoursTask = GetPeakAppointmentHoursAsync(appointments);

            //Revenue Analytics Methods calls

            IQueryable<Appointment> completedAppointments = appointments.Where(a=>a.Status == AppointmentStatus.Completed);

            var revenueTrendTask = GetRevenueTrendAsync(completedAppointments);
            var revenueByDoctorTask = GetRevenueByDoctorsAsync(completedAppointments);
            var topRevenueByDoctorsTask = GetTopRevenueGeneratingDoctors(completedAppointments);
            var avergeRevenueByDoctorTask = GetAverageRevenueGeneratedByDoctors(completedAppointments);

            await Task.WhenAll(appointmentStatusTask, appointmentsTrendTask, appointmentByDoctorTask, appointmentPeakHoursTask, revenueTrendTask, revenueByDoctorTask, topRevenueByDoctorsTask, avergeRevenueByDoctorTask);

            //Appoitments Analytics Methods
            var appointmentStatus = await appointmentStatusTask;
            var appointmentsTrend = await appointmentsTrendTask;
            var appointmentByDoctor = await appointmentByDoctorTask;
            var appointmentPeakHour = await appointtask



            //return new BusinessInsightsDto
            //{
            //    GetAppointmentStatuses = 
            //};


        }
    }
}

