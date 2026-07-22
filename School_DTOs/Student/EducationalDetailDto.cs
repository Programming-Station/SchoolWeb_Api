namespace School_DTOs.Student
{
    public class EducationalDetailDto
    {
        public int Id { get; set; }
        public int StudentRegistrationId { get; set; }
        public string ExamName { get; set; } = null!;
        public string? PassingYear { get; set; }
        public string? InstituteName { get; set; }
        public string? InstituteAddress { get; set; }
        public decimal? TotalMarks { get; set; }
        public decimal? ObtainedMarks { get; set; }
        public string? Certificate { get; set; }

    }
}

