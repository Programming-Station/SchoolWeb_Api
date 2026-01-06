using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces; 
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace School.Infrastructure.Repositories
{

    public class CityRepository : Repository<City>, ICityRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public CityRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork)
            : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
        public async Task<City> AddCityAsync(City entity)
        {
            var existingCity = await DbSet.FirstOrDefaultAsync(x =>
                               x.Name.ToLower() == entity.Name.ToLower() &&
                               x.StateId == entity.StateId);

            if (existingCity != null)
            {
                existingCity.Id = 0;
                return existingCity;
            }
            await AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;


        }

        public async Task<int> DeleteCityAsync(int id)
        {
            var entity = await _context.Cities
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            var result = await FindAsync(expression: x => x.Id == id);

            if (result != null)
            {
                result.UpdatedDate = DateTime.Now;
                Delete(result);
                return await _unitOfWork.CommitAsync().ConfigureAwait(false);

            }
            else
                return 0;
        }

        public async Task<IEnumerable<City>> GetAllAsync(int? stateId = null)
        {
            return stateId == null ? await List(expression: x => !x.IsDeleted).ToListAsync() :
                await List(expression: x => !x.IsDeleted && x.StateId == stateId).ToListAsync();
        }


        public async Task<City> GetCityByIdAsync(int id)
        {
            return await FindAsync(expression: x => x.Id == id) ?? new City();
        }


        public async Task<int> ToggleCityStatusAsync(int id)
        {
            var entity = await _context.Cities
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

        public async Task<int> UpdateCityAsync(City entity)
        {
            Attach(entity, updatedProperties: new Expression<Func<City, object>>[]
            {
                u => u.Name,
                u => u.CityCode!,
                u => u.Name!,
                u => u.StateId!,
                u => u.Description!,
                u => u.UpdatedBy!,

            });
            return await _unitOfWork.CommitAsync().ConfigureAwait(false);
        }
    }
}
