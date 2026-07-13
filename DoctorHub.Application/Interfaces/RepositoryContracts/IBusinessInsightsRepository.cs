using DoctorsHub.Application.DTOs.BusinessInsigts.AppointmentAnalyticsDto;
using DoctorsHub.Application.DTOs.BusinessInsigts.RevenueAnalyticsDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.Interfaces.RepositoryContracts
{
    public interface IBusinessInsightsRepository
    {
        //Appointment Analytics Methods
        #region Appointment Analytics RepositoryContracts
        Task<List<AppointmentStatusChartDto>> GetAppointmentStatusChartAsync();

        Task<List<AppointmentTrendDto>> GetAppointmentTrendAsync();

        Task<List<AppointmentsByDoctorDto>> GetAppointmentsByDoctorsAsync();

        Task<List<PeakAppointmentHoursDto>> GetPeakAppointmentHoursAsync();
        #endregion

        //Appointment Analytics Methods
        #region Revenue Analytics RepositoryContracts
        Task<List<RevenueTrendDto>> GetRevenueTrendAsync();

        Task<List<RevenueByDoctorDto>> GetRevenueByDoctorsAsync();

        Task<List<TopRevenueGeneratingDoctorDto>> GetTopRevenueGeneratingDoctors();

        Task<List<AverageRevenueGeneratedByDoctorDto>> GetAverageRevenueGeneratedByDoctors();
        #endregion

    }
}
