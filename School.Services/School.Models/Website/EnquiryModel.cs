using System.ComponentModel.DataAnnotations;

namespace School.Models.Website
{
    public class EnquiryModel
    {
        public int? Id { get; set; }
        public string EnquiryFromNo { get; set; } = null!;


        [Required(ErrorMessage = "Name is required")]
        [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string Name { get; set; } = null!;


        [Required(ErrorMessage = "Email is required")]
        [MaxLength(200, ErrorMessage = "Email cannot exceed 200 characters")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Mobile number is required")]
        [MaxLength(15, ErrorMessage = "Mobile number cannot exceed 15 characters")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Mobile number must be 10 digits")]
        public string Mobile { get; set; } = null!;

        [MaxLength(200, ErrorMessage = "Subject cannot exceed 200 characters")]
        public string? Subject { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [MaxLength(2000, ErrorMessage = "Message cannot exceed 2000 characters")]
        public string Message { get; set; } = null!;

        [MaxLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        public string? Address { get; set; }

        [MaxLength(100, ErrorMessage = "City cannot exceed 100 characters")]
        public string? City { get; set; }

        [MaxLength(10, ErrorMessage = "Pin code cannot exceed 10 characters")]
        [RegularExpression(@"^[0-9]{6}$", ErrorMessage = "Pin code must be 6 digits")]
        public string? PinCode { get; set; }

        public int? CourseId { get; set; }

        [MaxLength(200, ErrorMessage = "Course name cannot exceed 200 characters")]
        public string? CourseName { get; set; }

        public int StatusId { get; set; } // Will be set programmatically, default to "New" status if not provided

        [MaxLength(2000, ErrorMessage = "Admin reply cannot exceed 2000 characters")]
        public string? AdminReply { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? RepliedBy { get; set; }
    }
}



