$basePath = "e:\GIT\SchoolSAAS\SchoolWeb_Api\School.Domain\Hr"

# EmployeeDocument
$content = @"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Hr
{
    public class EmployeeDocument : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        [Required, MaxLength(200)]
        public string DocumentName { get; set; } = null!;

        [Required, MaxLength(100)]
        public string DocumentType { get; set; } = null!; // ID, Address, Certificate etc.

        [Required, MaxLength(500)]
        public string FilePath { get; set; } = null!;

        [MaxLength(200)]
        public string? OriginalFileName { get; set; }

        [MaxLength(50)]
        public string? FileExtension { get; set; }

        public decimal FileSize { get; set; } // in MB

        public int Version { get; set; } = 1;

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
"@
Set-Content -Path "$basePath\EmployeeDocument.cs" -Value $content

# EmployeeBankDetail
$content = @"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Hr
{
    public class EmployeeBankDetail : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        [Required, MaxLength(200)]
        public string BankName { get; set; } = null!;

        [MaxLength(200)]
        public string? Branch { get; set; }

        [Required, MaxLength(50)]
        public string AccountNumber { get; set; } = null!;

        [Required, MaxLength(20)]
        public string IFSC { get; set; } = null!;

        [MaxLength(50)]
        public string? UPI { get; set; }

        public bool IsSalaryAccount { get; set; } = true;

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
"@
Set-Content -Path "$basePath\EmployeeBankDetail.cs" -Value $content

# EmployeeEducation
$content = @"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Hr
{
    public class EmployeeEducation : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        [Required, MaxLength(200)]
        public string Degree { get; set; } = null!;

        [MaxLength(200)]
        public string? Board { get; set; }

        [Required, MaxLength(200)]
        public string University { get; set; } = null!;

        [MaxLength(20)]
        public string? PassingYear { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Percentage { get; set; }

        [MaxLength(500)]
        public string? Certificates { get; set; } // Path to files if any

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
"@
Set-Content -Path "$basePath\EmployeeEducation.cs" -Value $content

# EmployeeExperience
$content = @"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Hr
{
    public class EmployeeExperience : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        [Required, MaxLength(200)]
        public string Company { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Designation { get; set; } = null!;

        public DateTime JoiningDate { get; set; }

        public DateTime LeavingDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Salary { get; set; }

        [MaxLength(500)]
        public string? ReasonForLeaving { get; set; }

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
"@
Set-Content -Path "$basePath\EmployeeExperience.cs" -Value $content

# EmployeeSalaryDetail
$content = @"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Hr
{
    public class EmployeeSalaryDetail : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Basic { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal HRA { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal DA { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal PF { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal ESI { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Bonus { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Allowances { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Deductions { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal NetSalary { get; set; } = 0;

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
"@
Set-Content -Path "$basePath\EmployeeSalaryDetail.cs" -Value $content

# LeaveRequest
$content = @"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Hr
{
    public class LeaveRequest : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        public int LeaveTypeId { get; set; }
        [ForeignKey(nameof(LeaveTypeId))]
        public virtual LeaveType LeaveType { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal TotalDays { get; set; }

        [MaxLength(1000)]
        public string? Reason { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = `"Pending`"; // Pending, Approved, Rejected

        public int? ApprovedById { get; set; }
        [ForeignKey(nameof(ApprovedById))]
        public virtual Employee? ApprovedBy { get; set; }

        [MaxLength(1000)]
        public string? Remarks { get; set; }

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
"@
Set-Content -Path "$basePath\LeaveRequest.cs" -Value $content

# LeaveBalance
$content = @"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Hr
{
    public class LeaveBalance : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        public int LeaveTypeId { get; set; }
        [ForeignKey(nameof(LeaveTypeId))]
        public virtual LeaveType LeaveType { get; set; } = null!;

        [Required, MaxLength(10)]
        public string Year { get; set; } = null!; // e.g., 2026

        [Column(TypeName = "decimal(5,2)")]
        public decimal TotalLeaves { get; set; } = 0;

        [Column(TypeName = "decimal(5,2)")]
        public decimal UsedLeaves { get; set; } = 0;

        [Column(TypeName = "decimal(5,2)")]
        public decimal AvailableLeaves { get; set; } = 0;

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
"@
Set-Content -Path "$basePath\LeaveBalance.cs" -Value $content

# Attendance
$content = @"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Hr
{
    public class Attendance : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        public DateTime AttendanceDate { get; set; }

        public TimeSpan? CheckInTime { get; set; }

        public TimeSpan? CheckOutTime { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; } = `"Present`"; // Present, Absent, HalfDay, Leave, Holiday

        [MaxLength(1000)]
        public string? Remarks { get; set; }

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
"@
Set-Content -Path "$basePath\Attendance.cs" -Value $content

# AttendanceLog
$content = @"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Hr
{
    public class AttendanceLog : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        public DateTime LogTime { get; set; }

        [Required, MaxLength(50)]
        public string LogType { get; set; } = null!; // In, Out

        [MaxLength(100)]
        public string? Source { get; set; } // Biometric, Manual, App

        [MaxLength(100)]
        public string? DeviceId { get; set; }

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
"@
Set-Content -Path "$basePath\AttendanceLog.cs" -Value $content

# Timesheet
$content = @"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Hr
{
    public class Timesheet : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = `"Draft`"; // Draft, Submitted, Approved, Rejected

        public int? ApprovedById { get; set; }
        [ForeignKey(nameof(ApprovedById))]
        public virtual Employee? ApprovedBy { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal TotalHours { get; set; } = 0;

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;

        public virtual ICollection<TimesheetEntry> Entries { get; set; } = new List<TimesheetEntry>();
    }
}
"@
Set-Content -Path "$basePath\Timesheet.cs" -Value $content

# TimesheetEntry
$content = @"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Hr
{
    public class TimesheetEntry : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int TimesheetId { get; set; }
        [ForeignKey(nameof(TimesheetId))]
        public virtual Timesheet Timesheet { get; set; } = null!;

        public DateTime EntryDate { get; set; }

        [Required, MaxLength(200)]
        public string TaskName { get; set; } = null!;

        [MaxLength(200)]
        public string? ProjectName { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal HoursWorked { get; set; } = 0;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
"@
Set-Content -Path "$basePath\TimesheetEntry.cs" -Value $content
