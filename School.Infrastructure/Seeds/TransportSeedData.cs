using Microsoft.EntityFrameworkCore;
using School.Domain.Transport;

namespace School.Infrastructure.Seeds
{
    public static class TransportSeedData
    {
        public static async Task SeedAsync(SchoolDbContext context)
        {
            // Ensure a school exists (use Id = 1)
            var school = await context.SchoolRegistrations.FirstOrDefaultAsync(s => s.Id == 1);
            if (school == null) return;

            var academicYear = await context.AcademicYears.FirstOrDefaultAsync(ay => ay.SchoolRegistrationId == school.Id && ay.IsCurrent);
            if (academicYear == null)
            {
                academicYear = await context.AcademicYears.FirstOrDefaultAsync(ay => ay.SchoolRegistrationId == school.Id);
            }
            if (academicYear == null) return;

            // Vehicle (default bus)
            var vehicle = new Vehicle
            {
                Name = "Bus-01",
                RegistrationNumber = "MH12AB1234",
                VehicleType = "Bus",
                Capacity = 50,
                SchoolRegistrationId = school.Id,
                CreatedBy = "seed",
                CreatedDate = DateTime.UtcNow
            };
            await context.Vehicles.AddAsync(vehicle);
            await context.SaveChangesAsync();

            // TransportRoute
            var route = new TransportRoute
            {
                RouteName = "Main Campus Loop",
                RouteCode = "R001",
                DistanceKm = 12.5,
                EstimatedTimeMinutes = 45,
                SchoolRegistrationId = school.Id,
                VehicleId = vehicle.Id,
                CreatedBy = "seed",
                CreatedDate = DateTime.UtcNow
            };
            await context.TransportRoutes.AddAsync(route);
            await context.SaveChangesAsync();

            // Stops
            var stopA = new TransportStop
            {
                StopName = "Gate A",
                Latitude = 19.0760m,
                Longitude = 72.8777m,
                SchoolRegistrationId = school.Id,
                CreatedBy = "seed",
                CreatedDate = DateTime.UtcNow
            };
            var stopB = new TransportStop
            {
                StopName = "Gate B",
                Latitude = 19.0800m,
                Longitude = 72.8800m,
                SchoolRegistrationId = school.Id,
                CreatedBy = "seed",
                CreatedDate = DateTime.UtcNow
            };
            await context.TransportStops.AddRangeAsync(stopA, stopB);
            await context.SaveChangesAsync();

            // RouteStopMapping
            var mappingA = new RouteStopMapping
            {
                RouteId = route.Id,
                StopId = stopA.Id,
                SequenceOrder = 1,
                SchoolRegistrationId = school.Id,
                CreatedBy = "seed",
                CreatedDate = DateTime.UtcNow
            };
            var mappingB = new RouteStopMapping
            {
                RouteId = route.Id,
                StopId = stopB.Id,
                SequenceOrder = 2,
                SchoolRegistrationId = school.Id,
                CreatedBy = "seed",
                CreatedDate = DateTime.UtcNow
            };
            await context.RouteStopMappings.AddRangeAsync(mappingA, mappingB);
            await context.SaveChangesAsync();

            // TransportTrip
            var trip = new TransportTrip
            {
                RouteId = route.Id,
                VehicleId = vehicle.Id,
                DriverId = 1, // adjust if needed
                TripDate = DateTime.UtcNow.Date,
                ScheduledStart = DateTime.UtcNow.Date.AddHours(8),
                SchoolRegistrationId = school.Id,
                CreatedBy = "seed",
                CreatedDate = DateTime.UtcNow
            };
            await context.TransportTrips.AddAsync(trip);
            await context.SaveChangesAsync();

            // Gate Log
            var gateLog = new TransportGateLog
            {
                VehicleNumber = vehicle.RegistrationNumber,
                DriverName = "John Doe",
                Purpose = "Morning Pickup",
                EntryTime = DateTime.UtcNow.AddMinutes(-10),
                Status = "In",
                SchoolRegistrationId = school.Id,
                CreatedBy = "seed",
                CreatedDate = DateTime.UtcNow
            };
            await context.TransportGateLogs.AddAsync(gateLog);

            // Inventory
            var inventory = new TransportInventory
            {
                ItemName = "GPS Device",
                Category = "GPSDevice",
                SerialNumber = "GPS-001",
                InstallationDate = DateTime.UtcNow.AddMonths(-3),
                Status = "Active",
                SchoolRegistrationId = school.Id,
                CreatedBy = "seed",
                CreatedDate = DateTime.UtcNow
            };
            await context.TransportInventories.AddAsync(inventory);

            // Allocation
            var allocation = new TransportAllocation
            {
                EmployeeId = 1,
                TransportRouteId = route.Id,
                PickupStopId = stopA.Id,
                DropStopId = stopB.Id,
                MonthlyCharge = 1500m,
                StartDate = DateTime.UtcNow.Date,
                Status = "Active",
                AcademicYearId = academicYear.Id,
                SchoolRegistrationId = school.Id,
                CreatedBy = "seed",
                CreatedDate = DateTime.UtcNow
            };
            await context.TransportAllocations.AddAsync(allocation);

            // Vehicle Incident
            var incident = new VehicleIncident
            {
                VehicleId = vehicle.Id,
                IncidentDate = DateTime.UtcNow.AddDays(-5),
                Description = "Minor rear‑end collision",
                ClaimNumber = "CLM-2023-001",
                ClaimAmount = 7500m,
                SchoolRegistrationId = school.Id,
                CreatedBy = "seed",
                CreatedDate = DateTime.UtcNow
            };
            await context.VehicleIncidents.AddAsync(incident);

            // Vehicle Maintenance
            var maintenance = new VehicleMaintenance
            {
                VehicleId = vehicle.Id,
                MaintenanceType = "OilChange",
                ServiceDate = DateTime.UtcNow.AddMonths(-1),
                Odometer = 12000,
                Cost = 2500m,
                VendorName = "AutoCare Ltd.",
                SchoolRegistrationId = school.Id,
                CreatedBy = "seed",
                CreatedDate = DateTime.UtcNow
            };
            await context.VehicleMaintenances.AddAsync(maintenance);

            // ---------- Additional realistic entries (10 each) ----------
            // Vehicles
            var vehicles = new List<Vehicle>();
            for (int i = 2; i <= 11; i++)
            {
                vehicles.Add(new Vehicle
                {
                    Name = $"Bus-{i:D2}",
                    RegistrationNumber = $"MH12AB{i:D4}",
                    VehicleType = "Bus",
                    Capacity = 45 + i,
                    SchoolRegistrationId = school.Id,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.UtcNow
                });
            }
            await context.Vehicles.AddRangeAsync(vehicles);
            await context.SaveChangesAsync();

            // TransportRoutes, Stops, Mappings, Trips, GateLogs, Inventories, Allocations, Incidents, Maintenances
            for (int i = 2; i <= 11; i++)
            {
                var routeX = new TransportRoute
                {
                    RouteName = $"Route {i}",
                    RouteCode = $"R{i:D3}",
                    DistanceKm = 10 + i,
                    EstimatedTimeMinutes = 30 + i * 2,
                    SchoolRegistrationId = school.Id,
                    VehicleId = vehicles[i - 2].Id,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.UtcNow
                };
                await context.TransportRoutes.AddAsync(routeX);
                await context.SaveChangesAsync();

                var stopAX = new TransportStop
                {
                    StopName = $"Gate A{i}",
                    Latitude = 19.07m + i * 0.001m,
                    Longitude = 72.87m + i * 0.001m,
                    SchoolRegistrationId = school.Id,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.UtcNow
                };
                var stopBX = new TransportStop
                {
                    StopName = $"Gate B{i}",
                    Latitude = 19.08m + i * 0.001m,
                    Longitude = 72.88m + i * 0.001m,
                    SchoolRegistrationId = school.Id,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.UtcNow
                };
                await context.TransportStops.AddRangeAsync(stopAX, stopBX);
                await context.SaveChangesAsync();

                var mappingAX = new RouteStopMapping
                {
                    RouteId = routeX.Id,
                    StopId = stopAX.Id,
                    SequenceOrder = 1,
                    SchoolRegistrationId = school.Id,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.UtcNow
                };
                var mappingBX = new RouteStopMapping
                {
                    RouteId = routeX.Id,
                    StopId = stopBX.Id,
                    SequenceOrder = 2,
                    SchoolRegistrationId = school.Id,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.UtcNow
                };
                await context.RouteStopMappings.AddRangeAsync(mappingAX, mappingBX);
                await context.SaveChangesAsync();

                var tripX = new TransportTrip
                {
                    RouteId = routeX.Id,
                    VehicleId = vehicles[i - 2].Id,
                    DriverId = 1,
                    TripDate = DateTime.UtcNow.Date,
                    ScheduledStart = DateTime.UtcNow.Date.AddHours(8),
                    SchoolRegistrationId = school.Id,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.UtcNow
                };
                await context.TransportTrips.AddAsync(tripX);

                var gateLogX = new TransportGateLog
                {
                    VehicleNumber = vehicles[i - 2].RegistrationNumber,
                    DriverName = $"Driver {i}",
                    Purpose = "Morning Pickup",
                    EntryTime = DateTime.UtcNow.AddMinutes(-10 - i),
                    Status = "In",
                    SchoolRegistrationId = school.Id,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.UtcNow
                };
                await context.TransportGateLogs.AddAsync(gateLogX);

                var inventoryX = new TransportInventory
                {
                    ItemName = $"GPS Device {i}",
                    Category = "GPSDevice",
                    SerialNumber = $"GPS-{i:D3}",
                    InstallationDate = DateTime.UtcNow.AddMonths(-i),
                    Status = "Active",
                    SchoolRegistrationId = school.Id,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.UtcNow
                };
                await context.TransportInventories.AddAsync(inventoryX);

                var allocationX = new TransportAllocation
                {
                    EmployeeId = 1,
                    TransportRouteId = routeX.Id,
                    PickupStopId = stopAX.Id,
                    DropStopId = stopBX.Id,
                    MonthlyCharge = 1500m + i * 10,
                    StartDate = DateTime.UtcNow.Date,
                    Status = "Active",
                    AcademicYearId = academicYear.Id,
                    SchoolRegistrationId = school.Id,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.UtcNow
                };
                await context.TransportAllocations.AddAsync(allocationX);

                var incidentX = new VehicleIncident
                {
                    VehicleId = vehicles[i - 2].Id,
                    IncidentDate = DateTime.UtcNow.AddDays(-i),
                    Description = $"Minor incident {i}",
                    ClaimNumber = $"CLM-2023-{i:D3}",
                    ClaimAmount = 5000m + i * 100,
                    SchoolRegistrationId = school.Id,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.UtcNow
                };
                await context.VehicleIncidents.AddAsync(incidentX);

                var maintenanceX = new VehicleMaintenance
                {
                    VehicleId = vehicles[i - 2].Id,
                    MaintenanceType = "OilChange",
                    ServiceDate = DateTime.UtcNow.AddMonths(-i),
                    Odometer = 10000 + i * 500,
                    Cost = 2000m + i * 50,
                    VendorName = "AutoCare Ltd.",
                    SchoolRegistrationId = school.Id,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.UtcNow
                };
                await context.VehicleMaintenances.AddAsync(maintenanceX);
            }
            // Final save
            await context.SaveChangesAsync();
        }
    }
}
