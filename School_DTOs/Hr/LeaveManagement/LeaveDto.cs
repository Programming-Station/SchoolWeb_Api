using System;

namespace School_DTOs.Hr.LeaveManagement
{
    public class LeaveRequestDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = null!;
        public int LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalDays { get; set; }
        public string? Reason { get; set; }
        public string Status { get; set; } = null!;
        public string? Remarks { get; set; }
    }

    public class CreateLeaveRequestDto
    {
        public int EmployeeId { get; set; }
        public int LeaveTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Reason { get; set; }
    }

    public class ApproveLeaveRequestDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = "Approved"; // Approved or Rejected
        public string? Remarks { get; set; }
    }

    public class LeaveBalanceDto
    {
        public int Id { get; set; }
        public int LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; } = null!;
        public string Year { get; set; } = null!;
        public decimal TotalLeaves { get; set; }
        public decimal UsedLeaves { get; set; }
        public decimal AvailableLeaves { get; set; }
    }
}
