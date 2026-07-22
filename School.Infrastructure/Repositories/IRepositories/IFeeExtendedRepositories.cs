using School.Domain.FeeManagnment;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IFeeFineRepository
    {
        Task<FeeFine> AddAsync(FeeFine entity);
        Task<FeeFine?> GetByIdAsync(int id);
        Task<IEnumerable<FeeFine>> GetByStudentAsync(int studentId, int schoolId);
        Task<IEnumerable<FeeFine>> GetPendingAsync(int schoolId);
        Task<int> UpdateStatusAsync(int id, string status);
        Task<int> DeleteAsync(int id);
    }

    public interface IStudentScholarshipRepository
    {
        Task<StudentScholarship> AddAsync(StudentScholarship entity);
        Task<StudentScholarship?> GetByIdAsync(int id);
        Task<IEnumerable<StudentScholarship>> GetByStudentAsync(int studentId, int schoolId);
        Task<IEnumerable<StudentScholarship>> GetActiveAsync(int schoolId);
        Task<int> UpdateAsync(StudentScholarship entity);
        Task<int> DeleteAsync(int id);
    }

    public interface IFeeRefundRepository
    {
        Task<FeeRefund> AddAsync(FeeRefund entity);
        Task<FeeRefund?> GetByIdAsync(int id);
        Task<IEnumerable<FeeRefund>> GetByStudentAsync(int studentId, int schoolId);
        Task<IEnumerable<FeeRefund>> GetPendingAsync(int schoolId);
        Task<int> UpdateStatusAsync(int id, string status, string approvedBy);
        Task<decimal> GetTotalRefundedAsync(DateTime from, DateTime to, int schoolId);
    }

    public interface IFineRuleRepository
    {
        Task<FineRule> AddAsync(FineRule entity);
        Task<FineRule?> GetByIdAsync(int id);
        Task<FineRule?> GetByFeeTypeAsync(int feeTypeId, int schoolId);
        Task<IEnumerable<FineRule>> GetAllAsync(int schoolId);
        Task<int> UpdateAsync(FineRule entity);
        Task<int> DeleteAsync(int id);
    }
}
