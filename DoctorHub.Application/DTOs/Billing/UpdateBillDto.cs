using DoctorsHub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.DTOs.Billing
{
    public class UpdateBillDto
    {
        public decimal ConsultationFee { get; set; }

        public decimal AdditionalCharges { get; set; }

        public decimal Discount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}
