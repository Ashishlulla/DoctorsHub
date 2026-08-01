using DoctorsHub.Application.DTOs.Reports.AppointmentsReport;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Domain.Entities;
using DoctorsHub.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DoctorsHub.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        //Private Feilds 
        private readonly ApplicationDbContext _db;

        //Constructor
        public ReportRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<AppointmentReportDto>> GetAppointmentReportsAsync(AppointmentReportFilteredDto appointmentReportFiltered)
        {
            //Create Query
            IQueryable<Appointment> query = _db.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .AsQueryable();

            //Filter by Date Range
            if (appointmentReportFiltered.FromDate != null)
            {
                query = query.Where(a => a.AppointmentDate >= appointmentReportFiltered.FromDate);
            }

            if (appointmentReportFiltered.ToDate != null)
            {
                query = query.Where(a => a.AppointmentDate <= appointmentReportFiltered.ToDate);
            }

            //Filter By Doctor Id
            if (appointmentReportFiltered.DoctorId > 0)
            {
                query = query.Where(a => a.DoctorId == appointmentReportFiltered.DoctorId);
            }

            //Execute Query
            List<AppointmentReportDto> reports = await query.Select(a=> new AppointmentReportDto
            {
                Id = a.Id,
                PatientName = a.Patient.FullName,
                DoctorName = a.Doctor.FullName,
                AppointmentDate = a.AppointmentDate,
                Status = a.Status,
            }).ToListAsync();

            return reports;
        }
    }
}
