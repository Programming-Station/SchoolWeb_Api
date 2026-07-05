using System;
namespace School_DTOs.Hr
{
    public class EmployeeDetailDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string? FatherName { get; set; } public string? MotherName { get; set; } public string? SpouseName { get; set; } public string? AadhaarNumber { get; set; } public string? PanNumber { get; set; }
    }

    public class CreateEmployeeDetailDto
    {
        public int EmployeeId { get; set; }
        public string? FatherName { get; set; } public string? MotherName { get; set; } public string? SpouseName { get; set; } public string? AadhaarNumber { get; set; } public string? PanNumber { get; set; }
    }

    public class UpdateEmployeeDetailDto : CreateEmployeeDetailDto
    {
        public int Id { get; set; }
    }
}