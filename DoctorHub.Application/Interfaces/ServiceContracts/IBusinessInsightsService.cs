using DoctorsHub.Application.DTOs.BusinessInsigts.AppointmentAnalyticsDto;
using DoctorsHub.Application.DTOs.BusinessInsigts.RevenueAnalyticsDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.Interfaces.ServiceContracts
{
    public interface IBusinessInsightsService
    {
        Task<List<AppointmentStatusChartDto>> GetAppointmentStatusChartAsync();

        Task<List<AppointmentTrendDto>> GetAppointmentTrendsAsync();

        Task<List<AppointmentsByDoctorDto>> GetAppointmentsByDoctorsAsync();

        Task<List<PeakAppointmentHoursDto>> GetPeakAppointmentHoursAsync();

        //Revenue Analytics Methods Interface
        Task<List<RevenueTrendDto>> GetRevenueTrendsAsync();
    }
}
