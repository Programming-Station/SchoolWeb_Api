using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Utilities.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace School.Infrastructure.Repositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public CourseRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<Course> AddCourseAsync(Course entity)
        {
            var existingCourse = await DbSet.FirstOrDefaultAsync(x =>
                               x.CourseCode.ToLower() == entity.CourseCode.ToLower() &&
                               !x.IsDeleted);

            if (existingCourse != null)
            {
                existingCourse.Id = 0;
                return existingCourse;
            }
            await base.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<int> DeleteCourseAsync(int id)
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

        public async Task<IEnumerable<Course>> GetAllCoursesAsync(int? courseType = null)
        {
            if (courseType.HasValue)
            {
                return await List(expression: x => !x.IsDeleted && x.CourseType == (CourseType)courseType.Value)
                    .OrderBy(x => x.Name)
                    .ToListAsync();
            }
            return await List(expression: x => !x.IsDeleted)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            return await FindAsync(expression: x => x.Id == id && !x.IsDeleted) ?? new Course();
        }

        public async Task<int> ToggleCourseStatusAsync(int id)
        {
            var entity = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (entity != null)
            {
                entity.IsActive = !entity.IsActive;
                Update(entity);
                return await _unitOfWork.CommitAsync().ConfigureAwait(false);
            }
            else
                return 0;
        }

        public async Task<int> UpdateCourseAsync(Course entity)
        {
            Attach(entity, updatedProperties: new Expression<Func<Course, object>>[]
            {
                u => u.Name,
                u => u.CourseCode!,
                u => u.CourseType!,
                u => u.Duration!,
                u => u.Fees,
                u => u.Description!,
                u => u.IsActive,
                u => u.UpdatedBy!,
            });
            return await _unitOfWork.CommitAsync().ConfigureAwait(false);
        }
    }
}
