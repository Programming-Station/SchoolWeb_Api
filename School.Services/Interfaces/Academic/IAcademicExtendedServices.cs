using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace School.Services.Interfaces.Academic
{
    // ─── Timetable ───────────────────────────────────────────────────────────
    public class TimetablePeriodDto
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; } = null!;
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = null!;
        public int? TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public int DayOfWeek { get; set; }
        public string DayName => DayOfWeek switch { 1=>"Monday",2=>"Tuesday",3=>"Wednesday",4=>"Thursday",5=>"Friday",6=>"Saturday",_=>"" };
        public int PeriodNo { get; set; }
        public string StartTime { get; set; } = null!;
        public string EndTime { get; set; } = null!;
        public string? RoomNo { get; set; }
    }

    public class SaveTimetableRequest
    {
        public int ClassId { get; set; }
        public int AcademicYearId { get; set; }
        public List<TimetablePeriodDto> Periods { get; set; } = new();
    }

    public interface ITimetableService
    {
        Task<(bool Success, string Message)> SaveTimetableAsync(SaveTimetableRequest request, string savedBy, int schoolId);
        Task<IEnumerable<TimetablePeriodDto>> GetByClassAsync(int classId, int academicYearId, int schoolId);
        Task<Dictionary<string, List<TimetablePeriodDto>>> GetWeeklyTimetableAsync(int classId, int academicYearId, int schoolId);
    }

    // ─── Homework ─────────────────────────────────────────────────────────────
    public class HomeworkDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = null!;
        public int ClassId { get; set; }
        public string ClassName { get; set; } = null!;
        public int? BatchId { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime DueDate { get; set; }
        public string? AttachmentPath { get; set; }
        public string AssignedByName { get; set; } = null!;
        public string Status { get; set; } = "Active";
        public int TotalSubmissions { get; set; }
    }

    public class HomeworkSubmissionDto
    {
        public int Id { get; set; }
        public int HomeworkId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public DateTime? SubmittedDate { get; set; }
        public string? FilePath { get; set; }
        public string Status { get; set; } = "Missing";
        public string? Grade { get; set; }
        public string? TeacherFeedback { get; set; }
    }

    public class CreateHomeworkRequest
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int SubjectId { get; set; }
        public int ClassId { get; set; }
        public int? BatchId { get; set; }
        public DateTime DueDate { get; set; }
        public string? AttachmentPath { get; set; }
    }

    public interface IHomeworkService
    {
        Task<(bool Success, string Message, HomeworkDto Hw)> CreateAsync(CreateHomeworkRequest req, string createdBy, int schoolId);
        Task<IEnumerable<HomeworkDto>> GetByClassAsync(int classId, int schoolId);
        Task<HomeworkDto?> GetByIdAsync(int id);
        Task<IEnumerable<HomeworkSubmissionDto>> GetSubmissionsAsync(int homeworkId);
        Task<(bool Success, string Message)> SubmitAsync(int homeworkId, int studentId, string? filePath, string? remarks, int schoolId);
        Task<(bool Success, string Message)> GradeSubmissionAsync(int submissionId, string grade, string feedback);
        Task<(bool Success, string Message)> DeleteAsync(int id);
    }

    // ─── Assignment ───────────────────────────────────────────────────────────
    public class AssignmentDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Instructions { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = null!;
        public int ClassId { get; set; }
        public string ClassName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal MaxMarks { get; set; }
        public string? AttachmentPath { get; set; }
        public string Status { get; set; } = "Published";
        public int TotalSubmissions { get; set; }
    }

    public class AssignmentSubmissionDto
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public DateTime? SubmittedDate { get; set; }
        public string? FilePath { get; set; }
        public string Status { get; set; } = "Pending";
        public decimal? MarksObtained { get; set; }
        public string? TeacherFeedback { get; set; }
    }

    public class CreateAssignmentRequest
    {
        public string Title { get; set; } = null!;
        public string? Instructions { get; set; }
        public int SubjectId { get; set; }
        public int ClassId { get; set; }
        public int? BatchId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal MaxMarks { get; set; } = 100;
        public string? AttachmentPath { get; set; }
    }

    public interface IAssignmentService
    {
        Task<(bool Success, string Message, AssignmentDto Assignment)> CreateAsync(CreateAssignmentRequest req, string createdBy, int schoolId);
        Task<IEnumerable<AssignmentDto>> GetByClassAsync(int classId, int schoolId);
        Task<AssignmentDto?> GetByIdAsync(int id);
        Task<IEnumerable<AssignmentSubmissionDto>> GetSubmissionsAsync(int assignmentId);
        Task<(bool Success, string Message)> SubmitAsync(int assignmentId, int studentId, string? filePath, string? remarks, int schoolId);
        Task<(bool Success, string Message)> GradeAsync(int submissionId, decimal marks, string feedback);
        Task<(bool Success, string Message)> DeleteAsync(int id);
    }

    // ─── Online Classes ───────────────────────────────────────────────────────
    public class OnlineClassDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = null!;
        public int ClassId { get; set; }
        public string ClassName { get; set; } = null!;
        public int? BatchId { get; set; }
        public int? TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public DateTime ScheduledAt { get; set; }
        public int DurationMinutes { get; set; }
        public string Platform { get; set; } = "Zoom";
        public string? MeetingLink { get; set; }
        public string? MeetingId { get; set; }
        public string? MeetingPassword { get; set; }
        public string Status { get; set; } = "Scheduled";
        public string? RecordingLink { get; set; }
    }

    public interface IOnlineClassService
    {
        Task<(bool Success, string Message, OnlineClassDto Class)> CreateAsync(OnlineClassDto dto, string createdBy, int schoolId);
        Task<IEnumerable<OnlineClassDto>> GetByClassAsync(int classId, int schoolId);
        Task<IEnumerable<OnlineClassDto>> GetUpcomingAsync(int schoolId);
        Task<OnlineClassDto?> GetByIdAsync(int id);
        Task<(bool Success, string Message)> UpdateStatusAsync(int id, string status);
        Task<(bool Success, string Message)> UpdateAsync(OnlineClassDto dto);
        Task<(bool Success, string Message)> DeleteAsync(int id);
    }

    // ─── Syllabus ─────────────────────────────────────────────────────────────
    public class SyllabusChapterDto
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = null!;
        public int? ClassId { get; set; }
        public string? ClassName { get; set; }
        public int ChapterNo { get; set; }
        public string ChapterName { get; set; } = null!;
        public string? Description { get; set; }
        public int? TotalPeriods { get; set; }
        public int CompletedPeriods { get; set; }
        public double CompletionPercent => TotalPeriods > 0 ? Math.Round((double)CompletedPeriods / TotalPeriods.Value * 100, 1) : 0;
        public string Status { get; set; } = "NotStarted";
        public DateTime? StartedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
    }

    public class LessonPlanDto
    {
        public int Id { get; set; }
        public int SyllabusChapterId { get; set; }
        public string ChapterName { get; set; } = null!;
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = null!;
        public int ClassId { get; set; }
        public DateTime PlannedDate { get; set; }
        public string Topic { get; set; } = null!;
        public string? Objectives { get; set; }
        public string? TeachingMethod { get; set; }
        public string? MaterialsRequired { get; set; }
        public string? AttachmentPath { get; set; }
        public string Status { get; set; } = "Planned";
        public string? TeacherNotes { get; set; }
    }

    public interface ISyllabusService
    {
        Task<(bool Success, string Message, SyllabusChapterDto Chapter)> AddChapterAsync(SyllabusChapterDto dto, string createdBy, int schoolId);
        Task<IEnumerable<SyllabusChapterDto>> GetChaptersAsync(int subjectId, int classId, int schoolId);
        Task<(bool Success, string Message)> UpdateProgressAsync(int chapterId, int completedPeriods, int schoolId);
        Task<(bool Success, string Message)> DeleteChapterAsync(int id);
        Task<(bool Success, string Message, LessonPlanDto Plan)> AddLessonPlanAsync(LessonPlanDto dto, string createdBy, int schoolId);
        Task<IEnumerable<LessonPlanDto>> GetLessonPlansByChapterAsync(int chapterId);
        Task<IEnumerable<LessonPlanDto>> GetLessonPlansByDateAsync(int classId, DateTime date, int schoolId);
        Task<(bool Success, string Message)> UpdateLessonPlanStatusAsync(int id, string status, string? notes);
    }
}
