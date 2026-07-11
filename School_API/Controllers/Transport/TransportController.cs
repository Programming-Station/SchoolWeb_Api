using Microsoft.AspNetCore.Mvc;
using School.Services.Interfaces;
using School_API.Common.Interface;
using School.Infrastructure.Interfaces;
using School_DTOs.Transport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace School_API.Controllers.Transport
{
    public class TransportController : BaseController
    {
        private readonly ITransportService _transportService;
        private readonly ITenantService _tenant;

        public TransportController(
            ICurrentUserService currentUser,
            ITransportService transportService,
            ITenantService tenant)
            : base(currentUser)
        {
            _transportService = transportService;
            _tenant = tenant;
        }

        // ══════════════════════════════════════════════════════════════════════════
        // VEHICLES ENDPOINTS
        // ══════════════════════════════════════════════════════════════════════════

        [HttpGet("Vehicles")]
        public async Task<IActionResult> GetVehicles()
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var list = await _transportService.GetVehiclesAsync(schoolId);
            return Ok(list);
        }

        [HttpGet("Vehicles/{id}")]
        public async Task<IActionResult> GetVehicle(int id)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var vehicle = await _transportService.GetVehicleByIdAsync(id, schoolId);
            if (vehicle == null) return NotFound(new { message = "Vehicle not found." });
            return Ok(vehicle);
        }

        [HttpPost("Vehicles")]
        public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleDto dto)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var vehicle = await _transportService.CreateVehicleAsync(dto, schoolId, UserId);
            return Ok(vehicle);
        }

        [HttpPut("Vehicles/{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] CreateVehicleDto dto)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var success = await _transportService.UpdateVehicleAsync(id, dto, schoolId, UserId);
            if (!success) return BadRequest(new { message = "Failed to update vehicle." });
            return Ok(new { message = "Vehicle updated successfully." });
        }

        [HttpDelete("Vehicles/{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var success = await _transportService.DeleteVehicleAsync(id, schoolId);
            if (!success) return BadRequest(new { message = "Failed to delete vehicle." });
            return Ok(new { message = "Vehicle deleted successfully." });
        }

        [HttpPost("Vehicles/{id}/restore")]
        public async Task<IActionResult> RestoreVehicle(int id)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var success = await _transportService.RestoreVehicleAsync(id, schoolId);
            if (!success) return BadRequest(new { message = "Failed to restore vehicle." });
            return Ok(new { message = "Vehicle restored successfully." });
        }

        // ══════════════════════════════════════════════════════════════════════════
        // ROUTES ENDPOINTS
        // ══════════════════════════════════════════════════════════════════════════

        [HttpGet("Routes")]
        public async Task<IActionResult> GetRoutes()
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var list = await _transportService.GetRoutesAsync(schoolId);
            return Ok(list);
        }

        [HttpGet("Routes/{id}")]
        public async Task<IActionResult> GetRoute(int id)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var route = await _transportService.GetRouteByIdAsync(id, schoolId);
            if (route == null) return NotFound(new { message = "Route not found." });
            return Ok(route);
        }

        [HttpPost("Routes")]
        public async Task<IActionResult> CreateRoute([FromBody] CreateTransportRouteDto dto)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var route = await _transportService.CreateRouteAsync(dto, schoolId, UserId);
            return Ok(route);
        }

        [HttpPut("Routes/{id}")]
        public async Task<IActionResult> UpdateRoute(int id, [FromBody] CreateTransportRouteDto dto)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var success = await _transportService.UpdateRouteAsync(id, dto, schoolId, UserId);
            if (!success) return BadRequest(new { message = "Failed to update route." });
            return Ok(new { message = "Route updated successfully." });
        }

        [HttpDelete("Routes/{id}")]
        public async Task<IActionResult> DeleteRoute(int id)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var success = await _transportService.DeleteRouteAsync(id, schoolId);
            if (!success) return BadRequest(new { message = "Failed to delete route." });
            return Ok(new { message = "Route deleted successfully." });
        }

        // ══════════════════════════════════════════════════════════════════════════
        // STOPS ENDPOINTS
        // ══════════════════════════════════════════════════════════════════════════

        [HttpGet("Stops")]
        public async Task<IActionResult> GetStops()
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var list = await _transportService.GetStopsAsync(schoolId);
            return Ok(list);
        }

        [HttpGet("Stops/{id}")]
        public async Task<IActionResult> GetStop(int id)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var stop = await _transportService.GetStopByIdAsync(id, schoolId);
            if (stop == null) return NotFound(new { message = "Stop not found." });
            return Ok(stop);
        }

        [HttpPost("Stops")]
        public async Task<IActionResult> CreateStop([FromBody] CreateTransportStopDto dto)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var stop = await _transportService.CreateStopAsync(dto, schoolId, UserId);
            return Ok(stop);
        }

        [HttpPut("Stops/{id}")]
        public async Task<IActionResult> UpdateStop(int id, [FromBody] CreateTransportStopDto dto)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var success = await _transportService.UpdateStopAsync(id, dto, schoolId, UserId);
            if (!success) return BadRequest(new { message = "Failed to update stop." });
            return Ok(new { message = "Stop updated successfully." });
        }

        [HttpDelete("Stops/{id}")]
        public async Task<IActionResult> DeleteStop(int id)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var success = await _transportService.DeleteStopAsync(id, schoolId);
            if (!success) return BadRequest(new { message = "Failed to delete stop." });
            return Ok(new { message = "Stop deleted successfully." });
        }

        // ══════════════════════════════════════════════════════════════════════════
        // ROUTE STOPS MAPPING ENDPOINTS
        // ══════════════════════════════════════════════════════════════════════════

        [HttpGet("Routes/{routeId}/Stops")]
        public async Task<IActionResult> GetRouteStops(int routeId)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var list = await _transportService.GetRouteStopsAsync(routeId, schoolId);
            return Ok(list);
        }

        [HttpPost("Routes/Stops")]
        public async Task<IActionResult> AssignStopToRoute([FromBody] CreateRouteStopMappingDto dto)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var success = await _transportService.AssignStopToRouteAsync(dto, schoolId, UserId);
            if (!success) return BadRequest(new { message = "Failed to assign stop to route." });
            return Ok(new { message = "Stop assigned to route successfully." });
        }

        [HttpDelete("Routes/Stops/{id}")]
        public async Task<IActionResult> RemoveStopFromRoute(int id)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var success = await _transportService.RemoveStopFromRouteAsync(id, schoolId);
            if (!success) return BadRequest(new { message = "Failed to remove stop from route." });
            return Ok(new { message = "Stop removed from route successfully." });
        }

        // ══════════════════════════════════════════════════════════════════════════
        // CONDUCTORS ENDPOINTS
        // ══════════════════════════════════════════════════════════════════════════

        [HttpGet("Conductors")]
        public async Task<IActionResult> GetConductors()
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var list = await _transportService.GetConductorsAsync(schoolId);
            return Ok(list);
        }

        [HttpGet("Conductors/{id}")]
        public async Task<IActionResult> GetConductor(int id)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var conductor = await _transportService.GetConductorByIdAsync(id, schoolId);
            if (conductor == null) return NotFound(new { message = "Conductor not found." });
            return Ok(conductor);
        }

        [HttpPost("Conductors")]
        public async Task<IActionResult> CreateConductor([FromBody] CreateConductorDto dto)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var conductor = await _transportService.CreateConductorAsync(dto, schoolId, UserId);
            return Ok(conductor);
        }

        [HttpPut("Conductors/{id}")]
        public async Task<IActionResult> UpdateConductor(int id, [FromBody] CreateConductorDto dto)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var success = await _transportService.UpdateConductorAsync(id, dto, schoolId, UserId);
            if (!success) return BadRequest(new { message = "Failed to update conductor." });
            return Ok(new { message = "Conductor updated successfully." });
        }

        [HttpDelete("Conductors/{id}")]
        public async Task<IActionResult> DeleteConductor(int id)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var success = await _transportService.DeleteConductorAsync(id, schoolId);
            if (!success) return BadRequest(new { message = "Failed to delete conductor." });
            return Ok(new { message = "Conductor deleted successfully." });
        }

        // ══════════════════════════════════════════════════════════════════════════
        // ROUTE ASSIGNMENTS ENDPOINTS
        // ══════════════════════════════════════════════════════════════════════════

        [HttpGet("Assignments")]
        public async Task<IActionResult> GetRouteAssignments()
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var list = await _transportService.GetRouteAssignmentsAsync(schoolId);
            return Ok(list);
        }

        [HttpGet("Assignments/{id}")]
        public async Task<IActionResult> GetRouteAssignment(int id)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var assignment = await _transportService.GetRouteAssignmentByIdAsync(id, schoolId);
            if (assignment == null) return NotFound(new { message = "Route assignment not found." });
            return Ok(assignment);
        }

        [HttpPost("Assignments")]
        public async Task<IActionResult> CreateRouteAssignment([FromBody] CreateRouteAssignmentDto dto)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var assignment = await _transportService.CreateRouteAssignmentAsync(dto, schoolId, UserId);
            return Ok(assignment);
        }

        [HttpPut("Assignments/{id}")]
        public async Task<IActionResult> UpdateRouteAssignment(int id, [FromBody] CreateRouteAssignmentDto dto)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var success = await _transportService.UpdateRouteAssignmentAsync(id, dto, schoolId, UserId);
            if (!success) return BadRequest(new { message = "Failed to update route assignment." });
            return Ok(new { message = "Route assignment updated successfully." });
        }

        [HttpDelete("Assignments/{id}")]
        public async Task<IActionResult> DeleteRouteAssignment(int id)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var success = await _transportService.DeleteRouteAssignmentAsync(id, schoolId);
            if (!success) return BadRequest(new { message = "Failed to delete route assignment." });
            return Ok(new { message = "Route assignment deleted successfully." });
        }

        // ══════════════════════════════════════════════════════════════════════════
        // ALLOCATIONS ENDPOINTS
        // ══════════════════════════════════════════════════════════════════════════

        [HttpGet("Allocations")]
        public async Task<IActionResult> GetAllocations()
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var list = await _transportService.GetAllocationsAsync(schoolId);
            return Ok(list);
        }

        [HttpGet("Allocations/Student/{studentId}")]
        public async Task<IActionResult> GetStudentAllocations(int studentId)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var list = await _transportService.GetStudentAllocationsAsync(studentId, schoolId);
            return Ok(list);
        }

        [HttpGet("Allocations/Employee/{employeeId}")]
        public async Task<IActionResult> GetEmployeeAllocations(int employeeId)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var list = await _transportService.GetEmployeeAllocationsAsync(employeeId, schoolId);
            return Ok(list);
        }

        [HttpPost("Allocations")]
        public async Task<IActionResult> AllocateTransport([FromBody] CreateTransportAllocationDto dto)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            try
            {
                var alloc = await _transportService.AllocateTransportAsync(dto, schoolId, UserId);
                return Ok(alloc);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("Allocations/{id}/status")]
        public async Task<IActionResult> UpdateAllocationStatus(int id, [FromBody] string status)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var success = await _transportService.UpdateAllocationStatusAsync(id, status, schoolId, UserId);
            if (!success) return BadRequest(new { message = "Failed to update allocation status." });
            return Ok(new { message = "Allocation status updated successfully." });
        }

        [HttpDelete("Allocations/{id}")]
        public async Task<IActionResult> DeleteAllocation(int id)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var success = await _transportService.DeleteAllocationAsync(id, schoolId);
            if (!success) return BadRequest(new { message = "Failed to delete allocation." });
            return Ok(new { message = "Allocation deleted successfully." });
        }

        // ══════════════════════════════════════════════════════════════════════════
        // TRIPS ENDPOINTS
        // ══════════════════════════════════════════════════════════════════════════

        [HttpGet("Trips")]
        public async Task<IActionResult> GetTrips()
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var list = await _transportService.GetTripsAsync(schoolId);
            return Ok(list);
        }

        [HttpGet("Trips/{id}")]
        public async Task<IActionResult> GetTrip(int id)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var trip = await _transportService.GetTripByIdAsync(id, schoolId);
            if (trip == null) return NotFound(new { message = "Trip not found." });
            return Ok(trip);
        }

        [HttpPost("Trips")]
        public async Task<IActionResult> CreateTrip([FromBody] CreateTransportTripDto dto)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var trip = await _transportService.CreateTripAsync(dto, schoolId, UserId);
            return Ok(trip);
        }

        [HttpPost("Trips/{id}/start")]
        public async Task<IActionResult> StartTrip(int id)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var success = await _transportService.StartTripAsync(id, schoolId, UserId);
            if (!success) return BadRequest(new { message = "Failed to start trip." });
            return Ok(new { message = "Trip started successfully." });
        }

        [HttpPost("Trips/{id}/end")]
        public async Task<IActionResult> EndTrip(int id)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var success = await _transportService.EndTripAsync(id, schoolId, UserId);
            if (!success) return BadRequest(new { message = "Failed to end trip." });
            return Ok(new { message = "Trip ended successfully." });
        }

        [HttpPost("Trips/{id}/cancel")]
        public async Task<IActionResult> CancelTrip(int id, [FromBody] string reason)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var success = await _transportService.CancelTripAsync(id, reason, schoolId, UserId);
            if (!success) return BadRequest(new { message = "Failed to cancel trip." });
            return Ok(new { message = "Trip cancelled successfully." });
        }

        // ══════════════════════════════════════════════════════════════════════════
        // RFID SCAN ENDPOINTS
        // ══════════════════════════════════════════════════════════════════════════

        [HttpGet("Rfid/Logs")]
        public async Task<IActionResult> GetScanLogs([FromQuery] int? tripId)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var list = await _transportService.GetScanLogsAsync(schoolId, tripId);
            return Ok(list);
        }

        [HttpPost("Rfid/Scan")]
        public async Task<IActionResult> RecordScan([FromBody] RfidScanLogDto dto)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var success = await _transportService.RecordScanAsync(dto, schoolId, UserId);
            if (!success) return BadRequest(new { message = "Failed to record scan." });
            return Ok(new { message = "Scan recorded successfully." });
        }

        // ══════════════════════════════════════════════════════════════════════════
        // FUEL LOGS ENDPOINTS
        // ══════════════════════════════════════════════════════════════════════════

        [HttpGet("Fuel/Logs")]
        public async Task<IActionResult> GetFuelLogs()
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var list = await _transportService.GetFuelLogsAsync(schoolId);
            return Ok(list);
        }

        [HttpPost("Fuel/Log")]
        public async Task<IActionResult> CreateFuelLog([FromBody] CreateFuelLogDto dto)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var log = await _transportService.CreateFuelLogAsync(dto, schoolId, UserId);
            return Ok(log);
        }

        // ══════════════════════════════════════════════════════════════════════════
        // MAINTENANCE ENDPOINTS
        // ══════════════════════════════════════════════════════════════════════════

        [HttpGet("Maintenance/Logs")]
        public async Task<IActionResult> GetMaintenances()
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var list = await _transportService.GetMaintenancesAsync(schoolId);
            return Ok(list);
        }

        [HttpPost("Maintenance")]
        public async Task<IActionResult> CreateMaintenance([FromBody] CreateVehicleMaintenanceDto dto)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var maint = await _transportService.CreateMaintenanceAsync(dto, schoolId, UserId);
            return Ok(maint);
        }

        // ══════════════════════════════════════════════════════════════════════════
        // INCIDENTS ENDPOINTS
        // ══════════════════════════════════════════════════════════════════════════

        [HttpGet("Incidents")]
        public async Task<IActionResult> GetIncidents()
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var list = await _transportService.GetIncidentsAsync(schoolId);
            return Ok(list);
        }

        [HttpPost("Incidents")]
        public async Task<IActionResult> RecordIncident([FromBody] CreateVehicleIncidentDto dto)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var incident = await _transportService.RecordIncidentAsync(dto, schoolId, UserId);
            return Ok(incident);
        }

        // ══════════════════════════════════════════════════════════════════════════
        // INVENTORY ENDPOINTS
        // ══════════════════════════════════════════════════════════════════════════

        [HttpGet("Inventory")]
        public async Task<IActionResult> GetInventory()
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var list = await _transportService.GetInventoryAsync(schoolId);
            return Ok(list);
        }

        [HttpPost("Inventory")]
        public async Task<IActionResult> CreateInventoryItem([FromBody] CreateTransportInventoryDto dto)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var item = await _transportService.CreateInventoryItemAsync(dto, schoolId, UserId);
            return Ok(item);
        }

        // ══════════════════════════════════════════════════════════════════════════
        // GATE LOGS ENDPOINTS
        // ══════════════════════════════════════════════════════════════════════════

        [HttpGet("GateLogs")]
        public async Task<IActionResult> GetGateLogs()
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var list = await _transportService.GetGateLogsAsync(schoolId);
            return Ok(list);
        }

        [HttpPost("GateLogs/Entry")]
        public async Task<IActionResult> RecordGateEntry([FromBody] CreateTransportGateLogDto dto)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var entry = await _transportService.RecordGateEntryAsync(dto, schoolId, UserId);
            return Ok(entry);
        }

        [HttpPost("GateLogs/{id}/Exit")]
        public async Task<IActionResult> RecordGateExit(int id, [FromBody] DateTime exitTime)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var success = await _transportService.RecordGateExitAsync(id, exitTime, schoolId, UserId);
            if (!success) return BadRequest(new { message = "Failed to record exit time." });
            return Ok(new { message = "Gate exit recorded successfully." });
        }

        // ══════════════════════════════════════════════════════════════════════════
        // DASHBOARD ENDPOINT
        // ══════════════════════════════════════════════════════════════════════════

        [HttpGet("Dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var stats = await _transportService.GetTransportDashboardAsync(schoolId);
            return Ok(stats);
        }
    }
}
