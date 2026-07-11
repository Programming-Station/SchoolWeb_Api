using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using School.Infrastructure.Interfaces;
using School.Services.Hostel;
using School_API.Common.Interface;
using School_DTOs.Hostel;

namespace School_API.Controllers.Hostel
{
    [Route("api/[controller]")]
    [ApiController]
    public class HostelController : BaseController
    {
        private readonly IHostelService _hostel;
        private readonly ITenantService _tenant;

        public HostelController(ICurrentUserService currentUser, IHostelService hostel, ITenantService tenant)
            : base(currentUser)
        {
            _hostel = hostel;
            _tenant = tenant;
        }

        private int SchoolId => _tenant.GetTenantId() ?? 1;

        // ════════════════════════════════════════════════════════════════════
        // DASHBOARD
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("Dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var data = await _hostel.GetDashboardAsync(SchoolId);
            return Ok(new { success = true, data });
        }

        // ════════════════════════════════════════════════════════════════════
        // HOSTELS CRUD
        // ════════════════════════════════════════════════════════════════════
        [HttpGet]
        public async Task<IActionResult> GetHostels([FromQuery] string search = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string status = null)
        {
            var result = await _hostel.GetHostelsAsync(SchoolId, search, page, pageSize, status);
            return Ok(new { success = true, data = result });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHostel(int id)
        {
            var item = await _hostel.GetHostelByIdAsync(id, SchoolId);
            return item == null ? NotFound(new { success = false, message = "Hostel not found" }) : Ok(new { success = true, data = item });
        }

        [HttpPost]
        public async Task<IActionResult> CreateHostel([FromBody] CreateHostelDto dto)
        {
            var item = await _hostel.CreateHostelAsync(dto, SchoolId, UserId);
            return Ok(new { success = true, data = item });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHostel(int id, [FromBody] CreateHostelDto dto)
        {
            var ok = await _hostel.UpdateHostelAsync(id, dto, SchoolId, UserId);
            return ok ? Ok(new { success = true, message = "Hostel updated successfully" }) : BadRequest(new { success = false, message = "Could not update hostel" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHostel(int id)
        {
            var ok = await _hostel.DeleteHostelAsync(id, SchoolId);
            return ok ? Ok(new { success = true, message = "Hostel deleted successfully" }) : BadRequest(new { success = false, message = "Could not delete hostel" });
        }

        [HttpPost("{id}/restore")]
        public async Task<IActionResult> RestoreHostel(int id)
        {
            var ok = await _hostel.RestoreHostelAsync(id, SchoolId);
            return ok ? Ok(new { success = true, message = "Hostel restored successfully" }) : BadRequest(new { success = false, message = "Could not restore hostel" });
        }

        // ════════════════════════════════════════════════════════════════════
        // BUILDINGS CRUD
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("Buildings")]
        public async Task<IActionResult> GetBuildings([FromQuery] string search = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string status = null, [FromQuery] int? hostelId = null)
        {
            var result = await _hostel.GetBuildingsAsync(SchoolId, search, page, pageSize, status, hostelId);
            return Ok(new { success = true, data = result });
        }

        [HttpGet("Buildings/Dropdown")]
        public async Task<IActionResult> GetBuildingsDropdown([FromQuery] int? hostelId = null)
        {
            var data = await _hostel.GetAllBuildingsDropdownAsync(SchoolId, hostelId);
            return Ok(new { success = true, data });
        }

        [HttpGet("Buildings/{id}")]
        public async Task<IActionResult> GetBuilding(int id)
        {
            var item = await _hostel.GetBuildingByIdAsync(id, SchoolId);
            return item == null ? NotFound(new { success = false, message = "Building not found" }) : Ok(new { success = true, data = item });
        }

        [HttpPost("Buildings")]
        public async Task<IActionResult> CreateBuilding([FromBody] CreateBuildingDto dto)
        {
            var item = await _hostel.CreateBuildingAsync(dto, SchoolId, UserId);
            return Ok(new { success = true, data = item });
        }

        [HttpPut("Buildings/{id}")]
        public async Task<IActionResult> UpdateBuilding(int id, [FromBody] CreateBuildingDto dto)
        {
            var ok = await _hostel.UpdateBuildingAsync(id, dto, SchoolId, UserId);
            return ok ? Ok(new { success = true, message = "Building updated" }) : BadRequest(new { success = false });
        }

        [HttpDelete("Buildings/{id}")]
        public async Task<IActionResult> DeleteBuilding(int id)
        {
            var ok = await _hostel.DeleteBuildingAsync(id, SchoolId);
            return ok ? Ok(new { success = true, message = "Building deleted" }) : BadRequest(new { success = false });
        }

        [HttpPost("Buildings/{id}/restore")]
        public async Task<IActionResult> RestoreBuilding(int id)
        {
            var ok = await _hostel.RestoreBuildingAsync(id, SchoolId);
            return ok ? Ok(new { success = true, message = "Building restored" }) : BadRequest(new { success = false });
        }

        // ════════════════════════════════════════════════════════════════════
        // FLOORS CRUD
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("Floors")]
        public async Task<IActionResult> GetFloors([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] int? buildingId = null)
        {
            var result = await _hostel.GetFloorsAsync(SchoolId, page, pageSize, buildingId);
            return Ok(new { success = true, data = result });
        }

        [HttpGet("Floors/Dropdown/{buildingId}")]
        public async Task<IActionResult> GetFloorsDropdown(int buildingId)
        {
            var data = await _hostel.GetFloorsDropdownAsync(SchoolId, buildingId);
            return Ok(new { success = true, data });
        }

        [HttpGet("Floors/{id}")]
        public async Task<IActionResult> GetFloor(int id)
        {
            var item = await _hostel.GetFloorByIdAsync(id, SchoolId);
            return item == null ? NotFound(new { success = false }) : Ok(new { success = true, data = item });
        }

        [HttpPost("Floors")]
        public async Task<IActionResult> CreateFloor([FromBody] CreateFloorDto dto)
        {
            var item = await _hostel.CreateFloorAsync(dto, SchoolId, UserId);
            return Ok(new { success = true, data = item });
        }

        [HttpPut("Floors/{id}")]
        public async Task<IActionResult> UpdateFloor(int id, [FromBody] CreateFloorDto dto)
        {
            var ok = await _hostel.UpdateFloorAsync(id, dto, SchoolId, UserId);
            return ok ? Ok(new { success = true }) : BadRequest(new { success = false });
        }

        [HttpDelete("Floors/{id}")]
        public async Task<IActionResult> DeleteFloor(int id)
        {
            var ok = await _hostel.DeleteFloorAsync(id, SchoolId);
            return ok ? Ok(new { success = true }) : BadRequest(new { success = false });
        }

        // ════════════════════════════════════════════════════════════════════
        // ROOM CATEGORIES CRUD
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("RoomCategories")]
        public async Task<IActionResult> GetRoomCategories()
        {
            var data = await _hostel.GetRoomCategoriesAsync(SchoolId);
            return Ok(new { success = true, data });
        }

        [HttpGet("RoomCategories/{id}")]
        public async Task<IActionResult> GetRoomCategory(int id)
        {
            var item = await _hostel.GetRoomCategoryByIdAsync(id, SchoolId);
            return item == null ? NotFound(new { success = false }) : Ok(new { success = true, data = item });
        }

        [HttpPost("RoomCategories")]
        public async Task<IActionResult> CreateRoomCategory([FromBody] CreateRoomCategoryDto dto)
        {
            var item = await _hostel.CreateRoomCategoryAsync(dto, SchoolId, UserId);
            return Ok(new { success = true, data = item });
        }

        [HttpPut("RoomCategories/{id}")]
        public async Task<IActionResult> UpdateRoomCategory(int id, [FromBody] CreateRoomCategoryDto dto)
        {
            var ok = await _hostel.UpdateRoomCategoryAsync(id, dto, SchoolId, UserId);
            return ok ? Ok(new { success = true }) : BadRequest(new { success = false });
        }

        [HttpDelete("RoomCategories/{id}")]
        public async Task<IActionResult> DeleteRoomCategory(int id)
        {
            var ok = await _hostel.DeleteRoomCategoryAsync(id, SchoolId);
            return ok ? Ok(new { success = true }) : BadRequest(new { success = false });
        }

        // ════════════════════════════════════════════════════════════════════
        // ROOMS CRUD
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("Rooms")]
        public async Task<IActionResult> GetRooms([FromQuery] string search = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string status = null, [FromQuery] int? hostelId = null, [FromQuery] int? buildingId = null, [FromQuery] int? floorId = null, [FromQuery] int? categoryId = null)
        {
            var result = await _hostel.GetRoomsAsync(SchoolId, search, page, pageSize, status, hostelId, buildingId, floorId, categoryId);
            return Ok(new { success = true, data = result });
        }

        [HttpGet("Rooms/Dropdown/{floorId}")]
        public async Task<IActionResult> GetRoomsDropdown(int floorId)
        {
            var data = await _hostel.GetRoomsDropdownAsync(SchoolId, floorId);
            return Ok(new { success = true, data });
        }

        [HttpGet("Rooms/{id}")]
        public async Task<IActionResult> GetRoom(int id)
        {
            var item = await _hostel.GetRoomByIdAsync(id, SchoolId);
            return item == null ? NotFound(new { success = false }) : Ok(new { success = true, data = item });
        }

        [HttpPost("Rooms")]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDto dto)
        {
            var item = await _hostel.CreateRoomAsync(dto, SchoolId, UserId);
            return Ok(new { success = true, data = item });
        }

