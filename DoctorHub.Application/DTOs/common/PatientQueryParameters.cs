using DoctorsHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.DTOs.common
{
    public class PatientQueryParameters
    {
        //Filtering Parameters
        public string? searchBy { get; set; } = nameof(Patient.FullName);

        public string? searchString { get; set; } = string.Empty;

        //Sorting
        public string sortBy { get; set; } = nameof(Patient.FullName);

        public string? sortOrder { get; set; } = "asc";

        //Pagination
        public int PageSize { get; set; } = 5;
        public int PageNumber { get; set; } = 1;
    }
}
