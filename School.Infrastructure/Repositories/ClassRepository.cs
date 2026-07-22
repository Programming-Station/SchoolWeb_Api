using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories
{
    public class ClassRepository : Repository<Class>, IClassRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public ClassRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<Class> AddClassAsync(Class entity)
        {
            await base.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<int> DeleteClassAsync(int id)
        {
            var result = await FindAsync(expression: x => x.Id == id && !x.IsDeleted);

            if (result != null)
            {
                result.UpdatedDate = DateTime.Now;
                Delete(result);
                return await _unitOfWork.CommitAsync().ConfigureAwait(false);
            }
            else
                return 0;
        }

        public async Task<IEnumerable<Class>> GetAllClassesAsync()
        {
            return await _context.Classes
                .Where(x => !x.IsDeleted)
                .Include(x => x.Course)
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Section)
                .ToListAsync();
        }

        public async Task<Class> GetClassByIdAsync(int id)
        {
            return await _context.Classes
                .Include(x => x.Course)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted) ?? new Class();
        }

        public async Task<IEnumerable<Class>> GetClassesByCourseIdAsync(int courseId)
        {
            return await _context.Classes
                .Where(x => !x.IsDeleted && x.CourseId == courseId)
                .Include(x => x.Course)
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Section)
                .ToListAsync();
        }

        public async Task<int> ToggleClassStatusAsync(int id)
        {
            var entity = await _context.Classes
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (entity != null)
            {
                entity.Status = entity.Status.ToLower() == "active" ? "inactive" : "active";
                Update(entity);
                return await _unitOfWork.CommitAsync().ConfigureAwait(false);
            }
            else
                return 0;
        }

        public async Task<int> UpdateClassAsync(Class entity)
        {
            Attach(entity, updatedProperties: new Expression<Func<Class, object>>[]
            {
                u => u.Name,
                u => u.Section!,
                u => u.CourseId,
                u => u.AcademicYear!,
                u => u.Capacity,
                u => u.CurrentStrength,
                u => u.ClassTeacher!,
                u => u.RoomNumber!,
                u => u.Status!,
                u => u.UpdatedBy!,
            });
            return await _unitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public async Task<int> UpdateClassStrengthAsync(int id, int newStrength)
        {
            var entity = await _context.Classes
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (entity != null)
            {
                entity.CurrentStrength = newStrength;
                Update(entity);
                return await _unitOfWork.CommitAsync().ConfigureAwait(false);
            }
            else
                return 0;
        }
    }
}
