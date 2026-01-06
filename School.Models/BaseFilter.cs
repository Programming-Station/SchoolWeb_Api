using System.ComponentModel.DataAnnotations;

namespace School.Models
{
    /// <summary>
    /// Base filter model for common filtering, pagination, sorting, and search operations
    /// </summary>
    public class BaseFilter
    {
        /// <summary>
        /// Page number (1-based). Default: 1
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Page index must be greater than 0")]
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// Number of records per page. Default: 10, Max: 100
        /// </summary>
        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Search term for filtering records
        /// </summary>
        public string? Search { get; set; }

        /// <summary>
        /// Column name for sorting. Default: null (no sorting)
        /// </summary>
        public string? OrderBy { get; set; }

        /// <summary>
        /// Sort direction: "asc" or "desc". Default: "asc"
        /// </summary>
        public string? SortDirection { get; set; } = "asc";

        /// <summary>
        /// Start date for date range filtering
        /// </summary>
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// End date for date range filtering
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Filter by status (Active/Inactive). Null means all
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Filter by deleted status. Default: false (exclude deleted)
        /// </summary>
        public bool? IsDeleted { get; set; } = false;

        /// <summary>
        /// Validate and normalize filter values
        /// </summary>
        public virtual void Validate()
        {
            if (PageIndex < 1) PageIndex = 1;
            if (PageSize < 1) PageSize = 10;
            if (PageSize > 100) PageSize = 100;
            if (string.IsNullOrWhiteSpace(SortDirection))
                SortDirection = "asc";
            else
                SortDirection = SortDirection.ToLower() == "desc" ? "desc" : "asc";

            // Validate date range
            if (FromDate.HasValue && ToDate.HasValue && FromDate > ToDate)
            {
                var temp = FromDate;
                FromDate = ToDate;
                ToDate = temp;
            }
        }
    }
}

