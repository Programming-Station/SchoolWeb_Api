using School.Domain.Website;
using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace School.Infrastructure.Repositories
{
    public class EnquiryRepository : Repository<Enquiry>, IEnquiryRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public EnquiryRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> GenerateEnquiryNoAsync()
        {
            var now = DateTime.UtcNow;

            string year = now.Year.ToString();
            string month = now.Month.ToString("D2");

            string prefix = $"ENQ-{year}-{month}-";

            var lastNo = await _context.Enquiries
                .Where(x => x.EnquiryFromNo.StartsWith(prefix))
                .OrderByDescending(x => x.Id)
                .Select(x => x.EnquiryFromNo)
                .FirstOrDefaultAsync();

            int next = 1;

            if (!string.IsNullOrEmpty(lastNo))
            {
                var lastSeq = lastNo.Split('-').Last();
                if (int.TryParse(lastSeq, out int parsedSeq))
                {
                    next = parsedSeq + 1;
                }
            }

            return $"{prefix}{next:D5}";
        }

        public async Task<Enquiry> AddEnquiryAsync(Enquiry entity)
        {
            // Check if enquiry number already exists
            var existingByNo = await DbSet.FirstOrDefaultAsync(x =>
                               x.EnquiryFromNo == entity.EnquiryFromNo &&
                               !x.IsDeleted);

            if (existingByNo != null)
            {
                existingByNo.Id = 0;
                return existingByNo;
            }

            await AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<Enquiry> GetEnquiryByIdAsync(int id)
        {
            return await DbSet
                .Include(x => x.Status)
                .Include(x => x.Course)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted) ?? new Enquiry();
        }

        public async Task<IEnumerable<Enquiry>> GetAllAsync(int? statusId = null, int? pageNumber = null, int? pageSize = null)
        {
            var query = List(expression: x => !x.IsDeleted)
                .Include(x => x.Status)
                .Include(x => x.Course)
                .AsQueryable();

            if (statusId.HasValue && statusId.Value > 0)
            {
                query = query.Where(x => x.StatusId == statusId.Value);
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

        public async Task<int> GetTotalCountAsync(int? statusId = null)
        {
            var query = List(expression: x => !x.IsDeleted).AsQueryable();

            if (statusId.HasValue && statusId.Value > 0)
            {
                query = query.Where(x => x.StatusId == statusId.Value);
            }

            return await query.CountAsync();
        }

        public async Task<int> UpdateEnquiryAsync(Enquiry entity)
        {
            Attach(entity, updatedProperties: new Expression<Func<Enquiry, object>>[]
            {
                u => u.Name,
                u => u.Email,
                u => u.Mobile,
                u => u.Subject!,
                u => u.Message,
                u => u.Address!,
                u => u.City!,
                u => u.PinCode!,
                u => u.CourseId!,
                u => u.CourseName!,
                u => u.StatusId,
                u => u.UpdatedBy!,
                u => u.UpdatedDate!
            });
            return await _unitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public async Task<int> UpdateEnquiryStatusAsync(int id, int statusId, string? adminReply = null, string? repliedBy = null)
        {
            var enquiry = await DbSet
                .Include(e => e.Status)
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

            if (enquiry == null)
            {
                return 0;
            }

            enquiry.StatusId = statusId;
            enquiry.UpdatedDate = DateTime.UtcNow;
            enquiry.UpdatedBy = repliedBy;

            if (!string.IsNullOrEmpty(adminReply))
            {
                enquiry.AdminReply = adminReply;
                enquiry.RepliedDate = DateTime.UtcNow;
                enquiry.RepliedBy = repliedBy;
            }

            Update(enquiry);
            return await _unitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public async Task<int> DeleteEnquiryAsync(int id)
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
    }
}
