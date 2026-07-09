using System.Collections.Generic;
using System.Threading.Tasks;
using School_DTOs.Transport;

namespace School.Services.Interfaces
{
    public interface ITransportService
    {
        // Vehicles
        Task<IEnumerable<VehicleDto>> GetVehiclesAsync(int schoolId);
        Task<VehicleDto?> GetVehicleByIdAsync(int id, int schoolId);
        Task<VehicleDto> CreateVehicleAsync(CreateVehicleDto dto, int schoolId, string userId);
        Task<bool> UpdateVehicleAsync(int id, CreateVehicleDto dto, int schoolId, string userId);
        Task<bool> DeleteVehicleAsync(int id, int schoolId);

        // Routes
        Task<IEnumerable<TransportRouteDto>> GetRoutesAsync(int schoolId);
        Task<TransportRouteDto?> GetRouteByIdAsync(int id, int schoolId);
        Task<TransportRouteDto> CreateRouteAsync(CreateTransportRouteDto dto, int schoolId, string userId);
        Task<bool> UpdateRouteAsync(int id, CreateTransportRouteDto dto, int schoolId, string userId);
        Task<bool> DeleteRouteAsync(int id, int schoolId);

        // Allocations
        Task<IEnumerable<TransportAllocationDto>> GetAllocationsAsync(int schoolId);
        Task<IEnumerable<TransportAllocationDto>> GetStudentAllocationsAsync(int studentId, int schoolId);
        Task<TransportAllocationDto> AllocateStudentAsync(CreateTransportAllocationDto dto, int schoolId, string userId);
        Task<bool> UpdateAllocationStatusAsync(int id, string status, int schoolId, string userId);
    }
}
