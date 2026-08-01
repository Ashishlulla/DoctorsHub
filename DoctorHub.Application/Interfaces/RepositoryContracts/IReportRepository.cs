

using DoctorsHub.Application.DTOs.Reports.AppointmentsReport;

namespace DoctorsHub.Application.Interfaces.RepositoryContracts
{
    public interface IReportRepository
    {
        Task<List<AppointmentReportDto>> GetAppointmentReportsAsync(AppointmentReportFilteredDto appointmentReportFiltered);
    }
}
