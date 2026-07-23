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
        private List<AverageRevenueGeneratedByDoctorDto> avergeRevenueByDoctorTask;

        //Constructor
        public BusinessInsightsRepository(ApplicationDbContext db) 
        {
            _db = db;
        }


        public async Task<BusinessInsightsDto> GetBusinessInsightsAsync(AnalyticsTimeFilter timeFilter)
        {
            IQueryable<Appointment> appointments = GetFilteredAppointments(timeFilter);

            // Appointment Analytics methods call

            var appointmentStatusTask = await GetAppointmentStatusChartAsync(appointments);
            var appointmentsTrendTask = await GetAppointmentTrendAsync(appointments);
            var appointmentByDoctorTask = await GetAppointmentsByDoctorsAsync(appointments);
            var appointmentPeakHoursTask = await GetPeakAppointmentHoursAsync(appointments);

            //Revenue Analytics Methods calls
            

            IQueryable<Bill> paidBills = _db.Bills.Where(b => b.PaymentStatus == PaymentStatus.Paid);

            var revenueTrendTask = await GetRevenueTrendAsync(paidBills);

            var revenueByDoctorTask = await GetRevenueByDoctorsAsync(paidBills);

            var topRevenueByDoctorsTask = await GetTopRevenueGeneratingDoctors(paidBills);

            var avergeRevenueByDoctorTask = await GetAverageRevenueGeneratedByDoctors(paidBills);



            //Appoitments Analytics Methods
            var appointmentStatus =  appointmentStatusTask;
            var appointmentsTrend =  appointmentsTrendTask;
            var appointmentByDoctor =  appointmentByDoctorTask;
            var appointmentPeakHour =  appointmentPeakHoursTask;

            //Revenue Analytics Methods 
            var revenueTrend =  revenueTrendTask;
            var revenueByDoctor =  revenueByDoctorTask;
            var topRevenueByDoctor =  topRevenueByDoctorsTask;
            var averageRevenueByDoctor = avergeRevenueByDoctorTask;

            return new BusinessInsightsDto
            {
                //Appointments
                GetAppointmentStatuses = appointmentStatus,
                GetAppointmentsTrend = appointmentsTrend,
                GetAppointmentsByDoctors = appointmentByDoctor,
                GetPeakAppointmentsHours = appointmentPeakHour,

                //Revenue
                GetRevenueTrends = revenueTrend,
                GetRevenueByDoctors = revenueByDoctor,
                GetTopRevenueGeneratingDoctors = topRevenueByDoctor,
                GetAverageRevenueGeneratedByDoctors = averageRevenueByDoctor,
            };
        }


        private IQueryable<Appointment> GetFilteredAppointments(AnalyticsTimeFilter timeFilter)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            return timeFilter switch
            {
                AnalyticsTimeFilter.Today => _db.Appointments.Where(a => a.AppointmentDate == today),
                AnalyticsTimeFilter.Week => _db.Appointments.Where(a => a.AppointmentDate >= today.AddDays(-7)),
                AnalyticsTimeFilter.Month => _db.Appointments.Where(a => a.AppointmentDate >= today.AddMonths(-1)),
                AnalyticsTimeFilter.Quarter => _db.Appointments.Where(a => a.AppointmentDate >= today.AddMonths(-3)),
                AnalyticsTimeFilter.SixMonths => _db.Appointments.Where(a => a.AppointmentDate >= today.AddMonths(-6)),

                AnalyticsTimeFilter.Year => _db.Appointments.Where(a => a.AppointmentDate >= today.AddYears(-1)),

                _ => _db.Appointments
            };
        }

        private async Task<List<AppointmentsByDoctorDto>> GetAppointmentsByDoctorsAsync(IQueryable<Appointment> appointments)
        {
            
            return await appointments
                .Include(d => d.Doctor)
                .GroupBy(d => d.Doctor.FullName)
                .Select(g => new AppointmentsByDoctorDto 
                {
                    DoctorName = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(o=>o.Count)
                .ToListAsync();
        }

        private async Task<List<AppointmentStatusChartDto>> GetAppointmentStatusChartAsync(IQueryable<Appointment> appointments)
        { 
            return  await appointments
                .GroupBy(a => a.Status)
                .Select(g => new AppointmentStatusChartDto
                    {
                        Status = g.Key,
                        Count = g.Count()
                    }
                )
                .OrderByDescending(o=>o.Count)
                .ToListAsync();
        }

        private async Task<List<AppointmentTrendDto>> GetAppointmentTrendAsync(IQueryable<Appointment> appointments)
        {
            var appointmentDates = await appointments.Select(a => a.AppointmentDate).ToListAsync();

            return  appointmentDates.
                GroupBy(d => d.DayOfWeek)
                .OrderBy(g => g.Key)
                .Select(g => new AppointmentTrendDto
                {
                    label = g.Key.ToString(),
                    Count = g.Count()
                })
                .OrderByDescending(o=>o.Count)
                .ToList();
        }

        private async Task<List<AverageRevenueGeneratedByDoctorDto>> GetAverageRevenueGeneratedByDoctors(IQueryable<Bill> bills)
        {
            return await bills
               
               .GroupBy(b => b.Appointment.Doctor.FullName)
               .Select(g => new AverageRevenueGeneratedByDoctorDto
               {
                   DoctorName = g.Key,
                    AverageRevenue = g.Average(b=>b.TotalAmount)
               })
               .OrderByDescending(o=>o.AverageRevenue)
               .ToListAsync();
        }

        

        private async Task<List<PeakAppointmentHoursDto>> GetPeakAppointmentHoursAsync(IQueryable<Appointment> appointments)
        {
            return await appointments
                .GroupBy(a => a.StartTime.Hours)
                .Select(g => new PeakAppointmentHoursDto
                {
                    Hour = g.Key,
                    Count = g.Count()

                })
                .OrderByDescending(o=>o.Count)
                .ToListAsync();
        }

        private async Task<List<RevenueByDoctorDto>> GetRevenueByDoctorsAsync(IQueryable<Bill> bills)
        {
            return await bills
                .Where(b=>b.PaymentStatus == PaymentStatus.Paid)
                .GroupBy(d => d.Appointment.Doctor.FullName)
                .Select(g => new RevenueByDoctorDto
                {
                    DoctorName = g.Key,
                    Revenue = g.Sum(b=>b.TotalAmount)
                })
                .OrderByDescending(o=>o.Revenue)
                .ToListAsync();
        }

        private Task<List<RevenueTrendDto>> GetRevenueTrendAsync(IQueryable<Bill> bills)
        {
            string[] months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
            
            return bills
                .Where(b=>b.PaymentStatus == PaymentStatus.Paid)
                .GroupBy(b=> new
                {
                    b.BillDate.Year,b.BillDate.Month
                })
                .OrderBy(g=>g.Key.Year)
                .ThenBy(g=>g.Key.Month)
                .Select(g=> new RevenueTrendDto 
                {
                    MonthYear = $"{months[g.Key.Month-1]} {g.Key.Year}",
                    Revenue = g.Sum(b => b.TotalAmount)
                })
                
                .ToListAsync();
        }

        private async Task<List<TopRevenueGeneratingDoctorDto>> GetTopRevenueGeneratingDoctors(IQueryable<Bill> bills)
        {
            return await bills
                .GroupBy(d => d.Appointment.Doctor.FullName)
                .Select(g => new TopRevenueGeneratingDoctorDto
                {
                    DoctorName = g.Key,
                    RevenueGenerated = g.Sum(b=>b.TotalAmount)
                })
                .OrderByDescending(o=>o.RevenueGenerated)
                .Take(5)
                .ToListAsync();
        }
    }
}

