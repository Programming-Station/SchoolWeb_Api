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
        Task<bool> RestoreVehicleAsync(int id, int schoolId);

        // Routes
        Task<IEnumerable<TransportRouteDto>> GetRoutesAsync(int schoolId);
        Task<TransportRouteDto?> GetRouteByIdAsync(int id, int schoolId);
        Task<TransportRouteDto> CreateRouteAsync(CreateTransportRouteDto dto, int schoolId, string userId);
        Task<bool> UpdateRouteAsync(int id, CreateTransportRouteDto dto, int schoolId, string userId);
        Task<bool> DeleteRouteAsync(int id, int schoolId);

        // Stops
        Task<IEnumerable<TransportStopDto>> GetStopsAsync(int schoolId);
        Task<TransportStopDto?> GetStopByIdAsync(int id, int schoolId);
        Task<TransportStopDto> CreateStopAsync(CreateTransportStopDto dto, int schoolId, string userId);
        Task<bool> UpdateStopAsync(int id, CreateTransportStopDto dto, int schoolId, string userId);
        Task<bool> DeleteStopAsync(int id, int schoolId);

        // Route Stop Mapping
        Task<IEnumerable<RouteStopMappingDto>> GetRouteStopsAsync(int routeId, int schoolId);
        Task<bool> AssignStopToRouteAsync(CreateRouteStopMappingDto dto, int schoolId, string userId);
        Task<bool> RemoveStopFromRouteAsync(int id, int schoolId);

        // Conductors
        Task<IEnumerable<ConductorDto>> GetConductorsAsync(int schoolId);
        Task<ConductorDto?> GetConductorByIdAsync(int id, int schoolId);
        Task<ConductorDto> CreateConductorAsync(CreateConductorDto dto, int schoolId, string userId);
        Task<bool> UpdateConductorAsync(int id, CreateConductorDto dto, int schoolId, string userId);
        Task<bool> DeleteConductorAsync(int id, int schoolId);

        // Route Assignments
        Task<IEnumerable<RouteAssignmentDto>> GetRouteAssignmentsAsync(int schoolId);
        Task<RouteAssignmentDto?> GetRouteAssignmentByIdAsync(int id, int schoolId);
        Task<RouteAssignmentDto> CreateRouteAssignmentAsync(CreateRouteAssignmentDto dto, int schoolId, string userId);
        Task<bool> UpdateRouteAssignmentAsync(int id, CreateRouteAssignmentDto dto, int schoolId, string userId);
        Task<bool> DeleteRouteAssignmentAsync(int id, int schoolId);

        // Allocations
        Task<IEnumerable<TransportAllocationDto>> GetAllocationsAsync(int schoolId);
        Task<IEnumerable<TransportAllocationDto>> GetStudentAllocationsAsync(int studentId, int schoolId);
        Task<IEnumerable<TransportAllocationDto>> GetEmployeeAllocationsAsync(int employeeId, int schoolId);
        Task<TransportAllocationDto> AllocateTransportAsync(CreateTransportAllocationDto dto, int schoolId, string userId);
        Task<bool> UpdateAllocationStatusAsync(int id, string status, int schoolId, string userId);
        Task<bool> DeleteAllocationAsync(int id, int schoolId);

        // Trips & Scheduling
        Task<IEnumerable<TransportTripDto>> GetTripsAsync(int schoolId);
        Task<TransportTripDto?> GetTripByIdAsync(int id, int schoolId);
        Task<TransportTripDto> CreateTripAsync(CreateTransportTripDto dto, int schoolId, string userId);
        Task<bool> StartTripAsync(int tripId, int schoolId, string userId);
        Task<bool> EndTripAsync(int tripId, int schoolId, string userId);
        Task<bool> CancelTripAsync(int tripId, string reason, int schoolId, string userId);

        // RFID / Scan Logs
        Task<IEnumerable<RfidScanLogDto>> GetScanLogsAsync(int schoolId, int? tripId);
        Task<bool> RecordScanAsync(RfidScanLogDto dto, int schoolId, string userId);

        // Fuel Logs
        Task<IEnumerable<FuelLogDto>> GetFuelLogsAsync(int schoolId);
        Task<FuelLogDto> CreateFuelLogAsync(CreateFuelLogDto dto, int schoolId, string userId);

        // Vehicle Maintenance
        Task<IEnumerable<VehicleMaintenanceDto>> GetMaintenancesAsync(int schoolId);
        Task<VehicleMaintenanceDto> CreateMaintenanceAsync(CreateVehicleMaintenanceDto dto, int schoolId, string userId);

        // Incidents
        Task<IEnumerable<VehicleIncidentDto>> GetIncidentsAsync(int schoolId);
        Task<VehicleIncidentDto> RecordIncidentAsync(CreateVehicleIncidentDto dto, int schoolId, string userId);

        // Inventory
        Task<IEnumerable<TransportInventoryDto>> GetInventoryAsync(int schoolId);
        Task<TransportInventoryDto> CreateInventoryItemAsync(CreateTransportInventoryDto dto, int schoolId, string userId);

        // Gate Logs
        Task<IEnumerable<TransportGateLogDto>> GetGateLogsAsync(int schoolId);
        Task<TransportGateLogDto> RecordGateEntryAsync(CreateTransportGateLogDto dto, int schoolId, string userId);
        Task<bool> RecordGateExitAsync(int id, DateTime exitTime, int schoolId, string userId);

        // Dashboard Metrics
        Task<TransportDashboardDto> GetTransportDashboardAsync(int schoolId);
    }
}
