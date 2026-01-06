namespace School_DTOs.Teacher
{
    public class TeacherDto : BaseDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? TeacherId { get; set; }
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? AlternatePhoneNumber { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public int CityId { get; set; }
        public string? CityName { get; set; }
        public int StateId { get; set; }
        public string? StateName { get; set; }
        public string? PinCode { get; set; }
        public string? Qualification { get; set; }
        public string? Specialization { get; set; }
        public string? Experience { get; set; }
        public string? Faculty { get; set; }
        public string? Department { get; set; }
        public int CourseId { get; set; }
        public string? CourseName { get; set; }
        public string? ProfilePhotoUrl { get; set; }
        public string? Bio { get; set; }
        public string Status { get; set; } = "active";
        public DateTime? JoiningDate { get; set; }
        public string? Remarks { get; set; }
    }
}

