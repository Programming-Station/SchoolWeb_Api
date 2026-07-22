namespace School_DTOs.Dashboard
{
    public class EmployeeDashboardDto
    {
        public EmployeeWelcomeDto Welcome { get; set; } = new EmployeeWelcomeDto();
        public EmployeeStatsDto Stats { get; set; } = new EmployeeStatsDto();
        public List<EmployeeTimetableSlotDto> Timetable { get; set; } = new List<EmployeeTimetableSlotDto>();
        public List<EmployeeTimetableSlotDto> UpcomingClasses { get; set; } = new List<EmployeeTimetableSlotDto>();
        public List<EmployeeAssignedStudentDto> Students { get; set; } = new List<EmployeeAssignedStudentDto>();
        public List<EmployeeAttendanceLogDto> AttendanceLogs { get; set; } = new List<EmployeeAttendanceLogDto>();
        public List<EmployeeAssignmentDto> Assignments { get; set; } = new List<EmployeeAssignmentDto>();
        public List<EmployeeExamScheduleDto> Exams { get; set; } = new List<EmployeeExamScheduleDto>();
        public List<EmployeeLeaveRequestDto> Leaves { get; set; } = new List<EmployeeLeaveRequestDto>();
        public EmployeeSalarySlipDto SalarySlip { get; set; } = new EmployeeSalarySlipDto();
        public List<EmployeeNoticeDto> Notices { get; set; } = new List<EmployeeNoticeDto>();
        public List<EmployeeCalendarEventDto> Calendar { get; set; } = new List<EmployeeCalendarEventDto>();
    }

    public class EmployeeWelcomeDto
    {
        public string EmployeeName { get; set; } = string.Empty;
        public string EmployeeCode { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string ProfilePhoto { get; set; } = string.Empty;
        public string SchoolName { get; set; } = string.Empty;
        public string LastLogin { get; set; } = string.Empty;
    }

    public class EmployeeStatsDto
    {
        public int TodayClassesCount { get; set; }
        public int TotalAssignedClassesCount { get; set; }
        public string TodayAttendanceStatus { get; set; } = "Not Checked In";
        public double MonthlyAttendanceRate { get; set; }

        public int PendingLeavesCount { get; set; }
        public int ApprovedLeavesCount { get; set; }
        public int RejectedLeavesCount { get; set; }

        public int AssignmentsPendingCount { get; set; }
        public int AssignmentsCheckedCount { get; set; }

        public int UpcomingExamsCount { get; set; }
        public string SalaryStatus { get; set; } = "Paid";
        public int UnreadNotificationsCount { get; set; }

        // Clock In / Check-in properties
        public string ClockInTime { get; set; } = string.Empty;
        public string ClockOutTime { get; set; } = string.Empty;
        public double WorkingHoursToday { get; set; }
        public double LeaveBalance { get; set; }
    }

    public class EmployeeTimetableSlotDto
    {
        public int Id { get; set; }
        public string DayOfWeek { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;
        public bool IsActiveSlot { get; set; }
    }

    public class EmployeeAssignedStudentDto
    {
        public int Id { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string EnrollmentNo { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public double AttendancePercentage { get; set; }
        public string PerformanceGrade { get; set; } = string.Empty; // Excellent, Good, Average, Low
        public bool IsLowAttendanceWarning { get; set; }
    }

    public class EmployeeAttendanceLogDto
    {
        public string Date { get; set; } = string.Empty;
        public string ClockIn { get; set; } = string.Empty;
        public string ClockOut { get; set; } = string.Empty;
        public double HoursWorked { get; set; }
        public string Status { get; set; } = string.Empty; // Present, Late, Half Day, Absent
    }

    public class EmployeeAssignmentDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string DueDate { get; set; } = string.Empty;
        public int SubmissionsCount { get; set; }
        public int PendingEvaluationsCount { get; set; }
        public string Status { get; set; } = string.Empty; // Active, Evaluating, Completed
    }

    public class EmployeeExamScheduleDto
    {
        public int Id { get; set; }
        public string ExamName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public bool MarksEntered { get; set; }
    }

    public class EmployeeLeaveRequestDto
    {
        public int Id { get; set; }
        public string LeaveType { get; set; } = string.Empty;
        public string StartDate { get; set; } = string.Empty;
        public string EndDate { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // Pending, Approved, Rejected
    }

    public class EmployeeSalarySlipDto
    {
        public string PayPeriod { get; set; } = string.Empty;
        public decimal BasicSalary { get; set; }
        public decimal Allowances { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetSalary { get; set; }
        public string PaymentDate { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // Paid, Processing, Held
        public List<EmployeePayrollHistoryDto> History { get; set; } = new List<EmployeePayrollHistoryDto>();
    }

    public class EmployeePayrollHistoryDto
    {
        public string PayPeriod { get; set; } = string.Empty;
        public decimal AmountPaid { get; set; }
        public string DatePaid { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
    }

    public class EmployeeNoticeDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // General, Holiday, Fee, Administrative
    }

    public class EmployeeCalendarEventDto
    {
        public string Title { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty; // "yyyy-MM-dd"
        public string Time { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // Meeting, Event, Holiday, Birthday
    }
}
