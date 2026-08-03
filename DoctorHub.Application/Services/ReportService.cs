using DoctorsHub.Application.DTOs.Reports.AppointmentsReport;
using DoctorsHub.Application.DTOs.Reports.BillingReport;
using DoctorsHub.Application.DTOs.Reports.DoctorsReport;
using DoctorsHub.Application.DTOs.Reports.PatientsReport;
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

        public async Task<List<BillingReportDto>> GetBillingReportsAsync(BillingReportFilterDto billingReportFilter)
        {
            return await _reportRepository.GetBillingReportsAsync(billingReportFilter); 
        }

        public async Task<List<DoctorsReportDto>> GetDoctorsReportsAsync(DoctorsReportFilteredDto doctorsReportFiltered)
        {
           return await _reportRepository.GetDoctorsReportsAsync(doctorsReportFiltered);
        }

        public async Task<List<PatientsReportDto>> GetPatientsReportsAsync(PatientsReportFilteredDto patientsReportFiltered)
        {
           return await _reportRepository.GetPatientsReportsAsync(patientsReportFiltered);
        }
    }
}
