using DoctorsHub.Domain.Enums;
using System.Text.Json.Serialization;


namespace DoctorsHub.Domain.Entities
{
    public class Bill
    {
        public int Id { get; set; }

        public int AppointmentId { get; set; }

        public Appointment Appointment { get; set; }

        public decimal ConsultationFee { get; set; }

        public decimal AdditionalCharges { get; set; }

        public decimal Discount { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime BillDate { get; set; }

        
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
    }
}
