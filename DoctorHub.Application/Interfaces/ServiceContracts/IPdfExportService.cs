using DoctorsHub.Application.DTOs.Billing;

namespace DoctorsHub.Application.Interfaces.ServiceContracts
{
    public interface IPdfExportService
    {
        byte[] GenerateBillPdf(BillDto bill);
    }
}
