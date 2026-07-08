using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using School.Domain.FeeManagnment;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IFeeInstallmentRepository
    {
        Task<FeeInstallment> AddAsync(FeeInstallment entity);
        Task AddRangeAsync(IEnumerable<FeeInstallment> entities);
        Task<FeeInstallment?> GetByIdAsync(int id);
        Task<IEnumerable<FeeInstallment>> GetByStudentAsync(int studentId, int schoolRegistrationId);
        Task<IEnumerable<FeeInstallment>> GetPendingByStudentAsync(int studentId, int schoolRegistrationId);
        Task<IEnumerable<FeeInstallment>> GetOverdueAsync(int schoolRegistrationId);
        Task<int> UpdateStatusAsync(int id, string status, DateTime? paidDate);
        Task<int> UpdateAsync(FeeInstallment entity);
    }

    public interface IFeePaymentRepository
    {
        Task<FeePayment> AddAsync(FeePayment entity);
        Task<FeePayment?> GetByIdAsync(int id);
        Task<FeePayment?> GetByReceiptNoAsync(string receiptNo, int schoolRegistrationId);
        Task<IEnumerable<FeePayment>> GetByStudentAsync(int studentId, int schoolRegistrationId);
        Task<IEnumerable<FeePayment>> GetByDateRangeAsync(DateTime from, DateTime to, int schoolRegistrationId);
        Task<decimal> GetTotalCollectedAsync(DateTime from, DateTime to, int schoolRegistrationId);
        Task<string> GenerateReceiptNoAsync(int schoolRegistrationId);
    }
}
