namespace School_DTOs.Dashboard
{
    public class SuperAdminDashboardDto
    {
        public SuperAdminDashboardStatsDto Stats { get; set; } = new SuperAdminDashboardStatsDto();
        public SuperAdminDashboardChartsDto Charts { get; set; } = new SuperAdminDashboardChartsDto();
        public SuperAdminSchoolAnalyticsDto Schools { get; set; } = new SuperAdminSchoolAnalyticsDto();
        public SuperAdminSubscriptionAnalyticsDto Subscriptions { get; set; } = new SuperAdminSubscriptionAnalyticsDto();
        public SuperAdminFinanceAnalyticsDto Finance { get; set; } = new SuperAdminFinanceAnalyticsDto();
        public SuperAdminUserAnalyticsDto Users { get; set; } = new SuperAdminUserAnalyticsDto();
        public SuperAdminSystemHealthDto SystemHealth { get; set; } = new SuperAdminSystemHealthDto();
        public List<SuperAdminActivityDto> RecentActivities { get; set; } = new List<SuperAdminActivityDto>();
        public List<SuperAdminCalendarEventDto> Calendar { get; set; } = new List<SuperAdminCalendarEventDto>();
        public List<SuperAdminLatestSchoolDto> LatestSchools { get; set; } = new List<SuperAdminLatestSchoolDto>();
        public List<SuperAdminLatestPaymentDto> LatestPayments { get; set; } = new List<SuperAdminLatestPaymentDto>();
        public List<SuperAdminLatestLoginDto> LatestLogins { get; set; } = new List<SuperAdminLatestLoginDto>();
    }

    public class SuperAdminDashboardStatsDto
    {
        // School Metrics
        public int TotalSchools { get; set; }
        public int ActiveSchools { get; set; }
        public int InactiveSchools { get; set; }
        public int TrialSchools { get; set; }
        public int ExpiredSchools { get; set; }

