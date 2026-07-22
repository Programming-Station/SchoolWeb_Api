using Microsoft.EntityFrameworkCore;
using School.Domain.Transport;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories
{
    // ══════════════════════════════════════════════════════════════════════════
    // VEHICLE REPOSITORY
    // ══════════════════════════════════════════════════════════════════════════
    public class VehicleRepository : Repository<Vehicle>, IVehicleRepository
    {
        private readonly SchoolDbContext _ctx;
        private readonly IUnitOfWork _uow;

        public VehicleRepository(DbFactory f, SchoolDbContext ctx, IUnitOfWork uow) : base(f)
        {
            _ctx = ctx;
            _uow = uow;
        }

        public new async Task<int> AddAsync(Vehicle entity)
        {
            await base.AddAsync(entity);
            return await _uow.CommitAsync();
        }

        public new async Task<int> UpdateAsync(Vehicle entity)
        {
            _ctx.Entry(entity).State = EntityState.Modified;
            return await _uow.CommitAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var e = await DbSet.FindAsync(id);
            if (e == null) return 0;
            e.IsDeleted = true;
            _ctx.Entry(e).State = EntityState.Modified;
            return await _uow.CommitAsync();
        }

        public async Task<Vehicle?> GetByIdAsync(int id) =>
            await DbSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        public async Task<IEnumerable<Vehicle>> GetAllBySchoolAsync(int schoolId) =>
            await DbSet.Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted).ToListAsync();
    }

    // ══════════════════════════════════════════════════════════════════════════
    // ROUTE REPOSITORY
    // ══════════════════════════════════════════════════════════════════════════
    public class TransportRouteRepository : Repository<TransportRoute>, ITransportRouteRepository
    {
        private readonly SchoolDbContext _ctx;
        private readonly IUnitOfWork _uow;

        public TransportRouteRepository(DbFactory f, SchoolDbContext ctx, IUnitOfWork uow) : base(f)
        {
            _ctx = ctx;
            _uow = uow;
        }

        public new async Task<int> AddAsync(TransportRoute entity)
        {
            await base.AddAsync(entity);
            return await _uow.CommitAsync();
        }

        public new async Task<int> UpdateAsync(TransportRoute entity)
        {
            _ctx.Entry(entity).State = EntityState.Modified;
            return await _uow.CommitAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var e = await DbSet.FindAsync(id);
            if (e == null) return 0;
            e.IsDeleted = true;
            _ctx.Entry(e).State = EntityState.Modified;
            return await _uow.CommitAsync();
        }

        public async Task<TransportRoute?> GetByIdAsync(int id) =>
            await DbSet.Include(x => x.Vehicle).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        public async Task<IEnumerable<TransportRoute>> GetAllBySchoolAsync(int schoolId) =>
            await DbSet.Include(x => x.Vehicle).Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted).ToListAsync();
    }

    // ══════════════════════════════════════════════════════════════════════════
    // ALLOCATION REPOSITORY
    // ══════════════════════════════════════════════════════════════════════════
    public class TransportAllocationRepository : Repository<TransportAllocation>, ITransportAllocationRepository
    {
        private readonly SchoolDbContext _ctx;
        private readonly IUnitOfWork _uow;

        public TransportAllocationRepository(DbFactory f, SchoolDbContext ctx, IUnitOfWork uow) : base(f)
        {
            _ctx = ctx;
            _uow = uow;
        }

        public new async Task<int> AddAsync(TransportAllocation entity)
        {
            await base.AddAsync(entity);
            return await _uow.CommitAsync();
        }

        public new async Task<int> UpdateAsync(TransportAllocation entity)
        {
            _ctx.Entry(entity).State = EntityState.Modified;
            return await _uow.CommitAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var e = await DbSet.FindAsync(id);
            if (e == null) return 0;
            e.IsDeleted = true;
            _ctx.Entry(e).State = EntityState.Modified;
            return await _uow.CommitAsync();
        }

        public async Task<TransportAllocation?> GetByIdAsync(int id) =>
            await DbSet.Include(x => x.Student)
                       .Include(x => x.TransportRoute)
                         .ThenInclude(r => r.Vehicle)
                       .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        public async Task<IEnumerable<TransportAllocation>> GetByStudentAsync(int studentId, int schoolId) =>
            await DbSet.Include(x => x.TransportRoute)
                         .ThenInclude(r => r.Vehicle)
                       .Where(x => x.StudentId == studentId && x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                       .ToListAsync();

        public async Task<IEnumerable<TransportAllocation>> GetAllBySchoolAsync(int schoolId) =>
            await DbSet.Include(x => x.Student)
                       .Include(x => x.TransportRoute)
                         .ThenInclude(r => r.Vehicle)
                       .Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                       .ToListAsync();
    }
}
