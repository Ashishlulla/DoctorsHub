using DoctorsHub.Application.DTOs.CRM;
using DoctorsHub.Domain.Entities;


namespace DoctorsHub.Application.Interfaces.RepositoryContracts
{
    public interface ICRMRepository
    {
        
        Task<DashBoardDto> GetDashBoardAsync();
        

        Task<List<Appointment>> GetRecentAppointmentsAsync();
        

        Task<List<Appointment>> GetUpcomingAppointmentsAsync();
        

        Task<List<Appointment>> GetTodaysAppointmentsAsync();

        Task<List<Appointment>> GetScheduledApointmentsAsync();

        
        Task<List<AppointmentStatusChartDto>> GetAppointmentsStatusChartAsync();

        
        Task<List<MonthlyAppointmentChartDto>> GetMonthlyAppointmentChartAsync();


        Task<List<AppointmentsByDoctorDto>> GetappointmentsByDoctorsAsync();
    }
}
