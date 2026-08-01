using DoctorsHub.Application.DTOs.Reports.AppointmentsReport;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Application.Interfaces.ServiceContracts;


namespace DoctorsHub.Application.Services
{
    public class ReportService : IReportService
    {
        //Private Feilds
        private readonly IReportRepository _reportRepository;

        //Constructor
        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<List<AppointmentReportDto>> GetAppointmentReportsAsync(AppointmentReportFilteredDto appointmentReportFiltered)
        {
            return await _reportRepository.GetAppointmentReportsAsync(appointmentReportFiltered);
        }
    }
}
