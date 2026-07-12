using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLibraryAndTransportModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Library");

            migrationBuilder.EnsureSchema(
                name: "Transport");

            migrationBuilder.RenameTable(
                name: "Vehicles",
                newName: "Vehicles",
                newSchema: "Transport");

            migrationBuilder.RenameTable(
                name: "VehicleMaintenances",
                newName: "VehicleMaintenances",
                newSchema: "Transport");

            migrationBuilder.RenameTable(
                name: "VehicleIncidents",
                newName: "VehicleIncidents",
                newSchema: "Transport");

            migrationBuilder.RenameTable(
                name: "TransportTrips",
                newName: "TransportTrips",
                newSchema: "Transport");

            migrationBuilder.RenameTable(
                name: "TransportStops",
                newName: "TransportStops",
                newSchema: "Transport");

            migrationBuilder.RenameTable(
                name: "TransportRoutes",
                newName: "TransportRoutes",
                newSchema: "Transport");

            migrationBuilder.RenameTable(
                name: "TransportInventories",
                newName: "TransportInventories",
                newSchema: "Transport");

            migrationBuilder.RenameTable(
                name: "TransportGateLogs",
                newName: "TransportGateLogs",
                newSchema: "Transport");

            migrationBuilder.RenameTable(
                name: "TransportAllocations",
                newName: "TransportAllocations",
                newSchema: "Transport");

            migrationBuilder.RenameTable(
                name: "RouteStopMappings",
                newName: "RouteStopMappings",
                newSchema: "Transport");

            migrationBuilder.RenameTable(
                name: "RouteAssignments",
                newName: "RouteAssignments",
                newSchema: "Transport");

            migrationBuilder.RenameTable(
                name: "RfidScanLogs",
                newName: "RfidScanLogs",
                newSchema: "Transport");

            migrationBuilder.RenameTable(
                name: "LibraryMembers",
                newName: "LibraryMembers",
                newSchema: "Library");

            migrationBuilder.RenameTable(
                name: "LibraryFineRules",
                newName: "LibraryFineRules",
                newSchema: "Library");

            migrationBuilder.RenameTable(
                name: "LibraryBooks",
                newName: "LibraryBooks",
                newSchema: "Library");

            migrationBuilder.RenameTable(
                name: "FuelLogs",
                newName: "FuelLogs",
                newSchema: "Transport");

            migrationBuilder.RenameTable(
                name: "DigitalResources",
                newName: "DigitalResources",
                newSchema: "Library");

            migrationBuilder.RenameTable(
                name: "Conductors",
                newName: "Conductors",
                newSchema: "Transport");

            migrationBuilder.RenameTable(
                name: "BookVendors",
                newName: "BookVendors",
                newSchema: "Library");

            migrationBuilder.RenameTable(
                name: "BookReservations",
                newName: "BookReservations",
                newSchema: "Library");

            migrationBuilder.RenameTable(
                name: "BookPublishers",
                newName: "BookPublishers",
                newSchema: "Library");

            migrationBuilder.RenameTable(
                name: "BookIssueLogs",
                newName: "BookIssueLogs",
                newSchema: "Library");

            migrationBuilder.RenameTable(
                name: "BookCategories",
                newName: "BookCategories",
                newSchema: "Library");

            migrationBuilder.RenameTable(
                name: "BookAuthors",
                newName: "BookAuthors",
                newSchema: "Library");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Vehicles",
                schema: "Transport",
                newName: "Vehicles");

            migrationBuilder.RenameTable(
                name: "VehicleMaintenances",
                schema: "Transport",
                newName: "VehicleMaintenances");

            migrationBuilder.RenameTable(
                name: "VehicleIncidents",
                schema: "Transport",
                newName: "VehicleIncidents");

            migrationBuilder.RenameTable(
                name: "TransportTrips",
                schema: "Transport",
                newName: "TransportTrips");

            migrationBuilder.RenameTable(
                name: "TransportStops",
                schema: "Transport",
                newName: "TransportStops");

            migrationBuilder.RenameTable(
                name: "TransportRoutes",
                schema: "Transport",
                newName: "TransportRoutes");

            migrationBuilder.RenameTable(
                name: "TransportInventories",
                schema: "Transport",
                newName: "TransportInventories");

            migrationBuilder.RenameTable(
                name: "TransportGateLogs",
                schema: "Transport",
                newName: "TransportGateLogs");

            migrationBuilder.RenameTable(
                name: "TransportAllocations",
                schema: "Transport",
                newName: "TransportAllocations");

            migrationBuilder.RenameTable(
                name: "RouteStopMappings",
                schema: "Transport",
                newName: "RouteStopMappings");

            migrationBuilder.RenameTable(
                name: "RouteAssignments",
                schema: "Transport",
                newName: "RouteAssignments");

            migrationBuilder.RenameTable(
                name: "RfidScanLogs",
                schema: "Transport",
                newName: "RfidScanLogs");

            migrationBuilder.RenameTable(
                name: "LibraryMembers",
                schema: "Library",
                newName: "LibraryMembers");

            migrationBuilder.RenameTable(
                name: "LibraryFineRules",
                schema: "Library",
                newName: "LibraryFineRules");

            migrationBuilder.RenameTable(
                name: "LibraryBooks",
                schema: "Library",
                newName: "LibraryBooks");

            migrationBuilder.RenameTable(
                name: "FuelLogs",
                schema: "Transport",
                newName: "FuelLogs");

            migrationBuilder.RenameTable(
                name: "DigitalResources",
                schema: "Library",
                newName: "DigitalResources");

            migrationBuilder.RenameTable(
                name: "Conductors",
                schema: "Transport",
                newName: "Conductors");

            migrationBuilder.RenameTable(
                name: "BookVendors",
                schema: "Library",
                newName: "BookVendors");

            migrationBuilder.RenameTable(
                name: "BookReservations",
                schema: "Library",
                newName: "BookReservations");

            migrationBuilder.RenameTable(
                name: "BookPublishers",
                schema: "Library",
                newName: "BookPublishers");

            migrationBuilder.RenameTable(
                name: "BookIssueLogs",
                schema: "Library",
                newName: "BookIssueLogs");

            migrationBuilder.RenameTable(
                name: "BookCategories",
                schema: "Library",
                newName: "BookCategories");

            migrationBuilder.RenameTable(
                name: "BookAuthors",
                schema: "Library",
                newName: "BookAuthors");
        }
    }
}
