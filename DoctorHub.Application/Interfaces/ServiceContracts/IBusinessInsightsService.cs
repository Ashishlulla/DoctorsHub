using DoctorsHub.Application.DTOs.BusinessInsigts;
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
        Task<BusinessInsightsDto> GetBusinessInsightsAsync(AnalyticsTimeFilter timeFilter);
    }
}
