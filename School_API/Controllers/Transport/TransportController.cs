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

        [HttpPost("Allocations")]
        public async Task<IActionResult> AllocateStudent([FromBody] CreateTransportAllocationDto dto)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            try
            {
                var alloc = await _transportService.AllocateStudentAsync(dto, schoolId, UserId);
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
        }

        [HttpPut("Allocations/{id}/status")]
        public async Task<IActionResult> UpdateAllocationStatus(int id, [FromBody] string status)
        {
            var schoolId = _tenant.GetTenantId() ?? 0;
            var success = await _transportService.UpdateAllocationStatusAsync(id, status, schoolId, UserId);
            if (!success) return BadRequest(new { message = "Failed to update allocation status." });
            return Ok(new { message = "Allocation status updated successfully." });
        }
    }
}
