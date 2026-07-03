using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.DTOs.common
{
    using System;
    using System.Collections.Generic;

    namespace DoctorsHub.Application.DTOs.Common
    {
        public class PagedResult<T>
        {
            public List<T> Items { get; set; } = new();

            public int PageSize { get; set; }

            public int PageNumber { get; set; }

            public int TotalCount { get; set; }

            public int TotalPages { get; set; }
        }
    }
}
