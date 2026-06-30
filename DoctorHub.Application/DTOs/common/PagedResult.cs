using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.DTOs.common
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }

        public int PageSize { get; set; }
        
        public int PageNumber { get; set; }
        
        public int TotalRecords { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);

    }
}
