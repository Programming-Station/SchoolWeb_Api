using AutoMapper;
using Microsoft.EntityFrameworkCore;
using School.Domain.Transport;
using School.Infrastructure;
using School.Services.Interfaces;
using School_DTOs.Transport;

namespace School.Services.Transport
{
#nullable disable

    public class TransportService : ITransportService
    {
        private readonly SchoolDbContext _db;
        private readonly IMapper _mapper;

        public TransportService(SchoolDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        // ══════════════════════════════════════════════════════════════════════════
        // VEHICLES
        // ══════════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<VehicleDto>> GetVehiclesAsync(int schoolId)
        {
            var list = await _db.Vehicles
                .Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .ToListAsync();
            return _mapper.Map<IEnumerable<VehicleDto>>(list);
        }

        public async Task<VehicleDto?> GetVehicleByIdAsync(int id, int schoolId)
        {
            var x = await _db.Vehicles
                .FirstOrDefaultAsync(v => v.Id == id && v.SchoolRegistrationId == schoolId && !v.IsDeleted);
            return _mapper.Map<VehicleDto?>(x);
        }

        public async Task<VehicleDto> CreateVehicleAsync(CreateVehicleDto dto, int schoolId, string userId)
        {
            var entity = _mapper.Map<Vehicle>(dto);
            entity.SchoolRegistrationId = schoolId;
            entity.CreatedBy = userId;
            entity.CreatedDate = DateTime.UtcNow;
            entity.Status = dto.Status ?? "Active";

            _db.Vehicles.Add(entity);
            await _db.SaveChangesAsync();
            return _mapper.Map<VehicleDto>(entity);
        }

        public async Task<bool> UpdateVehicleAsync(int id, CreateVehicleDto dto, int schoolId, string userId)
        {
            var x = await _db.Vehicles
                .FirstOrDefaultAsync(v => v.Id == id && v.SchoolRegistrationId == schoolId && !v.IsDeleted);
            if (x == null) return false;

            _mapper.Map(dto, x);
            x.UpdatedBy = userId;
            x.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteVehicleAsync(int id, int schoolId)
        {
            var x = await _db.Vehicles
                .FirstOrDefaultAsync(v => v.Id == id && v.SchoolRegistrationId == schoolId && !v.IsDeleted);
            if (x == null) return false;

            x.IsDeleted = true;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreVehicleAsync(int id, int schoolId)
        {
            var x = await _db.Vehicles
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(v => v.Id == id && v.SchoolRegistrationId == schoolId);
            if (x == null) return false;

            x.IsDeleted = false;
            x.UpdatedDate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }

        // ══════════════════════════════════════════════════════════════════════════
        // ROUTES
        // ══════════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<TransportRouteDto>> GetRoutesAsync(int schoolId)
        {
            var routes = await _db.TransportRoutes
                .Include(r => r.Vehicle)
                .Where(r => r.SchoolRegistrationId == schoolId && !r.IsDeleted)
                .ToListAsync();

            var allocations = await _db.TransportAllocations
                .Where(a => a.SchoolRegistrationId == schoolId && a.Status == "Active" && !a.IsDeleted)
                .ToListAsync();

            var list = new List<TransportRouteDto>();
            foreach (var r in routes)
            {
                var occupied = allocations.Count(a => a.TransportRouteId == r.Id);
                var dto = _mapper.Map<TransportRouteDto>(r);
                dto.OccupiedSeats = occupied;
                dto.VehicleName = r.Vehicle?.Name ?? "No Bus Assigned";
                dto.DriverName = r.Vehicle?.DriverName ?? "N/A";
                dto.Capacity = r.Vehicle?.Capacity ?? 0;
                list.Add(dto);
            }
            return list;
        }

        public async Task<TransportRouteDto?> GetRouteByIdAsync(int id, int schoolId)
        {
            var r = await _db.TransportRoutes
                .Include(r => r.Vehicle)
                .FirstOrDefaultAsync(r => r.Id == id && r.SchoolRegistrationId == schoolId && !r.IsDeleted);
            if (r == null) return null;

            var occupied = await _db.TransportAllocations
                .CountAsync(a => a.TransportRouteId == r.Id && a.Status == "Active" && a.SchoolRegistrationId == schoolId && !a.IsDeleted);

            var dto = _mapper.Map<TransportRouteDto>(r);
            dto.OccupiedSeats = occupied;
            dto.VehicleName = r.Vehicle?.Name ?? "No Bus Assigned";
            dto.DriverName = r.Vehicle?.DriverName ?? "N/A";
            dto.Capacity = r.Vehicle?.Capacity ?? 0;
            return dto;
        }

        public async Task<TransportRouteDto> CreateRouteAsync(CreateTransportRouteDto dto, int schoolId, string userId)
        {
            var entity = _mapper.Map<TransportRoute>(dto);
            entity.SchoolRegistrationId = schoolId;
            entity.CreatedBy = userId;
            entity.CreatedDate = DateTime.UtcNow;
            entity.Status = dto.Status ?? "active";

            _db.TransportRoutes.Add(entity);
            await _db.SaveChangesAsync();

            var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.Id == dto.VehicleId && v.SchoolRegistrationId == schoolId);
            var result = _mapper.Map<TransportRouteDto>(entity);
            result.VehicleName = vehicle?.Name ?? "No Bus Assigned";
            result.DriverName = vehicle?.DriverName ?? "N/A";
            result.Capacity = vehicle?.Capacity ?? 0;
            result.OccupiedSeats = 0;
            return result;
        }

        public async Task<bool> UpdateRouteAsync(int id, CreateTransportRouteDto dto, int schoolId, string userId)
        {
            var x = await _db.TransportRoutes
                .FirstOrDefaultAsync(r => r.Id == id && r.SchoolRegistrationId == schoolId && !r.IsDeleted);
            if (x == null) return false;

            _mapper.Map(dto, x);
            x.UpdatedBy = userId;
            x.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRouteAsync(int id, int schoolId)
        {
            var x = await _db.TransportRoutes
                .FirstOrDefaultAsync(r => r.Id == id && r.SchoolRegistrationId == schoolId && !r.IsDeleted);
            if (x == null) return false;

            x.IsDeleted = true;
            await _db.SaveChangesAsync();
            return true;
        }

        // ══════════════════════════════════════════════════════════════════════════
        // STOPS
        // ══════════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<TransportStopDto>> GetStopsAsync(int schoolId)
        {
            var list = await _db.TransportStops
                .Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .ToListAsync();
            return _mapper.Map<IEnumerable<TransportStopDto>>(list);
        }

        public async Task<TransportStopDto?> GetStopByIdAsync(int id, int schoolId)
        {
            var stop = await _db.TransportStops
                .FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            return _mapper.Map<TransportStopDto?>(stop);
        }

        public async Task<TransportStopDto> CreateStopAsync(CreateTransportStopDto dto, int schoolId, string userId)
        {
            var stop = _mapper.Map<TransportStop>(dto);
            stop.SchoolRegistrationId = schoolId;
            stop.CreatedBy = userId;
            stop.CreatedDate = DateTime.UtcNow;
            stop.Status = dto.Status ?? "Active";

            _db.TransportStops.Add(stop);
            await _db.SaveChangesAsync();
            return _mapper.Map<TransportStopDto>(stop);
        }

        public async Task<bool> UpdateStopAsync(int id, CreateTransportStopDto dto, int schoolId, string userId)
        {
            var stop = await _db.TransportStops.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (stop == null) return false;

            _mapper.Map(dto, stop);
            stop.UpdatedBy = userId;
            stop.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteStopAsync(int id, int schoolId)
        {
            var stop = await _db.TransportStops.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (stop == null) return false;

            stop.IsDeleted = true;
            await _db.SaveChangesAsync();
            return true;
        }

        // ══════════════════════════════════════════════════════════════════════════
        // ROUTE STOP MAPPING
        // ══════════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<RouteStopMappingDto>> GetRouteStopsAsync(int routeId, int schoolId)
        {
            var mappings = await _db.RouteStopMappings
                .Include(x => x.Route)
                .Include(x => x.Stop)
                .Where(x => x.RouteId == routeId && x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .OrderBy(x => x.SequenceOrder)
                .ToListAsync();
            return _mapper.Map<IEnumerable<RouteStopMappingDto>>(mappings);
        }

        public async Task<bool> AssignStopToRouteAsync(CreateRouteStopMappingDto dto, int schoolId, string userId)
        {
            var route = await _db.TransportRoutes.FirstOrDefaultAsync(x => x.Id == dto.RouteId && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            var stop = await _db.TransportStops.FirstOrDefaultAsync(x => x.Id == dto.StopId && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (route == null || stop == null) return false;

            var existing = await _db.RouteStopMappings
                .FirstOrDefaultAsync(x => x.RouteId == dto.RouteId && x.StopId == dto.StopId && x.SchoolRegistrationId == schoolId && !x.IsDeleted);

            if (existing != null)
            {
                existing.SequenceOrder = dto.SequenceOrder;
                existing.UpdatedBy = userId;
                existing.UpdatedDate = DateTime.UtcNow;
            }
            else
            {
                var mapping = new RouteStopMapping
                {
                    RouteId = dto.RouteId,
                    StopId = dto.StopId,
                    SequenceOrder = dto.SequenceOrder,
                    SchoolRegistrationId = schoolId,
                    CreatedBy = userId,
                    CreatedDate = DateTime.UtcNow
                };
                _db.RouteStopMappings.Add(mapping);
            }

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveStopFromRouteAsync(int id, int schoolId)
        {
            var mapping = await _db.RouteStopMappings.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (mapping == null) return false;

            _db.RouteStopMappings.Remove(mapping);
            await _db.SaveChangesAsync();
            return true;
        }

        // ══════════════════════════════════════════════════════════════════════════
        // CONDUCTORS
        // ══════════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<ConductorDto>> GetConductorsAsync(int schoolId)
        {
            var conductors = await _db.Conductors
                .Include(c => c.Employee)
                .Where(c => c.SchoolRegistrationId == schoolId && !c.IsDeleted)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ConductorDto>>(conductors);
        }

        public async Task<ConductorDto?> GetConductorByIdAsync(int id, int schoolId)
        {
            var conductor = await _db.Conductors
                .Include(c => c.Employee)
                .FirstOrDefaultAsync(c => c.Id == id && c.SchoolRegistrationId == schoolId && !c.IsDeleted);
            return _mapper.Map<ConductorDto?>(conductor);
        }

        public async Task<ConductorDto> CreateConductorAsync(CreateConductorDto dto, int schoolId, string userId)
        {
            var conductor = _mapper.Map<Conductor>(dto);
            conductor.SchoolRegistrationId = schoolId;
            conductor.CreatedBy = userId;
            conductor.CreatedDate = DateTime.UtcNow;
            conductor.Status = dto.Status ?? "Active";

            _db.Conductors.Add(conductor);
            await _db.SaveChangesAsync();

            var fresh = await _db.Conductors
                .Include(c => c.Employee)
                .FirstAsync(c => c.Id == conductor.Id);
            return _mapper.Map<ConductorDto>(fresh);
        }

        public async Task<bool> UpdateConductorAsync(int id, CreateConductorDto dto, int schoolId, string userId)
        {
            var conductor = await _db.Conductors.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (conductor == null) return false;

            _mapper.Map(dto, conductor);
            conductor.UpdatedBy = userId;
            conductor.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteConductorAsync(int id, int schoolId)
        {
            var conductor = await _db.Conductors.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (conductor == null) return false;

            conductor.IsDeleted = true;
            await _db.SaveChangesAsync();
            return true;
        }

        // ══════════════════════════════════════════════════════════════════════════
        // ROUTE ASSIGNMENTS
        // ══════════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<RouteAssignmentDto>> GetRouteAssignmentsAsync(int schoolId)
        {
            var list = await _db.RouteAssignments
                .Include(ra => ra.Route)
                .Include(ra => ra.Vehicle)
                .Include(ra => ra.Driver)
                .Include(ra => ra.Conductor).ThenInclude(c => c.Employee)
                .Include(ra => ra.AcademicYear)
                .Include(ra => ra.BackupVehicle)
                .Include(ra => ra.BackupDriver)
                .Where(ra => ra.SchoolRegistrationId == schoolId && !ra.IsDeleted)
                .ToListAsync();
            return _mapper.Map<IEnumerable<RouteAssignmentDto>>(list);
        }

        public async Task<RouteAssignmentDto?> GetRouteAssignmentByIdAsync(int id, int schoolId)
        {
            var x = await _db.RouteAssignments
                .Include(ra => ra.Route)
                .Include(ra => ra.Vehicle)
                .Include(ra => ra.Driver)
                .Include(ra => ra.Conductor).ThenInclude(c => c.Employee)
                .Include(ra => ra.AcademicYear)
                .Include(ra => ra.BackupVehicle)
                .Include(ra => ra.BackupDriver)
                .FirstOrDefaultAsync(ra => ra.Id == id && ra.SchoolRegistrationId == schoolId && !ra.IsDeleted);
            return _mapper.Map<RouteAssignmentDto?>(x);
        }

        public async Task<RouteAssignmentDto> CreateRouteAssignmentAsync(CreateRouteAssignmentDto dto, int schoolId, string userId)
        {
            var ra = _mapper.Map<RouteAssignment>(dto);
            ra.SchoolRegistrationId = schoolId;
            ra.CreatedBy = userId;
            ra.CreatedDate = DateTime.UtcNow;
            ra.Status = dto.Status ?? "Active";

            _db.RouteAssignments.Add(ra);
            await _db.SaveChangesAsync();

            var fresh = await _db.RouteAssignments
                .Include(r => r.Route)
                .Include(r => r.Vehicle)
                .Include(r => r.Driver)
                .Include(r => r.Conductor).ThenInclude(c => c.Employee)
                .Include(r => r.AcademicYear)
                .Include(r => r.BackupVehicle)
                .Include(r => r.BackupDriver)
                .FirstAsync(r => r.Id == ra.Id);
            return _mapper.Map<RouteAssignmentDto>(fresh);
        }

        public async Task<bool> UpdateRouteAssignmentAsync(int id, CreateRouteAssignmentDto dto, int schoolId, string userId)
        {
            var ra = await _db.RouteAssignments.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (ra == null) return false;

            _mapper.Map(dto, ra);
            ra.UpdatedBy = userId;
            ra.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRouteAssignmentAsync(int id, int schoolId)
        {
            var ra = await _db.RouteAssignments.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (ra == null) return false;

            ra.IsDeleted = true;
            await _db.SaveChangesAsync();
            return true;
        }

        // ══════════════════════════════════════════════════════════════════════════
        // ALLOCATIONS
        // ══════════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<TransportAllocationDto>> GetAllocationsAsync(int schoolId)
        {
            var list = await _db.TransportAllocations
                .Include(x => x.Student).ThenInclude(s => s.Class)
                .Include(x => x.Employee)
                .Include(x => x.TransportRoute).ThenInclude(r => r.Vehicle)
                .Include(x => x.PickupStop)
                .Include(x => x.DropStop)
                .Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .ToListAsync();
            return _mapper.Map<IEnumerable<TransportAllocationDto>>(list);
        }

        public async Task<IEnumerable<TransportAllocationDto>> GetStudentAllocationsAsync(int studentId, int schoolId)
        {
            var list = await _db.TransportAllocations
                .Include(x => x.Student).ThenInclude(s => s.Class)
                .Include(x => x.TransportRoute).ThenInclude(r => r.Vehicle)
                .Include(x => x.PickupStop)
                .Include(x => x.DropStop)
                .Where(x => x.StudentId == studentId && x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .ToListAsync();
            return _mapper.Map<IEnumerable<TransportAllocationDto>>(list);
        }

        public async Task<IEnumerable<TransportAllocationDto>> GetEmployeeAllocationsAsync(int employeeId, int schoolId)
        {
            var list = await _db.TransportAllocations
                .Include(x => x.Employee)
                .Include(x => x.TransportRoute).ThenInclude(r => r.Vehicle)
                .Include(x => x.PickupStop)
                .Include(x => x.DropStop)
                .Where(x => x.EmployeeId == employeeId && x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .ToListAsync();
            return _mapper.Map<IEnumerable<TransportAllocationDto>>(list);
        }

        public async Task<TransportAllocationDto> AllocateTransportAsync(CreateTransportAllocationDto dto, int schoolId, string userId)
        {
            var route = await _db.TransportRoutes
                .Include(r => r.Vehicle)
                .FirstOrDefaultAsync(r => r.Id == dto.TransportRouteId && r.SchoolRegistrationId == schoolId && !r.IsDeleted);
            if (route == null)
            {
                throw new KeyNotFoundException("Specified transport route not found.");
            }

            var vehicle = route.Vehicle;
            if (vehicle != null)
            {
                var activeCount = await _db.TransportAllocations
                    .CountAsync(a => a.TransportRouteId == dto.TransportRouteId && a.Status == "Active" && a.SchoolRegistrationId == schoolId && !a.IsDeleted);
                if (activeCount >= vehicle.Capacity)
                {
                    throw new InvalidOperationException($"Vehicle is at maximum capacity ({vehicle.Capacity} seats). Allocation rejected.");
                }
            }

            if (dto.StudentId.HasValue)
            {
                var student = await _db.Students.FirstOrDefaultAsync(s => s.Id == dto.StudentId.Value && !s.IsDeleted);
                if (student == null)
                {
                    throw new KeyNotFoundException("Student record not found.");
                }
            }
            else if (dto.EmployeeId.HasValue)
            {
                var employee = await _db.Employees.FirstOrDefaultAsync(e => e.Id == dto.EmployeeId.Value && !e.IsDeleted);
                if (employee == null)
                {
                    throw new KeyNotFoundException("Employee record not found.");
                }
            }
            else
            {
                throw new ArgumentException("Allocation must specify either a StudentId or an EmployeeId.");
            }

            var entity = _mapper.Map<TransportAllocation>(dto);
            entity.Status = "Active";
            entity.SchoolRegistrationId = schoolId;
            entity.CreatedBy = userId;
            entity.CreatedDate = DateTime.UtcNow;

            _db.TransportAllocations.Add(entity);
            await _db.SaveChangesAsync();

            var fresh = await _db.TransportAllocations
                .Include(x => x.Student).ThenInclude(s => s.Class)
                .Include(x => x.Employee)
                .Include(x => x.TransportRoute).ThenInclude(r => r.Vehicle)
                .Include(x => x.PickupStop)
                .Include(x => x.DropStop)
                .FirstAsync(x => x.Id == entity.Id);
            return _mapper.Map<TransportAllocationDto>(fresh);
        }

        public async Task<bool> UpdateAllocationStatusAsync(int id, string status, int schoolId, string userId)
        {
            var x = await _db.TransportAllocations.FirstOrDefaultAsync(a => a.Id == id && a.SchoolRegistrationId == schoolId && !a.IsDeleted);
            if (x == null) return false;

            x.Status = status;
            x.UpdatedBy = userId;
            x.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAllocationAsync(int id, int schoolId)
        {
            var x = await _db.TransportAllocations.FirstOrDefaultAsync(a => a.Id == id && a.SchoolRegistrationId == schoolId && !a.IsDeleted);
            if (x == null) return false;

            x.IsDeleted = true;
            await _db.SaveChangesAsync();
            return true;
        }

        // ══════════════════════════════════════════════════════════════════════════
        // TRIPS & SCHEDULING
        // ══════════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<TransportTripDto>> GetTripsAsync(int schoolId)
        {
            var list = await _db.TransportTrips
                .Include(x => x.Route)
                .Include(x => x.Vehicle)
                .Include(x => x.Driver)
                .Include(x => x.Conductor).ThenInclude(c => c.Employee)
                .Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .ToListAsync();
            return _mapper.Map<IEnumerable<TransportTripDto>>(list);
        }

        public async Task<TransportTripDto?> GetTripByIdAsync(int id, int schoolId)
        {
            var x = await _db.TransportTrips
                .Include(x => x.Route)
                .Include(x => x.Vehicle)
                .Include(x => x.Driver)
                .Include(x => x.Conductor).ThenInclude(c => c.Employee)
                .FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            return _mapper.Map<TransportTripDto?>(x);
        }

        public async Task<TransportTripDto> CreateTripAsync(CreateTransportTripDto dto, int schoolId, string userId)
        {
            var trip = new TransportTrip
            {
                RouteId = dto.RouteId,
                VehicleId = dto.VehicleId,
                DriverId = dto.DriverId,
                ConductorId = dto.ConductorId,
                TripDate = dto.TripDate,
                ScheduledStart = dto.ScheduledStart,
                Status = dto.Status ?? "Scheduled",
                SchoolRegistrationId = schoolId,
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow
            };
            _db.TransportTrips.Add(trip);
            await _db.SaveChangesAsync();

            var fresh = await _db.TransportTrips
                .Include(x => x.Route)
                .Include(x => x.Vehicle)
                .Include(x => x.Driver)
                .Include(x => x.Conductor).ThenInclude(c => c.Employee)
                .FirstAsync(x => x.Id == trip.Id);
            return _mapper.Map<TransportTripDto>(fresh);
        }

        public async Task<bool> StartTripAsync(int tripId, int schoolId, string userId)
        {
            var trip = await _db.TransportTrips.FirstOrDefaultAsync(x => x.Id == tripId && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (trip == null) return false;

            trip.Status = "Running";
            trip.ActualStart = DateTime.UtcNow;
            trip.UpdatedBy = userId;
            trip.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EndTripAsync(int tripId, int schoolId, string userId)
        {
            var trip = await _db.TransportTrips.FirstOrDefaultAsync(x => x.Id == tripId && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (trip == null) return false;

            trip.Status = "Completed";
            trip.ActualEnd = DateTime.UtcNow;
            if (trip.ActualEnd.HasValue && trip.ScheduledStart != default)
            {
                var diff = trip.ActualEnd.Value - trip.ScheduledStart;
                trip.DelayMinutes = diff.TotalMinutes > 0 ? (int)diff.TotalMinutes : 0;
            }
            trip.UpdatedBy = userId;
            trip.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelTripAsync(int tripId, string reason, int schoolId, string userId)
        {
            var trip = await _db.TransportTrips.FirstOrDefaultAsync(x => x.Id == tripId && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (trip == null) return false;

            trip.Status = "Cancelled";
            trip.CancellationReason = reason;
            trip.UpdatedBy = userId;
            trip.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        // ══════════════════════════════════════════════════════════════════════════
        // RFID / SCAN LOGS
        // ══════════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<RfidScanLogDto>> GetScanLogsAsync(int schoolId, int? tripId)
        {
            var query = _db.RfidScanLogs
                .Include(x => x.Trip).ThenInclude(t => t.Route)
                .Include(x => x.Student)
                .Include(x => x.Employee)
                .Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted);

            if (tripId.HasValue)
            {
                query = query.Where(x => x.TripId == tripId.Value);
            }

            var list = await query.OrderByDescending(x => x.ScanTime).ToListAsync();
            return _mapper.Map<IEnumerable<RfidScanLogDto>>(list);
        }

        public async Task<bool> RecordScanAsync(RfidScanLogDto dto, int schoolId, string userId)
        {
            int? studentId = dto.StudentId;
            int? employeeId = dto.EmployeeId;

            if (!studentId.HasValue && !employeeId.HasValue && !string.IsNullOrEmpty(dto.RfidTag))
            {
                var alloc = await _db.TransportAllocations
                    .FirstOrDefaultAsync(x => x.RfidTag == dto.RfidTag && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
                if (alloc != null)
                {
                    studentId = alloc.StudentId;
                    employeeId = alloc.EmployeeId;
                }
            }

            var log = new RfidScanLog
            {
                TripId = dto.TripId,
                StudentId = studentId,
                EmployeeId = employeeId,
                RfidTag = dto.RfidTag ?? string.Empty,
                ScanTime = dto.ScanTime != default ? dto.ScanTime : DateTime.UtcNow,
                ScanType = dto.ScanType ?? "Boarding",
                SchoolRegistrationId = schoolId,
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow
            };

            _db.RfidScanLogs.Add(log);
            await _db.SaveChangesAsync();
            return true;
        }

        // ══════════════════════════════════════════════════════════════════════════
        // FUEL LOGS
        // ══════════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<FuelLogDto>> GetFuelLogsAsync(int schoolId)
        {
            var list = await _db.FuelLogs
                .Include(x => x.Vehicle)
                .Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .ToListAsync();
            return _mapper.Map<IEnumerable<FuelLogDto>>(list);
        }

        public async Task<FuelLogDto> CreateFuelLogAsync(CreateFuelLogDto dto, int schoolId, string userId)
        {
            var fuel = _mapper.Map<FuelLog>(dto);
            fuel.SchoolRegistrationId = schoolId;
            fuel.CreatedBy = userId;
            fuel.CreatedDate = DateTime.UtcNow;

            _db.FuelLogs.Add(fuel);

            var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.Id == dto.VehicleId && v.SchoolRegistrationId == schoolId && !v.IsDeleted);
            if (vehicle != null && dto.OdometerReading > vehicle.CurrentOdometer)
            {
                vehicle.CurrentOdometer = dto.OdometerReading;
            }

            await _db.SaveChangesAsync();

            var fresh = await _db.FuelLogs
                .Include(x => x.Vehicle)
                .FirstAsync(x => x.Id == fuel.Id);
            return _mapper.Map<FuelLogDto>(fresh);
        }

        // ══════════════════════════════════════════════════════════════════════════
        // VEHICLE MAINTENANCE
        // ══════════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<VehicleMaintenanceDto>> GetMaintenancesAsync(int schoolId)
        {
            var list = await _db.VehicleMaintenances
                .Include(x => x.Vehicle)
                .Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .ToListAsync();
            return _mapper.Map<IEnumerable<VehicleMaintenanceDto>>(list);
        }

        public async Task<VehicleMaintenanceDto> CreateMaintenanceAsync(CreateVehicleMaintenanceDto dto, int schoolId, string userId)
        {
            var maint = _mapper.Map<VehicleMaintenance>(dto);
            maint.SchoolRegistrationId = schoolId;
            maint.CreatedBy = userId;
            maint.CreatedDate = DateTime.UtcNow;

            _db.VehicleMaintenances.Add(maint);

            var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.Id == dto.VehicleId && v.SchoolRegistrationId == schoolId && !v.IsDeleted);
            if (vehicle != null && dto.Odometer > vehicle.CurrentOdometer)
            {
                vehicle.CurrentOdometer = dto.Odometer;
            }

            await _db.SaveChangesAsync();

            var fresh = await _db.VehicleMaintenances
                .Include(x => x.Vehicle)
                .FirstAsync(x => x.Id == maint.Id);
            return _mapper.Map<VehicleMaintenanceDto>(fresh);
        }

        // ══════════════════════════════════════════════════════════════════════════
        // INCIDENTS
        // ══════════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<VehicleIncidentDto>> GetIncidentsAsync(int schoolId)
        {
            var list = await _db.VehicleIncidents
                .Include(x => x.Vehicle)
                .Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .ToListAsync();
            return _mapper.Map<IEnumerable<VehicleIncidentDto>>(list);
        }

        public async Task<VehicleIncidentDto> RecordIncidentAsync(CreateVehicleIncidentDto dto, int schoolId, string userId)
        {
            var incident = _mapper.Map<VehicleIncident>(dto);
            incident.SchoolRegistrationId = schoolId;
            incident.CreatedBy = userId;
            incident.CreatedDate = DateTime.UtcNow;

            _db.VehicleIncidents.Add(incident);
            await _db.SaveChangesAsync();

            var fresh = await _db.VehicleIncidents
                .Include(x => x.Vehicle)
                .FirstAsync(x => x.Id == incident.Id);
            return _mapper.Map<VehicleIncidentDto>(fresh);
        }

        // ══════════════════════════════════════════════════════════════════════════
        // INVENTORY
        // ══════════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<TransportInventoryDto>> GetInventoryAsync(int schoolId)
        {
            var list = await _db.TransportInventories
                .Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .ToListAsync();
            return _mapper.Map<IEnumerable<TransportInventoryDto>>(list);
        }

        public async Task<TransportInventoryDto> CreateInventoryItemAsync(CreateTransportInventoryDto dto, int schoolId, string userId)
        {
            var inv = _mapper.Map<TransportInventory>(dto);
            inv.SchoolRegistrationId = schoolId;
            inv.CreatedBy = userId;
            inv.CreatedDate = DateTime.UtcNow;
            inv.Status = dto.Status ?? "Active";

            _db.TransportInventories.Add(inv);
            await _db.SaveChangesAsync();
            return _mapper.Map<TransportInventoryDto>(inv);
        }

        // ══════════════════════════════════════════════════════════════════════════
        // GATE LOGS
        // ══════════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<TransportGateLogDto>> GetGateLogsAsync(int schoolId)
        {
            var list = await _db.TransportGateLogs
                .Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .OrderByDescending(x => x.EntryTime)
                .ToListAsync();
            return _mapper.Map<IEnumerable<TransportGateLogDto>>(list);
        }

        public async Task<TransportGateLogDto> RecordGateEntryAsync(CreateTransportGateLogDto dto, int schoolId, string userId)
        {
            var entry = _mapper.Map<TransportGateLog>(dto);
            entry.SchoolRegistrationId = schoolId;
            entry.CreatedBy = userId;
            entry.CreatedDate = DateTime.UtcNow;
            entry.Status = dto.Status ?? "In";

            _db.TransportGateLogs.Add(entry);
            await _db.SaveChangesAsync();
            return _mapper.Map<TransportGateLogDto>(entry);
        }

        public async Task<bool> RecordGateExitAsync(int id, DateTime exitTime, int schoolId, string userId)
        {
            var log = await _db.TransportGateLogs
                .FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (log == null) return false;

            log.ExitTime = exitTime != default ? exitTime : DateTime.UtcNow;
            log.Status = "Out";
            log.UpdatedBy = userId;
            log.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        // ══════════════════════════════════════════════════════════════════════════
        // DASHBOARD METRICS
        // ══════════════════════════════════════════════════════════════════════════
        public async Task<TransportDashboardDto> GetTransportDashboardAsync(int schoolId)
        {
            var totalVehicles = await _db.Vehicles.CountAsync(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            var activeVehicles = await _db.Vehicles.CountAsync(x => x.SchoolRegistrationId == schoolId && x.Status == "Active" && !x.IsDeleted);
            var inactiveVehicles = await _db.Vehicles.CountAsync(x => x.SchoolRegistrationId == schoolId && x.Status == "Inactive" && !x.IsDeleted);
            var maintenanceVehicles = await _db.Vehicles.CountAsync(x => x.SchoolRegistrationId == schoolId && x.Status == "Maintenance" && !x.IsDeleted);

            var today = DateTime.UtcNow.Date;
            var todayTrips = await _db.TransportTrips.CountAsync(x => x.SchoolRegistrationId == schoolId && x.TripDate.Date == today && !x.IsDeleted);
            var completedTrips = await _db.TransportTrips.CountAsync(x => x.SchoolRegistrationId == schoolId && x.TripDate.Date == today && x.Status == "Completed" && !x.IsDeleted);
            var runningTrips = await _db.TransportTrips.CountAsync(x => x.SchoolRegistrationId == schoolId && x.TripDate.Date == today && x.Status == "Running" && !x.IsDeleted);
            var cancelledTrips = await _db.TransportTrips.CountAsync(x => x.SchoolRegistrationId == schoolId && x.TripDate.Date == today && x.Status == "Cancelled" && !x.IsDeleted);

            var totalRoutes = await _db.TransportRoutes.CountAsync(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            var totalStops = await _db.TransportStops.CountAsync(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted);

            var totalDrivers = await _db.Employees
                .Include(e => e.Designation)
                .CountAsync(e => e.SchoolRegistrationId == schoolId && e.Designation != null && e.Designation.Name.Contains("Driver") && !e.IsDeleted);

            if (totalDrivers == 0)
            {
                totalDrivers = await _db.RouteAssignments
                    .Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                    .Select(x => x.DriverId)
                    .Distinct()
                    .CountAsync();
            }

            var totalConductors = await _db.Conductors.CountAsync(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted);

            var studentsAllocated = await _db.TransportAllocations
                .CountAsync(x => x.SchoolRegistrationId == schoolId && x.StudentId.HasValue && x.Status == "Active" && !x.IsDeleted);
            var employeesAllocated = await _db.TransportAllocations
                .CountAsync(x => x.SchoolRegistrationId == schoolId && x.EmployeeId.HasValue && x.Status == "Active" && !x.IsDeleted);

            var fuelCost = await _db.FuelLogs
                .Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .SumAsync(x => (decimal?)x.TotalCost) ?? 0m;

            var maintenanceCost = await _db.VehicleMaintenances
                .Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .SumAsync(x => (decimal?)x.Cost) ?? 0m;

            var activeGpsVehicles = await _db.Vehicles
                .CountAsync(x => x.SchoolRegistrationId == schoolId && !string.IsNullOrEmpty(x.GpsDeviceNumber) && x.Status == "Active" && !x.IsDeleted);
            var activeEmergencyAlerts = await _db.VehicleIncidents
                .CountAsync(x => x.SchoolRegistrationId == schoolId && x.IncidentDate.Date == today && !x.IsDeleted);

            var routes = await _db.TransportRoutes
                .Where(r => r.SchoolRegistrationId == schoolId && !r.IsDeleted)
                .ToListAsync();

            var allocations = await _db.TransportAllocations
                .Where(a => a.SchoolRegistrationId == schoolId && a.Status == "Active" && !a.IsDeleted)
                .ToListAsync();

            var vehicles = await _db.Vehicles
                .Where(v => v.SchoolRegistrationId == schoolId && !v.IsDeleted)
                .ToListAsync();

            var routeStudentLoads = routes.Select(r =>
            {
                var allocs = allocations.Count(a => a.TransportRouteId == r.Id);
                var vehicle = vehicles.FirstOrDefault(v => v.Id == r.VehicleId);
                return new RouteStudentLoadDto
                {
                    RouteName = r.RouteName,
                    StudentCount = allocs,
                    CapacityLimit = vehicle?.Capacity ?? r.MaximumCapacity
                };
            }).ToList();

            var fuelLogs = await _db.FuelLogs
                .Where(f => f.SchoolRegistrationId == schoolId && !f.IsDeleted)
                .ToListAsync();

            var maintLogs = await _db.VehicleMaintenances
                .Where(m => m.SchoolRegistrationId == schoolId && !m.IsDeleted)
                .ToListAsync();

            var vehicleFuelCosts = vehicles.Select(v =>
            {
                var fCost = fuelLogs.Where(f => f.VehicleId == v.Id).Sum(f => (decimal)f.TotalCost);
                var mCost = maintLogs.Where(m => m.VehicleId == v.Id).Sum(m => (decimal)m.Cost);
                return new VehicleFuelCostDto
                {
                    VehicleName = v.Name,
                    FuelCost = fCost,
                    MaintenanceCost = mCost
                };
            }).ToList();

            var monthlyFeeTrends = new List<MonthlyFeeTrendDto>();
            var months = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            var totalAllocatedMonthlyCharge = allocations.Sum(a => a.MonthlyCharge);

            for (int i = 0; i < 6; i++)
            {
                var monthIndex = (DateTime.UtcNow.Month - 5 + i + 12) % 12;
                monthlyFeeTrends.Add(new MonthlyFeeTrendDto
                {
                    Month = months[monthIndex],
                    Collection = totalAllocatedMonthlyCharge * 0.9m,
                    Outstanding = totalAllocatedMonthlyCharge * 0.1m
                });
            }

            return new TransportDashboardDto
            {
                TotalVehicles = totalVehicles,
                ActiveVehicles = activeVehicles,
                InactiveVehicles = inactiveVehicles,
                VehiclesUnderMaintenance = maintenanceVehicles,
                TodayTrips = todayTrips,
                CompletedTrips = completedTrips,
                RunningTrips = runningTrips,
                CancelledTrips = cancelledTrips,
                TotalRoutes = totalRoutes,
                TotalStops = totalStops,
                TotalDrivers = totalDrivers,
                TotalConductors = totalConductors,
                TotalStudentsAllocated = studentsAllocated,
                TotalEmployeesAllocated = employeesAllocated,
                PendingFeeCollection = totalAllocatedMonthlyCharge * 0.15m,
                TodayCollection = totalAllocatedMonthlyCharge * 0.05m,
                TotalFuelCost = fuelCost,
                TotalMaintenanceCost = maintenanceCost,
                ActiveGpsVehicles = activeGpsVehicles,
                ActiveEmergencyAlerts = activeEmergencyAlerts,
                RouteStudentLoads = routeStudentLoads,
                VehicleFuelCosts = vehicleFuelCosts,
                MonthlyFeeTrends = monthlyFeeTrends
            };
        }
    }
}
