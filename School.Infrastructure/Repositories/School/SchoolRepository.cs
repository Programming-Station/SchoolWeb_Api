using Microsoft.EntityFrameworkCore;
using School.Domain.School;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories.School
{
    public class SchoolRepository : Repository<SchoolRegistration>, ISchoolRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public SchoolRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<SchoolRegistration> AddSchoolAsync(SchoolRegistration entity)
        {
            entity = await base.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;

        }

        public async Task<int> DeleteSchoolAsync(int id)
        {
            var result = await FindAsync(x => x.Id == id);
            if (result != null)
            {
                result.UpdatedDate = DateTime.Now;
                result.UpdatedBy = "Superadmin";
                this.Delete(result);
                return await _unitOfWork.CommitAsync().ConfigureAwait(false);
            }
            else
                return 0;

        }

        public async Task<IEnumerable<SchoolRegistration>> GetAllSchoolsAsync()
        {
            return await List(expression: x => !x.IsDeleted).ToListAsync();
        }

        public IQueryable<SchoolRegistration> GetAllSchoolsQueryable()
        {
            return List(expression: x => !x.IsDeleted);
        }

        public async Task<SchoolRegistration?> GetSchoolByIdAsync(int id)
        {
            return await _context.SchoolRegistrations
                .Include(x => x.SchoolProfileSetting)
                .Include(x => x.SchoolSubscriptions)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> UpdateSchoolAsync(SchoolRegistration entity)
        {
            var existingEntity = await _context.SchoolRegistrations
                .Include(x => x.SchoolProfileSetting)
                .Include(x => x.SchoolSubscriptions)
                .FirstOrDefaultAsync(x => x.Id == entity.Id);

            if (existingEntity == null)
                return 0;

            // Update SchoolRegistration scalar properties
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

            // Update SchoolProfileSetting (1-to-1 relation)
            if (entity.SchoolProfileSetting != null)
            {
                if (existingEntity.SchoolProfileSetting == null)
                {
                    existingEntity.SchoolProfileSetting = entity.SchoolProfileSetting;
                }
                else
                {
                    _context.Entry(existingEntity.SchoolProfileSetting).CurrentValues.SetValues(entity.SchoolProfileSetting);
                }
            }

            // Update SchoolSubscriptions (1-to-many relation)
            if (entity.SchoolSubscriptions != null)
            {
                // Remove deleted subscriptions
                foreach (var existingSub in existingEntity.SchoolSubscriptions.ToList())
                {
                    if (!entity.SchoolSubscriptions.Any(s => s.Id == existingSub.Id))
                    {
                        _context.SchoolSubscriptions.Remove(existingSub);
                    }
                }

                // Add or update subscriptions
                foreach (var sub in entity.SchoolSubscriptions)
                {
                    var existingSub = existingEntity.SchoolSubscriptions.FirstOrDefault(s => s.Id == sub.Id && sub.Id > 0);
                    if (existingSub == null)
                    {
                        existingEntity.SchoolSubscriptions.Add(sub);
                    }
                    else
                    {
                        _context.Entry(existingSub).CurrentValues.SetValues(sub);
                    }
                }
            }

            return await _unitOfWork.CommitAsync().ConfigureAwait(false);
        }
    }
}
