using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public EventRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<Event> AddEventAsync(Event entity)
        {
            var existingEvent = await DbSet.FirstOrDefaultAsync(x =>
                               x.Title.ToLower() == entity.Title.ToLower() &&
                               x.EventDate.Date == entity.EventDate.Date &&
                               !x.IsDeleted);

            if (existingEvent != null)
            {
                existingEvent.Id = 0;
                return existingEvent;
            }

            entity.IsUpcoming = entity.EventDate.Date >= DateTime.Today;

            await base.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<int> DeleteEventAsync(int id)
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

        public async Task<IEnumerable<Event>> GetAllEventsAsync(bool? upcomingOnly = null)
        {
            var today = DateTime.Today;

            if (upcomingOnly == true)
            {
                return await List(expression: x => !x.IsDeleted && x.EventDate.Date >= today && x.IsActive)
                    .OrderBy(x => x.EventDate)
                    .ToListAsync();
            }
            else
            {
                return await List(expression: x => !x.IsDeleted)
                    .OrderByDescending(x => x.EventDate)
                    .ToListAsync();
            }
        }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            return await FindAsync(expression: x => x.Id == id && !x.IsDeleted) ?? new Event();
        }

        public async Task<int> ToggleEventStatusAsync(int id)
        {
            var entity = await _context.Events
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

        public async Task<int> UpdateEventAsync(Event entity)
        {
            entity.IsUpcoming = entity.EventDate.Date >= DateTime.Today;

            Attach(entity, updatedProperties: new Expression<Func<Event, object>>[]
            {
                e => e.Title,
                e => e.Description!,
                e => e.EventDate,
                e => e.Location!,
                e => e.ImagePath!,
                e => e.IsActive,
                e => e.IsUpcoming,
                e => e.UpdatedBy!,
            });
            return await _unitOfWork.CommitAsync().ConfigureAwait(false);
        }
    }
}

