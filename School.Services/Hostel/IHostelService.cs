using School_DTOs.Hostel;

namespace School.Services.Hostel
{
    public interface IHostelService
    {
        // ── Hostels ───────────────────────────────────────────────────────────
        Task<PagedResultDto<HostelDto>> GetHostelsAsync(int schoolId, string search, int page, int pageSize, string status);
        Task<HostelDto> GetHostelByIdAsync(int id, int schoolId);
        Task<HostelDto> CreateHostelAsync(CreateHostelDto dto, int schoolId, string userId);
        Task<bool> UpdateHostelAsync(int id, CreateHostelDto dto, int schoolId, string userId);
        Task<bool> DeleteHostelAsync(int id, int schoolId);
        Task<bool> RestoreHostelAsync(int id, int schoolId);

        // ── Buildings ─────────────────────────────────────────────────────────
        Task<PagedResultDto<BuildingDto>> GetBuildingsAsync(int schoolId, string search, int page, int pageSize, string status, int? hostelId);
        Task<BuildingDto> GetBuildingByIdAsync(int id, int schoolId);
        Task<BuildingDto> CreateBuildingAsync(CreateBuildingDto dto, int schoolId, string userId);
        Task<bool> UpdateBuildingAsync(int id, CreateBuildingDto dto, int schoolId, string userId);
        Task<bool> DeleteBuildingAsync(int id, int schoolId);
        Task<bool> RestoreBuildingAsync(int id, int schoolId);
        Task<IEnumerable<BuildingDto>> GetAllBuildingsDropdownAsync(int schoolId, int? hostelId);

        // ── Floors ────────────────────────────────────────────────────────────
        Task<PagedResultDto<FloorDto>> GetFloorsAsync(int schoolId, int page, int pageSize, int? buildingId);
        Task<FloorDto> GetFloorByIdAsync(int id, int schoolId);
        Task<FloorDto> CreateFloorAsync(CreateFloorDto dto, int schoolId, string userId);
        Task<bool> UpdateFloorAsync(int id, CreateFloorDto dto, int schoolId, string userId);
        Task<bool> DeleteFloorAsync(int id, int schoolId);
        Task<IEnumerable<FloorDto>> GetFloorsDropdownAsync(int schoolId, int buildingId);

        // ── Room Categories ──────────────────────────────────────────────────
        Task<IEnumerable<RoomCategoryDto>> GetRoomCategoriesAsync(int schoolId);
        Task<RoomCategoryDto> GetRoomCategoryByIdAsync(int id, int schoolId);
        Task<RoomCategoryDto> CreateRoomCategoryAsync(CreateRoomCategoryDto dto, int schoolId, string userId);
        Task<bool> UpdateRoomCategoryAsync(int id, CreateRoomCategoryDto dto, int schoolId, string userId);
        Task<bool> DeleteRoomCategoryAsync(int id, int schoolId);

        // ── Rooms ────────────────────────────────═════════════════════════════
        Task<PagedResultDto<RoomDto>> GetRoomsAsync(int schoolId, string search, int page, int pageSize, string status, int? hostelId, int? buildingId, int? floorId, int? categoryId);
        Task<RoomDto> GetRoomByIdAsync(int id, int schoolId);
        Task<RoomDto> CreateRoomAsync(CreateRoomDto dto, int schoolId, string userId);
        Task<bool> UpdateRoomAsync(int id, CreateRoomDto dto, int schoolId, string userId);
        Task<bool> DeleteRoomAsync(int id, int schoolId);
        Task<bool> RestoreRoomAsync(int id, int schoolId);
        Task<IEnumerable<RoomDto>> GetRoomsDropdownAsync(int schoolId, int floorId);

        // ── Beds ─────────────────────────────────═════════════════════════════
        Task<PagedResultDto<BedDto>> GetBedsAsync(int schoolId, string search, int page, int pageSize, string status, int? roomId);
        Task<BedDto> GetBedByIdAsync(int id, int schoolId);
        Task<BedDto> CreateBedAsync(CreateBedDto dto, int schoolId, string userId);
        Task<bool> UpdateBedAsync(int id, CreateBedDto dto, int schoolId, string userId);
        Task<bool> DeleteBedAsync(int id, int schoolId);
        Task<bool> RestoreBedAsync(int id, int schoolId);
        Task<IEnumerable<BedDto>> GetBedsDropdownAsync(int schoolId, int roomId, string status);

        // ── Wardens ──────────────────────────────═════════════════════════════
        Task<PagedResultDto<HostelWardenDto>> GetWardensAsync(int schoolId, string search, int page, int pageSize, string status, int? hostelId);
        Task<HostelWardenDto> GetWardenByIdAsync(int id, int schoolId);
        Task<HostelWardenDto> CreateWardenAsync(CreateHostelWardenDto dto, int schoolId, string userId);
        Task<bool> UpdateWardenAsync(int id, CreateHostelWardenDto dto, int schoolId, string userId);
        Task<bool> DeleteWardenAsync(int id, int schoolId);

        // ── Admissions / Allocation ──────────────═════════════════════════════
        Task<PagedResultDto<HostelAdmissionDto>> GetAdmissionsAsync(int schoolId, string search, int page, int pageSize, string status, int? hostelId, int? roomId);
        Task<HostelAdmissionDto> GetAdmissionByIdAsync(int id, int schoolId);
        Task<HostelAdmissionDto> CreateAdmissionAsync(CreateHostelAdmissionDto dto, int schoolId, string userId);
        Task<bool> CheckInStudentAsync(int admissionId, string biometricId, string rfidTag, int schoolId, string userId);
        Task<bool> CheckOutStudentAsync(int admissionId, int schoolId, string userId);
        Task<bool> TransferRoomAsync(RoomTransferDto dto, int schoolId, string userId);
        Task<bool> ExchangeRoomAsync(RoomExchangeDto dto, int schoolId, string userId);

