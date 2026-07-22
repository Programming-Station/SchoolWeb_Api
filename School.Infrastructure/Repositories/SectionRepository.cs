using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories
{
    public class SectionRepository : Repository<Section>, ISectionRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public SectionRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<Section> AddSectionAsync(Section entity)
        {
            await base.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<int> DeleteSectionAsync(int id)
        {
            var result = await FindAsync(x => x.Id == id && !x.IsDeleted);
            if (result != null)
            {
                result.UpdatedDate = DateTime.Now;
                Delete(result);
                return await _unitOfWork.CommitAsync();
            }
            return 0;
        }

        public async Task<IEnumerable<Section>> GetAllSectionsAsync()
        {
            return await _context.Sections
                .Where(x => !x.IsDeleted)
                .Include(x => x.Class)
                .OrderBy(x => x.Class.Name)
                .ThenBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Section> GetSectionByIdAsync(int id)
        {
            return await _context.Sections
                .Include(x => x.Class)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted) ?? new Section();
        }

        public async Task<IEnumerable<Section>> GetSectionsByClassIdAsync(int classId)
        {
            return await _context.Sections
                .Where(x => !x.IsDeleted && x.ClassId == classId)
                .Include(x => x.Class)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<int> ToggleSectionStatusAsync(int id)
        {
            var entity = await _context.Sections.FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
            if (entity != null)
            {
                entity.Status = entity.Status.ToLower() == "active" ? "inactive" : "active";
                Update(entity);
                return await _unitOfWork.CommitAsync();
            }
            return 0;
        }

        public async Task<int> UpdateSectionAsync(Section entity)
        {
            Attach(entity, new Expression<Func<Section, object>>[]
            {
                u => u.Name,
                u => u.ClassId,
                u => u.Status,
                u => u.UpdatedBy!
            });
            return await _unitOfWork.CommitAsync();
        }
    }
}
