
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

        //Pagination
        public int PageSize { get; set; } = 5;
        public int PageNumber { get; set; } = 1;
    }
}
