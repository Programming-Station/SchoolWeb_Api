namespace School_DTOs.Hr.Timesheet
{
    public class TimesheetDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "Draft"; public int? ApprovedById { get; set; }
        public decimal TotalHours { get; set; } = 0;
    }

    public class CreateTimesheetDto
    {
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "Draft"; public int? ApprovedById { get; set; }
        public decimal TotalHours { get; set; } = 0;
    }

    public class UpdateTimesheetDto : CreateTimesheetDto
    {
        public int Id { get; set; }
    }
}