using DoctorsHub.Application.DTOs.Reports.AppointmentsReport;

namespace DoctorsHub.Application.Interfaces.ServiceContracts
{
    public interface IReportService
    {
        Task<List<AppointmentReportDto>> GetAppointmentReportsAsync(AppointmentReportFilteredDto appointmentReportFiltered);
    }
}