        [HttpPut("Rooms/{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] CreateRoomDto dto)
        {
            var ok = await _hostel.UpdateRoomAsync(id, dto, SchoolId, UserId);
            return ok ? Ok(new { success = true }) : BadRequest(new { success = false });
        }

        [HttpDelete("Rooms/{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var ok = await _hostel.DeleteRoomAsync(id, SchoolId);
            return ok ? Ok(new { success = true }) : BadRequest(new { success = false });
        }

        [HttpPost("Rooms/{id}/restore")]
        public async Task<IActionResult> RestoreRoom(int id)
        {
            var ok = await _hostel.RestoreRoomAsync(id, SchoolId);
            return ok ? Ok(new { success = true }) : BadRequest(new { success = false });
        }

        // ════════════════════════════════════════════════════════════════════
        // BEDS CRUD
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("Beds")]
        public async Task<IActionResult> GetBeds([FromQuery] string search = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string status = null, [FromQuery] int? roomId = null)
        {
            var result = await _hostel.GetBedsAsync(SchoolId, search, page, pageSize, status, roomId);
            return Ok(new { success = true, data = result });
        }

        [HttpGet("Beds/Dropdown/{roomId}")]
        public async Task<IActionResult> GetBedsDropdown(int roomId, [FromQuery] string status = null)
        {
            var data = await _hostel.GetBedsDropdownAsync(SchoolId, roomId, status);
            return Ok(new { success = true, data });
        }

        [HttpGet("Beds/{id}")]
        public async Task<IActionResult> GetBed(int id)
        {
            var item = await _hostel.GetBedByIdAsync(id, SchoolId);
            return item == null ? NotFound(new { success = false }) : Ok(new { success = true, data = item });
        }

        [HttpPost("Beds")]
        public async Task<IActionResult> CreateBed([FromBody] CreateBedDto dto)
        {
            var item = await _hostel.CreateBedAsync(dto, SchoolId, UserId);
            return Ok(new { success = true, data = item });
        }

        [HttpPut("Beds/{id}")]
        public async Task<IActionResult> UpdateBed(int id, [FromBody] CreateBedDto dto)
        {
            var ok = await _hostel.UpdateBedAsync(id, dto, SchoolId, UserId);
            return ok ? Ok(new { success = true }) : BadRequest(new { success = false });
        }

        [HttpDelete("Beds/{id}")]
        public async Task<IActionResult> DeleteBed(int id)
        {
            var ok = await _hostel.DeleteBedAsync(id, SchoolId);
            return ok ? Ok(new { success = true }) : BadRequest(new { success = false });
        }

        [HttpPost("Beds/{id}/restore")]
        public async Task<IActionResult> RestoreBed(int id)
        {
            var ok = await _hostel.RestoreBedAsync(id, SchoolId);
            return ok ? Ok(new { success = true }) : BadRequest(new { success = false });
        }

        // ════════════════════════════════════════════════════════════════════
        // WARDENS CRUD
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("Wardens")]
        public async Task<IActionResult> GetWardens([FromQuery] string search = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string status = null, [FromQuery] int? hostelId = null)
        {
            var result = await _hostel.GetWardensAsync(SchoolId, search, page, pageSize, status, hostelId);
            return Ok(new { success = true, data = result });
        }

        [HttpGet("Wardens/{id}")]
        public async Task<IActionResult> GetWarden(int id)
        {
            var item = await _hostel.GetWardenByIdAsync(id, SchoolId);
            return item == null ? NotFound(new { success = false }) : Ok(new { success = true, data = item });
        }

        [HttpPost("Wardens")]
        public async Task<IActionResult> CreateWarden([FromBody] CreateHostelWardenDto dto)
        {
            var item = await _hostel.CreateWardenAsync(dto, SchoolId, UserId);
            return Ok(new { success = true, data = item });
        }

        [HttpPut("Wardens/{id}")]
        public async Task<IActionResult> UpdateWarden(int id, [FromBody] CreateHostelWardenDto dto)
        {
            var ok = await _hostel.UpdateWardenAsync(id, dto, SchoolId, UserId);
            return ok ? Ok(new { success = true }) : BadRequest(new { success = false });
        }

        [HttpDelete("Wardens/{id}")]
        public async Task<IActionResult> DeleteWarden(int id)
        {
            var ok = await _hostel.DeleteWardenAsync(id, SchoolId);
            return ok ? Ok(new { success = true }) : BadRequest(new { success = false });
        }

        // ════════════════════════════════════════════════════════════════════
        // ADMISSIONS / ALLOCATIONS
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("Admissions")]
        public async Task<IActionResult> GetAdmissions([FromQuery] string search = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string status = null, [FromQuery] int? hostelId = null, [FromQuery] int? roomId = null)
        {
            var result = await _hostel.GetAdmissionsAsync(SchoolId, search, page, pageSize, status, hostelId, roomId);
            return Ok(new { success = true, data = result });
        }

