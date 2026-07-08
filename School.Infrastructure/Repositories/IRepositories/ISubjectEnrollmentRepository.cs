using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using School.Domain.Academic;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface ISubjectEnrollmentRepository
    {
        Task<SubjectEnrollment> AddAsync(SubjectEnrollment entity);
        Task<SubjectEnrollment?> GetByIdAsync(int id);
        Task<IEnumerable<SubjectEnrollment>> GetByStudentAsync(int studentId, int schoolRegistrationId);
        Task<IEnumerable<SubjectEnrollment>> GetByClassAsync(int classId, int schoolRegistrationId);
        Task<IEnumerable<SubjectEnrollment>> GetByBatchAsync(int batchId, int schoolRegistrationId);
        Task<IEnumerable<SubjectEnrollment>> GetBySubjectAsync(int subjectId, int schoolRegistrationId);
        Task<bool> IsEnrolledAsync(int studentId, int subjectId);
        Task<int> UpdateStatusAsync(int id, string status, string? remarks);
        Task<int> DeleteAsync(int id);
    }
}
