using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using School.Domain.Location;

namespace School.Infrastructure.Repositories
{
    public class StateRepository : Repository<State>, IStateRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public StateRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork)
            : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<State> AddStateAsync(State entity)
        {
            var existingState = await DbSet.FirstOrDefaultAsync(x =>
                               x.Name.ToLower() == entity.Name.ToLower() &&
                               !x.IsDeleted);

            if (existingState != null)
            {
                existingState.Id = 0;
                return existingState;
            }
            await base.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<int> DeleteStateAsync(int id)
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

        public async Task<IEnumerable<State>> GetAllAsync()
        {
            return await List(expression: x => !x.IsDeleted).OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<State> GetStateByIdAsync(int id)
        {
            return await FindAsync(expression: x => x.Id == id && !x.IsDeleted) ?? new State();
        }

        public async Task<int> ToggleStateStatusAsync(int id)
        {
            var entity = await _context.States
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

            if (entity != null)
            {
                entity.IsActive = !entity.IsActive;
                Update(entity);
                return await _unitOfWork.CommitAsync().ConfigureAwait(false);
            }
            else
                return 0;
        }

        public async Task<int> UpdateStateAsync(State entity)
        {
            Attach(entity, updatedProperties: new Expression<Func<State, object>>[]
            {
                u => u.Name,
                u => u.StateCode!,
                u => u.Description!,
                u => u.UpdatedBy!,
            });
            return await _unitOfWork.CommitAsync().ConfigureAwait(false);
        }
    }
}
