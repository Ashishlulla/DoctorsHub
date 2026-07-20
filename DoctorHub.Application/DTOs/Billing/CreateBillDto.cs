using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.DTOs.Billing
{
    public class CreateBillDto
    {
        public int AppointmentId { get; set; }

        public decimal ConsultationFee { get; set; }

        public decimal AdditionalCharges { get; set; }

        public decimal Discount { get; set; }
    }
}
