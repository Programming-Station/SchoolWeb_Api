namespace School_DTOs.Hr
{
    public class EmployeeEducationDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Degree { get; set; } = null!; public string? Board { get; set; }
        public string University { get; set; } = null!; public string? PassingYear { get; set; }
        public decimal? Percentage { get; set; }
    }

    public class CreateEmployeeEducationDto
    {
        public int EmployeeId { get; set; }
        public string Degree { get; set; } = null!; public string? Board { get; set; }
        public string University { get; set; } = null!; public string? PassingYear { get; set; }
        public decimal? Percentage { get; set; }
    }

    public class UpdateEmployeeEducationDto : CreateEmployeeEducationDto
    {
        public int Id { get; set; }
    }
}