namespace School_DTOs.Payroll
{
    public class SalaryComponentDto{public int Id{get;set;} public string Name{get;set;}=null!; public string Type{get;set;}="Earning"; public decimal Amount{get;set;} public string Status{get;set;}="active";}
    public class CreateSalaryComponentDto{public string Name{get;set;}=null!; public string Type{get;set;}="Earning"; public decimal Amount{get;set;} public string Status{get;set;}="active";}
    public class UpdateSalaryComponentDto:CreateSalaryComponentDto{public int Id{get;set;}}
    public class PayrollRunDto{public int Id{get;set;} public int EmployeeId{get;set;} public string Month{get;set;}=null!; public decimal GrossSalary{get;set;} public decimal TotalDeductions{get;set;} public decimal NetSalary{get;set;} public string Status{get;set;}="Draft";}
    public class CreatePayrollRunDto{public int EmployeeId{get;set;} public string Month{get;set;}=null!; public decimal GrossSalary{get;set;} public decimal TotalDeductions{get;set;} public decimal NetSalary{get;set;} public string Status{get;set;}="Draft";}
    public class UpdatePayrollRunDto:CreatePayrollRunDto{public int Id{get;set;}}
}

