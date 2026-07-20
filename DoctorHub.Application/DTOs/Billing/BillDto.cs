using DoctorsHub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.DTOs.Billing
{
    public class BillDto
    {
        public int Id { get; set; }

        public string DoctorName { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;

        public int AppointmentId { get; set; }

        public decimal ConsultationFee { get; set; }

        public decimal AdditionalCharges { get; set; }

        public decimal Discount { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime BillDate { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

    }
}
