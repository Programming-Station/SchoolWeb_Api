namespace School_DTOs.Hostel
{
#nullable disable

    // ════════════════════════════════════════════════════════════════════════
    // HOSTEL DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class HostelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string HostelType { get; set; } // Boys, Girls, Staff
        public int Capacity { get; set; }
        public string Address { get; set; }
        public int BuildingCount { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string PhotoUrl { get; set; }
        public string GeoLocation { get; set; }
        public string EmergencyContact { get; set; }
        public int SchoolRegistrationId { get; set; }
    }

    public class CreateHostelDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string HostelType { get; set; }
        public int Capacity { get; set; }
        public string Address { get; set; }
        public int BuildingCount { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } = "active";
        public string PhotoUrl { get; set; }
        public string GeoLocation { get; set; }
        public string EmergencyContact { get; set; }
    }

    // ════════════════════════════════════════════════════════════════════════
    // BUILDING DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class BuildingDto
    {
        public int Id { get; set; }
        public int HostelId { get; set; }
        public string HostelName { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int NumberOfFloors { get; set; }
        public int ConstructionYear { get; set; }
        public string Status { get; set; }
    }

    public class CreateBuildingDto
    {
        public int HostelId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int NumberOfFloors { get; set; }
        public int ConstructionYear { get; set; }
        public string Status { get; set; } = "active";
    }

    // ════════════════════════════════════════════════════════════════════════
    // FLOOR DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class FloorDto
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public string BuildingName { get; set; }
        public string HostelName { get; set; }
        public int FloorNumber { get; set; }
        public string Description { get; set; }
    }

    public class CreateFloorDto
    {
        public int BuildingId { get; set; }
        public int FloorNumber { get; set; }
        public string Description { get; set; }
    }

    // ════════════════════════════════════════════════════════════════════════
    // ROOM CATEGORY DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class RoomCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAc { get; set; }
        public bool HasAttachedBathroom { get; set; }
        public bool HasWifi { get; set; }
        public decimal DefaultFee { get; set; }
    }

    public class CreateRoomCategoryDto
    {
        public string Name { get; set; }
        public bool IsAc { get; set; }
        public bool HasAttachedBathroom { get; set; }
        public bool HasWifi { get; set; }
        public decimal DefaultFee { get; set; }
    }

    // ════════════════════════════════════════════════════════════════════════
    // ROOM DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class RoomDto
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public int HostelId { get; set; }
        public string HostelName { get; set; }
        public int BuildingId { get; set; }
        public string BuildingName { get; set; }
        public int FloorId { get; set; }
        public int FloorNumber { get; set; }
        public int RoomCategoryId { get; set; }
        public string RoomCategoryName { get; set; }
        public int Capacity { get; set; }
        public int OccupiedBeds { get; set; }
        public int AvailableBeds { get; set; }
        public string FurnitureDetails { get; set; }
        public string PhotoUrl { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }

    public class CreateRoomDto
    {
        public string RoomNumber { get; set; }
        public int HostelId { get; set; }
        public int BuildingId { get; set; }
        public int FloorId { get; set; }
        public int RoomCategoryId { get; set; }
        public int Capacity { get; set; }
        public string FurnitureDetails { get; set; }
        public string PhotoUrl { get; set; }
        public string Status { get; set; } = "active";
        public string Remarks { get; set; }
    }

    // ════════════════════════════════════════════════════════════════════════
    // BED DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class BedDto
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string HostelName { get; set; }
        public string BedNumber { get; set; }
        public string Status { get; set; } // vacant, occupied, reserved, repair
        public string LockerNumber { get; set; }
        public string CupboardNumber { get; set; }
        public string MattressNumber { get; set; }
        public string QrCode { get; set; }
        public string Barcode { get; set; }
        public string RfidTag { get; set; }
        public string CleaningStatus { get; set; }
    }

    public class CreateBedDto
    {
        public int RoomId { get; set; }
        public string BedNumber { get; set; }
        public string Status { get; set; } = "vacant";
        public string LockerNumber { get; set; }
        public string CupboardNumber { get; set; }
        public string MattressNumber { get; set; }
        public string RfidTag { get; set; }
    }

    // ════════════════════════════════════════════════════════════════════════
    // WARDEN DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class HostelWardenDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public int HostelId { get; set; }
        public string HostelName { get; set; }
        public string RoleType { get; set; } // ChiefWarden, AssistantWarden, Supervisor
        public string EmergencyContact { get; set; }
        public string Status { get; set; }
    }

    public class CreateHostelWardenDto
    {
        public int EmployeeId { get; set; }
        public int HostelId { get; set; }
        public string RoleType { get; set; }
        public string EmergencyContact { get; set; }
        public string Status { get; set; } = "active";
    }

    // ════════════════════════════════════════════════════════════════════════
    // ADMISSION DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class HostelAdmissionDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public int HostelId { get; set; }
        public string HostelName { get; set; }
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public int BedId { get; set; }
        public string BedNumber { get; set; }
        public int AcademicYearId { get; set; }
        public string AcademicYearName { get; set; }
        public string AdmissionNumber { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? ExpectedCheckOutDate { get; set; }
        public DateTime? ActualCheckOutDate { get; set; }
        public string Status { get; set; } // admitted, checkedin, checkedout, cancelled
        public decimal SecurityDeposit { get; set; }
        public bool SecurityDepositRefunded { get; set; }
        public string DocumentsUrl { get; set; }
        public string MedicalDetails { get; set; }
        public string SpecialNotes { get; set; }
        public string BiometricId { get; set; }
        public string RfidTag { get; set; }
    }

    public class CreateHostelAdmissionDto
    {
        public int StudentId { get; set; }
        public int HostelId { get; set; }
        public int RoomId { get; set; }
        public int BedId { get; set; }
        public int AcademicYearId { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateTime? ExpectedCheckOutDate { get; set; }
        public decimal SecurityDeposit { get; set; }
        public string DocumentsUrl { get; set; }
        public string MedicalDetails { get; set; }
        public string SpecialNotes { get; set; }
        public string BiometricId { get; set; }
        public string RfidTag { get; set; }
    }

    // ════════════════════════════════════════════════════════════════════════
    // ROOM TRANSFER & EXCHANGE DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class RoomTransferDto
    {
        public int StudentId { get; set; }
        public int ToRoomId { get; set; }
        public int ToBedId { get; set; }
        public string Reason { get; set; }
        public string ApprovedBy { get; set; }
    }

    public class RoomExchangeDto
    {
        public int StudentAId { get; set; }
        public int StudentBId { get; set; }
        public string Reason { get; set; }
        public string ApprovedBy { get; set; }
    }

    // ════════════════════════════════════════════════════════════════════════
    // BED RESERVATION DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class BedReservationDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int BedId { get; set; }
        public string BedNumber { get; set; }
        public string RoomNumber { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Status { get; set; }
    }

    public class CreateBedReservationDto
    {
        public int StudentId { get; set; }
        public int BedId { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

    // ════════════════════════════════════════════════════════════════════════
    // MESS MENU DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class MessMenuDto
    {
        public int Id { get; set; }
        public int HostelId { get; set; }
        public string HostelName { get; set; }
        public string DayOfWeek { get; set; }
        public string MealType { get; set; }
        public string FoodItems { get; set; }
        public string SpecialItems { get; set; }
    }

    public class CreateMessMenuDto
    {
        public int HostelId { get; set; }
        public string DayOfWeek { get; set; }
        public string MealType { get; set; }
        public string FoodItems { get; set; }
        public string SpecialItems { get; set; }
    }

    // ════════════════════════════════════════════════════════════════════════
    // MEAL ATTENDANCE DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class MealAttendanceDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public string MealType { get; set; }
        public DateTime Date { get; set; }
        public string ScannedVia { get; set; }
        public string TokenNumber { get; set; }
    }

    public class CreateMealAttendanceDto
    {
        public int StudentId { get; set; }
        public string MealType { get; set; }
        public string ScannedVia { get; set; } = "Manual";
        public string TokenNumber { get; set; }
    }

    // ════════════════════════════════════════════════════════════════════════
    // VISITOR DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class HostelVisitorDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public string VisitorName { get; set; }
        public string Relation { get; set; }
        public string ContactNumber { get; set; }
        public string IdProofType { get; set; }
        public string IdProofNumber { get; set; }
        public string PhotoUrl { get; set; }
        public string Purpose { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }
        public string ApprovedBy { get; set; }
        public string Status { get; set; }
    }

    public class CreateHostelVisitorDto
    {
        public int StudentId { get; set; }
        public string VisitorName { get; set; }
        public string Relation { get; set; }
        public string ContactNumber { get; set; }
        public string IdProofType { get; set; }
        public string IdProofNumber { get; set; }
        public string PhotoUrl { get; set; }
        public string Purpose { get; set; }
    }

    // ════════════════════════════════════════════════════════════════════════
    // GATE PASS DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class HostelGatePassDto
    {
        public int Id { get; set; }
        public int AdmissionId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public string RoomNumber { get; set; }
        public DateTime OutTime { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime ExpectedReturn { get; set; }
        public string Reason { get; set; }
        public string WardenApproval { get; set; }
        public string ParentApproval { get; set; }
        public string QrCode { get; set; }
        public string Status { get; set; }
    }

    public class CreateHostelGatePassDto
    {
        public int AdmissionId { get; set; }
        public int StudentId { get; set; }
        public DateTime OutTime { get; set; }
        public DateTime ExpectedReturn { get; set; }
        public string Reason { get; set; }
    }

    // ════════════════════════════════════════════════════════════════════════
    // ATTENDANCE DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class HostelAttendanceDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public string RoomNumber { get; set; }
        public DateTime Date { get; set; }
        public string AttendanceType { get; set; } // Morning, Night
        public string Status { get; set; } // Present, Absent, Leave
        public string Remarks { get; set; }
    }

    public class CreateHostelAttendanceDto
    {
        public int StudentId { get; set; }
        public DateTime Date { get; set; }
        public string AttendanceType { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }

    // ════════════════════════════════════════════════════════════════════════
    // COMPLAINT DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class HostelComplaintDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string RoomNumber { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public int? AssignedStaffId { get; set; }
        public string AssignedStaffName { get; set; }
        public string PhotoUrl { get; set; }
        public string ResolutionDetails { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    public class CreateHostelComplaintDto
    {
        public int StudentId { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; } = "Medium";
        public string PhotoUrl { get; set; }
    }

    // ════════════════════════════════════════════════════════════════════════
    // MAINTENANCE DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class HostelMaintenanceDto
    {
        public int Id { get; set; }
        public int? ComplaintId { get; set; }
        public string ComplaintDescription { get; set; }
        public int? AssetId { get; set; }
        public string Description { get; set; }
        public string TechnicianName { get; set; }
        public decimal Cost { get; set; }
        public string Status { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string PhotoUrl { get; set; }
    }

    public class CreateHostelMaintenanceDto
    {
        public int? ComplaintId { get; set; }
        public int? AssetId { get; set; }
        public string Description { get; set; }
        public string TechnicianName { get; set; }
        public decimal Cost { get; set; }
        public string Status { get; set; } = "Pending";
    }

    // ════════════════════════════════════════════════════════════════════════
    // LAUNDRY DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class LaundryTransactionDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public string TokenNumber { get; set; }
        public int ItemCount { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime ExpectedDelivery { get; set; }
        public DateTime? ActualDelivery { get; set; }
        public decimal Charges { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }

    public class CreateLaundryTransactionDto
    {
        public int StudentId { get; set; }
        public string TokenNumber { get; set; }
        public int ItemCount { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime ExpectedDelivery { get; set; }
        public decimal Charges { get; set; }
        public string Remarks { get; set; }
    }

    // ════════════════════════════════════════════════════════════════════════
    // INVENTORY DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class HostelInventoryDto
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string HostelName { get; set; }
        public string AssetName { get; set; }
        public string AssetTag { get; set; }
        public string Barcode { get; set; }
        public string Status { get; set; }
        public DateTime? AuditDate { get; set; }
    }

    public class CreateHostelInventoryDto
    {
        public int RoomId { get; set; }
        public string AssetName { get; set; }
        public string AssetTag { get; set; }
        public string Barcode { get; set; }
        public string Status { get; set; } = "Active";
    }

    // ════════════════════════════════════════════════════════════════════════
    // MEDICAL LOG DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class HostelMedicalLogDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string IncidentDescription { get; set; }
        public string Temperature { get; set; }
        public string Bp { get; set; }
        public bool DoctorVisited { get; set; }
        public string MedicinesGiven { get; set; }
        public bool IsolationRequired { get; set; }
        public int? IsolationRoomId { get; set; }
        public string IsolationRoomNumber { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    public class CreateHostelMedicalLogDto
    {
        public int StudentId { get; set; }
        public string IncidentDescription { get; set; }
        public string Temperature { get; set; }
        public string Bp { get; set; }
        public bool DoctorVisited { get; set; }
        public string MedicinesGiven { get; set; }
        public bool IsolationRequired { get; set; }
        public int? IsolationRoomId { get; set; }
        public string Status { get; set; } = "UnderTreatment";
    }

    // ════════════════════════════════════════════════════════════════════════
    // DISCIPLINE DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class HostelDisciplineDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public string Offense { get; set; }
        public string Severity { get; set; }
        public string ActionTaken { get; set; }
        public decimal FineAmount { get; set; }
        public DateTime IncidentDate { get; set; }
        public string WardenRemarks { get; set; }
    }

    public class CreateHostelDisciplineDto
    {
        public int StudentId { get; set; }
        public string Offense { get; set; }
        public string Severity { get; set; }
        public string ActionTaken { get; set; }
        public decimal FineAmount { get; set; }
        public DateTime IncidentDate { get; set; }
        public string WardenRemarks { get; set; }
    }

    // ════════════════════════════════════════════════════════════════════════
    // DASHBOARD & ANALYTICS DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class HostelDashboardDto
    {
        public int TotalHostels { get; set; }
        public int BoysHostels { get; set; }
        public int GirlsHostels { get; set; }
        public int TotalBuildings { get; set; }
        public int TotalFloors { get; set; }
        public int TotalRooms { get; set; }
        public int TotalBeds { get; set; }
        public int OccupiedBeds { get; set; }
        public int AvailableBeds { get; set; }
        public int ReservedBeds { get; set; }
        public int RepairBeds { get; set; }
        public int TotalStudents { get; set; }
        public int MessAttendanceToday { get; set; }
        public int VisitorsToday { get; set; }
        public int PendingComplaints { get; set; }
        public int ActiveGatePasses { get; set; }

        public List<HostelOccupancyStatDto> HostelOccupancyStats { get; set; } = new();
        public List<MonthlyAdmissionsStatDto> MonthlyAdmissionsStats { get; set; } = new();
        public List<ComplaintCategoryStatDto> ComplaintCategoryStats { get; set; } = new();
        public List<VisitorTrendStatDto> VisitorTrendStats { get; set; } = new();
    }

    public class HostelOccupancyStatDto
    {
        public string HostelName { get; set; }
        public double OccupancyPercentage { get; set; }
    }

    public class MonthlyAdmissionsStatDto
    {
        public string MonthName { get; set; }
        public int Count { get; set; }
    }

    public class ComplaintCategoryStatDto
    {
        public string CategoryName { get; set; }
        public int Count { get; set; }
    }

    public class VisitorTrendStatDto
    {
        public string DateLabel { get; set; }
        public int Count { get; set; }
    }

    public class PagedResultDto<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}
