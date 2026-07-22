namespace School_DTOs.Dashboard
{
    public class StudentDashboardDto
    {
        public StudentWelcomeDto Profile { get; set; } = new StudentWelcomeDto();
        public StudentStatsDto Stats { get; set; } = new StudentStatsDto();
        public List<StudentSubjectDto> Subjects { get; set; } = new List<StudentSubjectDto>();
        public List<StudentTimetableSlotDto> Timetable { get; set; } = new List<StudentTimetableSlotDto>();
        public List<StudentAssignmentDto> Assignments { get; set; } = new List<StudentAssignmentDto>();
        public List<StudentExamDto> Exams { get; set; } = new List<StudentExamDto>();
        public List<StudentExamResultDto> Results { get; set; } = new List<StudentExamResultDto>();
        public List<StudentFeeLogDto> Fees { get; set; } = new List<StudentFeeLogDto>();
        public List<StudentLibraryBookDto> LibraryBooks { get; set; } = new List<StudentLibraryBookDto>();
        public StudentTransportDto Transport { get; set; } = new StudentTransportDto();
        public StudentHostelDto Hostel { get; set; } = new StudentHostelDto();
        public List<StudentNoticeDto> Notices { get; set; } = new List<StudentNoticeDto>();
        public List<StudentCalendarEventDto> Calendar { get; set; } = new List<StudentCalendarEventDto>();
    }

    public class StudentWelcomeDto
    {
        public int Id { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string EnrollmentNumber { get; set; } = string.Empty;
        public string RollNumber { get; set; } = "N/A";
        public string Name { get; set; } = string.Empty;
        public string FathersName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
        public string ClassName { get; set; } = "N/A";
        public string SectionName { get; set; } = "N/A";
        public string BatchName { get; set; } = "N/A";
        public string BloodGroup { get; set; } = "N/A";
        public string EmergencyContact { get; set; } = "N/A";
        public string PostalAddress { get; set; } = "N/A";
        public string MobileNumber { get; set; } = "N/A";
        public string CourseOpted { get; set; } = "N/A";
        public string StudentPhoto { get; set; } = "assets/images/user.png";
        public string SchoolName { get; set; } = "School ERP";
        public string LastLogin { get; set; } = "N/A";
    }

    public class StudentStatsDto
    {
        public double AttendancePercentage { get; set; } = 100.0;
        public int TotalSubjects { get; set; } = 0;
        public int TodayClassesCount { get; set; } = 0;
        public int UpcomingExamsCount { get; set; } = 0;
        public int PendingAssignmentsCount { get; set; } = 0;
        public int CompletedAssignmentsCount { get; set; } = 0;
        public decimal PendingFees { get; set; } = 0;
        public int LibraryBooksCount { get; set; } = 0;
        public int UnreadNotificationsCount { get; set; } = 0;
    }

    public class StudentSubjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string TeacherName { get; set; } = "TBD";
    }

    public class StudentTimetableSlotDto
    {
        public int Id { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public string DayOfWeek { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public string RoomNo { get; set; } = string.Empty;
        public string SubjectColor { get; set; } = "#4f46e5";
    }

    public class StudentAssignmentDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string DueDate { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending"; // Pending, Submitted, Completed
        public string Description { get; set; } = string.Empty;
        public int? MarksObtained { get; set; }
        public int? MaxMarks { get; set; }
    }

    public class StudentExamDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string ExamDate { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public int MaxMarks { get; set; } = 100;
        public int PassingMarks { get; set; } = 40;
        public string RoomNo { get; set; } = "N/A";
    }

    public class StudentExamResultDto
    {
        public int Id { get; set; }
        public string ExamName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public int ScoredMarks { get; set; }
        public int OutOf { get; set; } = 100;
        public string Grade { get; set; } = "F";
        public string Status { get; set; } = "Fail"; // Pass, Fail
        public int ClassRank { get; set; }
    }

    public class StudentFeeLogDto
    {
        public int Id { get; set; }
        public string FeeType { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Pending"; // Paid, Pending, Overdue
        public string DueDate { get; set; } = string.Empty;
        public string PaymentDate { get; set; } = "N/A";
        public string ReceiptNumber { get; set; } = "N/A";
    }

    public class StudentLibraryBookDto
    {
        public int Id { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string IssueDate { get; set; } = string.Empty;
        public string DueDate { get; set; } = string.Empty;
        public string ReturnDate { get; set; } = "N/A";
        public string Status { get; set; } = "Issued"; // Issued, Returned, Overdue
        public decimal FineAmount { get; set; } = 0;
    }

    public class StudentTransportDto
    {
        public string RouteName { get; set; } = "N/A";
        public string VehicleNo { get; set; } = "N/A";
        public string DriverName { get; set; } = "N/A";
        public string DriverContact { get; set; } = "N/A";
        public string PickupPoint { get; set; } = "N/A";
        public string PickupTime { get; set; } = "N/A";
    }

    public class StudentHostelDto
    {
        public string HostelName { get; set; } = "N/A";
        public string RoomNo { get; set; } = "N/A";
        public string RoomType { get; set; } = "N/A";
        public string WardenName { get; set; } = "N/A";
        public string WardenContact { get; set; } = "N/A";
        public string AttendanceToday { get; set; } = "N/A";
    }

    public class StudentNoticeDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string NoticeType { get; set; } = "General"; // Holiday, ExamAlert, General, FeeAlert
        public string PublishDate { get; set; } = string.Empty;
    }

    public class StudentCalendarEventDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string EventDate { get; set; } = string.Empty;
        public string EventType { get; set; } = "Event"; // Exam, Holiday, Event, Birthday
        public string Description { get; set; } = string.Empty;
    }
}
