using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Models.Teacher;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace School.Infrastructure.Repositories
{
    public class TeacherRepository : Repository<Teacher>, ITeacherRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public TeacherRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<Teacher> AddTeacherAsync(Teacher entity)
        {
            // Check if teacher with same UserId exists
            var existingTeacher = await DbSet.FirstOrDefaultAsync(x =>
                               x.UserId == entity.UserId &&
                               !x.IsDeleted);

            if (existingTeacher != null)
            {
                existingTeacher.Id = 0;
                return existingTeacher;
            }

            // Check if teacher ID already exists (if provided)
            if (!string.IsNullOrEmpty(entity.TeacherId))
            {
                var existingTeacherId = await DbSet.FirstOrDefaultAsync(x =>
                                   x.TeacherId == entity.TeacherId &&
                                   !x.IsDeleted);

                if (existingTeacherId != null)
                {
                    existingTeacherId.Id = 0;
                    return existingTeacherId;
                }
            }

            await AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<int> DeleteTeacherAsync(int id)
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

        public async Task<(IEnumerable<Teacher> Teachers, int TotalCount)> GetAllTeachersAsync(TeacherFilter filter)
        {
            var query = DbSet
                .Include(t => t.City)
                .Include(t => t.State)
                .Include(t => t.Course)
                .Include(t => t.Faculty)
                .Include(t => t.User)
                .Where(x => !x.IsDeleted)
                .AsQueryable();

            // Apply filters
            if (filter.StateId.HasValue)
            {
                query = query.Where(x => x.StateId == filter.StateId.Value);
            }

            if (filter.CityId.HasValue)
            {
                query = query.Where(x => x.CityId == filter.CityId.Value);
            }

            if (filter.CourseId.HasValue)
            {
                query = query.Where(x => x.CourseId == filter.CourseId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Faculty))
            {
                query = query.Where(x => x.Faculty != null && x.Faculty.Name != null && x.Faculty.Name.ToLower().Contains(filter.Faculty.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(filter.Department))
            {
                query = query.Where(x => x.Department != null && x.Department.ToLower().Contains(filter.Department.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                query = query.Where(x => x.Status.ToLower() == filter.Status.ToLower());
            }

            // Apply search
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var searchTerm = filter.Search.ToLower();
                query = query.Where(x =>
                    (x.User != null && x.User.FirstName != null && x.User.FirstName.ToLower().Contains(searchTerm)) ||
                    (x.User != null && x.User.LastName != null && x.User.LastName.ToLower().Contains(searchTerm)) ||
                    (x.TeacherId != null && x.TeacherId.ToLower().Contains(searchTerm)) ||
                    (x.User != null && x.User.Email != null && x.User.Email.ToLower().Contains(searchTerm)) ||
                    (x.User != null && x.User.PhoneNumber != null && x.User.PhoneNumber.Contains(searchTerm)) ||
                    (x.Faculty != null && x.Faculty.Name != null && x.Faculty.Name.ToLower().Contains(searchTerm)) ||
                    (x.Department != null && x.Department.ToLower().Contains(searchTerm))
                );
            }

            // Apply status filter
            if (filter.IsActive.HasValue)
            {
                query = query.Where(x => x.Status == "active" == filter.IsActive.Value);
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(filter.OrderBy))
            {
                switch (filter.OrderBy.ToLower())
                {
                    case "firstname":
                        query = filter.SortDirection?.ToLower() == "desc"
                            ? query.OrderByDescending(x => x.User != null ? x.User.FirstName : "")
                            : query.OrderBy(x => x.User != null ? x.User.FirstName : "");
                        break;
                    case "lastname":
                        query = filter.SortDirection?.ToLower() == "desc"
                            ? query.OrderByDescending(x => x.User != null ? x.User.LastName : "")
                            : query.OrderBy(x => x.User != null ? x.User.LastName : "");
                        break;
                    case "email":
                        query = filter.SortDirection?.ToLower() == "desc"
                            ? query.OrderByDescending(x => x.User != null ? x.User.Email : "")
                            : query.OrderBy(x => x.User != null ? x.User.Email : "");
                        break;
                    case "joiningdate":
                        query = filter.SortDirection?.ToLower() == "desc"
                            ? query.OrderByDescending(x => x.JoiningDate)
                            : query.OrderBy(x => x.JoiningDate);
                        break;
                    default:
                        query = query.OrderByDescending(x => x.CreatedDate);
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(x => x.CreatedDate);
            }

            // Apply pagination
            if (filter.PageIndex > 0 && filter.PageSize > 0)
            {
                query = query.Skip((filter.PageIndex - 1) * filter.PageSize)
                            .Take(filter.PageSize);
            }

            var teachers = await query.ToListAsync();
            return (teachers, totalCount);
        }

        public async Task<Teacher> GetTeacherByIdAsync(int id)
        {
            return await DbSet
                .Include(t => t.City)
                .Include(t => t.State)
                .Include(t => t.Course)
                .Include(t => t.Faculty)
                .Include(t => t.User)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted) ?? new Teacher();
        }

        public async Task<Teacher?> GetTeacherByTeacherIdAsync(string teacherId)
        {
            return await DbSet
                .Include(t => t.City)
                .Include(t => t.State)
                .Include(t => t.Course)
                .FirstOrDefaultAsync(x => x.TeacherId == teacherId && !x.IsDeleted);
        }

        public async Task<Teacher?> GetTeacherByEmailAsync(string email)
        {
            // This method is kept for backward compatibility but should use GetTeacherByUserIdAsync
            // after finding user by email
            return await DbSet
                .Include(t => t.City)
                .Include(t => t.State)
                .Include(t => t.Course)
                .Include(t => t.Faculty)
                .Include(t => t.User)
                .FirstOrDefaultAsync(x => x.User != null && x.User.Email != null && x.User.Email.ToLower() == email.ToLower() && !x.IsDeleted);
        }

        public async Task<Teacher?> GetTeacherByUserIdAsync(string userId)
        {
            return await DbSet
                .Include(t => t.City)
                .Include(t => t.State)
                .Include(t => t.Course)
                .Include(t => t.Faculty)
                .Include(t => t.User)
                .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsDeleted);
        }

        public async Task<int> ToggleTeacherStatusAsync(int id)
        {
            var entity = await _context.Teachers
                .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

            if (entity != null)
            {
                entity.Status = entity.Status == "active" ? "inactive" : "active";
                Update(entity);
                return await _unitOfWork.CommitAsync().ConfigureAwait(false);
            }
            else
                return 0;
        }

        public async Task<int> UpdateTeacherAsync(Teacher entity)
        {
            Attach(entity, updatedProperties: new Expression<Func<Teacher, object>>[]
            {
                t => t.TeacherId!,
                t => t.AlternatePhoneNumber!,
                t => t.Gender!,
                t => t.DateOfBirth!,
                t => t.Address!,
                t => t.CityId,
                t => t.StateId,
                t => t.PinCode!,
                t => t.Qualification!,
                t => t.Specialization!,
                t => t.Experience!,
                t => t.Faculty!,
                t => t.Department!,
                t => t.CourseId,
                t => t.ProfilePhotoUrl!,
                t => t.Bio!,
                t => t.Status,
                t => t.JoiningDate!,
                t => t.Remarks!,
                t => t.UpdatedBy!,
            });
            return await _unitOfWork.CommitAsync().ConfigureAwait(false);
        }
    }
}

