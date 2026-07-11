using DoctorsHub.Application.DTOs.BusinessInsigts;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.Interfaces.ServiceContracts
{
    public interface IBusinessInsightsService
    {
        Task<List<AppointmentStatusChartDto>> GetAppointmentStatusChart();
    }
}
