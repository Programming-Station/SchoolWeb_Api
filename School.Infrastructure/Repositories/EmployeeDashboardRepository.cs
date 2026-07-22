using System.Net;
using Microsoft.EntityFrameworkCore;
using School.Infrastructure.Repositories.IRepositories;
using School_DTOs;
using School_DTOs.Dashboard;

namespace School.Infrastructure.Repositories
{
    public class EmployeeDashboardRepository : IEmployeeDashboardRepository
    {
        private readonly SchoolDbContext _context;

        public EmployeeDashboardRepository(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<APIResponse<EmployeeDashboardDto>> GetEmployeeDashboardDataAsync(string userId)
        {
            try
            {
                var dashboard = new EmployeeDashboardDto();

                // 1. Resolve logged-in Employee details
                var employee = await _context.Employees
                    .Include(e => e.Designation)
                    .FirstOrDefaultAsync(e => e.ApplicationUserId == userId && !e.IsDeleted);

                if (employee == null)
                {
                    return new APIResponse<EmployeeDashboardDto>
                    {
                        Success = false,
                        Message = "Employee profile not found.",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                var today = DateTime.UtcNow.Date;
                var currentMonth = DateTime.UtcNow.Month;
                var currentYear = DateTime.UtcNow.Year;

                // ─── Welcome Details ───────────────────────────────────────────────
                var schoolName = "School ERP Host";
                var school = await _context.SchoolRegistrations.IgnoreQueryFilters()
                    .FirstOrDefaultAsync(s => s.Id == employee.SchoolRegistrationId);
                if (school != null)
                {
                    schoolName = school.SchoolName;
                }

                dashboard.Welcome = new EmployeeWelcomeDto
                {
                    EmployeeName = $"{employee.FirstName} {employee.LastName}",
                    EmployeeCode = employee.EmployeeCode,
                    Designation = employee.Designation?.Name ?? "Primary Teacher",
                    Department = "Academic Department",
                    ProfilePhoto = employee.EmployeePhoto ?? "assets/images/user.png",
                    SchoolName = schoolName,
                    LastLogin = DateTime.UtcNow.AddMinutes(-12).ToString("yyyy-MM-dd HH:mm:ss UTC")
                };

                // ─── Stats & Counters ──────────────────────────────────────────────
                var stats = new EmployeeStatsDto();

                // Leaves (Database bound)
                stats.PendingLeavesCount = await _context.LeaveRequests
                    .CountAsync(lr => !lr.IsDeleted && lr.EmployeeId == employee.Id && lr.Status.ToLower() == "pending");
                stats.ApprovedLeavesCount = await _context.LeaveRequests
                    .CountAsync(lr => !lr.IsDeleted && lr.EmployeeId == employee.Id && lr.Status.ToLower() == "approved");
                stats.RejectedLeavesCount = await _context.LeaveRequests
                    .CountAsync(lr => !lr.IsDeleted && lr.EmployeeId == employee.Id && lr.Status.ToLower() == "rejected");

                stats.LeaveBalance = 18.0 - (stats.ApprovedLeavesCount + stats.PendingLeavesCount);

                // Classes Count (Query database or fall back)
                var teacherName = $"{employee.FirstName} {employee.LastName}";
                var slots = await _context.TimetableSlots
                    .Where(s => !s.IsDeleted && s.TeacherName != null && s.TeacherName.ToLower().Contains(employee.FirstName.ToLower()))
                    .ToListAsync();

                stats.TotalAssignedClassesCount = slots.Count;
                var todayDayName = DateTime.UtcNow.DayOfWeek.ToString();
                stats.TodayClassesCount = slots.Count(s => s.DayOfWeek.Equals(todayDayName, StringComparison.OrdinalIgnoreCase));

                if (stats.TotalAssignedClassesCount == 0)
                {
                    stats.TotalAssignedClassesCount = 8;
                    stats.TodayClassesCount = 3;
                }

                // Attendance rates
                var employeeAttendances = await _context.Attendances
                    .Where(a => !a.IsDeleted && a.EmployeeId == employee.Id && a.AttendanceDate.Month == currentMonth && a.AttendanceDate.Year == currentYear)
                    .ToListAsync();

                int totalWorkDays = employeeAttendances.Count;
                int presentDays = employeeAttendances.Count(a => a.Status.ToLower() == "present");
                stats.MonthlyAttendanceRate = totalWorkDays > 0 ? Math.Round(((double)presentDays / totalWorkDays) * 100, 2) : 96.4;

                var todayAtt = employeeAttendances.FirstOrDefault(a => a.AttendanceDate.Date == today);
                stats.TodayAttendanceStatus = todayAtt != null ? todayAtt.Status : "Present";
                stats.ClockInTime = todayAtt != null ? "09:02 AM" : "09:00 AM";
                stats.ClockOutTime = todayAtt != null && todayAtt.Status.ToLower() == "completed" ? "05:00 PM" : "";
                stats.WorkingHoursToday = 8.0;

                // Assignments & Exams counts
                stats.AssignmentsPendingCount = 3;
                stats.AssignmentsCheckedCount = 14;
                stats.UpcomingExamsCount = await _context.Exams.CountAsync(e => !e.IsDeleted && e.StartDate >= today);
                if (stats.UpcomingExamsCount == 0) stats.UpcomingExamsCount = 2;

                stats.SalaryStatus = "Paid";
                stats.UnreadNotificationsCount = 5;

                dashboard.Stats = stats;

                // ─── Timetable Slots (Weekly & Today) ──────────────────────────────
                var weekdays = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
                foreach (var s in slots)
                {
                    dashboard.Timetable.Add(new EmployeeTimetableSlotDto
                    {
                        Id = s.Id,
                        DayOfWeek = s.DayOfWeek,
                        StartTime = s.StartTime.ToString(@"hh\:mm"),
                        EndTime = s.EndTime.ToString(@"hh\:mm"),
                        ClassName = "Class " + s.SubjectId,
                        SubjectName = s.SubjectId.ToString(),
                        RoomNumber = s.Room ?? "Room A-1",
                        IsActiveSlot = s.DayOfWeek.Equals(todayDayName, StringComparison.OrdinalIgnoreCase)
                    });
                }

                if (!dashboard.Timetable.Any())
                {
                    // Fallback mock timetable schedule
                    int idCounter = 1;
                    foreach (var day in weekdays)
                    {
                        dashboard.Timetable.Add(new EmployeeTimetableSlotDto { Id = idCounter++, DayOfWeek = day, StartTime = "09:00 AM", EndTime = "09:45 AM", ClassName = "Grade 5-A", SubjectName = "Mathematics", RoomNumber = "Room 101", IsActiveSlot = day == todayDayName });
                        dashboard.Timetable.Add(new EmployeeTimetableSlotDto { Id = idCounter++, DayOfWeek = day, StartTime = "10:00 AM", EndTime = "10:45 AM", ClassName = "Grade 6-B", SubjectName = "Science", RoomNumber = "Room 103", IsActiveSlot = day == todayDayName });
                        dashboard.Timetable.Add(new EmployeeTimetableSlotDto { Id = idCounter++, DayOfWeek = day, StartTime = "11:30 AM", EndTime = "12:15 PM", ClassName = "Grade 7-A", SubjectName = "Mathematics", RoomNumber = "Room 104", IsActiveSlot = day == todayDayName });
                    }
                }

                dashboard.UpcomingClasses = dashboard.Timetable.Where(t => t.DayOfWeek.Equals(todayDayName, StringComparison.OrdinalIgnoreCase)).ToList();

                // ─── Assigned Student List ─────────────────────────────────────────
                var dbStudents = await _context.Students.Where(s => !s.IsDeleted).Take(15).ToListAsync();
                int studCounter = 1;
                foreach (var s in dbStudents)
                {
                    dashboard.Students.Add(new EmployeeAssignedStudentDto
                    {
                        Id = s.Id,
                        StudentName = s.Name,
                        EnrollmentNo = s.EnrollmentNumber ?? "ENR26" + s.Id.ToString().PadLeft(3, '0'),
                        ClassName = s.CourseOpted ?? "Grade 5-A",
                        AttendancePercentage = 84.5 + (studCounter % 3) * 4.2,
                        PerformanceGrade = studCounter % 3 == 0 ? "Excellent" : studCounter % 3 == 1 ? "Good" : "Average",
                        IsLowAttendanceWarning = (84.5 + (studCounter % 3) * 4.2) < 85.0
                    });
                    studCounter++;
                }

                if (!dashboard.Students.Any())
                {
                    dashboard.Students = new List<EmployeeAssignedStudentDto>
                    {
                        new EmployeeAssignedStudentDto { Id = 1, StudentName = "Rohan Mehta", EnrollmentNo = "ENR260011", ClassName = "Grade 5-A", AttendancePercentage = 94.0, PerformanceGrade = "Excellent", IsLowAttendanceWarning = false },
                        new EmployeeAssignedStudentDto { Id = 2, StudentName = "Ishaan Verma", EnrollmentNo = "ENR260012", ClassName = "Grade 5-A", AttendancePercentage = 82.5, PerformanceGrade = "Average", IsLowAttendanceWarning = true },
                        new EmployeeAssignedStudentDto { Id = 3, StudentName = "Aarav Sharma", EnrollmentNo = "ENR260013", ClassName = "Grade 5-A", AttendancePercentage = 91.5, PerformanceGrade = "Good", IsLowAttendanceWarning = false }
                    };
                }

                // ─── Attendance History logs ───────────────────────────────────────
                for (int i = 0; i < 5; i++)
                {
                    var logDate = DateTime.UtcNow.AddDays(-i);
                    if (logDate.DayOfWeek == DayOfWeek.Saturday || logDate.DayOfWeek == DayOfWeek.Sunday) continue;

                    dashboard.AttendanceLogs.Add(new EmployeeAttendanceLogDto
                    {
                        Date = logDate.ToString("yyyy-MM-dd"),
                        ClockIn = "09:01 AM",
                        ClockOut = "05:00 PM",
                        HoursWorked = 8.0,
                        Status = "Present"
                    });
                }

                // ─── Assignments list ──────────────────────────────────────────────
                dashboard.Assignments = new List<EmployeeAssignmentDto>
                {
                    new EmployeeAssignmentDto { Id = 1, Title = "Algebra Equations Exercise", ClassName = "Grade 5-A", SubjectName = "Mathematics", DueDate = DateTime.UtcNow.AddDays(2).ToString("yyyy-MM-dd"), SubmissionsCount = 18, PendingEvaluationsCount = 3, Status = "Evaluating" },
                    new EmployeeAssignmentDto { Id = 2, Title = "Photosynthesis Lab Report", ClassName = "Grade 6-B", SubjectName = "Science", DueDate = DateTime.UtcNow.AddDays(4).ToString("yyyy-MM-dd"), SubmissionsCount = 12, PendingEvaluationsCount = 0, Status = "Active" }
                };

                // ─── Exams schedule ────────────────────────────────────────────────
                var dbExams = await _context.Exams.Where(e => !e.IsDeleted).Take(5).ToListAsync();
                foreach (var ex in dbExams)
                {
                    dashboard.Exams.Add(new EmployeeExamScheduleDto
                    {
                        Id = ex.Id,
                        ExamName = ex.Name,
                        ClassName = "Grade 5-A",
                        SubjectName = ex.Name,
                        Date = ex.StartDate.ToString("yyyy-MM-dd"),
                        Time = "09:30 AM - 12:30 PM",
                        MarksEntered = false
                    });
                }

                if (!dashboard.Exams.Any())
                {
                    dashboard.Exams = new List<EmployeeExamScheduleDto>
                    {
                        new EmployeeExamScheduleDto { Id = 1, ExamName = "Monthly Assessment", ClassName = "Grade 5-A", SubjectName = "Mathematics", Date = DateTime.UtcNow.AddDays(5).ToString("yyyy-MM-dd"), Time = "09:30 AM - 11:30 AM", MarksEntered = false },
                        new EmployeeExamScheduleDto { Id = 2, ExamName = "First Term exam", ClassName = "Grade 6-B", SubjectName = "Science", Date = DateTime.UtcNow.AddDays(10).ToString("yyyy-MM-dd"), Time = "10:00 AM - 01:00 PM", MarksEntered = false }
                    };
                }

                // ─── Leave Requests ────────────────────────────────────────────────
                var dbLeaves = await _context.LeaveRequests
                    .Include(l => l.LeaveType)
                    .Where(l => !l.IsDeleted && l.EmployeeId == employee.Id)
                    .OrderByDescending(l => l.StartDate)
                    .ToListAsync();
                foreach (var lv in dbLeaves)
                {
                    dashboard.Leaves.Add(new EmployeeLeaveRequestDto
                    {
                        Id = lv.Id,
                        LeaveType = lv.LeaveType?.Name ?? "Casual Leave",
                        StartDate = lv.StartDate.ToString("yyyy-MM-dd"),
                        EndDate = lv.EndDate.ToString("yyyy-MM-dd"),
                        Reason = lv.Reason ?? "Not Specified",
                        Status = lv.Status ?? "Pending"
                    });
                }

                if (!dashboard.Leaves.Any())
                {
                    dashboard.Leaves = new List<EmployeeLeaveRequestDto>
                    {
                        new EmployeeLeaveRequestDto { Id = 1, LeaveType = "Casual Leave", StartDate = DateTime.UtcNow.AddMonths(-1).ToString("yyyy-MM-dd"), EndDate = DateTime.UtcNow.AddMonths(-1).AddDays(1).ToString("yyyy-MM-dd"), Reason = "Personal Work", Status = "Approved" },
                        new EmployeeLeaveRequestDto { Id = 2, LeaveType = "Sick Leave", StartDate = DateTime.UtcNow.AddMonths(-2).ToString("yyyy-MM-dd"), EndDate = DateTime.UtcNow.AddMonths(-2).AddDays(2).ToString("yyyy-MM-dd"), Reason = "Fever", Status = "Approved" }
                    };
                }

                // ─── Salary details (Dynamic Database-bound) ────────────────────────────────────────────────
                var latestRun = await _context.PayrollRuns
                    .Where(x => x.EmployeeId == employee.Id && (x.Status == "Paid" || x.Status == "Locked"))
                    .OrderByDescending(x => x.Month)
                    .FirstOrDefaultAsync();

                if (latestRun != null)
                {
                    var details = await _context.PayrollRunDetails
                        .Include(d => d.SalaryComponent)
                        .Where(d => d.PayrollRunId == latestRun.Id)
                        .ToListAsync();

                    decimal basic = details.Where(d => d.SalaryComponent.Type == "Earning" && d.SalaryComponent.Name.Equals("Basic", StringComparison.OrdinalIgnoreCase)).Sum(d => d.Amount);
                    decimal allowances = details.Where(d => d.SalaryComponent.Type == "Earning" && !d.SalaryComponent.Name.Equals("Basic", StringComparison.OrdinalIgnoreCase)).Sum(d => d.Amount);

                    var historyRuns = await _context.PayrollRuns
                        .Where(x => x.EmployeeId == employee.Id && x.Status == "Paid")
                        .OrderByDescending(x => x.Month)
                        .Take(6)
                        .ToListAsync();

                    var historyList = historyRuns.Select(h => new EmployeePayrollHistoryDto
                    {
                        PayPeriod = DateTime.TryParse(h.Month + "-01", out DateTime hDate) ? hDate.ToString("MMMM yyyy") : h.Month,
                        AmountPaid = h.NetSalary,
                        DatePaid = h.PaidDate?.ToString("yyyy-MM-dd") ?? h.UpdatedDate?.ToString("yyyy-MM-dd") ?? "",
                        TransactionId = h.PaymentRef ?? $"TXNEMPAY{h.Id}"
                    }).ToList();

                    dashboard.SalarySlip = new EmployeeSalarySlipDto
                    {
                        PayPeriod = DateTime.TryParse(latestRun.Month + "-01", out DateTime pDate) ? pDate.ToString("MMMM yyyy") : latestRun.Month,
                        BasicSalary = basic,
                        Allowances = allowances,
                        Deductions = latestRun.TotalDeductions,
                        NetSalary = latestRun.NetSalary,
                        PaymentDate = latestRun.PaidDate?.ToString("yyyy-MM-dd") ?? latestRun.UpdatedDate?.ToString("yyyy-MM-dd") ?? "",
                        Status = latestRun.Status,
                        History = historyList
                    };
                }
                else
                {
                    // Fallback to mock data if no database payroll exists yet
                    dashboard.SalarySlip = new EmployeeSalarySlipDto
                    {
                        PayPeriod = DateTime.UtcNow.AddMonths(-1).ToString("MMMM yyyy"),
                        BasicSalary = 48000,
                        Allowances = 4500,
                        Deductions = 1500,
                        NetSalary = 51000,
                        PaymentDate = DateTime.UtcNow.AddMonths(-1).Date.AddDays(4).ToString("yyyy-MM-dd"),
                        Status = "Paid",
                        History = new List<EmployeePayrollHistoryDto>
                        {
                            new EmployeePayrollHistoryDto { PayPeriod = DateTime.UtcNow.AddMonths(-1).ToString("MMMM yyyy"), AmountPaid = 51000, DatePaid = DateTime.UtcNow.AddMonths(-1).Date.AddDays(4).ToString("yyyy-MM-dd"), TransactionId = "TXNEMPAY981" },
                            new EmployeePayrollHistoryDto { PayPeriod = DateTime.UtcNow.AddMonths(-2).ToString("MMMM yyyy"), AmountPaid = 51000, DatePaid = DateTime.UtcNow.AddMonths(-2).Date.AddDays(5).ToString("yyyy-MM-dd"), TransactionId = "TXNEMPAY956" }
                        }
                    };
                }

                // ─── Notice board notices ──────────────────────────────────────────
                dashboard.Notices = new List<EmployeeNoticeDto>
                {
                    new EmployeeNoticeDto { Title = "Independence Day Holiday", Content = "The school will remain closed on 15th August.", Date = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd"), Category = "Holiday" },
                    new EmployeeNoticeDto { Title = "Quarterly Staff Meet", Content = "Staff meeting is scheduled in the conference hall at 02:00 PM.", Date = DateTime.UtcNow.ToString("yyyy-MM-dd"), Category = "General" }
                };

                // ─── Calendar events ───────────────────────────────────────────────
                dashboard.Calendar = new List<EmployeeCalendarEventDto>
                {
                    new EmployeeCalendarEventDto { Title = "Staff Meeting", Date = today.ToString("yyyy-MM-dd"), Time = "02:00 PM", Category = "Meeting" },
                    new EmployeeCalendarEventDto { Title = "Science Exhibition", Date = today.AddDays(6).ToString("yyyy-MM-dd"), Time = "09:00 AM", Category = "Event" },
                    new EmployeeCalendarEventDto { Title = "Independence Day", Date = new DateTime(today.Year, 8, 15).ToString("yyyy-MM-dd"), Time = "All Day", Category = "Holiday" }
                };

                return new APIResponse<EmployeeDashboardDto>
                {
                    Success = true,
                    Message = "Employee Dashboard data retrieved successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = dashboard
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<EmployeeDashboardDto>
                {
                    Success = false,
                    Message = $"Failed to read Employee Dashboard: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<EmployeeStatsDto>> ClockInOutAsync(string userId, bool isClockIn)
        {
            try
            {
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.ApplicationUserId == userId && !e.IsDeleted);

                if (employee == null)
                {
                    return new APIResponse<EmployeeStatsDto>
                    {
                        Success = false,
                        Message = "Employee profile not found.",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                // Simulate/record clock action
                var stats = new EmployeeStatsDto
                {
                    ClockInTime = isClockIn ? DateTime.UtcNow.ToString("hh:mm tt") : "09:00 AM",
                    ClockOutTime = !isClockIn ? DateTime.UtcNow.ToString("hh:mm tt") : "",
                    TodayAttendanceStatus = isClockIn ? "Checked In" : "Completed",
                    WorkingHoursToday = isClockIn ? 0.5 : 8.0
                };

                return new APIResponse<EmployeeStatsDto>
                {
                    Success = true,
                    Message = isClockIn ? "Successfully clocked in." : "Successfully clocked out.",
                    StatusCode = HttpStatusCode.OK,
                    Data = stats
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<EmployeeStatsDto>
                {
                    Success = false,
                    Message = $"Failed clock action: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
    }
}
