using Microsoft.EntityFrameworkCore;
using School.Domain.School;
using School.Infrastructure.Repositories.School;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace School.Infrastructure.Repositories
{
    public class SchoolOwnerRepository : Repository<SchoolOwner>, ISchoolOwnerRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public SchoolOwnerRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public IQueryable<SchoolOwner> GetAllQueryable()
        {
            return List(x => true);
        }

        public async Task<SchoolOwner?> GetByIdAsync(int id)
        {
            return await FindAsync(x => x.Id == id);
        }

        public new async Task<int> AddAsync(SchoolOwner entity)
        {
            await _context.Set<SchoolOwner>().AddAsync(entity);
            return await _unitOfWork.CommitAsync();
        }

        public async Task<int> UpdateAsync(SchoolOwner entity)
        {
            var existing = await FindAsync(x => x.Id == entity.Id);
            if (existing != null)
            {
                existing.SchoolRegistrationId = entity.SchoolRegistrationId;
                existing.SchoolSubscriptionId = entity.SchoolSubscriptionId;
                existing.ApplicationUserId = entity.ApplicationUserId;
                existing.ProfilePhoto = entity.ProfilePhoto;
                existing.StatusId = entity.StatusId;
                existing.EmailVerified = entity.EmailVerified;
                existing.MobileVerified = entity.MobileVerified;
                existing.IsLocked = entity.IsLocked;
                existing.UpdatedDate = DateTime.UtcNow;
                _context.Set<SchoolOwner>().Update(existing);
                return await _unitOfWork.CommitAsync();
            }
            return 0;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await FindAsync(x => x.Id == id);
            if (entity != null)
            {
                _context.Set<SchoolOwner>().Remove(entity);
                return await _unitOfWork.CommitAsync();
            }
            return 0;
        }
    }
}


