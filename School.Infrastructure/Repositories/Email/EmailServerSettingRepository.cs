using Microsoft.EntityFrameworkCore;
using School.Domain.Email;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;
using System.Linq.Expressions;

namespace School.Infrastructure.Repositories.Email
{
    public class EmailServerSettingRepository : Repository<EmailServerSetting>, IEmailServerSettingRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public EmailServerSettingRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork)
            : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<EmailServerSetting?> GetByIdAsync(int id)
            => await FindAsync(x => x.Id == id && !x.IsDeleted);

        public async Task<IEnumerable<EmailServerSetting>> GetAllBySchoolIdAsync(int schoolId)
            => await List(x => !x.IsDeleted && x.SchoolRegistrationId == schoolId)
                     .OrderByDescending(x => x.IsActive)
                     .ThenBy(x => x.DisplayName)
                     .ToListAsync();

        public async Task<EmailServerSetting> AddAsync(EmailServerSetting entity)
        {
            // Prevent duplicate SMTP config for same FromEmail in same school
            var exists = await DbSet.AnyAsync(x =>
                x.FromEmail.ToLower() == entity.FromEmail.ToLower() &&
                x.SchoolRegistrationId == entity.SchoolRegistrationId &&
                !x.IsDeleted);

            if (exists)
            {
                entity.Id = 0; // Signal duplicate
                return entity;
            }

            await base.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<int> UpdateAsync(EmailServerSetting entity)
        {
            Attach(entity, updatedProperties: new Expression<Func<EmailServerSetting, object>>[]
            {
                u => u.DisplayName!,
                u => u.FromEmail!,
                u => u.HostName!,
                u => u.Port,
                u => u.UserName!,
                u => u.EnableSSL,
                u => u.UseDefaultCredential,
                u => u.IsActive,
                u => u.UpdatedBy!,
                u => u.UpdatedDate
            });
            return await _unitOfWork.CommitAsync();
        }

        public async Task<int> UpdatePasswordAsync(EmailServerSetting entity)
        {
            Attach(entity, updatedProperties: new Expression<Func<EmailServerSetting, object>>[]
            {
                u => u.Password!,
                u => u.UpdatedBy!,
                u => u.UpdatedDate
            });
            return await _unitOfWork.CommitAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await FindAsync(x => x.Id == id);
            if (entity == null) return 0;
            entity.UpdatedDate = DateTime.UtcNow;
            Delete(entity);
            return await _unitOfWork.CommitAsync();
        }

        public async Task<int> ToggleStatusAsync(int id)
        {
            var entity = await FindAsync(x => x.Id == id && !x.IsDeleted);
            if (entity == null) return 0;
            entity.IsActive = !entity.IsActive;
            entity.UpdatedDate = DateTime.UtcNow;
            return await _unitOfWork.CommitAsync();
        }
    }
}
