namespace School_DTOs.Dashboard
{
    public class DashboardDto
    {
        public DashboardStatsDto Stats { get; set; } = new DashboardStatsDto();
        public List<ActivityDto> RecentActivities { get; set; } = new List<ActivityDto>();
        public List<RecentRegistrationDto> RecentRegistrations { get; set; } = new List<RecentRegistrationDto>();
        public List<UpcomingEventDto> UpcomingEvents { get; set; } = new List<UpcomingEventDto>();
        public FeeCollectionDto FeeCollection { get; set; } = new FeeCollectionDto();
        public AttendanceStatsDto AttendanceStats { get; set; } = new AttendanceStatsDto();
    }

    public class DashboardStatsDto
    {
        public int TotalStudents { get; set; }
        public int FacultyMembers { get; set; } // Can be 0 if not implemented
        public int ActiveCourses { get; set; }
        public int Departments { get; set; } // Affiliated colleges
        public decimal FeeCollectionTotal { get; set; }
        public decimal FeeCollectionCollected { get; set; }
        public decimal FeeCollectionPending { get; set; }
        public double FeeCollectionGrowth { get; set; }
        public int PendingApprovals { get; set; }
        public double AttendanceRate { get; set; }
        public int ExamsScheduled { get; set; }
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
        public string Time { get; set; } = string.Empty; // Format: "9:00 AM - 12:00 PM" or "All Day"
        public DateTime EventDate { get; set; }
    }




    public class FeeCollectionDto
    {
        public decimal Total { get; set; }
        public decimal Collected { get; set; }
        public decimal Pending { get; set; }
        public double Growth { get; set; } // Percentage growth from last month
    }

    public class AttendanceStatsDto
    {
        public double AttendanceRate { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }
    }
}