        // Aggregate User Metrics
        public int TotalStudents { get; set; }
        public int TotalEmployees { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalParents { get; set; }

        // Educational Metrics
        public int TotalCourses { get; set; }
        public int TotalBatches { get; set; }

        // Attendance
        public double TodayAttendanceRate { get; set; }
        public int TodayAttendancePresent { get; set; }
        public int TodayAttendanceAbsent { get; set; }

        // Revenue & Subscriptions
        public decimal TodayRevenue { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public decimal YearlyRevenue { get; set; }
        public int PendingSubscriptions { get; set; }
        public int ExpiredSubscriptions { get; set; }

        // User Platform Session
        public int OnlineUsers { get; set; }
        public int ActiveSessions { get; set; }
    }

    public class SuperAdminDashboardChartsDto
    {
        public List<ChartDataPoint<string, decimal>> MonthlyRevenue { get; set; } = new List<ChartDataPoint<string, decimal>>();
        public List<ChartDataPoint<string, int>> SchoolRegistrationTrend { get; set; } = new List<ChartDataPoint<string, int>>();
        public List<ChartDataPoint<string, int>> StudentGrowth { get; set; } = new List<ChartDataPoint<string, int>>();
        public List<ChartDataPoint<string, int>> EmployeeGrowth { get; set; } = new List<ChartDataPoint<string, int>>();
        public List<ChartDataPoint<string, int>> SubscriptionTrend { get; set; } = new List<ChartDataPoint<string, int>>();
        public List<ChartDataPoint<string, double>> AttendanceTrend { get; set; } = new List<ChartDataPoint<string, double>>();
        public List<ChartDataPoint<string, decimal>> FeeCollectionTrend { get; set; } = new List<ChartDataPoint<string, decimal>>();
        public List<ChartDataPoint<string, int>> MonthlyLoginTrend { get; set; } = new List<ChartDataPoint<string, int>>();
    }

    public class SuperAdminSchoolAnalyticsDto
    {
        public List<SuperAdminSchoolListItemDto> TopSchools { get; set; } = new List<SuperAdminSchoolListItemDto>(); // Top by Student count / Revenue
        public List<SuperAdminSchoolListItemDto> RecentlyRegistered { get; set; } = new List<SuperAdminSchoolListItemDto>();
        public List<SuperAdminSchoolListItemDto> ExpiringSoon { get; set; } = new List<SuperAdminSchoolListItemDto>();
        public List<SuperAdminSchoolListItemDto> Inactive { get; set; } = new List<SuperAdminSchoolListItemDto>();
    }

    public class SuperAdminSchoolListItemDto
    {
        public int Id { get; set; }
        public string SchoolName { get; set; } = string.Empty;
        public string SchoolCode { get; set; } = string.Empty;
        public int StudentCount { get; set; }
        public decimal RevenueGenerated { get; set; }
        public string RegistrationDate { get; set; } = string.Empty;
        public string ExpiryDate { get; set; } = string.Empty;
        public string PlanName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class SuperAdminSubscriptionAnalyticsDto
    {
        public List<ChartDataPoint<string, int>> PlanWiseSchools { get; set; } = new List<ChartDataPoint<string, int>>();
        public List<ChartDataPoint<string, int>> MonthlyRenewals { get; set; } = new List<ChartDataPoint<string, int>>();
        public List<SuperAdminUpcomingRenewalDto> UpcomingRenewals { get; set; } = new List<SuperAdminUpcomingRenewalDto>();
        public List<SuperAdminExpiredPlanDto> ExpiredPlans { get; set; } = new List<SuperAdminExpiredPlanDto>();
        public List<SuperAdminTrialUserDto> TrialUsers { get; set; } = new List<SuperAdminTrialUserDto>();
        public double RenewalPercentage { get; set; }
    }

    public class SuperAdminUpcomingRenewalDto
    {
        public string SchoolName { get; set; } = string.Empty;
        public string PlanName { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }

    public class SuperAdminExpiredPlanDto
    {
        public string SchoolName { get; set; } = string.Empty;
        public string PlanName { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public int ExpiredDays { get; set; }
    }

    public class SuperAdminTrialUserDto
    {
        public string SchoolName { get; set; } = string.Empty;
        public string PlanName { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public int RemainingDays { get; set; }
    }

    public class SuperAdminFinanceAnalyticsDto
    {
        public decimal TodayRevenue { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public decimal YearlyRevenue { get; set; }

        public int PendingPaymentsCount { get; set; }
        public decimal PendingPaymentsAmount { get; set; }

        public int SuccessfulPaymentsCount { get; set; }
        public decimal SuccessfulPaymentsAmount { get; set; }

        public int FailedPaymentsCount { get; set; }
        public decimal FailedPaymentsAmount { get; set; }

        public List<ChartDataPoint<string, decimal>> RevenueByPlan { get; set; } = new List<ChartDataPoint<string, decimal>>();
    }

    public class SuperAdminUserAnalyticsDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int InactiveUsers { get; set; }
        public int LockedUsers { get; set; }
        public int OnlineUsers { get; set; }
        public List<SuperAdminLatestLoginDto> RecentLogins { get; set; } = new List<SuperAdminLatestLoginDto>();
    }

    public class SuperAdminSystemHealthDto
    {
        public string ApiStatus { get; set; } = "Healthy";
        public string DatabaseStatus { get; set; } = "Healthy";
        public double CpuUsage { get; set; }
        public double RamUsage { get; set; }
        public double DiskUsage { get; set; }
        public string ApplicationVersion { get; set; } = "1.0.0";
        public string LastBackup { get; set; } = string.Empty;
        public string ServerTime { get; set; } = string.Empty;
    }

    public class SuperAdminActivityDto
    {
        public string Type { get; set; } = string.Empty; // SchoolRegistered, SubscriptionPurchased, StudentAdded, EmployeeAdded, FeePaid, AttendanceMarked, LoginHistory
        public string Message { get; set; } = string.Empty;
        public string RelativeTime { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }

    public class SuperAdminCalendarEventDto
    {
        public string Title { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty; // "yyyy-MM-dd"
        public string Category { get; set; } = string.Empty; // Event, Renewal, Holiday, Meeting
    }

    public class SuperAdminLatestSchoolDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RegistrationDate { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class SuperAdminLatestPaymentDto
    {
        public int Id { get; set; }
        public string SchoolName { get; set; } = string.Empty;
        public string PlanName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Date { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // Success, Pending, Failed
        public string TransactionId { get; set; } = string.Empty;
    }

    public class SuperAdminLatestLoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string SchoolName { get; set; } = string.Empty;
        public string LoginTime { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string Device { get; set; } = string.Empty;
    }
}
