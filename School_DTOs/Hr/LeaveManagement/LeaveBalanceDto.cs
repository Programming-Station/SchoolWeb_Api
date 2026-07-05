using System;
namespace School_DTOs.Hr.LeaveManagement
{
    public class LeaveBalanceDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int LeaveTypeId { get; set; } public string Year { get; set; } = null!; public decimal TotalLeaves { get; set; } = 0; public decimal UsedLeaves { get; set; } = 0; public decimal AvailableLeaves { get; set; } = 0;
    }

    public class CreateLeaveBalanceDto
    {
        public int EmployeeId { get; set; }
        public int LeaveTypeId { get; set; } public string Year { get; set; } = null!; public decimal TotalLeaves { get; set; } = 0; public decimal UsedLeaves { get; set; } = 0; public decimal AvailableLeaves { get; set; } = 0;
    }

    public class UpdateLeaveBalanceDto : CreateLeaveBalanceDto
    {
        public int Id { get; set; }
    }
}