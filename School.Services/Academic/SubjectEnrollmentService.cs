using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using School.Domain.Academic;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Services.Interfaces.Academic;

namespace School.Services.Academic
{
    public class SubjectEnrollmentService : ISubjectEnrollmentService
    {
        private readonly ISubjectEnrollmentRepository _repo;

        public SubjectEnrollmentService(ISubjectEnrollmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<(bool Success, string Message, List<SubjectEnrollmentDto> Enrolled)> EnrollSubjectsAsync(
            EnrollSubjectRequest request, string enrolledBy, int schoolRegistrationId)
        {
            var enrolled = new List<SubjectEnrollmentDto>();
            var skipped = new List<int>();

            foreach (var subjectId in request.SubjectIds)
            {
                var alreadyEnrolled = await _repo.IsEnrolledAsync(request.StudentId, subjectId);
                if (alreadyEnrolled) { skipped.Add(subjectId); continue; }

                var entity = new SubjectEnrollment
                {
                    StudentId          = request.StudentId,
                    SubjectId          = subjectId,
                    BatchId            = request.BatchId,
                    YearSemesterId     = request.YearSemesterId,
                    ClassId            = request.ClassId,
                    Status             = "Enrolled",
                    EnrolledDate       = DateTime.Now,
                    SchoolRegistrationId = schoolRegistrationId,
                    CreatedBy          = enrolledBy
                };

                await _repo.AddAsync(entity);
                enrolled.Add(MapToDto(entity));
            }

            var msg = skipped.Any()
                ? $"{enrolled.Count} subjects enrolled. {skipped.Count} were already enrolled and skipped."
                : $"{enrolled.Count} subjects enrolled successfully.";

            return (true, msg, enrolled);
        }

        public async Task<(bool Success, string Message)> DropSubjectAsync(int enrollmentId, string remarks, int schoolRegistrationId)
        {
            var updated = await _repo.UpdateStatusAsync(enrollmentId, "Dropped", remarks);
            return updated > 0
                ? (true, "Subject dropped successfully.")
                : (false, "Enrollment not found.");
        }

        public async Task<IEnumerable<SubjectEnrollmentDto>> GetByStudentAsync(int studentId, int schoolRegistrationId)
        {
            var data = await _repo.GetByStudentAsync(studentId, schoolRegistrationId);
            return data.Select(MapToDto);
        }

        public async Task<IEnumerable<SubjectEnrollmentDto>> GetByClassAsync(int classId, int schoolRegistrationId)
        {
            var data = await _repo.GetByClassAsync(classId, schoolRegistrationId);
            return data.Select(MapToDto);
        }

        public async Task<IEnumerable<SubjectEnrollmentDto>> GetByBatchAsync(int batchId, int schoolRegistrationId)
        {
            var data = await _repo.GetByBatchAsync(batchId, schoolRegistrationId);
            return data.Select(MapToDto);
        }

        private static SubjectEnrollmentDto MapToDto(SubjectEnrollment e) => new()
        {
            Id              = e.Id,
            StudentId       = e.StudentId,
            StudentName     = e.Student?.Name ?? string.Empty,
            StudentCode     = e.Student?.StudentId ?? string.Empty,
            SubjectId       = e.SubjectId,
            SubjectName     = e.Subject?.Name ?? string.Empty,
            SubjectCode     = e.Subject?.Code,
            BatchId         = e.BatchId,
            BatchName       = e.Batch?.Name,
            YearSemesterId  = e.YearSemesterId,
            YearSemesterName = e.YearSemester?.Name,
            ClassId         = e.ClassId,
            ClassName       = e.Class?.Name,
            Status          = e.Status,
            EnrolledDate    = e.EnrolledDate,
            Remarks         = e.Remarks
        };
    }
}
