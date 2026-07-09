using DoctorsHub.Application.DTOs.CRM;
using DoctorsHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.Interfaces.RepositoryContracts
{
    public interface ICRMRepository
    {
        Task<DashBoardDto> GetDashBoardAsync();
        Task<List<Appointment>> GetRecentAppointmentsAsync();
        Task<List<Appointment>> GetUpcomingAppointmentsAsync();
        Task<List<Appointment>> GetTodaysAppointmentsAsync();
    }
}
