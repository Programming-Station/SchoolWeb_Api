using Microsoft.EntityFrameworkCore;
using School.Domain.Library;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Infrastructure.Repositories
{
    // ══════════════════════════════════════════════════════════════════════════
    // BOOK CATALOG REPOSITORY
    // ══════════════════════════════════════════════════════════════════════════
    public class BookRepository : Repository<Book>, IBookRepository
    {
        private readonly SchoolDbContext _ctx;
        private readonly IUnitOfWork _uow;

        public BookRepository(DbFactory f, SchoolDbContext ctx, IUnitOfWork uow) : base(f)
        {
            _ctx = ctx;
            _uow = uow;
        }

        public new async Task<int> AddAsync(Book entity)
        {
            await base.AddAsync(entity);
            return await _uow.CommitAsync();
        }

        public new async Task<int> UpdateAsync(Book entity)
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

        public async Task<Book?> GetByIdAsync(int id) =>
            await DbSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        public async Task<IEnumerable<Book>> GetAllBySchoolAsync(int schoolId) =>
            await DbSet.Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted).ToListAsync();
    }

    // ══════════════════════════════════════════════════════════════════════════
    // BOOK ISSUE LOG REPOSITORY
    // ══════════════════════════════════════════════════════════════════════════
    public class BookIssueLogRepository : Repository<BookIssueLog>, IBookIssueLogRepository
    {
        private readonly SchoolDbContext _ctx;
        private readonly IUnitOfWork _uow;

        public BookIssueLogRepository(DbFactory f, SchoolDbContext ctx, IUnitOfWork uow) : base(f)
        {
            _ctx = ctx;
            _uow = uow;
        }

        public new async Task<int> AddAsync(BookIssueLog entity)
        {
            await base.AddAsync(entity);
            return await _uow.CommitAsync();
        }

        public new async Task<int> UpdateAsync(BookIssueLog entity)
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

        public async Task<BookIssueLog?> GetByIdAsync(int id) =>
            await DbSet.Include(x => x.Book).Include(x => x.Student).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        public async Task<IEnumerable<BookIssueLog>> GetByStudentAsync(int studentId, int schoolId) =>
            await DbSet.Include(x => x.Book).Include(x => x.Student)
                .Where(x => x.StudentId == studentId && x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .ToListAsync();

        public async Task<IEnumerable<BookIssueLog>> GetAllBySchoolAsync(int schoolId) =>
            await DbSet.Include(x => x.Book).Include(x => x.Student)
                .Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .ToListAsync();
    }
}
