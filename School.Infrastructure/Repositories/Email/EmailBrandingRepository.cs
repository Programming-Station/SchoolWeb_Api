using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using School.Domain.Email;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories.Email
{
    public class EmailBrandingRepository : Repository<EmailBranding>, IEmailBrandingRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public EmailBrandingRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork)
            : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<EmailBranding?> GetByIdAsync(int id)
            => await FindAsync(x => x.Id == id && !x.IsDeleted);

        public async Task<EmailBranding?> GetBySchoolIdAsync(int schoolId)
            => await FindAsync(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted);

        public async Task<EmailBranding> AddAsync(EmailBranding entity)
        {
            // Only one branding record per school allowed
            var exists = await DbSet.AnyAsync(x =>
                x.SchoolRegistrationId == entity.SchoolRegistrationId && !x.IsDeleted);

            if (exists)
            {
                entity.Id = 0; // Signal: already exists
                return entity;
            }

            await base.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<int> UpdateAsync(EmailBranding entity)
        {
            Attach(entity, updatedProperties: new Expression<Func<EmailBranding, object>>[]
            {
                u => u.ThemeColor!,
                u => u.HeaderHtml!,
                u => u.FooterHtml!,
                u => u.SupportEmail!,
                u => u.SupportPhone!,
                u => u.PrincipalName!,
                u => u.UpdatedBy!,
                u => u.UpdatedDate
            });
            return await _unitOfWork.CommitAsync();
        }
    }
}
