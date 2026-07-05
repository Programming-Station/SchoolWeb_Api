using System;
namespace School_DTOs.Hr
{
    public class EmployeeExperienceDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Company { get; set; } = null!; public string Designation { get; set; } = null!; public DateTime JoiningDate { get; set; } public DateTime LeavingDate { get; set; } public decimal? Salary { get; set; }
    }

    public class CreateEmployeeExperienceDto
    {
        public int EmployeeId { get; set; }
        public string Company { get; set; } = null!; public string Designation { get; set; } = null!; public DateTime JoiningDate { get; set; } public DateTime LeavingDate { get; set; } public decimal? Salary { get; set; }
    }

    public class UpdateEmployeeExperienceDto : CreateEmployeeExperienceDto
    {
        public int Id { get; set; }
    }
}