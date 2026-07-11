using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTransportManagementModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Vehicles",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "RegistrationNumber",
                table: "Vehicles",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "Vehicles",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChassisNumber",
                table: "Vehicles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CurrentOdometer",
                table: "Vehicles",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "DocumentsUrl",
                table: "Vehicles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EngineNumber",
                table: "Vehicles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FitnessCertificate",
                table: "Vehicles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FitnessExpiry",
                table: "Vehicles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FuelTankCapacity",
                table: "Vehicles",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "FuelType",
                table: "Vehicles",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GpsDeviceNumber",
                table: "Vehicles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InsuranceExpiry",
                table: "Vehicles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsuranceNumber",
                table: "Vehicles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsuranceUploadUrl",
                table: "Vehicles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "Vehicles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Mileage",
                table: "Vehicles",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Vehicles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PermitExpiry",
                table: "Vehicles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermitNumber",
                table: "Vehicles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Vehicles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PucCertificate",
                table: "Vehicles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PucExpiry",
                table: "Vehicles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QrCode",
                table: "Vehicles",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RcUploadUrl",
                table: "Vehicles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "Vehicles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RfidReaderId",
                table: "Vehicles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleType",
                table: "Vehicles",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Destination",
                table: "TransportRoutes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DistanceKm",
                table: "TransportRoutes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "EstimatedTimeMinutes",
                table: "TransportRoutes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaximumCapacity",
                table: "TransportRoutes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RouteCode",
                table: "TransportRoutes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RouteColor",
                table: "TransportRoutes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RouteMapPath",
                table: "TransportRoutes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "TransportRoutes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "TransportAllocations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "TransportAllocations",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AcademicYearId",
                table: "TransportAllocations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DropStopId",
                table: "TransportAllocations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "DropTime",
                table: "TransportAllocations",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "TransportAllocations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSuspended",
                table: "TransportAllocations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PickupStopId",
                table: "TransportAllocations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "PickupTime",
                table: "TransportAllocations",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QrCode",
                table: "TransportAllocations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RfidTag",
                table: "TransportAllocations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeatNumber",
                table: "TransportAllocations",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Conductors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    LicenseNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EmergencyContact = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PoliceVerified = table.Column<bool>(type: "bit", nullable: false),
                    BackgroundVerified = table.Column<bool>(type: "bit", nullable: false),
                    DocumentsUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conductors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conductors_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Conductors_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FuelLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VendorName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FuelQuantity = table.Column<double>(type: "float", nullable: false),
                    CostPerUnit = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    OdometerReading = table.Column<double>(type: "float", nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuelLogs_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FuelLogs_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TransportGateLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DriverName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Purpose = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExitTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportGateLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportGateLogs_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TransportInventories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    InstallationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportInventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportInventories_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TransportStops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StopName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    ArrivalTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    DepartureTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    DistanceFromSource = table.Column<double>(type: "float", nullable: false),
                    GoogleMapsLink = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Landmark = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportStops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportStops_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VehicleIncidents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    IncidentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ClaimNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ClaimAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    RepairCost = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PoliceReportFileUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleIncidents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleIncidents_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VehicleIncidents_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VehicleMaintenances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    MaintenanceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ServiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Odometer = table.Column<double>(type: "float", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    VendorName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Details = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NextServiceDue = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleMaintenances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleMaintenances_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VehicleMaintenances_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RouteAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    DriverId = table.Column<int>(type: "int", nullable: false),
                    ConductorId = table.Column<int>(type: "int", nullable: true),
                    AcademicYearId = table.Column<int>(type: "int", nullable: false),
                    BackupVehicleId = table.Column<int>(type: "int", nullable: true),
                    BackupDriverId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteAssignments_AcademicYears_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYears",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RouteAssignments_Conductors_ConductorId",
                        column: x => x.ConductorId,
                        principalTable: "Conductors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RouteAssignments_Employees_BackupDriverId",
                        column: x => x.BackupDriverId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RouteAssignments_Employees_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RouteAssignments_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RouteAssignments_TransportRoutes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "TransportRoutes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RouteAssignments_Vehicles_BackupVehicleId",
                        column: x => x.BackupVehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RouteAssignments_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TransportTrips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    DriverId = table.Column<int>(type: "int", nullable: false),
                    ConductorId = table.Column<int>(type: "int", nullable: true),
                    TripDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduledStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DelayMinutes = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CancellationReason = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DriverNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportTrips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportTrips_Conductors_ConductorId",
                        column: x => x.ConductorId,
                        principalTable: "Conductors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransportTrips_Employees_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransportTrips_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransportTrips_TransportRoutes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "TransportRoutes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransportTrips_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RouteStopMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    StopId = table.Column<int>(type: "int", nullable: false),
                    SequenceOrder = table.Column<int>(type: "int", nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteStopMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteStopMappings_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RouteStopMappings_TransportRoutes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "TransportRoutes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RouteStopMappings_TransportStops_StopId",
                        column: x => x.StopId,
                        principalTable: "TransportStops",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RfidScanLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    RfidTag = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ScanTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScanType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RfidScanLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RfidScanLogs_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RfidScanLogs_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RfidScanLogs_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RfidScanLogs_TransportTrips_TripId",
                        column: x => x.TripId,
                        principalTable: "TransportTrips",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransportAllocations_AcademicYearId",
                table: "TransportAllocations",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportAllocations_DropStopId",
                table: "TransportAllocations",
                column: "DropStopId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportAllocations_EmployeeId",
                table: "TransportAllocations",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportAllocations_PickupStopId",
                table: "TransportAllocations",
                column: "PickupStopId");

            migrationBuilder.CreateIndex(
                name: "IX_Conductors_EmployeeId",
                table: "Conductors",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Conductors_SchoolRegistrationId",
                table: "Conductors",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelLogs_SchoolRegistrationId",
                table: "FuelLogs",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelLogs_VehicleId",
                table: "FuelLogs",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_RfidScanLogs_EmployeeId",
                table: "RfidScanLogs",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_RfidScanLogs_SchoolRegistrationId",
                table: "RfidScanLogs",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_RfidScanLogs_StudentId",
                table: "RfidScanLogs",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_RfidScanLogs_TripId",
                table: "RfidScanLogs",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteAssignments_AcademicYearId",
                table: "RouteAssignments",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteAssignments_BackupDriverId",
                table: "RouteAssignments",
                column: "BackupDriverId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteAssignments_BackupVehicleId",
                table: "RouteAssignments",
                column: "BackupVehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteAssignments_ConductorId",
                table: "RouteAssignments",
                column: "ConductorId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteAssignments_DriverId",
                table: "RouteAssignments",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteAssignments_RouteId",
                table: "RouteAssignments",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteAssignments_SchoolRegistrationId",
                table: "RouteAssignments",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteAssignments_VehicleId",
                table: "RouteAssignments",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteStopMappings_RouteId",
                table: "RouteStopMappings",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteStopMappings_SchoolRegistrationId",
                table: "RouteStopMappings",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteStopMappings_StopId",
                table: "RouteStopMappings",
                column: "StopId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportGateLogs_SchoolRegistrationId",
                table: "TransportGateLogs",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportInventories_SchoolRegistrationId",
                table: "TransportInventories",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportStops_SchoolRegistrationId",
                table: "TransportStops",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportTrips_ConductorId",
                table: "TransportTrips",
                column: "ConductorId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportTrips_DriverId",
                table: "TransportTrips",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportTrips_RouteId",
                table: "TransportTrips",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportTrips_SchoolRegistrationId",
                table: "TransportTrips",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportTrips_VehicleId",
                table: "TransportTrips",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleIncidents_SchoolRegistrationId",
                table: "VehicleIncidents",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleIncidents_VehicleId",
                table: "VehicleIncidents",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleMaintenances_SchoolRegistrationId",
                table: "VehicleMaintenances",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleMaintenances_VehicleId",
                table: "VehicleMaintenances",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransportAllocations_AcademicYears_AcademicYearId",
                table: "TransportAllocations",
                column: "AcademicYearId",
                principalTable: "AcademicYears",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransportAllocations_Employees_EmployeeId",
                table: "TransportAllocations",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransportAllocations_TransportStops_DropStopId",
                table: "TransportAllocations",
                column: "DropStopId",
                principalTable: "TransportStops",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransportAllocations_TransportStops_PickupStopId",
                table: "TransportAllocations",
                column: "PickupStopId",
                principalTable: "TransportStops",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransportAllocations_AcademicYears_AcademicYearId",
                table: "TransportAllocations");

            migrationBuilder.DropForeignKey(
                name: "FK_TransportAllocations_Employees_EmployeeId",
                table: "TransportAllocations");

            migrationBuilder.DropForeignKey(
                name: "FK_TransportAllocations_TransportStops_DropStopId",
                table: "TransportAllocations");

            migrationBuilder.DropForeignKey(
                name: "FK_TransportAllocations_TransportStops_PickupStopId",
                table: "TransportAllocations");

            migrationBuilder.DropTable(
                name: "FuelLogs");

            migrationBuilder.DropTable(
                name: "RfidScanLogs");

            migrationBuilder.DropTable(
                name: "RouteAssignments");

            migrationBuilder.DropTable(
                name: "RouteStopMappings");

            migrationBuilder.DropTable(
                name: "TransportGateLogs");

            migrationBuilder.DropTable(
                name: "TransportInventories");

            migrationBuilder.DropTable(
                name: "VehicleIncidents");

            migrationBuilder.DropTable(
                name: "VehicleMaintenances");

            migrationBuilder.DropTable(
                name: "TransportTrips");

            migrationBuilder.DropTable(
                name: "TransportStops");

            migrationBuilder.DropTable(
                name: "Conductors");

            migrationBuilder.DropIndex(
                name: "IX_TransportAllocations_AcademicYearId",
                table: "TransportAllocations");

            migrationBuilder.DropIndex(
                name: "IX_TransportAllocations_DropStopId",
                table: "TransportAllocations");

            migrationBuilder.DropIndex(
                name: "IX_TransportAllocations_EmployeeId",
                table: "TransportAllocations");

            migrationBuilder.DropIndex(
                name: "IX_TransportAllocations_PickupStopId",
                table: "TransportAllocations");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "ChassisNumber",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "CurrentOdometer",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "DocumentsUrl",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "EngineNumber",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "FitnessCertificate",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "FitnessExpiry",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "FuelTankCapacity",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "FuelType",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "GpsDeviceNumber",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "InsuranceExpiry",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "InsuranceNumber",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "InsuranceUploadUrl",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Mileage",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "PermitExpiry",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "PermitNumber",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "PucCertificate",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "PucExpiry",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "QrCode",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "RcUploadUrl",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "RfidReaderId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "VehicleType",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Destination",
                table: "TransportRoutes");

            migrationBuilder.DropColumn(
                name: "DistanceKm",
                table: "TransportRoutes");

            migrationBuilder.DropColumn(
                name: "EstimatedTimeMinutes",
                table: "TransportRoutes");

            migrationBuilder.DropColumn(
                name: "MaximumCapacity",
                table: "TransportRoutes");

            migrationBuilder.DropColumn(
                name: "RouteCode",
                table: "TransportRoutes");

            migrationBuilder.DropColumn(
                name: "RouteColor",
                table: "TransportRoutes");

            migrationBuilder.DropColumn(
                name: "RouteMapPath",
                table: "TransportRoutes");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "TransportRoutes");

            migrationBuilder.DropColumn(
                name: "AcademicYearId",
                table: "TransportAllocations");

            migrationBuilder.DropColumn(
                name: "DropStopId",
                table: "TransportAllocations");

            migrationBuilder.DropColumn(
                name: "DropTime",
                table: "TransportAllocations");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "TransportAllocations");

            migrationBuilder.DropColumn(
                name: "IsSuspended",
                table: "TransportAllocations");

            migrationBuilder.DropColumn(
                name: "PickupStopId",
                table: "TransportAllocations");

            migrationBuilder.DropColumn(
                name: "PickupTime",
                table: "TransportAllocations");

            migrationBuilder.DropColumn(
                name: "QrCode",
                table: "TransportAllocations");

            migrationBuilder.DropColumn(
                name: "RfidTag",
                table: "TransportAllocations");

            migrationBuilder.DropColumn(
                name: "SeatNumber",
                table: "TransportAllocations");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Vehicles",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "RegistrationNumber",
                table: "Vehicles",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "TransportAllocations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "TransportAllocations",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);
        }
    }
}
