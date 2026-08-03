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
using DoctorsHub.Application.DTOs.Reports.BillingReport;
using DoctorsHub.Domain.Enums;
using DoctorsHub.Application.DTOs.Reports.DoctorsReport;

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

        public async Task<List<BillingReportDto>> GetBillingReportsAsync(BillingReportFilterDto billingReportFilter)
        {
            //Creating Query
            IQueryable<Bill> query = _db.Bills
                .AsNoTracking()
                .Include(b => b.Appointment)
                    .ThenInclude(a => a.Patient)
                .Include(b => b.Appointment)
                    .ThenInclude(a => a.Doctor);

            //Date Filtering
            if (billingReportFilter.FromDate != default)
            {
                query = query.Where(b => DateOnly.FromDateTime(b.BillDate) >= billingReportFilter.FromDate);
            }

            if (billingReportFilter.ToDate != default)
            {
                query = query.Where(b => DateOnly.FromDateTime(b.BillDate) <= billingReportFilter.ToDate);
            }

            //Doctor Filtering
            if (billingReportFilter.DoctorId.HasValue && billingReportFilter.DoctorId != 0) 
            {
                query = query.Where(b=>b.Appointment.DoctorId == billingReportFilter.DoctorId.Value);
            }

            //Patient Filtering
            if (billingReportFilter.PatientId.HasValue && billingReportFilter.PatientId != 0)
            {
                query = query.Where(b => b.Appointment.PatientId == billingReportFilter.PatientId.Value);
            }

            //Payment Status Filtering
            if (billingReportFilter.PaymentStatus.HasValue)
            {
                query = query.Where(b => b.PaymentStatus == billingReportFilter.PaymentStatus.Value);
            }

            //convert IQueryable to IEnumerable

            var bills = await query.ToListAsync();

            //Mapping to DTO
            var billingReport = bills.Select(b => new BillingReportDto
            {
                BillId = b.Id,
                AppointmentDate =b.Appointment.AppointmentDate,
                BillDate = DateOnly.FromDateTime(b.BillDate),
                DoctorName = b.Appointment.Doctor.FullName,
                PatientName = b.Appointment.Patient.FullName,
                ConsultationFee = b.ConsultationFee,
                AdditionalCharges = b.AdditionalCharges,
                Discount = b.Discount,
                TotalAmount = b.TotalAmount,
                PaymentStatus = b.PaymentStatus
            }).ToList();


            return billingReport;
        }

        public async Task<List<DoctorsReportDto>> GetDoctorsReportsAsync(DoctorsReportFilteredDto doctorsReportFiltered)
        {
            // Create query
            IQueryable<Doctor> query = _db.Doctors
                .AsNoTracking()
                .Include(d => d.User)
                .Include(d => d.Specialization);

            // Filter by Specialization
            if (doctorsReportFiltered.SpecializationId.HasValue &&
                doctorsReportFiltered.SpecializationId.Value > 0)
            {
                query = query.Where(d =>
                    d.SpecializationId == doctorsReportFiltered.SpecializationId.Value);
            }

            // Filter by Qualification
            if (!string.IsNullOrWhiteSpace(doctorsReportFiltered.Qualification))
            {
                query = query.Where(d =>
                    d.Qualification.Contains(doctorsReportFiltered.Qualification));
            }

            // Project to DTO
            return await query
                .Select(d => new DoctorsReportDto
                {
                    DoctorName = d.FullName,
                    Email = d.User.Email!,
                    PhoneNumber = d.PhoneNumber,
                    Qualification = d.Qualification,
                    Specialization = d.Specialization.Name,
                    Experience = d.ExperienceYears,
                    ConsultationFee = d.ConsultationFee
                })
                .ToListAsync();
        }
    }
}
