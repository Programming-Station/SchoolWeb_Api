using School_DTOs.Transport;
using School_DTOs.Common;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace School.Services.Interfaces.Transport
{
    public interface IVehicleService
    {
        Task<APIResponse<List<VehicleDto>>> GetAllAsync();
        Task<APIResponse<VehicleDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreateVehicleDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateVehicleDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
    }
    public interface ITransportRouteService
    {
        Task<APIResponse<List<TransportRouteDto>>> GetAllAsync();
        Task<APIResponse<TransportRouteDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreateTransportRouteDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateTransportRouteDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
    }
}
