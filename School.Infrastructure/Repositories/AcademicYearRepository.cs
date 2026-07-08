using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace School.Infrastructure.Repositories
{
    public class AcademicYearRepository : Repository<AcademicYear>, IAcademicYearRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public AcademicYearRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<AcademicYear> AddAcademicYearAsync(AcademicYear entity)
        {
            var existingYear = await DbSet.FirstOrDefaultAsync(x =>
                               x.YearName.ToLower() == entity.YearName.ToLower() &&
                               !x.IsDeleted);

            if (existingYear != null)
            {
                return new AcademicYear { Id = 0 }; // Return entity with Id = 0 to indicate duplicate name
            }



            if (entity.IsCurrent)
            {
                var currentYears = await _context.AcademicYears
                    .Where(ay => ay.IsCurrent && !ay.IsDeleted)
                    .ToListAsync();

                foreach (var year in currentYears)
                {
                    year.IsCurrent = false;
                    year.UpdatedDate = DateTime.Now;
                    Update(year);
                }
            }

            await base.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<int> DeleteAcademicYearAsync(int id)
        {
            var result = await FindAsync(expression: x => x.Id == id && !x.IsDeleted);

            if (result != null)
            {
                if (result.IsCurrent)
                {
                    return -1; // Return -1 to indicate cannot delete current year
                }

                result.UpdatedDate = DateTime.Now;
                Delete(result);
                return await _unitOfWork.CommitAsync().ConfigureAwait(false);
            }
            else
                return 0;
        }

        public async Task<IEnumerable<AcademicYear>> GetAllAcademicYearsAsync(bool? isActive = null)
        {
            if (isActive.HasValue)
            {
                return await List(expression: x => !x.IsDeleted && x.IsActive == isActive.Value)
                    .OrderByDescending(x => x.StartDate)
                    .ToListAsync();
            }
            return await List(expression: x => !x.IsDeleted)
                .OrderByDescending(x => x.StartDate)
                .ToListAsync();
        }

        public async Task<AcademicYear> GetAcademicYearByIdAsync(int id)
        {
            return await FindAsync(expression: x => x.Id == id && !x.IsDeleted) ?? new AcademicYear();
        }

        public async Task<AcademicYear> GetCurrentAcademicYearAsync()
        {
            return await _context.AcademicYears
                .FirstOrDefaultAsync(ay => ay.IsCurrent && !ay.IsDeleted && ay.IsActive) ?? new AcademicYear();
        }

        public async Task<int> SetCurrentAcademicYearAsync(int id)
        {
            var entity = await _context.AcademicYears
                .FirstOrDefaultAsync(ay => ay.Id == id && !ay.IsDeleted);

            if (entity != null)
            {
                if (!entity.IsActive)
                {
                    return -1; // Return -1 to indicate cannot set inactive year as current
                }

                var currentYears = await _context.AcademicYears
                    .Where(ay => ay.IsCurrent && !ay.IsDeleted && ay.Id != id)
                    .ToListAsync();

                foreach (var year in currentYears)
                {
                    year.IsCurrent = false;
                    year.UpdatedDate = DateTime.Now;
                    Update(year);
                }

                entity.IsCurrent = true;
                entity.UpdatedDate = DateTime.Now;
                Update(entity);
                return await _unitOfWork.CommitAsync().ConfigureAwait(false);
            }
            else
                return 0;
        }

        public async Task<int> ToggleAcademicYearStatusAsync(int id)
        {
            var entity = await _context.AcademicYears
                .FirstOrDefaultAsync(ay => ay.Id == id && !ay.IsDeleted);

            if (entity != null)
            {
                entity.IsActive = !entity.IsActive;
                Update(entity);
                return await _unitOfWork.CommitAsync().ConfigureAwait(false);
            }
            else
                return 0;
        }

        public async Task<int> UpdateAcademicYearAsync(AcademicYear entity)
        {
            var existingYear = await _context.AcademicYears
                .FirstOrDefaultAsync(ay => ay.YearName.ToLower() == entity.YearName.ToLower() && 
                    ay.Id != entity.Id && !ay.IsDeleted);

            if (existingYear != null)
            {
                return -1; // Return -1 to indicate duplicate name
            }

            var overlappingYear = await _context.AcademicYears
                .AnyAsync(ay => !ay.IsDeleted && ay.Id != entity.Id &&
                    ((entity.StartDate >= ay.StartDate && entity.StartDate <= ay.EndDate) ||
                     (entity.EndDate >= ay.StartDate && entity.EndDate <= ay.EndDate) ||
                     (entity.StartDate <= ay.StartDate && entity.EndDate >= ay.EndDate)));

            if (overlappingYear)
            {
                return -2; // Return -2 to indicate overlapping dates
            }

            if (entity.IsCurrent)
            {
                var currentYears = await _context.AcademicYears
                    .Where(ay => ay.IsCurrent && !ay.IsDeleted && ay.Id != entity.Id)
                    .ToListAsync();

                foreach (var year in currentYears)
                {
                    year.IsCurrent = false;
                    year.UpdatedDate = DateTime.Now;
                    Update(year);
                }
            }

            Attach(entity, updatedProperties: new Expression<Func<AcademicYear, object>>[]
            {
                u => u.YearName,
                u => u.StartDate,
                u => u.EndDate,
                u => u.Description!,
                u => u.IsActive,
                u => u.IsCurrent,
                u => u.UpdatedBy!,
            });
            return await _unitOfWork.CommitAsync().ConfigureAwait(false);
        }
    }
}
