using Microsoft.EntityFrameworkCore;
using School.Domain.Email;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;
using System.Linq.Expressions;

namespace School.Infrastructure.Repositories.Email
{
    public class EmailTemplateRepository : Repository<EmailTemplate>, IEmailTemplateRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public EmailTemplateRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork)
            : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<EmailTemplate?> GetByIdAsync(int id)
            => await FindAsync(x => x.Id == id && !x.IsDeleted);

        public async Task<IEnumerable<EmailTemplate>> GetAllBySchoolIdAsync(int schoolId)
            => await List(x => !x.IsDeleted && x.SchoolRegistrationId == schoolId)
                     .OrderBy(x => x.TemplateName)
                     .ToListAsync();

        public async Task<EmailTemplate> AddAsync(EmailTemplate entity)
        {
            var exists = await DbSet.AnyAsync(x =>
                x.TemplateName.ToLower() == entity.TemplateName.ToLower() &&
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

        public async Task<int> UpdateAsync(EmailTemplate entity)
        {
            Attach(entity, updatedProperties: new Expression<Func<EmailTemplate, object>>[]
            {
                u => u.TemplateName!,
                u => u.Subject!,
                u => u.BodyHtml!,
                u => u.IsActive,
                u => u.EmailServerSettingId!,
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
