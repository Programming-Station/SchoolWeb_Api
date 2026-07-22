namespace School.Services.Interfaces.Academic
{
    public class SubjectEnrollmentDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public string StudentCode { get; set; } = null!;
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = null!;
        public string? SubjectCode { get; set; }
        public int? BatchId { get; set; }
        public string? BatchName { get; set; }
        public int? YearSemesterId { get; set; }
        public string? YearSemesterName { get; set; }
        public int? ClassId { get; set; }
        public string? ClassName { get; set; }
        public string Status { get; set; } = "Enrolled";
        public DateTime EnrolledDate { get; set; }
        public string? Remarks { get; set; }
    }

    public class EnrollSubjectRequest
    {
        public int StudentId { get; set; }
        public List<int> SubjectIds { get; set; } = new();
        public int? BatchId { get; set; }
        public int? YearSemesterId { get; set; }
        public int? ClassId { get; set; }
    }

    public interface ISubjectEnrollmentService
    {
        Task<(bool Success, string Message, List<SubjectEnrollmentDto> Enrolled)> EnrollSubjectsAsync(EnrollSubjectRequest request, string enrolledBy, int schoolRegistrationId);
        Task<(bool Success, string Message)> DropSubjectAsync(int enrollmentId, string remarks, int schoolRegistrationId);
        Task<IEnumerable<SubjectEnrollmentDto>> GetByStudentAsync(int studentId, int schoolRegistrationId);
        Task<IEnumerable<SubjectEnrollmentDto>> GetByClassAsync(int classId, int schoolRegistrationId);
        Task<IEnumerable<SubjectEnrollmentDto>> GetByBatchAsync(int batchId, int schoolRegistrationId);
    }
}