        [HttpGet("Admissions/{id}")]
        public async Task<IActionResult> GetAdmission(int id)
        {
            var item = await _hostel.GetAdmissionByIdAsync(id, SchoolId);
            return item == null ? NotFound(new { success = false }) : Ok(new { success = true, data = item });
        }

        [HttpPost("Admissions")]
        public async Task<IActionResult> CreateAdmission([FromBody] CreateHostelAdmissionDto dto)
        {
            var item = await _hostel.CreateAdmissionAsync(dto, SchoolId, UserId);
            return Ok(new { success = true, data = item });
        }

        [HttpPost("Admissions/{id}/checkin")]
        public async Task<IActionResult> CheckInStudent(int id, [FromQuery] string biometricId = null, [FromQuery] string rfidTag = null)
        {
            var ok = await _hostel.CheckInStudentAsync(id, biometricId, rfidTag, SchoolId, UserId);
            return ok ? Ok(new { success = true, message = "Student checked in successfully" }) : BadRequest(new { success = false });
        }

        [HttpPost("Admissions/{id}/checkout")]
        public async Task<IActionResult> CheckOutStudent(int id)
        {
            var ok = await _hostel.CheckOutStudentAsync(id, SchoolId, UserId);
            return ok ? Ok(new { success = true, message = "Student checked out successfully" }) : BadRequest(new { success = false });
        }

