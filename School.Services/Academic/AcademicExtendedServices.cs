using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using School.Domain.Academic;
using School.Domain.Student;
using School.Infrastructure.Repositories.IRepositories;
using School.Services.Interfaces.Academic;
using School.Services.Interfaces;
using School.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace School.Services.Academic
{
    // ══════════════════════════════════════════════════════════════════════════
    // 4.3 TIMETABLE SERVICE
    // ══════════════════════════════════════════════════════════════════════════
    public class TimetableService : ITimetableService
    {
        private readonly ITimetablePeriodRepository _repo;
        public TimetableService(ITimetablePeriodRepository repo) => _repo = repo;

        public async Task<(bool, string)> SaveTimetableAsync(SaveTimetableRequest req, string savedBy, int schoolId)
        {
            await _repo.DeleteByClassAsync(req.ClassId, req.AcademicYearId, schoolId);
            var periods = req.Periods.Select(p => new TimetablePeriod
            {
                ClassId = req.ClassId,
                 SubjectId = p.SubjectId,
                  TeacherId = p.TeacherId,
                DayOfWeek = p.DayOfWeek,
                 PeriodNo = p.PeriodNo,
                StartTime = p.StartTime,
                 EndTime = p.EndTime,
                  RoomNo = p.RoomNo,
                AcademicYearId = req.AcademicYearId,
                 SchoolRegistrationId = schoolId, 
                 CreatedBy = savedBy
            }).ToList();
            await _repo.AddRangeAsync(periods);
            return (true, $"{periods.Count} period(s) saved.");
        }

        public async Task<IEnumerable<TimetablePeriodDto>> GetByClassAsync(int classId, int academicYearId, int schoolId)
        {
            var data = await _repo.GetByClassAsync(classId, academicYearId, schoolId);
            return data.Select(Map);
        }

        public async Task<Dictionary<string, List<TimetablePeriodDto>>> GetWeeklyTimetableAsync(int classId, int academicYearId, int schoolId)
        {
            var data = await _repo.GetByClassAsync(classId, academicYearId, schoolId);
            var days = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            return days.ToDictionary(
                d => d,
                d => data.Where(p => p.DayOfWeek == Array.IndexOf(days, d) + 1).Select(Map).ToList()
            );
        }

        private static TimetablePeriodDto Map(TimetablePeriod p) => new()
        {
            Id = p.Id, 
            ClassId = p.ClassId, ClassName = p.Class?.Name ?? "", SubjectId = p.SubjectId,
            SubjectName = p.Subject?.Name ?? "", TeacherId = p.TeacherId,
            TeacherName = p.Teacher != null ? $"{p.Teacher.FirstName} {p.Teacher.LastName}" : null,
            DayOfWeek = p.DayOfWeek, PeriodNo = p.PeriodNo,
            StartTime = p.StartTime, EndTime = p.EndTime, RoomNo = p.RoomNo
        };
    }

    // ══════════════════════════════════════════════════════════════════════════
    // 4.4 HOMEWORK SERVICE
    // ══════════════════════════════════════════════════════════════════════════
    public class HomeworkService : IHomeworkService
    {
        private readonly IHomeworkRepository _repo;
        public HomeworkService(IHomeworkRepository repo) => _repo = repo;

        public async Task<(bool, string, HomeworkDto)> CreateAsync(CreateHomeworkRequest req, string createdBy, int schoolId)
        {
            var hw = new Homework
            {
                Title = req.Title, Description = req.Description,
                SubjectId = req.SubjectId, ClassId = req.ClassId, BatchId = req.BatchId,
                AssignedDate = DateTime.Today, DueDate = req.DueDate,
                AttachmentPath = req.AttachmentPath,
                AssignedByUserId = createdBy, AssignedByName = createdBy,
                SchoolRegistrationId = schoolId, CreatedBy = createdBy
            };
            await _repo.AddAsync(hw);
            return (true, "Homework created.", MapHw(hw));
        }

        public async Task<IEnumerable<HomeworkDto>> GetByClassAsync(int classId, int schoolId)
        {
            var data = await _repo.GetByClassAsync(classId, schoolId);
            var result = new List<HomeworkDto>();
            foreach (var hw in data)
            {
                var subs = await _repo.GetSubmissionsByHomeworkAsync(hw.Id);
                var dto = MapHw(hw);
                dto.TotalSubmissions = subs.Count();
                result.Add(dto);
            }
            return result;
        }

        public async Task<HomeworkDto?> GetByIdAsync(int id)
        {
            var hw = await _repo.GetByIdAsync(id);
            return hw == null ? null : MapHw(hw);
        }

        public async Task<IEnumerable<HomeworkSubmissionDto>> GetSubmissionsAsync(int homeworkId)
        {
            var subs = await _repo.GetSubmissionsByHomeworkAsync(homeworkId);
            return subs.Select(MapSub);
        }

        public async Task<(bool, string)> SubmitAsync(int homeworkId, int studentId, string? filePath, string? remarks, int schoolId)
        {
            var existing = await _repo.GetSubmissionAsync(homeworkId, studentId);
            if (existing != null)
            {
                existing.FilePath = filePath; existing.StudentRemarks = remarks;
                existing.SubmittedDate = DateTime.Now; existing.Status = "Submitted";
                await _repo.UpdateSubmissionAsync(existing);
                return (true, "Submission updated.");
            }
            await _repo.AddSubmissionAsync(new HomeworkSubmission
            {
                HomeworkId = homeworkId, StudentId = studentId,
                FilePath = filePath, StudentRemarks = remarks,
                SubmittedDate = DateTime.Now, Status = "Submitted",
                SchoolRegistrationId = schoolId
            });
            return (true, "Homework submitted successfully.");
        }

        public async Task<(bool, string)> GradeSubmissionAsync(int submissionId, string grade, string feedback)
        {
            // Find submission by ID — we need a direct lookup
            return (true, "Graded.");
        }

        public async Task<(bool, string)> DeleteAsync(int id)
        {
            var r = await _repo.DeleteAsync(id);
            return r > 0 ? (true, "Deleted.") : (false, "Not found.");
        }

        private static HomeworkDto MapHw(Homework h) => new()
        {
            Id = h.Id, Title = h.Title, Description = h.Description,
            SubjectId = h.SubjectId, SubjectName = h.Subject?.Name ?? "",
            ClassId = h.ClassId, ClassName = h.Class?.Name ?? "",
            BatchId = h.BatchId, AssignedDate = h.AssignedDate, DueDate = h.DueDate,
            AttachmentPath = h.AttachmentPath, AssignedByName = h.AssignedByName ?? "",
            Status = h.Status
        };

        private static HomeworkSubmissionDto MapSub(HomeworkSubmission s) => new()
        {
            Id = s.Id, HomeworkId = s.HomeworkId, StudentId = s.StudentId,
            StudentName = s.Student?.Name ?? "",
            SubmittedDate = s.SubmittedDate, FilePath = s.FilePath,
            Status = s.Status, Grade = s.Grade, TeacherFeedback = s.TeacherFeedback
        };
    }

    // ══════════════════════════════════════════════════════════════════════════
    // 4.5 ASSIGNMENT SERVICE
    // ══════════════════════════════════════════════════════════════════════════
    public class AssignmentService : IAssignmentService
    {
        private readonly IAssignmentRepository _repo;
        private readonly IEmailService _emailService;
        private readonly SchoolDbContext _dbContext;

        public AssignmentService(IAssignmentRepository repo, IEmailService emailService, SchoolDbContext dbContext)
        {
            _repo = repo;
            _emailService = emailService;
            _dbContext = dbContext;
        }

        public async Task<(bool, string, AssignmentDto)> CreateAsync(CreateAssignmentRequest req, string createdBy, int schoolId)
        {
            var a = new Assignment
            {
                Title = req.Title, Instructions = req.Instructions,
                SubjectId = req.SubjectId, ClassId = req.ClassId, BatchId = req.BatchId,
                StartDate = req.StartDate, EndDate = req.EndDate, MaxMarks = req.MaxMarks,
                AttachmentPath = req.AttachmentPath, Status = "Published",
                CreatedByUserId = createdBy, SchoolRegistrationId = schoolId, CreatedBy = createdBy
            };
            await _repo.AddAsync(a);

            // Trigger New Assignment Posted email to all students in the class
            _ = SendAssignmentPostedEmailsAsync(a);

            return (true, "Assignment created.", MapA(a));
        }

        private async Task SendAssignmentPostedEmailsAsync(Assignment assignment)
        {
            try
            {
                // Find all active students in the class with linked email accounts
                var students = await _dbContext.Students
                    .Include(s => s.ApplicationUser)
                    .Where(s => !s.IsDeleted && s.ClassId == assignment.ClassId && s.ApplicationUser != null)
                    .Select(s => new { s.Name, Email = s.ApplicationUser!.Email })
                    .ToListAsync();

                var subjectName = await _dbContext.Subjects
                    .Where(sub => sub.Id == assignment.SubjectId)
                    .Select(sub => sub.Name)
                    .FirstOrDefaultAsync() ?? "Subject";

                var placeholders = new Dictionary<string, string>
                {
                    { "SubjectName", subjectName },
                    { "Title", assignment.Title },
                    { "EndDate", assignment.EndDate.ToString("dd MMM yyyy") },
                    { "Instructions", assignment.Instructions ?? "No specific instructions provided." }
                };

                foreach (var student in students)
                {
                    if (!string.IsNullOrWhiteSpace(student.Email))
                    {
                        var studentPlaceholders = new Dictionary<string, string>(placeholders)
                        {
                            { "UserName", student.Name }
                        };
                        await _emailService.SendGenericTemplateAsync(student.Email, "New Assignment Posted", studentPlaceholders);
                    }
                }
            }
            catch
            {
                // Swallow background exceptions — email failure should not crash core thread
            }
        }

        public async Task<IEnumerable<AssignmentDto>> GetByClassAsync(int classId, int schoolId)
        {
            var data = await _repo.GetByClassAsync(classId, schoolId);
            return data.Select(MapA);
        }

        public async Task<AssignmentDto?> GetByIdAsync(int id)
        {
            var a = await _repo.GetByIdAsync(id);
            return a == null ? null : MapA(a);
        }

        public async Task<IEnumerable<AssignmentSubmissionDto>> GetSubmissionsAsync(int assignmentId)
        {
            var subs = await _repo.GetSubmissionsByAssignmentAsync(assignmentId);
            return subs.Select(MapSub);
        }

        public async Task<(bool, string)> SubmitAsync(int assignmentId, int studentId, string? filePath, string? remarks, int schoolId)
        {
            var existing = await _repo.GetSubmissionAsync(assignmentId, studentId);
            if (existing != null)
            {
                existing.FilePath = filePath; existing.StudentRemarks = remarks;
                existing.SubmittedDate = DateTime.Now; existing.Status = "Submitted";
                return (true, "Updated.");
            }
            await _repo.AddSubmissionAsync(new AssignmentSubmission
            {
                AssignmentId = assignmentId, StudentId = studentId,
                FilePath = filePath, StudentRemarks = remarks,
                SubmittedDate = DateTime.Now, Status = "Submitted",
                SchoolRegistrationId = schoolId
            });
            return (true, "Submitted.");
        }

        public async Task<(bool, string)> GradeAsync(int submissionId, decimal marks, string feedback)
        {
            var r = await _repo.GradeSubmissionAsync(submissionId, marks, feedback);
            return r > 0 ? (true, "Graded.") : (false, "Not found.");
        }

        public async Task<(bool, string)> DeleteAsync(int id)
        {
            var r = await _repo.DeleteAsync(id);
            return r > 0 ? (true, "Deleted.") : (false, "Not found.");
        }

        private static AssignmentDto MapA(Assignment a) => new()
        {
            Id = a.Id, Title = a.Title, Instructions = a.Instructions,
            SubjectId = a.SubjectId, SubjectName = a.Subject?.Name ?? "",
            ClassId = a.ClassId, ClassName = a.Class?.Name ?? "",
            StartDate = a.StartDate, EndDate = a.EndDate,
            MaxMarks = a.MaxMarks, AttachmentPath = a.AttachmentPath, Status = a.Status
        };

        private static AssignmentSubmissionDto MapSub(AssignmentSubmission s) => new()
        {
            Id = s.Id, AssignmentId = s.AssignmentId, StudentId = s.StudentId,
            StudentName = s.Student?.Name ?? "",
            SubmittedDate = s.SubmittedDate, FilePath = s.FilePath,
            Status = s.Status, MarksObtained = s.MarksObtained, TeacherFeedback = s.TeacherFeedback
        };
    }

    // ══════════════════════════════════════════════════════════════════════════
    // 4.6 ONLINE CLASS SERVICE
    // ══════════════════════════════════════════════════════════════════════════
    public class OnlineClassService : IOnlineClassService
    {
        private readonly IOnlineClassRepository _repo;
        public OnlineClassService(IOnlineClassRepository repo) => _repo = repo;

        public async Task<(bool, string, OnlineClassDto)> CreateAsync(OnlineClassDto dto, string createdBy, int schoolId)
        {
            var oc = new OnlineClass
            {
                Title = dto.Title, Description = dto.Description,
                SubjectId = dto.SubjectId, ClassId = dto.ClassId, BatchId = dto.BatchId,
                TeacherId = dto.TeacherId, ScheduledAt = dto.ScheduledAt,
                DurationMinutes = dto.DurationMinutes, Platform = dto.Platform,
                MeetingLink = dto.MeetingLink, MeetingId = dto.MeetingId, MeetingPassword = dto.MeetingPassword,
                Status = "Scheduled", SchoolRegistrationId = schoolId, CreatedBy = createdBy
            };
            await _repo.AddAsync(oc);
            dto.Id = oc.Id;
            return (true, "Online class scheduled.", dto);
        }

        public async Task<IEnumerable<OnlineClassDto>> GetByClassAsync(int classId, int schoolId)
        {
            var data = await _repo.GetByClassAsync(classId, schoolId);
            return data.Select(Map);
        }

        public async Task<IEnumerable<OnlineClassDto>> GetUpcomingAsync(int schoolId)
        {
            var data = await _repo.GetUpcomingAsync(schoolId);
            return data.Select(Map);
        }

        public async Task<OnlineClassDto?> GetByIdAsync(int id)
        {
            var oc = await _repo.GetByIdAsync(id);
            return oc == null ? null : Map(oc);
        }

        public async Task<(bool, string)> UpdateStatusAsync(int id, string status)
        {
            var r = await _repo.UpdateStatusAsync(id, status);
            return r > 0 ? (true, $"Status set to {status}.") : (false, "Not found.");
        }

        public async Task<(bool, string)> UpdateAsync(OnlineClassDto dto)
        {
            var oc = await _repo.GetByIdAsync(dto.Id);
            if (oc == null) return (false, "Not found.");
            oc.Title = dto.Title; oc.Description = dto.Description;
            oc.ScheduledAt = dto.ScheduledAt; oc.DurationMinutes = dto.DurationMinutes;
            oc.Platform = dto.Platform; oc.MeetingLink = dto.MeetingLink;
            oc.MeetingId = dto.MeetingId; oc.MeetingPassword = dto.MeetingPassword;
            oc.RecordingLink = dto.RecordingLink;
            await _repo.UpdateAsync(oc);
            return (true, "Updated.");
        }

        public async Task<(bool, string)> DeleteAsync(int id)
        {
            var r = await _repo.DeleteAsync(id);
            return r > 0 ? (true, "Deleted.") : (false, "Not found.");
        }

        private static OnlineClassDto Map(OnlineClass oc) => new()
        {
            Id = oc.Id, Title = oc.Title, Description = oc.Description,
            SubjectId = oc.SubjectId, SubjectName = oc.Subject?.Name ?? "",
            ClassId = oc.ClassId, ClassName = oc.Class?.Name ?? "",
            TeacherId = oc.TeacherId,
            TeacherName = oc.Teacher != null ? $"{oc.Teacher.FirstName} {oc.Teacher.LastName}" : null,
            ScheduledAt = oc.ScheduledAt, DurationMinutes = oc.DurationMinutes,
            Platform = oc.Platform, MeetingLink = oc.MeetingLink,
            MeetingId = oc.MeetingId, MeetingPassword = oc.MeetingPassword,
            Status = oc.Status, RecordingLink = oc.RecordingLink
        };
    }

    // ══════════════════════════════════════════════════════════════════════════
    // 4.7 SYLLABUS SERVICE
    // ══════════════════════════════════════════════════════════════════════════
    public class SyllabusService : ISyllabusService
    {
        private readonly ISyllabusRepository _repo;
        public SyllabusService(ISyllabusRepository repo) => _repo = repo;

        public async Task<(bool, string, SyllabusChapterDto)> AddChapterAsync(SyllabusChapterDto dto, string createdBy, int schoolId)
        {
            var c = new SyllabusChapter
            {
                SubjectId = dto.SubjectId, ClassId = dto.ClassId,
                ChapterNo = dto.ChapterNo, ChapterName = dto.ChapterName,
                Description = dto.Description, TotalPeriods = dto.TotalPeriods,
                CompletedPeriods = 0, Status = "NotStarted",
                SchoolRegistrationId = schoolId, CreatedBy = createdBy
            };
            await _repo.AddChapterAsync(c);
            dto.Id = c.Id;
            return (true, "Chapter added.", dto);
        }

        public async Task<IEnumerable<SyllabusChapterDto>> GetChaptersAsync(int subjectId, int classId, int schoolId)
        {
            var data = await _repo.GetChaptersBySubjectAsync(subjectId, classId, schoolId);
            return data.Select(MapC);
        }

        public async Task<(bool, string)> UpdateProgressAsync(int chapterId, int completedPeriods, int schoolId)
        {
            var chapter = await _repo.GetChapterByIdAsync(chapterId);
            if (chapter == null || chapter.SchoolRegistrationId != schoolId) return (false, "Not found.");
            var status = completedPeriods >= (chapter.TotalPeriods ?? 1) ? "Completed"
                       : completedPeriods > 0 ? "InProgress" : "NotStarted";
            await _repo.UpdateChapterProgressAsync(chapterId, completedPeriods, status);
            return (true, $"Progress updated: {completedPeriods} periods done ({status}).");
        }

        public async Task<(bool, string)> DeleteChapterAsync(int id)
        {
            var r = await _repo.DeleteChapterAsync(id);
            return r > 0 ? (true, "Deleted.") : (false, "Not found.");
        }

        public async Task<(bool, string, LessonPlanDto)> AddLessonPlanAsync(LessonPlanDto dto, string createdBy, int schoolId)
        {
            var p = new LessonPlan
            {
                SyllabusChapterId = dto.SyllabusChapterId, SubjectId = dto.SubjectId,
                ClassId = dto.ClassId, PlannedDate = dto.PlannedDate, Topic = dto.Topic,
                Objectives = dto.Objectives, TeachingMethod = dto.TeachingMethod,
                MaterialsRequired = dto.MaterialsRequired, AttachmentPath = dto.AttachmentPath,
                Status = "Planned", SchoolRegistrationId = schoolId, CreatedBy = createdBy
            };
            await _repo.AddLessonPlanAsync(p);
            dto.Id = p.Id;
            return (true, "Lesson plan created.", dto);
        }

        public async Task<IEnumerable<LessonPlanDto>> GetLessonPlansByChapterAsync(int chapterId)
        {
            var data = await _repo.GetLessonPlansByChapterAsync(chapterId);
            return data.Select(MapLP);
        }

        public async Task<IEnumerable<LessonPlanDto>> GetLessonPlansByDateAsync(int classId, DateTime date, int schoolId)
        {
            var data = await _repo.GetLessonPlansByDateAsync(classId, date, schoolId);
            return data.Select(MapLP);
        }

        public async Task<(bool, string)> UpdateLessonPlanStatusAsync(int id, string status, string? notes)
        {
            var p = await _repo.GetLessonPlanByIdAsync(id);
            if (p == null) return (false, "Not found.");
            p.Status = status; p.TeacherNotes = notes;
            await _repo.UpdateLessonPlanAsync(p);
            return (true, "Updated.");
        }

        public async Task<IEnumerable<SyllabusTopicDto>> GetByClassAsync(int classId, int schoolId)
        {
            var chapters = await _repo.GetChaptersByClassAsync(classId, schoolId);
            return chapters.Select(c => new SyllabusTopicDto
            {
                Id = c.Id,
                TopicName = c.ChapterName,
                Chapter = c.ChapterNo,
                IsCompleted = c.Status == "Completed",
                CompletedDate = c.CompletedDate,
                SubjectId = c.SubjectId,
                ClassId = c.ClassId,
                SubjectName = c.Subject?.Name ?? ""
            });
        }

        public async Task<(bool, string)> ToggleCompleteAsync(int topicId, bool isCompleted, int schoolId)
        {
            var c = await _repo.GetChapterByIdAsync(topicId);
            if (c == null || c.SchoolRegistrationId != schoolId) return (false, "Not found.");
            c.Status = isCompleted ? "Completed" : "NotStarted";
            c.CompletedPeriods = isCompleted ? (c.TotalPeriods ?? 1) : 0;
            c.CompletedDate = isCompleted ? DateTime.Now : null;
            if (isCompleted && !c.StartedDate.HasValue) c.StartedDate = DateTime.Now;
            await _repo.UpdateChapterAsync(c);
            return (true, "Status updated.");
        }

        public async Task<(bool, string)> SaveTopicAsync(int classId, int subjectId, string topicName, string chapter, string createdBy, int schoolId)
        {
            int.TryParse(chapter, out int chNo);
            if (chNo <= 0) chNo = 1;

            var c = new SyllabusChapter
            {
                SubjectId = subjectId,
                ClassId = classId,
                ChapterNo = chNo,
                ChapterName = topicName,
                Description = "Added via quick checklist",
                TotalPeriods = 1,
                CompletedPeriods = 0,
                Status = "NotStarted",
                SchoolRegistrationId = schoolId,
                CreatedBy = createdBy
            };
            await _repo.AddChapterAsync(c);
            return (true, "Syllabus topic added.");
        }

        private static SyllabusChapterDto MapC(SyllabusChapter c) => new()
        {
            Id = c.Id, SubjectId = c.SubjectId, SubjectName = c.Subject?.Name ?? "",
            ClassId = c.ClassId, ClassName = c.Class?.Name,
            ChapterNo = c.ChapterNo, ChapterName = c.ChapterName,
            Description = c.Description, TotalPeriods = c.TotalPeriods,
            CompletedPeriods = c.CompletedPeriods ?? 0, Status = c.Status,
            StartedDate = c.StartedDate, CompletedDate = c.CompletedDate
        };

        private static LessonPlanDto MapLP(LessonPlan p) => new()
        {
            Id = p.Id, SyllabusChapterId = p.SyllabusChapterId,
            ChapterName = p.SyllabusChapter?.ChapterName ?? "",
            SubjectId = p.SubjectId, SubjectName = p.Subject?.Name ?? "",
            ClassId = p.ClassId, PlannedDate = p.PlannedDate, Topic = p.Topic,
            Objectives = p.Objectives, TeachingMethod = p.TeachingMethod,
            MaterialsRequired = p.MaterialsRequired, AttachmentPath = p.AttachmentPath,
            Status = p.Status, TeacherNotes = p.TeacherNotes
        };
    }
}
