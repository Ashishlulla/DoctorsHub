using DoctorsHub.Domain.Enums;
using System.Text.Json.Serialization;

namespace DoctorsHub.Application.DTOs.Reports.BillingReport
{
    public class BillingReportDto
    {
        public int BillId { get; set; }

        public DateOnly AppointmentDate { get; set; }
        public DateOnly BillDate { get; set; }

        public string DoctorName { get; set; } = string.Empty;

        public string PatientName { get; set; } = string.Empty;

        public decimal ConsultationFee { get; set; }

        public decimal AdditionalCharges { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentStatus PaymentStatus { get; set; }
    }
}
