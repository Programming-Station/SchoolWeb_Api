namespace School_DTOs.AcademicYear
{
    public class AcademicYearDto : BaseDto
    {
        public string YearName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsCurrent { get; set; }
    }
}

