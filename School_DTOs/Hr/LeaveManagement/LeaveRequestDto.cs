namespace School_DTOs.Hr.LeaveManagement
{
    public class LeaveRequestDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int LeaveTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalDays { get; set; }
        public string? Reason { get; set; }
        public string Status { get; set; } = "Pending"; public int? ApprovedById { get; set; }
        public string? Remarks { get; set; }
    }

    public class CreateLeaveRequestDto
    {
        public int EmployeeId { get; set; }
        public int LeaveTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalDays { get; set; }
        public string? Reason { get; set; }
        public string Status { get; set; } = "Pending"; public int? ApprovedById { get; set; }
        public string? Remarks { get; set; }
    }

    public class UpdateLeaveRequestDto : CreateLeaveRequestDto
    {
        public int Id { get; set; }
    }
}