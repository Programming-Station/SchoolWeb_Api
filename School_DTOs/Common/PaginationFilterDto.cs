using System.Collections.Generic;

namespace School_DTOs.Common
{
    public class PaginationFilterDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchText { get; set; }
        public string? SortBy { get; set; }
        public string SortDirection { get; set; } = "asc"; // "asc" or "desc"
    }
}
