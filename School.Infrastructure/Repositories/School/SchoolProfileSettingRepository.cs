using Microsoft.EntityFrameworkCore;
using School.Domain.School;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;
using System.Threading.Tasks;

namespace School.Infrastructure.Repositories
{
    public class SchoolProfileSettingRepository : Repository<SchoolProfileSetting>, ISchoolProfileSettingRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public SchoolProfileSettingRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<SchoolProfileSetting?> GetBySchoolIdAsync(int schoolRegistrationId)
        {
            return await _context.SchoolProfileSettings
                .Include(s => s.PrimaryMedium)
                .FirstOrDefaultAsync(x => x.SchoolRegistrationId == schoolRegistrationId);
        }

        public async Task<int> UpdateProfileSettingAsync(SchoolProfileSetting entity)
        {
            var existing = await _context.SchoolProfileSettings
                .FirstOrDefaultAsync(x => x.SchoolRegistrationId == entity.SchoolRegistrationId);

            if (existing != null)
            {
                // Update existing
                existing.BankAccountName = entity.BankAccountName;
                existing.BankAccountNumber = entity.BankAccountNumber;
                existing.BankIFSCCode = entity.BankIFSCCode;
                existing.BankName = entity.BankName;
                existing.BankBranch = entity.BankBranch;

                existing.Latitude = entity.Latitude;
                existing.Longitude = entity.Longitude;

                existing.FacebookUrl = entity.FacebookUrl;
                existing.InstagramUrl = entity.InstagramUrl;
                existing.TwitterUrl = entity.TwitterUrl;

                existing.Tagline = entity.Tagline;
                existing.PrimaryMediumId = entity.PrimaryMediumId;

                _context.SchoolProfileSettings.Update(existing);
            }
            else
            {
                // Add new
                await _context.SchoolProfileSettings.AddAsync(entity);
            }

            return await _unitOfWork.CommitAsync();
        }
    }
}