        [HttpPost("Admissions/transfer")]
        public async Task<IActionResult> TransferRoom([FromBody] RoomTransferDto dto)
        {
            var ok = await _hostel.TransferRoomAsync(dto, SchoolId, UserId);
            return ok ? Ok(new { success = true, message = "Room transfer completed successfully" }) : BadRequest(new { success = false });
        }

        [HttpPost("Admissions/exchange")]
        public async Task<IActionResult> ExchangeRoom([FromBody] RoomExchangeDto dto)
        {
            var ok = await _hostel.ExchangeRoomAsync(dto, SchoolId, UserId);
            return ok ? Ok(new { success = true, message = "Room swap completed successfully" }) : BadRequest(new { success = false });
        }

        // ════════════════════════════════════════════════════════════════════
        // BED RESERVATIONS
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("Reservations")]
        public async Task<IActionResult> GetReservations()
        {
            var data = await _hostel.GetReservationsAsync(SchoolId);
            return Ok(new { success = true, data });
        }

        [HttpPost("Reservations")]
        public async Task<IActionResult> ReserveBed([FromBody] CreateBedReservationDto dto)
        {
            var item = await _hostel.ReserveBedAsync(dto, SchoolId, UserId);
            return Ok(new { success = true, data = item });
        }

        [HttpPost("Reservations/{id}/cancel")]
        public async Task<IActionResult> CancelReservation(int id)
        {
            var ok = await _hostel.CancelReservationAsync(id, SchoolId, UserId);
            return ok ? Ok(new { success = true, message = "Reservation cancelled" }) : BadRequest(new { success = false });
        }

