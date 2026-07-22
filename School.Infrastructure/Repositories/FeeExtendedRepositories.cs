using Microsoft.EntityFrameworkCore;
using School.Domain.FeeManagnment;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories
{
    // ══════════════════════════════════════════════════════════════════════════
    // 6.4 FEE FINE
    // ══════════════════════════════════════════════════════════════════════════
    public class FeeFineRepository : Repository<FeeFine>, IFeeFineRepository
    {
        private readonly SchoolDbContext _ctx; private readonly IUnitOfWork _uow;
        public FeeFineRepository(DbFactory f, SchoolDbContext ctx, IUnitOfWork uow) : base(f) { _ctx = ctx; _uow = uow; }
        public async Task<FeeFine> AddAsync(FeeFine e) { await base.AddAsync(e); await _uow.CommitAsync(); return e; }
        public async Task<FeeFine?> GetByIdAsync(int id) =>
            await DbSet.Include(x => x.Student).Include(x => x.FeeInstallment)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        public async Task<IEnumerable<FeeFine>> GetByStudentAsync(int studentId, int schoolId) =>
            await DbSet.Include(x => x.FeeInstallment)
                .Where(x => x.StudentId == studentId && x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate).ToListAsync();
        public async Task<IEnumerable<FeeFine>> GetPendingAsync(int schoolId) =>
            await DbSet.Include(x => x.Student).Include(x => x.FeeInstallment)
                .Where(x => x.SchoolRegistrationId == schoolId && x.Status == "Pending" && !x.IsDeleted)
                .ToListAsync();
        public async Task<int> UpdateStatusAsync(int id, string status)
        {
            var e = await DbSet.FindAsync(id); if (e == null) return 0;
            e.Status = status; _ctx.Entry(e).State = EntityState.Modified; return await _uow.CommitAsync();
        }
        public async Task<int> DeleteAsync(int id) { var e = await DbSet.FindAsync(id); if (e == null) return 0; e.IsDeleted = true; _ctx.Entry(e).State = EntityState.Modified; return await _uow.CommitAsync(); }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // 6.5 SCHOLARSHIP
    // ══════════════════════════════════════════════════════════════════════════
    public class StudentScholarshipRepository : Repository<StudentScholarship>, IStudentScholarshipRepository
    {
        private readonly SchoolDbContext _ctx; private readonly IUnitOfWork _uow;
        public StudentScholarshipRepository(DbFactory f, SchoolDbContext ctx, IUnitOfWork uow) : base(f) { _ctx = ctx; _uow = uow; }
        public async Task<StudentScholarship> AddAsync(StudentScholarship e) { await base.AddAsync(e); await _uow.CommitAsync(); return e; }
        public async Task<StudentScholarship?> GetByIdAsync(int id) =>
            await DbSet.Include(x => x.Student).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        public async Task<IEnumerable<StudentScholarship>> GetByStudentAsync(int studentId, int schoolId) =>
            await DbSet.Where(x => x.StudentId == studentId && x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .OrderByDescending(x => x.ValidFrom).ToListAsync();
        public async Task<IEnumerable<StudentScholarship>> GetActiveAsync(int schoolId) =>
            await DbSet.Include(x => x.Student)
                .Where(x => x.SchoolRegistrationId == schoolId && x.Status == "Active"
                    && x.ValidFrom <= DateTime.Today && (x.ValidTo == null || x.ValidTo >= DateTime.Today) && !x.IsDeleted)
                .ToListAsync();
        public async Task<int> UpdateAsync(StudentScholarship e) { _ctx.Entry(e).State = EntityState.Modified; return await _uow.CommitAsync(); }
        public async Task<int> DeleteAsync(int id) { var e = await DbSet.FindAsync(id); if (e == null) return 0; e.IsDeleted = true; _ctx.Entry(e).State = EntityState.Modified; return await _uow.CommitAsync(); }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // 6.6 REFUND
    // ══════════════════════════════════════════════════════════════════════════
    public class FeeRefundRepository : Repository<FeeRefund>, IFeeRefundRepository
    {
        private readonly SchoolDbContext _ctx; private readonly IUnitOfWork _uow;
        public FeeRefundRepository(DbFactory f, SchoolDbContext ctx, IUnitOfWork uow) : base(f) { _ctx = ctx; _uow = uow; }
        public async Task<FeeRefund> AddAsync(FeeRefund e) { await base.AddAsync(e); await _uow.CommitAsync(); return e; }
        public async Task<FeeRefund?> GetByIdAsync(int id) =>
            await DbSet.Include(x => x.Student)
                    .ThenInclude(s => s.ApplicationUser)
                .Include(x => x.FeePayment)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        public async Task<IEnumerable<FeeRefund>> GetByStudentAsync(int studentId, int schoolId) =>
            await DbSet.Include(x => x.FeePayment)
                .Where(x => x.StudentId == studentId && x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .OrderByDescending(x => x.RefundDate).ToListAsync();
        public async Task<IEnumerable<FeeRefund>> GetPendingAsync(int schoolId) =>
            await DbSet.Include(x => x.Student)
                .Where(x => x.SchoolRegistrationId == schoolId && x.Status == "Pending" && !x.IsDeleted)
                .ToListAsync();
        public async Task<int> UpdateStatusAsync(int id, string status, string approvedBy)
        {
            var e = await DbSet.FindAsync(id); if (e == null) return 0;
            e.Status = status; e.ApprovedBy = approvedBy;
            _ctx.Entry(e).State = EntityState.Modified; return await _uow.CommitAsync();
        }
        public async Task<decimal> GetTotalRefundedAsync(DateTime from, DateTime to, int schoolId) =>
            await DbSet.Where(x => x.SchoolRegistrationId == schoolId && x.Status == "Processed"
                && x.RefundDate >= from && x.RefundDate <= to && !x.IsDeleted)
                .SumAsync(x => x.RefundAmount);
    }

    public class FineRuleRepository : Repository<FineRule>, IFineRuleRepository
    {
        private readonly SchoolDbContext _ctx;
        private readonly IUnitOfWork _uow;

        public FineRuleRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _ctx = context;
            _uow = unitOfWork;
        }

        public async Task<FineRule> AddAsync(FineRule entity)
        {
            entity.CreatedDate = DateTime.Now;
            await base.AddAsync(entity);
            await _uow.CommitAsync();
            return entity;
        }

        public async Task<FineRule?> GetByIdAsync(int id) =>
            await DbSet.Include(x => x.FeeType).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        public async Task<FineRule?> GetByFeeTypeAsync(int feeTypeId, int schoolId) =>
            await DbSet.FirstOrDefaultAsync(x => x.FeeTypeId == feeTypeId && x.SchoolRegistrationId == schoolId && x.IsActive && !x.IsDeleted);

        public async Task<IEnumerable<FineRule>> GetAllAsync(int schoolId) =>
            await DbSet.Include(x => x.FeeType).Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted).ToListAsync();

        public async Task<int> UpdateAsync(FineRule entity)
        {
            _ctx.Entry(entity).State = EntityState.Modified;
            return await _uow.CommitAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var e = await DbSet.FindAsync(id);
            if (e == null) return 0;
            e.IsDeleted = true;
            _ctx.Entry(e).State = EntityState.Modified;
            return await _uow.CommitAsync();
        }
    }
}
