namespace School_DTOs.Hr.Attendance
{
    public class AttendanceDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public TimeSpan? CheckInTime { get; set; }
        public TimeSpan? CheckOutTime { get; set; }
        public string Status { get; set; } = "Present"; public string? Remarks { get; set; }
    }

    public class CreateAttendanceDto
    {
        public int EmployeeId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public TimeSpan? CheckInTime { get; set; }
        public TimeSpan? CheckOutTime { get; set; }
        public string Status { get; set; } = "Present"; public string? Remarks { get; set; }
    }

    public class UpdateAttendanceDto : CreateAttendanceDto
    {
        public int Id { get; set; }
    }
}