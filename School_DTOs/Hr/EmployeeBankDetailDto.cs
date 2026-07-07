using System;
namespace School_DTOs.Hr
{
    public class EmployeeBankDetailDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string BankName { get; set; } = null!; public string AccountNumber { get; set; } = null!; public string IFSC { get; set; } = null!; public string? Branch { get; set; }
    }

    public class CreateEmployeeBankDetailDto
    {
        public int EmployeeId { get; set; }
        public string BankName { get; set; } = null!; public string AccountNumber { get; set; } = null!; public string IFSC { get; set; } = null!; public string? Branch { get; set; }
    }

    public class UpdateEmployeeBankDetailDto : CreateEmployeeBankDetailDto
    {
        public int Id { get; set; }
    }
}