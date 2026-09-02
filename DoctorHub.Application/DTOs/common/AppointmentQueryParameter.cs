

namespace DoctorsHub.Application.DTOs.common
{
    public class AppointmentQueryParameter
    {
        //Searching and Filtering Properties
        public string? searchBy { get; set; }
        public string? searchString { get; set; }

        public string? appointmentStatus { get; set; }

        //Sorting Properties
        public string sortBy { get; set; } = "AppointmentDate";
        public string sortOrder { get; set; } = "asc";

        //Pagination Properties
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }
}
