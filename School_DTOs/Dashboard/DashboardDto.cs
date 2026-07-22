namespace School_DTOs.Dashboard
{
    public class DashboardDto
    {
        public DashboardWelcomeDto Welcome { get; set; } = new DashboardWelcomeDto();
        public DashboardStatsDto Stats { get; set; } = new DashboardStatsDto();
        public List<ActivityDto> RecentActivities { get; set; } = new List<ActivityDto>();
        public List<RecentRegistrationDto> RecentRegistrations { get; set; } = new List<RecentRegistrationDto>();
        public List<UpcomingEventDto> UpcomingEvents { get; set; } = new List<UpcomingEventDto>();
        public FeeCollectionDto FeeCollection { get; set; } = new FeeCollectionDto();
        public AttendanceStatsDto AttendanceStats { get; set; } = new AttendanceStatsDto();

        // Resource summaries
        public DashboardLibraryDto Library { get; set; } = new DashboardLibraryDto();
        public DashboardTransportDto Transport { get; set; } = new DashboardTransportDto();
        public DashboardHostelDto Hostel { get; set; } = new DashboardHostelDto();
        public DashboardSystemStatusDto SystemStatus { get; set; } = new DashboardSystemStatusDto();

        // Extended lists and widgets
        public List<RecentAdmissionDto> RecentAdmissions { get; set; } = new List<RecentAdmissionDto>();
        public List<FeeDefaulterDto> PendingFeeDefaulters { get; set; } = new List<FeeDefaulterDto>();
        public List<LeaveRequestDashboardDto> LeaveRequests { get; set; } = new List<LeaveRequestDashboardDto>();
        public List<UpcomingExamDashboardDto> UpcomingExams { get; set; } = new List<UpcomingExamDashboardDto>();
        public List<NoticeDashboardDto> NoticeBoard { get; set; } = new List<NoticeDashboardDto>();
        public List<BirthdayDto> Birthdays { get; set; } = new List<BirthdayDto>();
        public List<DashboardTaskDto> PendingTasks { get; set; } = new List<DashboardTaskDto>();
        public DashboardChartsDto Charts { get; set; } = new DashboardChartsDto();
    }

    public class DashboardStatsDto
    {
        public int TotalStudents { get; set; }
        public int FacultyMembers { get; set; } // Represent general staff/employees
        public int ActiveCourses { get; set; }
        public int Departments { get; set; } // Affiliated colleges/departments
        public decimal FeeCollectionTotal { get; set; }
        public decimal FeeCollectionCollected { get; set; }
        public decimal FeeCollectionPending { get; set; }
        public double FeeCollectionGrowth { get; set; }
        public int PendingApprovals { get; set; }
        public double AttendanceRate { get; set; }
        public int ExamsScheduled { get; set; }

        // New dashboard stats
        public int TotalEmployees { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalParents { get; set; }
        public int TotalClasses { get; set; }
        public int TotalSections { get; set; }
        public int TotalSubjects { get; set; }
        public int TodayAttendancePresent { get; set; }
        public int TodayAttendanceAbsent { get; set; }
        public double TodayAttendanceRate { get; set; }
        public decimal TodayFeeCollection { get; set; }
        public int ActiveUsers { get; set; }
    }

    public class ActivityDto
    {
        public string Action { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // student, exam, payment, faculty, course, report
    }

    public class RecentRegistrationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Course { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // Approved, Pending, Under Review, Rejected
    }

    public class UpcomingEventDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty; // Format: "15 Dec"
        public string Time { get; set; } = string.Empty; // Format: "9:00 AM - 12:00 PM"
        public DateTime EventDate { get; set; }
    }

    public class FeeCollectionDto
    {
        public decimal Total { get; set; }
        public decimal Collected { get; set; }
        public decimal Pending { get; set; }
        public double Growth { get; set; } // Percentage growth from last month

        // Expanded owner finance metrics
        public decimal TotalIncome { get; set; }
        public decimal MonthlyIncome { get; set; }
        public decimal TodayCollection { get; set; }
        public decimal PendingCollection { get; set; }
        public decimal OverdueFees { get; set; }
        public decimal ScholarshipAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal RefundAmount { get; set; }
    }

    public class AttendanceStatsDto
    {
        public double AttendanceRate { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }
    }

    // Extended Dashboard child DTO classes
    public class RecentAdmissionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string EnrollmentNo { get; set; } = string.Empty;
    }

    public class FeeDefaulterDto
    {
        public string StudentName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public decimal PendingAmount { get; set; }
        public string DueDate { get; set; } = string.Empty;
    }

    public class LeaveRequestDashboardDto
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string LeaveType { get; set; } = string.Empty;
        public string DateRange { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // Pending, Approved, Rejected
    }

    public class UpcomingExamDashboardDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
    }

    public class NoticeDashboardDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // General, Exam, Holiday, Fee
    }

    public class BirthdayDto
    {
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // Student, Teacher, Staff
        public string Details { get; set; } = string.Empty; // Class/Designation
        public string DOB { get; set; } = string.Empty;
    }

    public class DashboardTaskDto
    {
        public string Title { get; set; } = string.Empty;
        public string DueDate { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty; // High, Medium, Low
        public bool IsCompleted { get; set; }
    }

    public class ChartDataPoint<TKey, TValue>
    {
        public TKey Label { get; set; } = default!;
        public TValue Value { get; set; } = default!;
    }

    public class DashboardChartsDto
    {
        public List<ChartDataPoint<string, int>> StudentAdmissionTrend { get; set; } = new List<ChartDataPoint<string, int>>();
        public List<ChartDataPoint<string, double>> AttendanceTrend { get; set; } = new List<ChartDataPoint<string, double>>();
        public List<ChartDataPoint<string, decimal>> MonthlyFeeCollection { get; set; } = new List<ChartDataPoint<string, decimal>>();
        public List<ChartDataPoint<string, int>> EmployeeGrowth { get; set; } = new List<ChartDataPoint<string, int>>();
        public List<ChartDataPoint<string, int>> StudentGrowth { get; set; } = new List<ChartDataPoint<string, int>>();

        // Owner specific trends
        public List<ChartDataPoint<string, decimal>> RevenueTrend { get; set; } = new List<ChartDataPoint<string, decimal>>();
        public List<ChartDataPoint<string, decimal>> FeeCollectionTrend { get; set; } = new List<ChartDataPoint<string, decimal>>();
    }

    public class DashboardWelcomeDto
    {
        public string SchoolLogo { get; set; } = string.Empty;
        public string SchoolName { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public string AcademicSession { get; set; } = string.Empty;
        public string SubscriptionPlan { get; set; } = string.Empty;
        public string SubscriptionStatus { get; set; } = string.Empty;
        public string ExpiryDate { get; set; } = string.Empty;
        public string LastLogin { get; set; } = string.Empty;
    }

    public class DashboardLibraryDto
    {
        public int TotalBooks { get; set; }
        public int IssuedBooks { get; set; }
        public int ReturnedToday { get; set; }
        public int OverdueBooks { get; set; }
    }

    public class DashboardTransportDto
    {
        public int VehiclesCount { get; set; }
        public int DriversCount { get; set; }
        public int RoutesCount { get; set; }
        public int TransportStudentsCount { get; set; }
    }

    public class DashboardHostelDto
    {
        public int RoomsCount { get; set; }
        public int OccupiedBeds { get; set; }
        public int AvailableBeds { get; set; }
        public int HostelStudentsCount { get; set; }
    }

    public class DashboardSystemStatusDto
    {
        public string DatabaseStatus { get; set; } = "Healthy";
        public string ApiStatus { get; set; } = "Healthy";
        public string ApplicationVersion { get; set; } = "v1.2.0-LTS";
        public string LastBackup { get; set; } = string.Empty;
    }
}
