
namespace DoctorsHub.Application.DTOs.common
{
    public class BillingQueryParameter
    {
        //searching
        public string searchBy { get; set; } = "PatientName";
        public string? searchString { get; set; } = "";

        //Sorting
        public string sortBy { get; set; } = "AppointmentId";
        public string? sortOrder { get; set; } = "asc";

        //PaymentStatus
        public string? paymentStatus { get; set; }

        //Pagination
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }
}
