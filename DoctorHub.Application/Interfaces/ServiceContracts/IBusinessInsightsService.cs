using DoctorsHub.Application.DTOs.BusinessInsigts.AppointmentAnalyticsDto;
using DoctorsHub.Application.DTOs.BusinessInsigts.RevenueAnalyticsDto;
using DoctorsHub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.Interfaces.ServiceContracts
{
    public interface IBusinessInsightsService
    {
        //Appointment Analytics Sevice Contracts
        #region Appointment Analytics Service Contracts
        Task<List<AppointmentStatusChartDto>> GetAppointmentStatusChartAsync(AnalyticsTimeFilter timeFilter);

        Task<List<AppointmentTrendDto>> GetAppointmentTrendsAsync();

        Task<List<AppointmentsByDoctorDto>> GetAppointmentsByDoctorsAsync();

        Task<List<PeakAppointmentHoursDto>> GetPeakAppointmentHoursAsync();
        #endregion


        //Revenue Analytics Methods Interface
        #region Revenue Analytics Service Contracts
        
        Task<List<RevenueTrendDto>> GetRevenueTrendsAsync();
        
        Task<List<RevenueByDoctorDto>> GetRevenueByDoctorsAsync();

        Task<List<TopRevenueGeneratingDoctorDto>> GetTopRevenueGeneratingDoctorsAsync();

        Task<List<AverageRevenueGeneratedByDoctorDto>> GetAverageRevenueGeneratedByDoctorsAsync();

        #endregion
    }
}
