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
            //Creating Query
            IQueryable<Appointment> query = _db.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor);

            //Date Filtering
            if (appointmentReportFiltered.FromDate != default) 
            {
                query = query.Where(a => a.AppointmentDate >= appointmentReportFiltered.FromDate);
            }

            if (appointmentReportFiltered.ToDate != default) 
            {
                query = query.Where(a => a.AppointmentDate <= appointmentReportFiltered.ToDate);
            }


            //Doctor Filtering
            if (appointmentReportFiltered.DoctorId.HasValue && appointmentReportFiltered.DoctorId != 0) 
            {
                query = query.Where(a => a.DoctorId == appointmentReportFiltered.DoctorId.Value);
            }

            //Patient Filtering
            if (appointmentReportFiltered.PatientId.HasValue && appointmentReportFiltered.PatientId != 0) 
            {
                query = query.Where(a => a.PatientId == appointmentReportFiltered.PatientId.Value);
            }

           //Status Filterig 
           if (appointmentReportFiltered.Status.HasValue)
           {
               query = query.Where(a => a.Status == appointmentReportFiltered.Status);
           }

           //Convert IQueryable to IEnumerable

           var appaointments = await query.ToListAsync();

            //Mapping to DTO
            var appointmentReport = appaointments.Select(a => new AppointmentReportDto
            {
                Id = a.Id,
                PatientName = a.Patient.FullName,
                DoctorName = a.Doctor.FullName,
                AppointmentDate = a.AppointmentDate,
                StartTime = a.StartTime,
                EndTime = a.EndTime,
                Status = a.Status
            }).ToList();

            return appointmentReport;
        }
    }
}
