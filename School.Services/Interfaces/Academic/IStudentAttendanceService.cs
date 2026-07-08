using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace School.Services.Interfaces.Academic
{
    public class AttendanceRecordDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public string StudentCode { get; set; } = null!;
        public int? SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public int? ClassId { get; set; }
        public string? ClassName { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string Status { get; set; } = "Present";
        public int? PeriodNo { get; set; }
        public string? Remarks { get; set; }
    }

    public class BulkAttendanceRequest
    {
        public DateTime Date { get; set; }
        public int ClassId { get; set; }
        public int? SubjectId { get; set; }
        public int? PeriodNo { get; set; }
        public List<StudentAttendanceEntry> Entries { get; set; } = new();
    }

    public class StudentAttendanceEntry
    {
        public int StudentId { get; set; }
        public string Status { get; set; } = "Present"; // Present/Absent/Late/Leave
        public string? Remarks { get; set; }
    }

    public class AttendanceSummaryDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public int Present { get; set; }
        public int Absent { get; set; }
        public int Late { get; set; }
        public int Leave { get; set; }
        public int Total { get; set; }
        public double AttendancePercentage => Total > 0 ? Math.Round((Present + Late) * 100.0 / Total, 2) : 0;
    }

    public interface IStudentAttendanceService
    {
        Task<(bool Success, string Message, int Marked)> MarkBulkAttendanceAsync(BulkAttendanceRequest request, string markedBy, int schoolRegistrationId);
        Task<IEnumerable<AttendanceRecordDto>> GetByDateAndClassAsync(DateTime date, int classId, int schoolRegistrationId);
        Task<IEnumerable<AttendanceRecordDto>> GetByStudentAsync(int studentId, int schoolRegistrationId, DateTime? from, DateTime? to);
        Task<AttendanceSummaryDto> GetStudentSummaryAsync(int studentId, int schoolRegistrationId, DateTime? from, DateTime? to);
        Task<IEnumerable<AttendanceSummaryDto>> GetClassSummaryAsync(int classId, int month, int year, int schoolRegistrationId);
        Task<(bool Success, string Message)> UpdateAttendanceAsync(int id, string status, string? remarks, int schoolRegistrationId);
    }
}
