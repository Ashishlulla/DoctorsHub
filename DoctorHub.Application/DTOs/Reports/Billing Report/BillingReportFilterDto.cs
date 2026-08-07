using DoctorsHub.Domain.Enums;

namespace DoctorsHub.Application.DTOs.Reports.BillingReport
{
    public class BillingReportFilterDto
    {
        //Date range for filtering the billing report
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }

        // Optional filters for doctor and patient
        public int? DoctorId { get; set; }
        public string DoctorName { get; set; } = "All";

        public int? PatientId { get; set; }

        public string PatientName { get; set; } = "All";

        // Optional filter for payment status
        public PaymentStatus? PaymentStatus { get; set; }
    }
}
