using DoctorsHub.Application.DTOs.Reports.AppointmentsReport;
using DoctorsHub.Application.DTOs.Reports.BillingReport;

namespace DoctorsHub.Application.Interfaces.ServiceContracts
{
    public interface IReportService
    {
        Task<List<AppointmentReportDto>> GetAppointmentReportsAsync(AppointmentReportFilteredDto appointmentReportFiltered);

        Task<List<BillingReportDto>> GetBillingReportsAsync(BillingReportFilterDto billingReportFilter);
    }
}
