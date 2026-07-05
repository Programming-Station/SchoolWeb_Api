using System;
namespace School_DTOs.Hr
{
    public class EmployeeSalaryDetailDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public decimal Basic { get; set; } public decimal HRA { get; set; } public decimal DA { get; set; } public decimal PF { get; set; } public decimal ESI { get; set; } public decimal NetSalary { get; set; }
    }

    public class CreateEmployeeSalaryDetailDto
    {
        public int EmployeeId { get; set; }
        public decimal Basic { get; set; } public decimal HRA { get; set; } public decimal DA { get; set; } public decimal PF { get; set; } public decimal ESI { get; set; } public decimal NetSalary { get; set; }
    }

    public class UpdateEmployeeSalaryDetailDto : CreateEmployeeSalaryDetailDto
    {
        public int Id { get; set; }
    }
}