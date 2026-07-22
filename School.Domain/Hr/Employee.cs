using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.Auth;
using School.Domain.Hr.Attendance;
using School.Domain.Hr.LeaveManagement;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Hr
{
    public class Employee : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string EmployeeCode { get; set; } = null!; // Auto Generated

        [MaxLength(450)]
        public string? ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public virtual ApplicationUser? ApplicationUser { get; set; }

        [MaxLength(500)]
        public string? EmployeePhoto { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; } = null!;

        [MaxLength(100)]
        public string? MiddleName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; } = null!;

        [Required, MaxLength(20)]
        public string Gender { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        [Required, MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required, MaxLength(20)]
        public string MobileNumber { get; set; } = null!;

        [Required, MaxLength(20)]
        public string Status { get; set; } = "active"; // active, inactive

        public DateTime JoiningDate { get; set; }

        public DateTime? LeavingDate { get; set; }

        public decimal Experience { get; set; } = 0; // In Years

        public int DepartmentId { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; } = null!;

        public int? DesignationId { get; set; }
        [ForeignKey(nameof(DesignationId))]
        public virtual Designation? Designation { get; set; }

        public int? ReportingManagerId { get; set; }
        [ForeignKey(nameof(ReportingManagerId))]
        public virtual Employee? ReportingManager { get; set; }

        public int? EmployeeTypeId { get; set; }
        [ForeignKey(nameof(EmployeeTypeId))]
        public virtual EmployeeType? EmployeeType { get; set; }

        public int? EmploymentStatusId { get; set; }
        [ForeignKey(nameof(EmploymentStatusId))]
        public virtual EmploymentStatus? EmploymentStatus { get; set; }

        public int? SalaryGradeId { get; set; }
        [ForeignKey(nameof(SalaryGradeId))]
        public virtual SalaryGrade? SalaryGrade { get; set; }

        public int? ShiftId { get; set; }
        [ForeignKey(nameof(ShiftId))]
        public virtual ShiftMaster? Shift { get; set; }

        public int? SpecializationId { get; set; }
        [ForeignKey(nameof(SpecializationId))]
        public virtual Specialization? Specialization { get; set; }

        public decimal WorkingHours { get; set; } = 8;

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;

        // Navigation Properties for details
        public virtual EmployeeDetail? EmployeeDetail { get; set; }
        public virtual ICollection<EmployeeDocument> Documents { get; set; } = new List<EmployeeDocument>();
        public virtual ICollection<EmployeeBankDetail> BankDetails { get; set; } = new List<EmployeeBankDetail>();
        public virtual ICollection<EmployeeEducation> Educations { get; set; } = new List<EmployeeEducation>();
        public virtual ICollection<EmployeeExperience> Experiences { get; set; } = new List<EmployeeExperience>();
        public virtual ICollection<EmployeeSalaryDetail> SalaryDetails { get; set; } = new List<EmployeeSalaryDetail>();
        public virtual ICollection<Class> ClassesTaught { get; set; } = new List<Class>();

        // Leave Management Navigation
        [InverseProperty(nameof(LeaveRequest.Employee))]
        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

        [InverseProperty(nameof(LeaveRequest.ApprovedBy))]
        public virtual ICollection<LeaveRequest> ApprovedLeaveRequests { get; set; } = new List<LeaveRequest>();

        public virtual ICollection<LeaveBalance> LeaveBalances { get; set; } = new List<LeaveBalance>();

        // Attendance Navigation
        public virtual ICollection<global::School.Domain.Hr.Attendance.Attendance> Attendances { get; set; } = new List<global::School.Domain.Hr.Attendance.Attendance>();
        public virtual ICollection<AttendanceLog> AttendanceLogs { get; set; } = new List<AttendanceLog>();

        // Timesheet Navigation
        [InverseProperty(nameof(Timesheet.Timesheet.Employee))]
        public virtual ICollection<global::School.Domain.Hr.Timesheet.Timesheet> Timesheets { get; set; } = new List<global::School.Domain.Hr.Timesheet.Timesheet>();

        [InverseProperty(nameof(Timesheet.Timesheet.ApprovedBy))]
        public virtual ICollection<global::School.Domain.Hr.Timesheet.Timesheet> ApprovedTimesheets { get; set; } = new List<global::School.Domain.Hr.Timesheet.Timesheet>();
    }
}
