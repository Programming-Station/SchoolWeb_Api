using Microsoft.EntityFrameworkCore;
using School.Domain.School;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace School.Infrastructure.Repositories
{
    public class SchoolSubscriptionRepository : Repository<SchoolSubscription>, ISchoolSubscriptionRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public SchoolSubscriptionRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public IQueryable<SchoolSubscription> GetAllQueryable()
        {
            return List(x => true);
        }

        public async Task<SchoolSubscription?> GetByIdAsync(int id)
        {
            return await FindAsync(x => x.Id == id);
        }

        public async Task<int> AddAsync(SchoolSubscription entity)
        {
            await _context.Set<SchoolSubscription>().AddAsync(entity);
            return await _unitOfWork.CommitAsync();
        }

        public async Task<int> UpdateAsync(SchoolSubscription entity)
        {
            var existing = await FindAsync(x => x.Id == entity.Id);
            if (existing != null)
            {
                existing.SubscriptionPlanId = entity.SubscriptionPlanId;
                existing.StartDate = entity.StartDate;
                existing.EndDate = entity.EndDate;
                existing.AmountPaid = entity.AmountPaid;
                existing.PaymentStatus = entity.PaymentStatus;
                existing.TransactionId = entity.TransactionId;
                existing.IsActive = entity.IsActive;
                existing.UpdatedDate = DateTime.UtcNow;
                _context.Set<SchoolSubscription>().Update(existing);
                return await _unitOfWork.CommitAsync();
            }
            return 0;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await FindAsync(x => x.Id == id);
            if (entity != null)
            {
                _context.Set<SchoolSubscription>().Remove(entity);
                return await _unitOfWork.CommitAsync();
            }
            return 0;
        }
    }
}

