using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories
{
    public class AffiliatedRepository : Repository<Affiliated>, IAffiliatedRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public AffiliatedRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<Affiliated> AddAffiliationCollegeAsync(Affiliated entity)
        {
            if (!string.IsNullOrEmpty(entity.CollegeCode))
            {
                var existingByCode = await DbSet.FirstOrDefaultAsync(x =>
                                   x.CollegeCode == entity.CollegeCode &&
                                   !x.IsDeleted);

                if (existingByCode != null)
                {
                    existingByCode.Id = 0;
                    return existingByCode;
                }
            }

            var existingByName = await DbSet.FirstOrDefaultAsync(x =>
                               x.CollegeName.ToLower() == entity.CollegeName.ToLower() &&
                               x.StateId == entity.StateId &&
                               x.CityId == entity.CityId &&
                               !x.IsDeleted);

            if (existingByName != null)
            {
                existingByName.Id = 0;
                return existingByName;
            }

            await base.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<Affiliated> GetAffiliationCollegeByIdAsync(int id)
        {
            return await DbSet
                .Include(x => x.State)
                    .ThenInclude(s => s.Cities)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted) ?? new Affiliated();
        }

        public async Task<IEnumerable<Affiliated>> GetAllAsync(int? stateId = null, int? cityId = null, bool? isActive = null)
        {
            var query = List(expression: x => !x.IsDeleted)
                .Include(x => x.State)
                    .ThenInclude(s => s.Cities)
                .AsQueryable();

            if (stateId.HasValue && stateId.Value > 0)
            {
                query = query.Where(x => x.StateId == stateId.Value);
            }

            if (cityId.HasValue && cityId.Value > 0)
            {
                query = query.Where(x => x.CityId == cityId.Value);
            }

            if (isActive.HasValue)
            {
                query = query.Where(x => x.IsActive == isActive.Value);
            }

            return await query
                .OrderBy(x => x.CollegeName)
                .ToListAsync();
        }

        public async Task<int> UpdateAffiliationCollegeAsync(Affiliated entity)
        {
            Attach(entity, updatedProperties: new Expression<Func<Affiliated, object>>[]
            {
                u => u.CollegeName,
                u => u.CollegeCode!,
                u => u.UniversityName!,
                u => u.UniversityCode!,
                u => u.StateId,
                u => u.CityId,
                u => u.Address!,
                u => u.Pincode!,
                u => u.ContactPerson!,
                u => u.MobileNo!,
                u => u.Email!,
                u => u.ImagePath!,
                u => u.IsActive,
                u => u.UpdatedBy!,
                u => u.UpdatedDate
            });
            return await _unitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public async Task<int> DeleteAffiliationCollegeAsync(int id)
        {
            var result = await FindAsync(expression: x => x.Id == id && !x.IsDeleted);

            if (result != null)
            {
                result.UpdatedDate = DateTime.UtcNow;
                Delete(result);
                return await _unitOfWork.CommitAsync().ConfigureAwait(false);
            }
            else
                return 0;
        }

        public async Task<int> ToggleAffiliationCollegeStatusAsync(int id)
        {
            var entity = await _context.Affiliateds
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

            if (entity != null)
            {
                entity.IsActive = !entity.IsActive;
                Update(entity);
                return await _unitOfWork.CommitAsync().ConfigureAwait(false);
            }
            else
                return 0;
        }
    }
}
