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
        Task<List<AppointmentStatusChartDto>> GetAppointmentStatusChartAsync();

        Task<List<AppointmentTrendDto>> GetAppointmentTrendAsync();

        Task<List<AppointmentsByDoctorDto>> GetAppointmentsByDoctorsAsync();

        Task<List<PeakAppointmentHoursDto>> GetPeakAppointmentHoursAsync();

        //Appointment Analytics Methods
        Task<List<RevenueTrendDto>> GetRevenueTrendAsync();
    }
}
