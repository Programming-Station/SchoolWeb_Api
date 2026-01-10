using School.Domain.Website; 
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace School.Infrastructure.Repositories
{
    public class SchoolRegistrationRepository : Repository<SchoolRegistration>, ISchoolRegistrationRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public SchoolRegistrationRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<SchoolRegistration> AddSchoolRegistrationAsync(SchoolRegistration entity)
        {
            // Check if email already exists
            var existingByEmail = await DbSet.FirstOrDefaultAsync(x =>
                               x.PrincipalEmail.ToLower() == entity.PrincipalEmail.ToLower() &&
                               !x.IsDeleted);

            if (existingByEmail != null)
            {
                existingByEmail.Id = 0;
                return existingByEmail;
            }

            // Check if mobile already exists
            var existingByMobile = await DbSet.FirstOrDefaultAsync(x =>
                               (x.PhoneNumber == entity.PhoneNumber || x.AlternatePhone == entity.PhoneNumber) &&
                               !x.IsDeleted);

            if (existingByMobile != null)
            {
                existingByMobile.Id = 0;
                return existingByMobile;
            }

            await AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<SchoolRegistration> GetSchoolRegistrationByIdAsync(int id)
        {
            return await DbSet
                .Include(x => x.Status)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted) ?? new SchoolRegistration();
        }

        public async Task<IEnumerable<SchoolRegistration>> GetAllAsync(string? status = null, int? pageNumber = null, int? pageSize = null)
        {
            var query = List(expression: x => !x.IsDeleted)
                .Include(x => x.Status)
                .AsQueryable();

            // Filter by status name if provided
            if (!string.IsNullOrEmpty(status))
            {
                // Try to find status by name first
                var statusEntity = await _context.Statuses
                    .FirstOrDefaultAsync(s => s.Name.ToLower() == status.ToLower());

                if (statusEntity != null)
                {
                    query = query.Where(x => x.StatusId == statusEntity.Id);
                }
                else if (int.TryParse(status, out int statusId))
                {
                    // If status is a number, use it as StatusId
                    query = query.Where(x => x.StatusId == statusId);
                }
            }

            // Apply pagination if provided
            if (pageNumber.HasValue && pageSize.HasValue && pageNumber.Value > 0 && pageSize.Value > 0)
            {
                var skip = (pageNumber.Value - 1) * pageSize.Value;
                query = query.Skip(skip).Take(pageSize.Value);
            }

            return await query
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync(string? status = null)
        {
            var query = List(expression: x => !x.IsDeleted).AsQueryable();

            // Filter by status name if provided
            if (!string.IsNullOrEmpty(status))
            {
                // Try to find status by name first
                var statusEntity = await _context.Statuses
                    .FirstOrDefaultAsync(s => s.Name.ToLower() == status.ToLower());

                if (statusEntity != null)
                {
                    query = query.Where(x => x.StatusId == statusEntity.Id);
                }
                else if (int.TryParse(status, out int statusId))
                {
                    // If status is a number, use it as StatusId
                    query = query.Where(x => x.StatusId == statusId);
                }
            }

            return await query.CountAsync();
        }

        public async Task<int> UpdateSchoolRegistrationAsync(SchoolRegistration entity)
        {
            Attach(entity, updatedProperties: new Expression<Func<SchoolRegistration, object>>[]
            {
                u => u.SchoolName,
                u => u.SchoolType,
                u => u.EstablishmentYear,
                u => u.PrincipalName,
                u => u.PrincipalEmail,
                u => u.Address,
                u => u.City,
                u => u.State,
                u => u.Pincode,
                u => u.PhoneNumber,
                u => u.AlternatePhone!,
                u => u.BoardAffiliation,
                u => u.GovernmentRegistrationNumber!,
                u => u.SchoolWebsite!,
                u => u.FacilitiesJson,
                u => u.Description!,
                u => u.TermsAccepted,
                u => u.StatusId,
                u => u.UpdatedBy!,
                u => u.UpdatedDate
            });
            return await _unitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public async Task<int> UpdateSchoolRegistrationStatusAsync(int id, int statusId, string? approvedBy = null, string? rejectionReason = null)
        {
            var registration = await DbSet
                .Include(r => r.Status)
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

            if (registration == null)
            {
                return 0;
            }

            registration.StatusId = statusId;
            registration.UpdatedDate = DateTime.UtcNow;
            registration.UpdatedBy = approvedBy;

            var newStatus = await _context.Statuses
                .FirstOrDefaultAsync(s => s.Id == statusId);

            // Check if status is "Approved" or "Active" (StatusId = 8 or 4)
            if (statusId == 8 || newStatus?.Name.ToLower() == "approved" || newStatus?.Name.ToLower() == "active")
            {
                registration.ApprovedAt = DateTime.UtcNow;
                registration.ApprovedBy = approvedBy;
                // Generate registration number if not exists
                if (string.IsNullOrEmpty(registration.RegistrationNumber))
                {
                    registration.RegistrationNumber = await GenerateRegistrationNumberAsync();
                }
            }
            // Check if status is "Rejected" or "Blocked" (StatusId = 9 or 6)
            else if (statusId == 9 || newStatus?.Name.ToLower() == "rejected" || newStatus?.Name.ToLower() == "blocked")
            {
                if (!string.IsNullOrEmpty(rejectionReason))
                {
                    registration.RejectionReason = rejectionReason;
                }
            }

            Update(registration);
            return await _unitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public async Task<int> DeleteSchoolRegistrationAsync(int id)
        {
            var result = await FindAsync(expression: x => x.Id == id && !x.IsDeleted);

            if (result != null)
            {
                result.UpdatedDate = DateTime.UtcNow;
                Delete(result);
                return await _unitOfWork.CommitAsync().ConfigureAwait(false);
            }
            else
                return 0;
        }

        public async Task<bool> ExistsByEmailAsync(string email, int? excludeId = null)
        {
            var query = DbSet.Where(x => x.PrincipalEmail.ToLower() == email.ToLower().Trim() && !x.IsDeleted);

            if (excludeId.HasValue)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> ExistsByMobileAsync(string mobile, int? excludeId = null)
        {
            var trimmedMobile = mobile.Trim();
            var query = DbSet.Where(x =>
                (x.PhoneNumber == trimmedMobile || x.AlternatePhone == trimmedMobile) &&
                !x.IsDeleted);

            if (excludeId.HasValue)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<SchoolRegistration> GetByEmailAsync(string email)
        {
            return await DbSet
                .Include(x => x.Status)
                .FirstOrDefaultAsync(x => x.PrincipalEmail.ToLower() == email.ToLower().Trim() && !x.IsDeleted) ?? new SchoolRegistration();
        }

        public async Task<SchoolRegistration> GetByMobileAsync(string mobile)
        {
            var trimmedMobile = mobile.Trim();
            return await DbSet
                .Include(x => x.Status)
                .FirstOrDefaultAsync(x =>
                    (x.PhoneNumber == trimmedMobile || x.AlternatePhone == trimmedMobile) &&
                    !x.IsDeleted) ?? new SchoolRegistration();
        }

        public async Task<string> GenerateRegistrationNumberAsync()
        {
            var year = DateTime.UtcNow.Year;
            var prefix = $"SR{year}";

            var lastRegistration = await _context.SchoolRegistrations
                .Where(r => r.RegistrationNumber != null && r.RegistrationNumber.StartsWith(prefix))
                .OrderByDescending(r => r.RegistrationNumber)
                .Select(r => r.RegistrationNumber)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (!string.IsNullOrEmpty(lastRegistration))
            {
                var lastPart = lastRegistration.Substring(prefix.Length);
                if (int.TryParse(lastPart, out int lastNum))
                {
                    nextNumber = lastNum + 1;
                }
            }

            return $"{prefix}{nextNumber:D4}";
        }
    }
}
