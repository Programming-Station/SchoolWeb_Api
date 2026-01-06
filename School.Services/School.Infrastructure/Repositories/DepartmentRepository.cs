using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace School.Infrastructure.Repositories
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<Department> AddDepartmentAsync(Department entity)
        {
            var existingDepartment = await DbSet.FirstOrDefaultAsync(x =>
                               (x.Name.ToLower() == entity.Name.ToLower() || 
                                (!string.IsNullOrEmpty(entity.Code) && x.Code != null && x.Code.ToLower() == entity.Code.ToLower())) &&
                               x.FacultyId == entity.FacultyId &&
                               !x.IsDeleted);

            if (existingDepartment != null)
            {
                existingDepartment.Id = 0;
                return existingDepartment;
            }

            await AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<int> DeleteDepartmentAsync(int id)
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

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            return await DbSet
                .Include(d => d.Faculty)
                .Where(x => !x.IsDeleted && x.Status == "active")
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Department>> GetDepartmentsByFacultyIdAsync(int facultyId)
        {
            return await DbSet
                .Include(d => d.Faculty)
                .Where(x => x.FacultyId == facultyId && !x.IsDeleted && x.Status == "active")
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Department> GetDepartmentByIdAsync(int id)
        {
            return await DbSet
                .Include(d => d.Faculty)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted) ?? new Department();
        }

        public async Task<int> ToggleDepartmentStatusAsync(int id)
        {
            var entity = await _context.Departments
                .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted);

            if (entity != null)
            {
                entity.Status = entity.Status == "active" ? "inactive" : "active";
                Update(entity);
                return await _unitOfWork.CommitAsync().ConfigureAwait(false);
            }
            else
                return 0;
        }

        public async Task<int> UpdateDepartmentAsync(Department entity)
        {
            Attach(entity, updatedProperties: new Expression<Func<Department, object>>[]
            {
                d => d.Name,
                d => d.Code!,
                d => d.FacultyId,
                d => d.Description!,
                d => d.DisplayOrder,
                d => d.Status,
                d => d.UpdatedBy!,
            });
            return await _unitOfWork.CommitAsync().ConfigureAwait(false);
        }
    }
}

