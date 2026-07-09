using DoctorsHub.Application.DTOs.CRM;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.Interfaces.ServiceContracts
{
    public interface ICRMService
    {
        Task<DashBoardDto> GetDashBoardAsync();

        Task<List<RecentAppointmentsDto>> GetRecentAppointmentsAsync();

        Task<List<UpcomingAppointmentsDto>> GetUpcomingAppointmentsAsync();

        Task<List<TodayAppointmentsDto>> GetTodaysAppointmentAsync();
    }

}
