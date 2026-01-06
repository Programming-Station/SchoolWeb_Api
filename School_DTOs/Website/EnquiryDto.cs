namespace School_DTOs.Website
{
    public class EnquiryDto
    {
        public int Id { get; set; }
        public string EnquiryFromNo { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Mobile { get; set; } = null!;
        public string? Subject { get; set; }
        public string Message { get; set; } = null!;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? PinCode { get; set; }
        public int? CourseId { get; set; }
        public string? CourseName { get; set; }
        public int StatusId { get; set; }
        public string? StatusName { get; set; }
        public string? AdminReply { get; set; }
        public DateTime? RepliedDate { get; set; }
        public string? RepliedBy { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}



