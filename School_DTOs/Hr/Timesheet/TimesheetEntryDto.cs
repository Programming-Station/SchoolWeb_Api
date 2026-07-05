using System;
namespace School_DTOs.Hr.Timesheet
{
    public class TimesheetEntryDto
    {
        public int Id { get; set; }
        public int TimesheetId { get; set; }
        public int TimesheetId { get; set; } public DateTime EntryDate { get; set; } public string TaskName { get; set; } = null!; public string? ProjectName { get; set; } public decimal HoursWorked { get; set; } = 0; public string? Description { get; set; }
    }

    public class CreateTimesheetEntryDto
    {
        public int TimesheetId { get; set; }
        public int TimesheetId { get; set; } public DateTime EntryDate { get; set; } public string TaskName { get; set; } = null!; public string? ProjectName { get; set; } public decimal HoursWorked { get; set; } = 0; public string? Description { get; set; }
    }

    public class UpdateTimesheetEntryDto : CreateTimesheetEntryDto
    {
        public int Id { get; set; }
    }
}
