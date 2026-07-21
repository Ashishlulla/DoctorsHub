
namespace DoctorsHub.Application.DTOs.Billing
{
    public class CreateBillDto
    {
        public int AppointmentId { get; set; }

        public decimal AdditionalCharges { get; set; }

        public decimal Discount { get; set; }
    }
}
