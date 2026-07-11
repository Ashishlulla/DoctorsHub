using DoctorsHub.Application.DTOs.BusinessInsigts;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.Interfaces.RepositoryContracts
{
    public interface IBusinessInsightsRepository
    {
        //Appointment Analytics Methods
        Task<List<AppointmentStatusChartDto>> GetAppointmentStatusChartAsync();
    }
}
