using System;
using System.Collections.Generic;

namespace School_DTOs.Hr.Timesheet
{
    public class TimesheetDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = null!;
        public decimal TotalHours { get; set; }
        public List<TimesheetEntryDto> Entries { get; set; } = new List<TimesheetEntryDto>();
    }

    public class TimesheetEntryDto
    {
        public int Id { get; set; }
        public int TimesheetId { get; set; }
        public DateTime EntryDate { get; set; }
        public string TaskName { get; set; } = null!;
        public string? ProjectName { get; set; }
        public decimal HoursWorked { get; set; }
        public string? Description { get; set; }
    }

    public class CreateTimesheetDto
    {
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<CreateTimesheetEntryDto> Entries { get; set; } = new List<CreateTimesheetEntryDto>();
    }

    public class CreateTimesheetEntryDto
    {
        public DateTime EntryDate { get; set; }
        public string TaskName { get; set; } = null!;
        public string? ProjectName { get; set; }
        public decimal HoursWorked { get; set; }
        public string? Description { get; set; }
    }
}