        // ── Bed Reservations ─────────────────────═════════════════════════════
        Task<IEnumerable<BedReservationDto>> GetReservationsAsync(int schoolId);
        Task<BedReservationDto> ReserveBedAsync(CreateBedReservationDto dto, int schoolId, string userId);
        Task<bool> CancelReservationAsync(int id, int schoolId, string userId);

        // ── Mess Management ──────────────────────═════════════════════════════
        Task<IEnumerable<MessMenuDto>> GetMessMenusAsync(int schoolId, int hostelId);
        Task<MessMenuDto> CreateMessMenuAsync(CreateMessMenuDto dto, int schoolId, string userId);
        Task<bool> UpdateMessMenuAsync(int id, CreateMessMenuDto dto, int schoolId, string userId);
        Task<bool> DeleteMessMenuAsync(int id, int schoolId);

        // ── Meal Attendance ──────────────────────═════════════════════════════
        Task<PagedResultDto<MealAttendanceDto>> GetMealAttendanceAsync(int schoolId, int page, int pageSize, string mealType, DateTime? date);
        Task<MealAttendanceDto> MarkMealAttendanceAsync(CreateMealAttendanceDto dto, int schoolId, string userId);

        // ── Visitor Management ───────────────────═════════════════════════════
        Task<PagedResultDto<HostelVisitorDto>> GetVisitorsAsync(int schoolId, string search, int page, int pageSize, string status);
        Task<HostelVisitorDto> RegisterVisitorAsync(CreateHostelVisitorDto dto, int schoolId, string userId);
        Task<bool> CheckoutVisitorAsync(int visitorId, int schoolId, string userId);

        // ── Gate Pass / Leaves ───────────────────═════════════════════════════
        Task<PagedResultDto<HostelGatePassDto>> GetGatePassesAsync(int schoolId, string search, int page, int pageSize, string status);
        Task<HostelGatePassDto> IssueGatePassAsync(CreateHostelGatePassDto dto, int schoolId, string userId);
        Task<bool> ApproveGatePassAsync(int gatePassId, string approverRole, string approvalStatus, int schoolId, string userId);
        Task<bool> ScanGatePassInAsync(int gatePassId, int schoolId, string userId);
        Task<bool> ScanGatePassOutAsync(int gatePassId, int schoolId, string userId);

        // ── Attendance ───────────────────────────═════════════════════════════
        Task<PagedResultDto<HostelAttendanceDto>> GetAttendanceAsync(int schoolId, string search, int page, int pageSize, string type, DateTime? date);
        Task<bool> MarkAttendanceAsync(List<CreateHostelAttendanceDto> list, int schoolId, string userId);

        // ── Complaints ───────────────────────────═════════════════════════════
        Task<PagedResultDto<HostelComplaintDto>> GetComplaintsAsync(int schoolId, string search, int page, int pageSize, string status, string priority);
        Task<HostelComplaintDto> RaiseComplaintAsync(CreateHostelComplaintDto dto, int schoolId, string userId);
        Task<bool> AssignComplaintAsync(int id, int staffId, int schoolId, string userId);
        Task<bool> ResolveComplaintAsync(int id, string details, int schoolId, string userId);

        // ── Maintenance ──────────────────────────═════════════════════════════
        Task<PagedResultDto<HostelMaintenanceDto>> GetMaintenanceRequestsAsync(int schoolId, int page, int pageSize, string status);
        Task<HostelMaintenanceDto> CreateMaintenanceRequestAsync(CreateHostelMaintenanceDto dto, int schoolId, string userId);
        Task<bool> CompleteMaintenanceRequestAsync(int id, decimal cost, string details, int schoolId, string userId);

        // ── Laundry ──────────────────────────────═════════════════════════════
        Task<PagedResultDto<LaundryTransactionDto>> GetLaundryTransactionsAsync(int schoolId, int page, int pageSize, string status);
        Task<LaundryTransactionDto> CreateLaundryTransactionAsync(CreateLaundryTransactionDto dto, int schoolId, string userId);
        Task<bool> UpdateLaundryStatusAsync(int id, string status, int schoolId, string userId);

        // ── Inventory ────────────────────────────═════════════════════════════
        Task<PagedResultDto<HostelInventoryDto>> GetInventoryAsync(int schoolId, string search, int page, int pageSize, string status, int? roomId);
        Task<HostelInventoryDto> CreateInventoryAsync(CreateHostelInventoryDto dto, int schoolId, string userId);
        Task<bool> AuditInventoryAsync(int id, string status, int schoolId, string userId);

        // ── Medical ──────────────────────────────═════════════════════════════
        Task<PagedResultDto<HostelMedicalLogDto>> GetMedicalLogsAsync(int schoolId, int page, int pageSize, string status);
        Task<HostelMedicalLogDto> CreateMedicalLogAsync(CreateHostelMedicalLogDto dto, int schoolId, string userId);
        Task<bool> UpdateMedicalStatusAsync(int id, string status, int schoolId, string userId);

        // ── Discipline ───────────────────────────═════════════════════════════
        Task<PagedResultDto<HostelDisciplineDto>> GetDisciplinesAsync(int schoolId, int page, int pageSize, string severity);
        Task<HostelDisciplineDto> RecordDisciplineAsync(CreateHostelDisciplineDto dto, int schoolId, string userId);

        // ── Dashboard & Reports ──────────────────═════════════════════════════
        Task<HostelDashboardDto> GetDashboardAsync(int schoolId);
        Task<IEnumerable<HostelAdmissionDto>> GetOccupancyReportAsync(int schoolId, int? hostelId, int? buildingId);
    }
}
