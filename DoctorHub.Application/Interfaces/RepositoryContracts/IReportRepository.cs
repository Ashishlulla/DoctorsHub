using DoctorsHub.Application.DTOs.Reports.AppointmentsReport;
using DoctorsHub.Application.DTOs.Reports.BillingReport;
using DoctorsHub.Application.DTOs.Reports.DoctorsReport;
using DoctorsHub.Application.DTOs.Reports.PatientsReport;

namespace DoctorsHub.Application.Interfaces.RepositoryContracts
{
    public interface IReportRepository
    {
        Task<List<AppointmentReportDto>> GetAppointmentReportsAsync(AppointmentReportFilteredDto appointmentReportFiltered);

        Task<List<BillingReportDto>> GetBillingReportsAsync(BillingReportFilterDto billingReportFilter);

        Task<List<DoctorsReportDto>> GetDoctorsReportsAsync(DoctorsReportFilteredDto doctorsReportFiltered);

        Task<List<PatientsReportDto>> GetPatientsReportsAsync(PatientsReportFilteredDto patientsReportFiltered);
    }
}
