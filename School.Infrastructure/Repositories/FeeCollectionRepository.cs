using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.Domain.FeeManagnment;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories
{
    // ──────────────────────────────────────────────────────────────────────────
    // FeeInstallmentRepository
    // ──────────────────────────────────────────────────────────────────────────
    public class FeeInstallmentRepository : Repository<FeeInstallment>, IFeeInstallmentRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public FeeInstallmentRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork)
            : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<FeeInstallment> AddAsync(FeeInstallment entity)
        {
            await base.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<FeeInstallment> entities)
        {
            foreach (var e in entities) e.CreatedDate = DateTime.Now;
            await _context.Set<FeeInstallment>().AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
        }

        public async Task<FeeInstallment?> GetByIdAsync(int id)
        {
            return await DbSet
                .Include(x => x.Student)
                .Include(x => x.FeeStructure)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<IEnumerable<FeeInstallment>> GetByStudentAsync(int studentId, int schoolRegistrationId)
        {
            return await DbSet
                .Include(x => x.FeeStructure)
                .Where(x => x.StudentId == studentId
                    && x.SchoolRegistrationId == schoolRegistrationId
                    && !x.IsDeleted)
                .OrderBy(x => x.InstallmentNo)
                .ToListAsync();
        }

        public async Task<IEnumerable<FeeInstallment>> GetPendingByStudentAsync(int studentId, int schoolRegistrationId)
        {
            return await DbSet
                .Include(x => x.FeeStructure)
                .Where(x => x.StudentId == studentId
                    && x.SchoolRegistrationId == schoolRegistrationId
                    && (x.Status == "Pending" || x.Status == "Overdue" || x.Status == "PartiallyPaid")
                    && !x.IsDeleted)
                .OrderBy(x => x.DueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<FeeInstallment>> GetOverdueAsync(int schoolRegistrationId)
        {
            return await DbSet
                .Include(x => x.Student)
                .Include(x => x.FeeStructure)
                .Where(x => x.SchoolRegistrationId == schoolRegistrationId
                    && x.Status != "Paid" && x.Status != "Waived"
                    && x.DueDate < DateTime.Today
                    && !x.IsDeleted)
                .OrderBy(x => x.DueDate)
                .ToListAsync();
        }

        public async Task<int> UpdateStatusAsync(int id, string status, DateTime? paidDate)
        {
            var entity = await DbSet.FindAsync(id);
            if (entity == null) return 0;
            entity.Status = status;
            if (paidDate.HasValue) entity.PaidDate = paidDate.Value;
            _context.Entry(entity).State = EntityState.Modified;
            return await _unitOfWork.CommitAsync();
        }

        public async Task<int> UpdateAsync(FeeInstallment entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return await _unitOfWork.CommitAsync();
        }
    }

    // ──────────────────────────────────────────────────────────────────────────
    // FeePaymentRepository
    // ──────────────────────────────────────────────────────────────────────────
    public class FeePaymentRepository : Repository<FeePayment>, IFeePaymentRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public FeePaymentRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork)
            : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<FeePayment> AddAsync(FeePayment entity)
        {
            await base.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<FeePayment?> GetByIdAsync(int id)
        {
            return await DbSet
                .Include(x => x.Student)
                .Include(x => x.FeeInstallment)
                    .ThenInclude(i => i.FeeStructure)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<FeePayment?> GetByReceiptNoAsync(string receiptNo, int schoolRegistrationId)
        {
            return await DbSet
                .Include(x => x.Student)
                .Include(x => x.FeeInstallment)
                .FirstOrDefaultAsync(x => x.ReceiptNo == receiptNo
                    && x.SchoolRegistrationId == schoolRegistrationId
                    && !x.IsDeleted);
        }

        public async Task<IEnumerable<FeePayment>> GetByStudentAsync(int studentId, int schoolRegistrationId)
        {
            return await DbSet
                .Include(x => x.FeeInstallment)
                .Where(x => x.StudentId == studentId
                    && x.SchoolRegistrationId == schoolRegistrationId
                    && !x.IsDeleted)
                .OrderByDescending(x => x.PaymentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<FeePayment>> GetByDateRangeAsync(DateTime from, DateTime to, int schoolRegistrationId)
        {
            return await DbSet
                .Include(x => x.Student)
                .Include(x => x.FeeInstallment)
                .Where(x => x.PaymentDate >= from && x.PaymentDate <= to
                    && x.SchoolRegistrationId == schoolRegistrationId
                    && x.Status == "Completed"
                    && !x.IsDeleted)
                .OrderByDescending(x => x.PaymentDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalCollectedAsync(DateTime from, DateTime to, int schoolRegistrationId)
        {
            return await DbSet
                .Where(x => x.PaymentDate >= from && x.PaymentDate <= to
                    && x.SchoolRegistrationId == schoolRegistrationId
                    && x.Status == "Completed"
                    && !x.IsDeleted)
                .SumAsync(x => x.AmountPaid);
        }

        public async Task<string> GenerateReceiptNoAsync(int schoolRegistrationId)
        {
            var year = DateTime.Now.Year.ToString().Substring(2); // e.g. "25"
            var count = await DbSet
                .CountAsync(x => x.SchoolRegistrationId == schoolRegistrationId);
            return $"RCP-{year}-{(count + 1):D6}"; // e.g. RCP-25-000001
        }
    }
}
