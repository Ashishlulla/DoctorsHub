using DoctorsHub.Application.DTOs.CRM;


namespace DoctorsHub.Application.Interfaces.ServiceContracts
{
    public interface ICRMService
    {
        Task<DashBoardDto> GetDashBoardAsync();

        Task<List<RecentAppointmentsDto>> GetRecentAppointmentsAsync();

        Task<List<UpcomingAppointmentsDto>> GetUpcomingAppointmentsAsync();

        Task<List<TodayAppointmentsDto>> GetTodaysAppointmentAsync();

        Task<List<ScheduledAppointmentsDto>> GetScheduledAppointmentsAsync();
    }

}
