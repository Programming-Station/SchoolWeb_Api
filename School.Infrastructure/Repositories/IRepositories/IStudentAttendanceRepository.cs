using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using School.Domain.Academic;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IStudentAttendanceRepository
    {
        Task<StudentAttendance> AddAsync(StudentAttendance entity);
        Task AddRangeAsync(IEnumerable<StudentAttendance> entities);
        Task<StudentAttendance?> GetByIdAsync(int id);
        Task<IEnumerable<StudentAttendance>> GetByDateAndClassAsync(DateTime date, int classId, int schoolRegistrationId);
        Task<IEnumerable<StudentAttendance>> GetByStudentAsync(int studentId, int schoolRegistrationId, DateTime? from = null, DateTime? to = null);
        Task<IEnumerable<StudentAttendance>> GetByClassAndMonthAsync(int classId, int month, int year, int schoolRegistrationId);
        Task<(int Present, int Absent, int Late, int Leave, int Total)> GetStudentSummaryAsync(int studentId, int schoolRegistrationId, DateTime? from = null, DateTime? to = null);
        Task<int> UpdateAsync(StudentAttendance entity);
        Task<int> DeleteAsync(int id);
        Task<bool> ExistsAsync(int studentId, DateTime date, int? subjectId, int schoolRegistrationId);
    }
}
