using System.Net;

namespace School_DTOs
{
    public class BaseResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; } = false;
        public APIException? Error { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
    public class APIResponse<T> : BaseResponse
    {
        public T? Data { get; set; }

        public string? RequestId { get; set; }
    }
    public class APIResponse : BaseResponse
    {
        public string? RequestId { get; set; }
    }
    public class PagedResponse<T> : BaseResponse
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();
        public int TotalRecords { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalRecords / (double)PageSize);
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}

