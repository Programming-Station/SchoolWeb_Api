using System;
using System.Collections.Generic;

namespace School_DTOs.Transport
{
    #nullable disable

    // ══════════════════════════════════════════════════════════════════════════
    // VEHICLE DTOs
    // ══════════════════════════════════════════════════════════════════════════
    public class VehicleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public string VehicleType { get; set; }
        public int Capacity { get; set; }
        public string GpsDeviceNumber { get; set; }
        public string RfidReaderId { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public int Year { get; set; }
        public string EngineNumber { get; set; }
        public string ChassisNumber { get; set; }
        public string InsuranceNumber { get; set; }
        public DateTime? InsuranceExpiry { get; set; }
        public string FitnessCertificate { get; set; }
        public DateTime? FitnessExpiry { get; set; }
        public string PermitNumber { get; set; }
        public DateTime? PermitExpiry { get; set; }
        public string PucCertificate { get; set; }
        public DateTime? PucExpiry { get; set; }
        public string RcUploadUrl { get; set; }
        public string InsuranceUploadUrl { get; set; }
        public string PhotoUrl { get; set; }
        public string DocumentsUrl { get; set; }
        public string FuelType { get; set; }
        public double Mileage { get; set; }
        public double FuelTankCapacity { get; set; }
        public double CurrentOdometer { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string QrCode { get; set; }
        public string Barcode { get; set; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
    }

    public class CreateVehicleDto
    {
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public string VehicleType { get; set; }
        public int Capacity { get; set; }
        public string GpsDeviceNumber { get; set; }
        public string RfidReaderId { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public int Year { get; set; }
        public string EngineNumber { get; set; }
        public string ChassisNumber { get; set; }
        public string InsuranceNumber { get; set; }
        public DateTime? InsuranceExpiry { get; set; }
        public string FitnessCertificate { get; set; }
        public DateTime? FitnessExpiry { get; set; }
        public string PermitNumber { get; set; }
        public DateTime? PermitExpiry { get; set; }
        public string PucCertificate { get; set; }
        public DateTime? PucExpiry { get; set; }
        public string RcUploadUrl { get; set; }
        public string InsuranceUploadUrl { get; set; }
        public string PhotoUrl { get; set; }
        public string DocumentsUrl { get; set; }
        public string FuelType { get; set; }
        public double Mileage { get; set; }
        public double FuelTankCapacity { get; set; }
        public double CurrentOdometer { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // ROUTE DTOs
    // ══════════════════════════════════════════════════════════════════════════
    public class TransportRouteDto
    {
        public int Id { get; set; }
        public string RouteName { get; set; }
        public string RouteCode { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public double DistanceKm { get; set; }
        public int EstimatedTimeMinutes { get; set; }
        public string RouteMapPath { get; set; }
        public string RouteColor { get; set; }
        public int MaximumCapacity { get; set; }
        public int VehicleId { get; set; }
        public string VehicleName { get; set; }
        public string DriverName { get; set; }
        public int Capacity { get; set; }
        public int OccupiedSeats { get; set; }
        public string Status { get; set; }
    }

    public class CreateTransportRouteDto
    {
        public string RouteName { get; set; }
        public string RouteCode { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public double DistanceKm { get; set; }
        public int EstimatedTimeMinutes { get; set; }
        public string RouteMapPath { get; set; }
        public string RouteColor { get; set; }
        public int MaximumCapacity { get; set; }
        public int VehicleId { get; set; }
        public string Status { get; set; }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // STOP DTOs
    // ══════════════════════════════════════════════════════════════════════════
    public class TransportStopDto
    {
        public int Id { get; set; }
        public string StopName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public double DistanceFromSource { get; set; }
        public string GoogleMapsLink { get; set; }
        public string Landmark { get; set; }
        public string Status { get; set; }
    }

    public class CreateTransportStopDto
    {
        public string StopName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public double DistanceFromSource { get; set; }
        public string GoogleMapsLink { get; set; }
        public string Landmark { get; set; }
        public string Status { get; set; }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // ROUTE STOP MAPPING DTOs
    // ══════════════════════════════════════════════════════════════════════════
    public class RouteStopMappingDto
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public int StopId { get; set; }
        public string StopName { get; set; }
        public int SequenceOrder { get; set; }
    }

    public class CreateRouteStopMappingDto
    {
        public int RouteId { get; set; }
        public int StopId { get; set; }
        public int SequenceOrder { get; set; }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // CONDUCTOR DTOs
    // ══════════════════════════════════════════════════════════════════════════
    public class ConductorDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string LicenseNumber { get; set; }
        public string ContactNumber { get; set; }
        public string EmergencyContact { get; set; }
        public bool PoliceVerified { get; set; }
        public bool BackgroundVerified { get; set; }
        public string DocumentsUrl { get; set; }
        public string Status { get; set; }
    }

    public class CreateConductorDto
    {
        public int EmployeeId { get; set; }
        public string LicenseNumber { get; set; }
        public string ContactNumber { get; set; }
        public string EmergencyContact { get; set; }
        public bool PoliceVerified { get; set; }
        public bool BackgroundVerified { get; set; }
        public string DocumentsUrl { get; set; }
        public string Status { get; set; }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // ROUTE ASSIGNMENT DTOs
    // ══════════════════════════════════════════════════════════════════════════
    public class RouteAssignmentDto
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public string RouteCode { get; set; }
        public int VehicleId { get; set; }
        public string VehicleName { get; set; }
        public string RegistrationNumber { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public int? ConductorId { get; set; }
        public string ConductorName { get; set; }
        public int AcademicYearId { get; set; }
        public string AcademicYearName { get; set; }
        public int? BackupVehicleId { get; set; }
        public string BackupVehicleName { get; set; }
        public int? BackupDriverId { get; set; }
        public string BackupDriverName { get; set; }
        public string Status { get; set; }
    }

    public class CreateRouteAssignmentDto
    {
        public int RouteId { get; set; }
        public int VehicleId { get; set; }
        public int DriverId { get; set; }
        public int? ConductorId { get; set; }
        public int AcademicYearId { get; set; }
        public int? BackupVehicleId { get; set; }
        public int? BackupDriverId { get; set; }
        public string Status { get; set; }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // ALLOCATION DTOs
    // ══════════════════════════════════════════════════════════════════════════
    public class TransportAllocationDto
    {
        public int Id { get; set; }
        public int? StudentId { get; set; }
        public string StudentName { get; set; }
        public string AdmissionNumber { get; set; }
        public string ClassName { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public int TransportRouteId { get; set; }
        public string RouteName { get; set; }
        public string VehicleName { get; set; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public int? PickupStopId { get; set; }
        public string PickupStopName { get; set; }
        public int? DropStopId { get; set; }
        public string DropStopName { get; set; }
        public TimeSpan? PickupTime { get; set; }
        public TimeSpan? DropTime { get; set; }
        public string SeatNumber { get; set; }
        public decimal MonthlyCharge { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int AcademicYearId { get; set; }
        public string QrCode { get; set; }
        public string RfidTag { get; set; }
        public bool IsSuspended { get; set; }
        public string Status { get; set; }
    }

    public class CreateTransportAllocationDto
    {
        public int? StudentId { get; set; }
        public int? EmployeeId { get; set; }
        public int TransportRouteId { get; set; }
        public int? PickupStopId { get; set; }
        public int? DropStopId { get; set; }
        public string SeatNumber { get; set; }
        public decimal MonthlyCharge { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int AcademicYearId { get; set; }
        public string RfidTag { get; set; }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // TRIP DTOs
    // ══════════════════════════════════════════════════════════════════════════
    public class TransportTripDto
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public int VehicleId { get; set; }
        public string VehicleName { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public int? ConductorId { get; set; }
        public string ConductorName { get; set; }
        public DateTime TripDate { get; set; }
        public DateTime ScheduledStart { get; set; }
        public DateTime? ActualStart { get; set; }
        public DateTime? ActualEnd { get; set; }
        public int DelayMinutes { get; set; }
        public string Status { get; set; }
        public string CancellationReason { get; set; }
        public string DriverNotes { get; set; }
    }

    public class CreateTransportTripDto
    {
        public int RouteId { get; set; }
        public int VehicleId { get; set; }
        public int DriverId { get; set; }
        public int? ConductorId { get; set; }
        public DateTime TripDate { get; set; }
        public DateTime ScheduledStart { get; set; }
        public string Status { get; set; }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // RFID SCAN LOG DTOs
    // ══════════════════════════════════════════════════════════════════════════
    public class RfidScanLogDto
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public string RouteName { get; set; }
        public int? StudentId { get; set; }
        public string StudentName { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string RfidTag { get; set; }
        public DateTime ScanTime { get; set; }
        public string ScanType { get; set; }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // FUEL LOG DTOs
    // ══════════════════════════════════════════════════════════════════════════
    public class FuelLogDto
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string VehicleName { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string VendorName { get; set; }
        public double FuelQuantity { get; set; }
        public decimal CostPerUnit { get; set; }
        public decimal TotalCost { get; set; }
        public double OdometerReading { get; set; }
    }

    public class CreateFuelLogDto
    {
        public int VehicleId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string VendorName { get; set; }
        public double FuelQuantity { get; set; }
        public decimal CostPerUnit { get; set; }
        public decimal TotalCost { get; set; }
        public double OdometerReading { get; set; }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // VEHICLE MAINTENANCE DTOs
    // ══════════════════════════════════════════════════════════════════════════
    public class VehicleMaintenanceDto
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string VehicleName { get; set; }
        public string MaintenanceType { get; set; }
        public DateTime ServiceDate { get; set; }
        public double Odometer { get; set; }
        public decimal Cost { get; set; }
        public string VendorName { get; set; }
        public string Details { get; set; }
        public DateTime? NextServiceDue { get; set; }
    }

    public class CreateVehicleMaintenanceDto
    {
        public int VehicleId { get; set; }
        public string MaintenanceType { get; set; }
        public DateTime ServiceDate { get; set; }
        public double Odometer { get; set; }
        public decimal Cost { get; set; }
        public string VendorName { get; set; }
        public string Details { get; set; }
        public DateTime? NextServiceDue { get; set; }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // VEHICLE INCIDENT DTOs
    // ══════════════════════════════════════════════════════════════════════════
    public class VehicleIncidentDto
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string VehicleName { get; set; }
        public DateTime IncidentDate { get; set; }
        public string Description { get; set; }
        public string ClaimNumber { get; set; }
        public decimal ClaimAmount { get; set; }
        public decimal RepairCost { get; set; }
        public string PoliceReportFileUrl { get; set; }
        public string PhotoUrl { get; set; }
    }

    public class CreateVehicleIncidentDto
    {
        public int VehicleId { get; set; }
        public DateTime IncidentDate { get; set; }
        public string Description { get; set; }
        public string ClaimNumber { get; set; }
        public decimal ClaimAmount { get; set; }
        public decimal RepairCost { get; set; }
        public string PoliceReportFileUrl { get; set; }
        public string PhotoUrl { get; set; }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // INVENTORY DTOs
    // ══════════════════════════════════════════════════════════════════════════
    public class TransportInventoryDto
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string Category { get; set; }
        public string SerialNumber { get; set; }
        public DateTime? InstallationDate { get; set; }
        public string Status { get; set; }
    }

    public class CreateTransportInventoryDto
    {
        public string ItemName { get; set; }
        public string Category { get; set; }
        public string SerialNumber { get; set; }
        public DateTime? InstallationDate { get; set; }
        public string Status { get; set; }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // GATE LOG DTOs
    // ══════════════════════════════════════════════════════════════════════════
    public class TransportGateLogDto
    {
        public int Id { get; set; }
        public string VehicleNumber { get; set; }
        public string DriverName { get; set; }
        public string Purpose { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }
        public string Status { get; set; }
    }

    public class CreateTransportGateLogDto
    {
        public string VehicleNumber { get; set; }
        public string DriverName { get; set; }
        public string Purpose { get; set; }
        public DateTime EntryTime { get; set; }
        public string Status { get; set; }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // DASHBOARD DTO
    // ══════════════════════════════════════════════════════════════════════════
    public class TransportDashboardDto
    {
        public int TotalVehicles { get; set; }
        public int ActiveVehicles { get; set; }
        public int InactiveVehicles { get; set; }
        public int VehiclesUnderMaintenance { get; set; }
        public int TodayTrips { get; set; }
        public int CompletedTrips { get; set; }
        public int RunningTrips { get; set; }
        public int CancelledTrips { get; set; }
        public int TotalRoutes { get; set; }
        public int TotalStops { get; set; }
        public int TotalDrivers { get; set; }
        public int TotalConductors { get; set; }
        public int TotalStudentsAllocated { get; set; }
        public int TotalEmployeesAllocated { get; set; }
        public decimal PendingFeeCollection { get; set; }
        public decimal TodayCollection { get; set; }
        public decimal TotalFuelCost { get; set; }
        public decimal TotalMaintenanceCost { get; set; }
        public int ActiveGpsVehicles { get; set; }
        public int ActiveEmergencyAlerts { get; set; }
        public List<MonthlyFeeTrendDto> MonthlyFeeTrends { get; set; } = new();
        public List<RouteStudentLoadDto> RouteStudentLoads { get; set; } = new();
        public List<VehicleFuelCostDto> VehicleFuelCosts { get; set; } = new();
    }

    public class MonthlyFeeTrendDto
    {
        public string Month { get; set; }
        public decimal Collection { get; set; }
        public decimal Outstanding { get; set; }
    }

    public class RouteStudentLoadDto
    {
        public string RouteName { get; set; }
        public int StudentCount { get; set; }
        public int CapacityLimit { get; set; }
    }

    public class VehicleFuelCostDto
    {
        public string VehicleName { get; set; }
        public decimal FuelCost { get; set; }
        public decimal MaintenanceCost { get; set; }
    }
}
