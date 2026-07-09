using School_DTOs;
using Microsoft.EntityFrameworkCore;
using School.Domain.Academic;
using School.Infrastructure;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Services.Interfaces;
using School.Services.Interfaces.Academic;
using School_DTOs.Academic;
using School_DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace School.Services.Academic
{
    public class ExamService : IExamService
    {
        private readonly IRepository<Exam> _repo;
        private readonly IUnitOfWork _uow;
        private readonly IEmailService _emailService;
        private readonly SchoolDbContext _dbContext;

        public ExamService(IRepository<Exam> repo, IUnitOfWork uow, IEmailService emailService, SchoolDbContext dbContext)
        {
            _repo = repo;
            _uow = uow;
            _emailService = emailService;
            _dbContext = dbContext;
        }

        public async Task<APIResponse<List<ExamDto>>> GetAllAsync()
        {
            var d = await _repo.List()
                .Select(x => new ExamDto { Id = x.Id, Name = x.Name, ExamType = x.ExamType, StartDate = x.StartDate, EndDate = x.EndDate, Status = x.Status, Description = x.Description, IsResultPublished = x.IsResultPublished, ResultPublishedDate = x.ResultPublishedDate, ResultPublishedBy = x.ResultPublishedBy })
                .ToListAsync();
            return new APIResponse<List<ExamDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = d };
        }

        public async Task<APIResponse<ExamDto>> GetByIdAsync(int id)
        {
            var x = await _repo.List().Where(e => e.Id == id)
                .Select(x => new ExamDto { Id = x.Id, Name = x.Name, ExamType = x.ExamType, StartDate = x.StartDate, EndDate = x.EndDate, Status = x.Status, Description = x.Description, IsResultPublished = x.IsResultPublished, ResultPublishedDate = x.ResultPublishedDate, ResultPublishedBy = x.ResultPublishedBy })
                .FirstOrDefaultAsync();
            if (x == null) return new APIResponse<ExamDto> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            return new APIResponse<ExamDto> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = x };
        }

        public async Task<APIResponse<object>> CreateAsync(CreateExamDto dto, string username)
        {
            var entity = new Exam
            {
                Name = dto.Name,
                ExamType = dto.ExamType,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = dto.Status,
                Description = dto.Description,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };
            await _repo.AddAsync(entity);
            await _uow.CommitAsync();

            // Send Exam Schedule Published email to all students in this school
            // Fire-and-forget to avoid blocking the API response
            _ = SendExamScheduleEmailsAsync(entity, username);

            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Created successfully" };
        }

        private async Task SendExamScheduleEmailsAsync(Exam exam, string username)
        {
            try
            {
                // Get all students for this school with a linked ApplicationUser email
                var students = await _dbContext.Students
                    .Include(s => s.ApplicationUser)
                    .Where(s => !s.IsDeleted && s.ApplicationUser != null && s.SchoolRegistrationId == exam.SchoolRegistrationId)
                    .Select(s => new { s.Name, Email = s.ApplicationUser!.Email, s.StudentId })
                    .ToListAsync();

                var placeholders = new Dictionary<string, string>
                {
                    { "SchoolName", "School" },
                    { "ExamName", exam.Name },
                    { "ExamType", exam.ExamType ?? "-" },
                    { "StartDate", exam.StartDate.ToString("dd MMM yyyy") },
                    { "EndDate", exam.EndDate.ToString("dd MMM yyyy") },
                    { "Status", exam.Status },
                    { "LoginUrl", "#" }
                };

                foreach (var student in students)
                {
                    if (!string.IsNullOrWhiteSpace(student.Email))
                    {
                        var studentPlaceholders = new Dictionary<string, string>(placeholders)
                        {
                            { "UserName", student.Name },
                            { "StudentId", student.StudentId ?? "-" }
                        };
                        await _emailService.SendGenericTemplateAsync(student.Email!, "Exam Schedule Published", studentPlaceholders);
                    }
                }
            }
            catch
            {
                // Swallow exceptions — email failure should not affect the core operation
            }
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateExamDto dto, string username)
        {
            var e = await _repo.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (e == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            e.Name = dto.Name; e.ExamType = dto.ExamType; e.StartDate = dto.StartDate;
            e.EndDate = dto.EndDate; e.Status = dto.Status; e.Description = dto.Description;
            e.UpdatedBy = username; e.UpdatedDate = DateTime.UtcNow;
            _repo.Update(e);
            await _uow.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Updated successfully" };
        }

        public async Task<APIResponse<object>> DeleteAsync(int id, string username)
        {
            var e = await _repo.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (e == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            _repo.Delete(e);
            await _uow.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Deleted successfully" };
        }

        public async Task<APIResponse<object>> PublishResultAsync(int examId, string publishedBy)
        {
            var exam = await _repo.List().Where(x => x.Id == examId).FirstOrDefaultAsync();
            if (exam == null)
                return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Exam not found" };

            if (exam.IsResultPublished)
                return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Result is already published" };

            // Validate that at least one result exists for this exam
            var resultCount = await _dbContext.Set<ExamResult>()
                .CountAsync(r => r.ExamId == examId && !r.IsDeleted);
            if (resultCount == 0)
                return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "No marks/results found for this exam. Please enter marks before publishing." };

            // Set published flag
            exam.IsResultPublished = true;
            exam.ResultPublishedDate = DateTime.UtcNow;
            exam.ResultPublishedBy = publishedBy;
            exam.UpdatedBy = publishedBy;
            exam.UpdatedDate = DateTime.UtcNow;
            _repo.Update(exam);
            await _uow.CommitAsync();

            // Fire-and-forget: email all students who have results
            _ = SendResultPublishedEmailsAsync(exam);

            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = $"Result published successfully. {resultCount} student result(s) available." };
        }

        private async Task SendResultPublishedEmailsAsync(Exam exam)
        {
            try
            {
                var results = await _dbContext.Set<ExamResult>()
                    .Include(r => r.Student).ThenInclude(s => s.ApplicationUser)
                    .Where(r => r.ExamId == exam.Id && !r.IsDeleted)
                    .ToListAsync();

                foreach (var result in results)
                {
                    var email = result.Student?.ApplicationUser?.Email;
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        await _emailService.SendGenericTemplateAsync(email!, "Exam Result Published", new Dictionary<string, string>
                        {
                            { "SchoolName", "School" },
                            { "UserName", result.Student!.Name },
                            { "ExamName", exam.Name },
                            { "ExamType", exam.ExamType ?? "-" },
                            { "MarksObtained", result.MarksObtained.ToString("F2") },
                            { "TotalMarks", result.TotalMarks.ToString("F2") },
                            { "Grade", result.Grade ?? "-" },
                            { "Status", result.Status },
                            { "LoginUrl", "#" }
                        });
                    }
                }
            }
            catch
            {
                // Swallow — email failure must not affect core publish operation
            }
        }
    }

    public class ExamResultService : IExamResultService
    {
        private readonly IRepository<ExamResult> _repo;
        private readonly IUnitOfWork _uow;
        private readonly IEmailService _emailService;
        private readonly SchoolDbContext _dbContext;

        public ExamResultService(IRepository<ExamResult> repo, IUnitOfWork uow, IEmailService emailService, SchoolDbContext dbContext)
        {
            _repo = repo;
            _uow = uow;
            _emailService = emailService;
            _dbContext = dbContext;
        }

        public async Task<APIResponse<List<ExamResultDto>>> GetAllByExamIdAsync(int examId)
        {
            var d = await _repo.List().Where(x => x.ExamId == examId)
                .Select(x => new ExamResultDto { Id = x.Id, ExamId = x.ExamId, StudentId = x.StudentId, SubjectId = x.SubjectId, MarksObtained = x.MarksObtained, TotalMarks = x.TotalMarks, Grade = x.Grade, Status = x.Status })
                .ToListAsync();
            return new APIResponse<List<ExamResultDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = d };
        }

        public async Task<APIResponse<ExamResultDto>> GetByIdAsync(int id)
        {
            var x = await _repo.List().Where(r => r.Id == id)
                .Select(x => new ExamResultDto { Id = x.Id, ExamId = x.ExamId, StudentId = x.StudentId, SubjectId = x.SubjectId, MarksObtained = x.MarksObtained, TotalMarks = x.TotalMarks, Grade = x.Grade, Status = x.Status })
                .FirstOrDefaultAsync();
            if (x == null) return new APIResponse<ExamResultDto> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            return new APIResponse<ExamResultDto> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = x };
        }

        public async Task<APIResponse<object>> CreateAsync(CreateExamResultDto dto, string username)
        {
            var entity = new ExamResult
            {
                ExamId = dto.ExamId,
                StudentId = dto.StudentId,
                SubjectId = dto.SubjectId,
                MarksObtained = dto.MarksObtained,
                TotalMarks = dto.TotalMarks,
                Grade = dto.Grade,
                Status = dto.Status,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };
            await _repo.AddAsync(entity);
            await _uow.CommitAsync();

            // Send Result Published email to student — fire-and-forget
            _ = SendResultEmailAsync(entity);

            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Created successfully" };
        }

        private async Task SendResultEmailAsync(ExamResult result)
        {
            try
            {
                var student = await _dbContext.Students
                    .Include(s => s.Class)
                    .Include(s => s.ApplicationUser)
                    .FirstOrDefaultAsync(s => s.Id == result.StudentId);

                var exam = await _dbContext.Set<Exam>().FindAsync(result.ExamId);
                var subject = await _dbContext.Set<Subject>().FindAsync(result.SubjectId);

                var studentEmail = student?.ApplicationUser?.Email;

                if (!string.IsNullOrWhiteSpace(studentEmail))
                {
                    await _emailService.SendGenericTemplateAsync(studentEmail!, "Result Published", new Dictionary<string, string>
                    {
                        { "SchoolName", "School" },
                        { "UserName", student!.Name },
                        { "StudentId", student.StudentId ?? "-" },
                        { "ExamName", exam?.Name ?? "-" },
                        { "ExamType", exam?.ExamType ?? "-" },
                        { "Subject", subject?.Name ?? "-" },
                        { "MarksObtained", result.MarksObtained.ToString("F2") },
                        { "TotalMarks", result.TotalMarks.ToString("F2") },
                        { "Grade", result.Grade ?? "-" },
                        { "Status", result.Status },
                        { "LoginUrl", "#" }
                    });
                }
            }
            catch
            {
                // Swallow exceptions — email failure should not affect the core operation
            }
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateExamResultDto dto, string username)
        {
            var e = await _repo.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (e == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            e.ExamId = dto.ExamId; e.StudentId = dto.StudentId; e.SubjectId = dto.SubjectId;
            e.MarksObtained = dto.MarksObtained; e.TotalMarks = dto.TotalMarks;
            e.Grade = dto.Grade; e.Status = dto.Status;
            e.UpdatedBy = username; e.UpdatedDate = DateTime.UtcNow;
            _repo.Update(e);
            await _uow.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Updated successfully" };
        }

        public async Task<APIResponse<object>> DeleteAsync(int id, string username)
        {
            var e = await _repo.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (e == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            _repo.Delete(e);
            await _uow.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Deleted successfully" };
        }
    }
}
