using DoctorsHub.Application.DTOs.BusinessInsigts;
using DoctorsHub.Application.DTOs.BusinessInsigts.AppointmentAnalyticsDto;
using DoctorsHub.Application.DTOs.BusinessInsigts.RevenueAnalyticsDto;
using DoctorsHub.Domain.Entities;
using DoctorsHub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.Interfaces.RepositoryContracts
{
    public interface IBusinessInsightsRepository
    {


        //Appointment Analytics Methods
        #region Appointment Analytics RepositoryContracts
        Task<List<AppointmentStatusChartDto>> GetAppointmentStatusChartAsync(IQueryable<Appointment> appointments);

       
        Task<List<AppointmentTrendDto>> GetAppointmentTrendAsync(IQueryable<Appointment> appointments);

        Task<List<AppointmentsByDoctorDto>> GetAppointmentsByDoctorsAsync(IQueryable<Appointment> appointments);

        Task<List<PeakAppointmentHoursDto>> GetPeakAppointmentHoursAsync(IQueryable<Appointment> appointments);
        #endregion

        //Appointment Analytics Methods
        #region Revenue Analytics RepositoryContracts
        Task<List<RevenueTrendDto>> GetRevenueTrendAsync(IQueryable<Appointment> appointments);

        Task<List<RevenueByDoctorDto>> GetRevenueByDoctorsAsync(IQueryable<Appointment> appointments);

        Task<List<TopRevenueGeneratingDoctorDto>> GetTopRevenueGeneratingDoctors(IQueryable<Appointment> appointments);

        Task<List<AverageRevenueGeneratedByDoctorDto>> GetAverageRevenueGeneratedByDoctors(IQueryable<Appointment> appointments);
        #endregion

        //All in Method
        Task<BusinessInsightsDto> GetbusinessInsightsAsync(AnalyticsTimeFilter timeFilter);

    }
}
