namespace School_DTOs.Hr.Attendance
{
    public class AttendanceLogDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime LogTime { get; set; }
        public string LogType { get; set; } = null!; public string? Source { get; set; }
        public string? DeviceId { get; set; }
    }

    public class CreateAttendanceLogDto
    {
        public int EmployeeId { get; set; }
        public DateTime LogTime { get; set; }
        public string LogType { get; set; } = null!; public string? Source { get; set; }
        public string? DeviceId { get; set; }
    }

    public class UpdateAttendanceLogDto : CreateAttendanceLogDto
    {
        public int Id { get; set; }
    }
}