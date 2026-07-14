using Microsoft.EntityFrameworkCore;
using School.Domain.School;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace School.Infrastructure.Repositories.School
{
    public class OrganizationProfileRepository : Repository<OrganizationProfile>, IOrganizationProfileRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public OrganizationProfileRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<OrganizationProfile?> GetByTenantIdAsync(int tenantId)
        {
            return await FindAsync(expression: x => x.SchoolRegistrationId == tenantId && !x.IsDeleted);
        }

        public async Task<OrganizationProfile> AddAsync(OrganizationProfile entity)
        {
            entity = await base.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<int> UpdateAsync(OrganizationProfile entity)
        {
            Attach(entity, updatedProperties: new Expression<Func<OrganizationProfile, object>>[]
            {
                u => u.OrganizationName,
                u => u.ShortName,
                u => u.SchoolName,
                u => u.CollegeName,
                u => u.UniversityName,
                u => u.CampusName,
                u => u.AffiliationNumber,
                u => u.RecognitionNumber,
                u => u.SchoolCode,
                u => u.CollegeCode,
                u => u.UniversityCode,
                u => u.Board,
                u => u.University,
                u => u.RegistrationNumber,
                u => u.GSTNumber,
                u => u.PANNumber,
                u => u.UDISENumber,
                u => u.AISHECode,
                u => u.AddressLine1,
                u => u.AddressLine2,
                u => u.City,
                u => u.District,
                u => u.State,
                u => u.Country,
                u => u.Pincode,
                u => u.Latitude,
                u => u.Longitude,
                u => u.Phone,
                u => u.Mobile,
                u => u.WhatsApp,
                u => u.Email,
                u => u.Website,
                u => u.FacebookUrl,
                u => u.InstagramUrl,
                u => u.LinkedInUrl,
                u => u.TwitterUrl,
                u => u.YouTubeUrl,
                u => u.PrincipalName,
                u => u.DirectorName,
                u => u.ChairmanName,
                u => u.SecretaryName,
                u => u.RegistrarName,
                u => u.PrincipalSignature!,
                u => u.DirectorSignature!,
                u => u.DigitalSignature!,
                u => u.OfficialSeal!,
                u => u.RoundStamp!,
                u => u.SquareStamp!,
                u => u.HeaderLogo!,
                u => u.FooterLogo!,
                u => u.ReportWatermark!,
                u => u.ReportBackground!,
                u => u.QRCode!,
                u => u.BarcodePrefix!,
                u => u.Favicon!,
                u => u.PrimaryColor!,
                u => u.SecondaryColor!,
                u => u.Theme!,
                u => u.ReportFooterText!,
                u => u.CopyrightText!,
                u => u.TermsAndConditions!,
                u => u.Status,
                u => u.UpdatedDate!,
                u => u.UpdatedBy!
            });

            return await _unitOfWork.CommitAsync().ConfigureAwait(false);
        }
    }
}
