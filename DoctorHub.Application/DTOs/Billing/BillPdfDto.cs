using DoctorsHub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.DTOs.Billing
{
    public class BillPdfDto
    {
        public int BillId { get; set; }
        public DateOnly BillDate { get; set; }
        public DateOnly AppointmentDate { get; set; }

        public string PatientName { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;

        public decimal ConsultationFee { get; set; }
        public decimal AdditionalCharges { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }

        public PaymentStatus PaymentStatus { get; set; }
    }
}
