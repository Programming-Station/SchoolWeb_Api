using Microsoft.EntityFrameworkCore;
using School.Domain.Student;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public StudentRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<Student> AddStudentAsync(Student entity)
        {
            if (!string.IsNullOrEmpty(entity.StudentId))
            {
                var existingByStudentId = await DbSet.FirstOrDefaultAsync(x =>
                    x.StudentId == entity.StudentId && !x.IsDeleted);

                if (existingByStudentId != null)
                {
                    existingByStudentId.Id = 0;
                    return existingByStudentId;
                }
            }

            await base.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<Student> GetStudentByIdAsync(int id)
        {
            return await List(expression: x => x.Id == id && !x.IsDeleted)
                .Include(x => x.Class)
                    .ThenInclude(c => c!.Course)
                .Include(x => x.Status)
                .Include(x => x.Course)
                .FirstOrDefaultAsync() ?? new Student();
        }

        public async Task<Student?> GetStudentByStudentIdAsync(string studentId)
        {
            return await List(expression: x => x.StudentId == studentId && !x.IsDeleted)
                .Include(x => x.Class)
                    .ThenInclude(c => c!.Course)
                .Include(x => x.Status)
                .Include(x => x.Course)
                .FirstOrDefaultAsync();
        }

        public async Task<(IEnumerable<Student> Students, int TotalCount)> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string? searchTerm = null,
            int? statusId = null,
            string? classFilter = null)
        {
            var query = List(expression: x => !x.IsDeleted)
                .Include(x => x.Class)
                    .ThenInclude(c => c!.Course)
                .Include(x => x.Status)
                .Include(x => x.Course)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(s =>
                    s.Name.ToLower().Contains(searchTerm) ||
                    s.StudentId.ToLower().Contains(searchTerm) ||
                    s.FathersName.ToLower().Contains(searchTerm) ||
                    (s.MobileNo1 != null && s.MobileNo1.Contains(searchTerm)) ||
                    (s.EnrollmentNumber != null && s.EnrollmentNumber.ToLower().Contains(searchTerm))
                );
            }

            if (statusId.HasValue && statusId.Value > 0)
            {
                query = query.Where(s => s.StatusId == statusId.Value);
            }

            if (!string.IsNullOrWhiteSpace(classFilter))
            {
                query = query.Where(s => s.Class != null && s.Class.Name == classFilter);
            }

            var totalCount = await query.CountAsync();

            var students = await query
                .OrderByDescending(s => s.CreatedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (students, totalCount);
        }

        public async Task<int> UpdateStudentAsync(Student entity)
        {
            Update(entity);
            return await _unitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public async Task<int> DeleteStudentAsync(int id)
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

        public async Task<string> GenerateStudentIdAsync()
        {
            var currentYear = DateTime.Now.Year;
            var yearPrefix = currentYear.ToString().Substring(2, 2); // Last 2 digits of year

            var lastStudent = await _context.Students
                .Where(s => s.StudentId.StartsWith($"STU{yearPrefix}"))
                .OrderByDescending(s => s.StudentId)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastStudent != null)
            {
                var lastNumberStr = lastStudent.StudentId.Substring(5); // Skip "STU24"
                if (int.TryParse(lastNumberStr, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            var studentId = $"STU{yearPrefix}{nextNumber:D4}";

            var exists = await _context.Students.AnyAsync(s => s.StudentId == studentId);
            int retryCount = 0;
            while (exists && retryCount < 10) // Max 10 retries
            {
                nextNumber++;
                studentId = $"STU{yearPrefix}{nextNumber:D4}";
                exists = await _context.Students.AnyAsync(s => s.StudentId == studentId);
                retryCount++;
            }

            if (exists)
            {
                return "";
            }

            return studentId;
        }

        public async Task<string> GenerateEnrollmentNumberAsync()
        {
            var currentYear = DateTime.Now.Year;
            var yearPrefix = currentYear.ToString().Substring(2, 2); // Last 2 digits of year

            var lastStudent = await _context.Students
                .Where(s => s.EnrollmentNumber != null && s.EnrollmentNumber.StartsWith($"ENR{yearPrefix}"))
                .OrderByDescending(s => s.EnrollmentNumber)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastStudent != null && lastStudent.EnrollmentNumber != null)
            {
                var lastNumberStr = lastStudent.EnrollmentNumber.Substring(5); // Skip "ENR26"
                if (int.TryParse(lastNumberStr, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            var enrollmentNumber = $"ENR{yearPrefix}{nextNumber:D4}";

            var exists = await _context.Students.AnyAsync(s => s.EnrollmentNumber == enrollmentNumber);
            int retryCount = 0;
            while (exists && retryCount < 10) // Max 10 retries
            {
                nextNumber++;
                enrollmentNumber = $"ENR{yearPrefix}{nextNumber:D4}";
                exists = await _context.Students.AnyAsync(s => s.EnrollmentNumber == enrollmentNumber);
                retryCount++;
            }

            if (exists)
            {
                return "";
            }

            return enrollmentNumber;
        }
    }
}
