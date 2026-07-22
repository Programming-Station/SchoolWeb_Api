using School.Domain.Transport;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IVehicleRepository : IRepository<Vehicle>
    {
        Task<Vehicle?> GetByIdAsync(int id);
        Task<IEnumerable<Vehicle>> GetAllBySchoolAsync(int schoolId);
        Task<int> AddAsync(Vehicle entity);
        Task<int> UpdateAsync(Vehicle entity);
        Task<int> DeleteAsync(int id);
    }

    public interface ITransportRouteRepository : IRepository<TransportRoute>
    {
        Task<TransportRoute?> GetByIdAsync(int id);
        Task<IEnumerable<TransportRoute>> GetAllBySchoolAsync(int schoolId);
        Task<int> AddAsync(TransportRoute entity);
        Task<int> UpdateAsync(TransportRoute entity);
        Task<int> DeleteAsync(int id);
    }

    public interface ITransportAllocationRepository : IRepository<TransportAllocation>
    {
        Task<TransportAllocation?> GetByIdAsync(int id);
        Task<IEnumerable<TransportAllocation>> GetByStudentAsync(int studentId, int schoolId);
        Task<IEnumerable<TransportAllocation>> GetAllBySchoolAsync(int schoolId);
        Task<int> AddAsync(TransportAllocation entity);
        Task<int> UpdateAsync(TransportAllocation entity);
        Task<int> DeleteAsync(int id);
    }
}