        // ════════════════════════════════════════════════════════════════════
        // MESS MENU
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("MessMenu/{hostelId}")]
        public async Task<IActionResult> GetMessMenu(int hostelId)
        {
            var data = await _hostel.GetMessMenusAsync(SchoolId, hostelId);
            return Ok(new { success = true, data });
        }

        [HttpPost("MessMenu")]
        public async Task<IActionResult> CreateMessMenu([FromBody] CreateMessMenuDto dto)
        {
            var item = await _hostel.CreateMessMenuAsync(dto, SchoolId, UserId);
            return Ok(new { success = true, data = item });
        }

        [HttpPut("MessMenu/{id}")]
        public async Task<IActionResult> UpdateMessMenu(int id, [FromBody] CreateMessMenuDto dto)
        {
            var ok = await _hostel.UpdateMessMenuAsync(id, dto, SchoolId, UserId);
            return ok ? Ok(new { success = true }) : BadRequest(new { success = false });
        }

        [HttpDelete("MessMenu/{id}")]
        public async Task<IActionResult> DeleteMessMenu(int id)
        {
            var ok = await _hostel.DeleteMessMenuAsync(id, SchoolId);
            return ok ? Ok(new { success = true }) : BadRequest(new { success = false });
        }

        // ════════════════════════════════════════════════════════════════════
        // MEAL ATTENDANCE
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("MealAttendance")]
        public async Task<IActionResult> GetMealAttendance([FromQuery] string mealType = null, [FromQuery] DateTime? date = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _hostel.GetMealAttendanceAsync(SchoolId, page, pageSize, mealType, date);
            return Ok(new { success = true, data = result });
        }

        [HttpPost("MealAttendance")]
        public async Task<IActionResult> MarkMealAttendance([FromBody] CreateMealAttendanceDto dto)
        {
            var item = await _hostel.MarkMealAttendanceAsync(dto, SchoolId, UserId);
            return Ok(new { success = true, data = item });
        }

        // ════════════════════════════════════════════════════════════════════
        // VISITORS
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("Visitors")]
        public async Task<IActionResult> GetVisitors([FromQuery] string search = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string status = null)
        {
            var result = await _hostel.GetVisitorsAsync(SchoolId, search, page, pageSize, status);
            return Ok(new { success = true, data = result });
        }

        [HttpPost("Visitors")]
        public async Task<IActionResult> RegisterVisitor([FromBody] CreateHostelVisitorDto dto)
        {
            var item = await _hostel.RegisterVisitorAsync(dto, SchoolId, UserId);
            return Ok(new { success = true, data = item });
        }

        [HttpPost("Visitors/{id}/checkout")]
        public async Task<IActionResult> CheckoutVisitor(int id)
        {
            var ok = await _hostel.CheckoutVisitorAsync(id, SchoolId, UserId);
            return ok ? Ok(new { success = true, message = "Visitor checked out" }) : BadRequest(new { success = false });
        }

        // ════════════════════════════════════════════════════════════════════
        // GATEPASSES
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("GatePasses")]
        public async Task<IActionResult> GetGatePasses([FromQuery] string search = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string status = null)
        {
            var result = await _hostel.GetGatePassesAsync(SchoolId, search, page, pageSize, status);
            return Ok(new { success = true, data = result });
        }

        [HttpPost("GatePasses")]
        public async Task<IActionResult> IssueGatePass([FromBody] CreateHostelGatePassDto dto)
        {
            var item = await _hostel.IssueGatePassAsync(dto, SchoolId, UserId);
            return Ok(new { success = true, data = item });
        }

        [HttpPost("GatePasses/{id}/approve")]
        public async Task<IActionResult> ApproveGatePass(int id, [FromQuery] string approverRole, [FromQuery] string status)
        {
            var ok = await _hostel.ApproveGatePassAsync(id, approverRole, status, SchoolId, UserId);
            return ok ? Ok(new { success = true, message = "Gate pass approval updated" }) : BadRequest(new { success = false });
        }

        [HttpPost("GatePasses/{id}/scan-out")]
        public async Task<IActionResult> ScanGatePassOut(int id)
        {
            var ok = await _hostel.ScanGatePassOutAsync(id, SchoolId, UserId);
            return ok ? Ok(new { success = true, message = "Student gate checkout logged" }) : BadRequest(new { success = false });
        }

        [HttpPost("GatePasses/{id}/scan-in")]
        public async Task<IActionResult> ScanGatePassIn(int id)
        {
            var ok = await _hostel.ScanGatePassInAsync(id, SchoolId, UserId);
            return ok ? Ok(new { success = true, message = "Student gate check-in logged" }) : BadRequest(new { success = false });
        }

        // ════════════════════════════════════════════════════════════════════
        // ATTENDANCE
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("Attendance")]
        public async Task<IActionResult> GetAttendance([FromQuery] string search = null, [FromQuery] string type = null, [FromQuery] DateTime? date = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _hostel.GetAttendanceAsync(SchoolId, search, page, pageSize, type, date);
            return Ok(new { success = true, data = result });
        }

        [HttpPost("Attendance")]
        public async Task<IActionResult> MarkAttendance([FromBody] List<CreateHostelAttendanceDto> list)
        {
            var ok = await _hostel.MarkAttendanceAsync(list, SchoolId, UserId);
            return ok ? Ok(new { success = true, message = "Attendance saved successfully" }) : BadRequest(new { success = false });
        }

        // ════════════════════════════════════════════════════════════════════
        // COMPLAINTS
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("Complaints")]
        public async Task<IActionResult> GetComplaints([FromQuery] string search = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string status = null, [FromQuery] string priority = null)
        {
            var result = await _hostel.GetComplaintsAsync(SchoolId, search, page, pageSize, status, priority);
            return Ok(new { success = true, data = result });
        }

        [HttpPost("Complaints")]
        public async Task<IActionResult> RaiseComplaint([FromBody] CreateHostelComplaintDto dto)
        {
            var item = await _hostel.RaiseComplaintAsync(dto, SchoolId, UserId);
            return Ok(new { success = true, data = item });
        }

        [HttpPost("Complaints/{id}/assign/{staffId}")]
        public async Task<IActionResult> AssignComplaint(int id, int staffId)
        {
            var ok = await _hostel.AssignComplaintAsync(id, staffId, SchoolId, UserId);
            return ok ? Ok(new { success = true, message = "Complaint assigned" }) : BadRequest(new { success = false });
        }

        [HttpPost("Complaints/{id}/resolve")]
        public async Task<IActionResult> ResolveComplaint(int id, [FromBody] string details)
        {
            var ok = await _hostel.ResolveComplaintAsync(id, details, SchoolId, UserId);
            return ok ? Ok(new { success = true, message = "Complaint resolved" }) : BadRequest(new { success = false });
        }

        // ════════════════════════════════════════════════════════════════════
        // MAINTENANCE
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("Maintenance")]
        public async Task<IActionResult> GetMaintenanceRequests([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string status = null)
        {
            var result = await _hostel.GetMaintenanceRequestsAsync(SchoolId, page, pageSize, status);
            return Ok(new { success = true, data = result });
        }

        [HttpPost("Maintenance")]
        public async Task<IActionResult> CreateMaintenanceRequest([FromBody] CreateHostelMaintenanceDto dto)
        {
            var item = await _hostel.CreateMaintenanceRequestAsync(dto, SchoolId, UserId);
            return Ok(new { success = true, data = item });
        }

        [HttpPost("Maintenance/{id}/complete")]
        public async Task<IActionResult> CompleteMaintenanceRequest(int id, [FromQuery] decimal cost, [FromBody] string details)
        {
            var ok = await _hostel.CompleteMaintenanceRequestAsync(id, cost, details, SchoolId, UserId);
            return ok ? Ok(new { success = true, message = "Maintenance completed" }) : BadRequest(new { success = false });
        }

        // ════════════════════════════════════════════════════════════════════
        // LAUNDRY
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("Laundry")]
        public async Task<IActionResult> GetLaundryTransactions([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string status = null)
        {
            var result = await _hostel.GetLaundryTransactionsAsync(SchoolId, page, pageSize, status);
            return Ok(new { success = true, data = result });
        }

        [HttpPost("Laundry")]
        public async Task<IActionResult> CreateLaundryTransaction([FromBody] CreateLaundryTransactionDto dto)
        {
            var item = await _hostel.CreateLaundryTransactionAsync(dto, SchoolId, UserId);
            return Ok(new { success = true, data = item });
        }

        [HttpPost("Laundry/{id}/status")]
        public async Task<IActionResult> UpdateLaundryStatus(int id, [FromQuery] string status)
        {
            var ok = await _hostel.UpdateLaundryStatusAsync(id, status, SchoolId, UserId);
            return ok ? Ok(new { success = true, message = "Laundry status updated" }) : BadRequest(new { success = false });
        }

        // ════════════════════════════════════════════════════════════════════
        // INVENTORY
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("Inventory")]
        public async Task<IActionResult> GetInventory([FromQuery] string search = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string status = null, [FromQuery] int? roomId = null)
        {
            var result = await _hostel.GetInventoryAsync(SchoolId, search, page, pageSize, status, roomId);
            return Ok(new { success = true, data = result });
        }

        [HttpPost("Inventory")]
        public async Task<IActionResult> CreateInventory([FromBody] CreateHostelInventoryDto dto)
        {
            var item = await _hostel.CreateInventoryAsync(dto, SchoolId, UserId);
            return Ok(new { success = true, data = item });
        }

        [HttpPost("Inventory/{id}/audit")]
        public async Task<IActionResult> AuditInventory(int id, [FromQuery] string status)
        {
            var ok = await _hostel.AuditInventoryAsync(id, status, SchoolId, UserId);
            return ok ? Ok(new { success = true, message = "Asset audited successfully" }) : BadRequest(new { success = false });
        }

        // ════════════════════════════════════════════════════════════════════
        // MEDICAL LOGS
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("Medical")]
        public async Task<IActionResult> GetMedicalLogs([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string status = null)
        {
            var result = await _hostel.GetMedicalLogsAsync(SchoolId, page, pageSize, status);
            return Ok(new { success = true, data = result });
        }

        [HttpPost("Medical")]
        public async Task<IActionResult> CreateMedicalLog([FromBody] CreateHostelMedicalLogDto dto)
        {
            var item = await _hostel.CreateMedicalLogAsync(dto, SchoolId, UserId);
            return Ok(new { success = true, data = item });
        }

        [HttpPost("Medical/{id}/status")]
        public async Task<IActionResult> UpdateMedicalStatus(int id, [FromQuery] string status)
        {
            var ok = await _hostel.UpdateMedicalStatusAsync(id, status, SchoolId, UserId);
            return ok ? Ok(new { success = true, message = "Medical log status updated" }) : BadRequest(new { success = false });
        }

        // ════════════════════════════════════════════════════════════════════
        // DISCIPLINE
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("Discipline")]
        public async Task<IActionResult> GetDisciplines([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string severity = null)
        {
            var result = await _hostel.GetDisciplinesAsync(SchoolId, page, pageSize, severity);
            return Ok(new { success = true, data = result });
        }

        [HttpPost("Discipline")]
        public async Task<IActionResult> RecordDiscipline([FromBody] CreateHostelDisciplineDto dto)
        {
            var item = await _hostel.RecordDisciplineAsync(dto, SchoolId, UserId);
            return Ok(new { success = true, data = item });
        }

        // ════════════════════════════════════════════════════════════════════
        // REPORTS
        // ════════════════════════════════════════════════════════════════════
        [HttpGet("Reports/Occupancy")]
        public async Task<IActionResult> GetOccupancyReport([FromQuery] int? hostelId = null, [FromQuery] int? buildingId = null)
        {
            var data = await _hostel.GetOccupancyReportAsync(SchoolId, hostelId, buildingId);
            return Ok(new { success = true, data });
        }
    }
}
