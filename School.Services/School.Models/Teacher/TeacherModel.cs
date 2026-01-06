using System.ComponentModel.DataAnnotations;

namespace School.Models.Teacher
{
    public class TeacherModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [MaxLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
        public string LastName { get; set; } = null!;

        [MaxLength(20, ErrorMessage = "Teacher ID cannot exceed 20 characters")]
        public string? TeacherId { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Email { get; set; } = null!;

        [MaxLength(15, ErrorMessage = "Phone number cannot exceed 15 characters")]
        public string? PhoneNumber { get; set; }

        [MaxLength(15, ErrorMessage = "Alternate phone number cannot exceed 15 characters")]
        public string? AlternatePhoneNumber { get; set; }

        [MaxLength(10, ErrorMessage = "Gender cannot exceed 10 characters")]
        public string? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "State is required")]
        public int StateId { get; set; }

        [MaxLength(10, ErrorMessage = "Pin code cannot exceed 10 characters")]
        public string? PinCode { get; set; }

        [MaxLength(200, ErrorMessage = "Qualification cannot exceed 200 characters")]
        public string? Qualification { get; set; }

        [MaxLength(200, ErrorMessage = "Specialization cannot exceed 200 characters")]
        public string? Specialization { get; set; }

        [MaxLength(100, ErrorMessage = "Experience cannot exceed 100 characters")]
        public string? Experience { get; set; }

        [Required(ErrorMessage = "Faculty is required")]
        public int FacultyId { get; set; }

        [MaxLength(200, ErrorMessage = "Department cannot exceed 200 characters")]
        public string? Department { get; set; }

        [Required(ErrorMessage = "Course is required")]
        public int CourseId { get; set; }

        [MaxLength(500, ErrorMessage = "Profile photo URL cannot exceed 500 characters")]
        public string? ProfilePhotoUrl { get; set; }

        [MaxLength(1000, ErrorMessage = "Bio cannot exceed 1000 characters")]
        public string? Bio { get; set; }

        [MaxLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
        public string Status { get; set; } = "active";

        public DateTime? JoiningDate { get; set; }

        [MaxLength(500, ErrorMessage = "Remarks cannot exceed 500 characters")]
        public string? Remarks { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}

