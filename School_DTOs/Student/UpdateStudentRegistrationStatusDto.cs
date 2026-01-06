namespace School_DTOs.Student
{
    public class UpdateStudentRegistrationStatusDto
    {
        public int Id { get; set; }
        public string RegistrationStatus { get; set; } = string.Empty; // pending, approved, rejected, under_review
        public string? Remarks { get; set; }
    }
}

