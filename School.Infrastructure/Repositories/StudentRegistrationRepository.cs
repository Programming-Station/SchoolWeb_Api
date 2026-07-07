using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Infrastructure.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using School.Domain.Student;
using System.Linq.Expressions;

namespace School.Infrastructure.Repositories
{
    public class StudentRegistrationRepository : Repository<StudentRegistration>, IStudentRegistrationRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SchoolDbContext _context;

        public StudentRegistrationRepository(DbFactory dbFactory, IUnitOfWork unitOfWork, SchoolDbContext context) : base(dbFactory)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<StudentRegistration> AddStudentRegistrationAsync(StudentRegistration entity)
        {
            var existingByMobile = await DbSet.FirstOrDefaultAsync(x =>
                               x.Mobile == entity.Mobile &&
                               !x.IsDeleted);

            if (existingByMobile != null)
            {
                existingByMobile.Id = 0;
                return existingByMobile;
            }

            var existingByAadhaar = await DbSet.FirstOrDefaultAsync(x =>
                               x.AadhaarNumber == entity.AadhaarNumber &&
                               !x.IsDeleted);

            if (existingByAadhaar != null)
            {
                existingByAadhaar.Id = 0;
                return existingByAadhaar;
            }

            await AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<int> DeleteStudentRegistrationAsync(int id)
        {
            var result = await FindAsync(expression: x => x.Id == id && !x.IsDeleted);

            if (result != null)
            {
                result.UpdatedDate = DateTime.Now;
                Delete(result);
                return await _unitOfWork.CommitAsync().ConfigureAwait(false);
            }
            else
                return 0;
        }

        public async Task<IEnumerable<StudentRegistration>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, string? status = null)
        {
            var query = List(expression: x => !x.IsDeleted) 
                .Include(x => x.Course)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(x =>
                    x.FullName.Contains(searchTerm) ||
                    x.Mobile.Contains(searchTerm) ||
                    x.AadhaarNumber.Contains(searchTerm) ||
                    (x.Email != null && x.Email.Contains(searchTerm)) ||
                    x.InstituteName.Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(x => x.RegistrationStatus == status);
            }

            return await query
                .OrderByDescending(x => x.CreatedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync(string? searchTerm = null, string? status = null)
        {
            var query = List(expression: x => !x.IsDeleted).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(x =>
                    x.FullName.Contains(searchTerm) ||
                    x.Mobile.Contains(searchTerm) ||
                    x.AadhaarNumber.Contains(searchTerm) ||
                    (x.Email != null && x.Email.Contains(searchTerm)) ||
                    x.InstituteName.Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(x => x.RegistrationStatus == status);
            }

            return await query.CountAsync();
        }

        public async Task<StudentRegistration> GetStudentRegistrationByIdAsync(int id)
        {
            return await DbSet
                .Include(x => x.ExperienceCertificates)
                .Include(x => x.EducationalDetails)
                .Include(x => x.Course)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted) ?? new StudentRegistration();
        }

        public async Task<int> UpdateStudentRegistrationAsync(StudentRegistration entity)
        {
            Attach(entity, updatedProperties: new Expression<Func<StudentRegistration, object>>[]
            {
                u => u.AcademicYear,
                u => u.CouncilEnrollmentNo!,
                u => u.InstituteName,
                u => u.CourseType,
                u => u.CourseId,
                u => u.PassYear,
                u => u.FullName,
                u => u.FathersName,
                u => u.MothersName,
                u => u.Gender,
                u => u.DateOfBirth!,
                u => u.BloodGroup!,
                u => u.PermanentAddress,
                u => u.PinCode,
                u => u.Mobile,
                u => u.GuardianMobile!,
                u => u.Email!,
                u => u.AadhaarNumber,
                u => u.PassportPhoto!,
                u => u.PaymentStatus,
                u => u.PaymentTransactionId!,
                u => u.PaymentAmount!,
                u => u.RegistrationStatus,
                u => u.Remarks!,
                u => u.UpdatedBy!,
            });
            return await _unitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public async Task<StudentRegistration> GetByMobileAsync(string mobile)
        {
            return await DbSet
                .Include(x => x.ExperienceCertificates)
                .Include(x => x.EducationalDetails)
                .Include(x => x.Course)
                .FirstOrDefaultAsync(x => x.Mobile == mobile && !x.IsDeleted) ?? new StudentRegistration();
        }

        public async Task<StudentRegistration> GetByAadhaarAsync(string aadhaarNumber)
        {
            return await DbSet
                .Include(x => x.ExperienceCertificates)
                .Include(x => x.EducationalDetails)
                .Include(x => x.Course)
                .FirstOrDefaultAsync(x => x.AadhaarNumber == aadhaarNumber && !x.IsDeleted) ?? new StudentRegistration();
        }

        public async Task<bool> ExistsByMobileAsync(string mobile, int? excludeId = null)
        {
            var query = DbSet.Where(x => x.Mobile == mobile && !x.IsDeleted);
            
            if (excludeId.HasValue)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> ExistsByAadhaarAsync(string aadhaarNumber, int? excludeId = null)
        {
            var query = DbSet.Where(x => x.AadhaarNumber == aadhaarNumber && !x.IsDeleted);
            
            if (excludeId.HasValue)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }
    }
}
