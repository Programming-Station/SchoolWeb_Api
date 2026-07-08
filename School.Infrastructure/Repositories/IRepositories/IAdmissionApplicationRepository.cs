using System.Collections.Generic;
using System.Threading.Tasks;
using School.Domain.Student;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IAdmissionApplicationRepository
    {
        Task<AdmissionApplication> AddAsync(AdmissionApplication entity);
        Task<AdmissionApplication> GetByIdAsync(int id);
        Task<IEnumerable<AdmissionApplication>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, string? status = null, int? campusId = null, int? programId = null);
        Task<int> GetTotalCountAsync(string? searchTerm = null, string? status = null, int? campusId = null, int? programId = null);
        Task<int> UpdateAsync(AdmissionApplication entity);
        Task<int> DeleteAsync(int id);
        Task<bool> ExistsByMobileAsync(string mobile, int? excludeId = null);
        Task<bool> ExistsByAadhaarAsync(string aadhaar, int? excludeId = null);
        Task<string> GetLastApplicationNoAsync(string prefix);
        Task<string> GetLastAdmissionNoAsync(string prefix);
        Task<string> GetLastEnrollmentNoAsync(string prefix);
        Task<string> GetLastStudentCodeAsync(string prefix);
        Task<int> AddAuditLogAsync(AdmissionAuditLog log);
        Task<IEnumerable<AdmissionAuditLog>> GetAuditLogsAsync(int applicationId);
    }
}
