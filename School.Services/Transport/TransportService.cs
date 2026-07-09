using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using School.Domain.Transport;
using School.Infrastructure.Repositories.IRepositories;
using School_DTOs.Transport;
using School.Services.Interfaces;

namespace School.Services.Transport
{
    #nullable disable

    public class TransportService : ITransportService
    {
        private readonly IVehicleRepository _vehicleRepo;
        private readonly ITransportRouteRepository _routeRepo;
        private readonly ITransportAllocationRepository _allocationRepo;
        private readonly IStudentRepository _studentRepo;

        public TransportService(
            IVehicleRepository vehicleRepo,
            ITransportRouteRepository routeRepo,
            ITransportAllocationRepository allocationRepo,
            IStudentRepository studentRepo)
        {
            _vehicleRepo = vehicleRepo;
            _routeRepo = routeRepo;
            _allocationRepo = allocationRepo;
            _studentRepo = studentRepo;
        }

        // ══════════════════════════════════════════════════════════════════════════
        // VEHICLES
        // ══════════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<VehicleDto>> GetVehiclesAsync(int schoolId)
        {
            var list = await _vehicleRepo.GetAllBySchoolAsync(schoolId);
            return list.Select(x => new VehicleDto
            {
                Id = x.Id,
                Name = x.Name,
                RegistrationNumber = x.RegistrationNumber,
                DriverName = x.DriverName,
                DriverPhone = x.DriverPhone,
                Capacity = x.Capacity,
                Status = x.Status
            });
        }

        public async Task<VehicleDto?> GetVehicleByIdAsync(int id, int schoolId)
        {
            var x = await _vehicleRepo.GetByIdAsync(id);
            if (x == null || x.SchoolRegistrationId != schoolId) return null;
            return new VehicleDto
            {
                Id = x.Id,
                Name = x.Name,
                RegistrationNumber = x.RegistrationNumber,
                DriverName = x.DriverName,
                DriverPhone = x.DriverPhone,
                Capacity = x.Capacity,
                Status = x.Status
            };
        }

        public async Task<VehicleDto> CreateVehicleAsync(CreateVehicleDto dto, int schoolId, string userId)
        {
            var entity = new Vehicle
            {
                Name = dto.Name,
                RegistrationNumber = dto.RegistrationNumber,
                DriverName = dto.DriverName,
                DriverPhone = dto.DriverPhone,
                Capacity = dto.Capacity,
                Status = "active",
                SchoolRegistrationId = schoolId,
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow
            };
            await _vehicleRepo.AddAsync(entity);
            return new VehicleDto
            {
                Id = entity.Id,
                Name = entity.Name,
                RegistrationNumber = entity.RegistrationNumber,
                DriverName = entity.DriverName,
                DriverPhone = entity.DriverPhone,
                Capacity = entity.Capacity,
                Status = entity.Status
            };
        }

        public async Task<bool> UpdateVehicleAsync(int id, CreateVehicleDto dto, int schoolId, string userId)
        {
            var x = await _vehicleRepo.GetByIdAsync(id);
            if (x == null || x.SchoolRegistrationId != schoolId) return false;

            x.Name = dto.Name;
            x.RegistrationNumber = dto.RegistrationNumber;
            x.DriverName = dto.DriverName;
            x.DriverPhone = dto.DriverPhone;
            x.Capacity = dto.Capacity;
            x.UpdatedBy = userId;
            x.UpdatedDate = DateTime.UtcNow;

            await _vehicleRepo.UpdateAsync(x);
            return true;
        }

        public async Task<bool> DeleteVehicleAsync(int id, int schoolId)
        {
            var x = await _vehicleRepo.GetByIdAsync(id);
            if (x == null || x.SchoolRegistrationId != schoolId) return false;
            await _vehicleRepo.DeleteAsync(id);
            return true;
        }

        // ══════════════════════════════════════════════════════════════════════════
        // ROUTES
        // ══════════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<TransportRouteDto>> GetRoutesAsync(int schoolId)
        {
            var routes = await _routeRepo.GetAllBySchoolAsync(schoolId);
            var allocations = await _allocationRepo.GetAllBySchoolAsync(schoolId);

            var list = new List<TransportRouteDto>();
            foreach (var r in routes)
            {
                var occupied = allocations.Count(a => a.TransportRouteId == r.Id && a.Status == "Active");
                list.Add(new TransportRouteDto
                {
                    Id = r.Id,
                    RouteName = r.RouteName,
                    Description = r.Description,
                    VehicleId = r.VehicleId,
                    VehicleName = r.Vehicle?.Name ?? "No Bus Assigned",
                    DriverName = r.Vehicle?.DriverName ?? "N/A",
                    Capacity = r.Vehicle?.Capacity ?? 0,
                    OccupiedSeats = occupied,
                    Status = r.Status
                });
            }
            return list;
        }

        public async Task<TransportRouteDto?> GetRouteByIdAsync(int id, int schoolId)
        {
            var r = await _routeRepo.GetByIdAsync(id);
            if (r == null || r.SchoolRegistrationId != schoolId) return null;

            var allocations = await _allocationRepo.GetAllBySchoolAsync(schoolId);
            var occupied = allocations.Count(a => a.TransportRouteId == r.Id && a.Status == "Active");

            return new TransportRouteDto
            {
                Id = r.Id,
                RouteName = r.RouteName,
                Description = r.Description,
                VehicleId = r.VehicleId,
                VehicleName = r.Vehicle?.Name ?? "No Bus Assigned",
                DriverName = r.Vehicle?.DriverName ?? "N/A",
                Capacity = r.Vehicle?.Capacity ?? 0,
                OccupiedSeats = occupied,
                Status = r.Status
            };
        }

        public async Task<TransportRouteDto> CreateRouteAsync(CreateTransportRouteDto dto, int schoolId, string userId)
        {
            var entity = new TransportRoute
            {
                RouteName = dto.RouteName,
                Description = dto.Description,
                VehicleId = dto.VehicleId,
                Status = "active",
                SchoolRegistrationId = schoolId,
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow
            };
            await _routeRepo.AddAsync(entity);

            var vehicle = await _vehicleRepo.GetByIdAsync(dto.VehicleId);
            return new TransportRouteDto
            {
                Id = entity.Id,
                RouteName = entity.RouteName,
                Description = entity.Description,
                VehicleId = entity.VehicleId,
                VehicleName = vehicle?.Name ?? "No Bus Assigned",
                DriverName = vehicle?.DriverName ?? "N/A",
                Capacity = vehicle?.Capacity ?? 0,
                OccupiedSeats = 0,
                Status = entity.Status
            };
        }

        public async Task<bool> UpdateRouteAsync(int id, CreateTransportRouteDto dto, int schoolId, string userId)
        {
            var x = await _routeRepo.GetByIdAsync(id);
            if (x == null || x.SchoolRegistrationId != schoolId) return false;

            x.RouteName = dto.RouteName;
            x.Description = dto.Description;
            x.VehicleId = dto.VehicleId;
            x.UpdatedBy = userId;
            x.UpdatedDate = DateTime.UtcNow;

            await _routeRepo.UpdateAsync(x);
            return true;
        }

        public async Task<bool> DeleteRouteAsync(int id, int schoolId)
        {
            var x = await _routeRepo.GetByIdAsync(id);
            if (x == null || x.SchoolRegistrationId != schoolId) return false;
            await _routeRepo.DeleteAsync(id);
            return true;
        }

        // ══════════════════════════════════════════════════════════════════════════
        // ALLOCATIONS
        // ══════════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<TransportAllocationDto>> GetAllocationsAsync(int schoolId)
        {
            var list = await _allocationRepo.GetAllBySchoolAsync(schoolId);
            return list.Select(x => new TransportAllocationDto
            {
                Id = x.Id,
                StudentId = x.StudentId,
                StudentName = x.Student?.Name ?? "N/A",
                AdmissionNumber = x.Student?.StudentId ?? "N/A",
                ClassName = x.Student?.Class?.Name ?? "N/A",
                TransportRouteId = x.TransportRouteId,
                RouteName = x.TransportRoute?.RouteName ?? "N/A",
                VehicleName = x.TransportRoute?.Vehicle?.Name ?? "N/A",
                DriverName = x.TransportRoute?.Vehicle?.DriverName ?? "N/A",
                DriverPhone = x.TransportRoute?.Vehicle?.DriverPhone ?? "N/A",
                MonthlyCharge = x.MonthlyCharge,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Status = x.Status
            });
        }

        public async Task<IEnumerable<TransportAllocationDto>> GetStudentAllocationsAsync(int studentId, int schoolId)
        {
            var list = await _allocationRepo.GetByStudentAsync(studentId, schoolId);
            return list.Select(x => new TransportAllocationDto
            {
                Id = x.Id,
                StudentId = x.StudentId,
                StudentName = "",
                AdmissionNumber = "",
                ClassName = "",
                TransportRouteId = x.TransportRouteId,
                RouteName = x.TransportRoute?.RouteName ?? "N/A",
                VehicleName = x.TransportRoute?.Vehicle?.Name ?? "N/A",
                DriverName = x.TransportRoute?.Vehicle?.DriverName ?? "N/A",
                DriverPhone = x.TransportRoute?.Vehicle?.DriverPhone ?? "N/A",
                MonthlyCharge = x.MonthlyCharge,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Status = x.Status
            });
        }

        public async Task<TransportAllocationDto> AllocateStudentAsync(CreateTransportAllocationDto dto, int schoolId, string userId)
        {
            // Validate Route and Seating Capacity
            var route = await _routeRepo.GetByIdAsync(dto.TransportRouteId);
            if (route == null || route.SchoolRegistrationId != schoolId)
            {
                throw new KeyNotFoundException("Specified transport route not found.");
            }

            var vehicle = route.Vehicle;
            if (vehicle != null)
            {
                var allocations = await _allocationRepo.GetAllBySchoolAsync(schoolId);
                var activeCount = allocations.Count(a => a.TransportRouteId == dto.TransportRouteId && a.Status == "Active");
                if (activeCount >= vehicle.Capacity)
                {
                    throw new InvalidOperationException($"Vehicle is at maximum capacity ({vehicle.Capacity} seats). Allocation rejected.");
                }
            }

            var student = await _studentRepo.GetStudentByIdAsync(dto.StudentId);
            if (student == null)
            {
                throw new KeyNotFoundException("Student record not found.");
            }

            var entity = new TransportAllocation
            {
                StudentId = dto.StudentId,
                TransportRouteId = dto.TransportRouteId,
                MonthlyCharge = dto.MonthlyCharge,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = "Active",
                SchoolRegistrationId = schoolId,
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow
            };

            await _allocationRepo.AddAsync(entity);

            // Re-fetch populated allocation for DTO mapping
            var fresh = await _allocationRepo.GetByIdAsync(entity.Id);
            return new TransportAllocationDto
            {
                Id = fresh.Id,
                StudentId = fresh.StudentId,
                StudentName = fresh.Student?.Name ?? "N/A",
                AdmissionNumber = fresh.Student?.StudentId ?? "N/A",
                ClassName = fresh.Student?.Class?.Name ?? "N/A",
                TransportRouteId = fresh.TransportRouteId,
                RouteName = fresh.TransportRoute?.RouteName ?? "N/A",
                VehicleName = fresh.TransportRoute?.Vehicle?.Name ?? "N/A",
                DriverName = fresh.TransportRoute?.Vehicle?.DriverName ?? "N/A",
                DriverPhone = fresh.TransportRoute?.Vehicle?.DriverPhone ?? "N/A",
                MonthlyCharge = fresh.MonthlyCharge,
                StartDate = fresh.StartDate,
                EndDate = fresh.EndDate,
                Status = fresh.Status
            };
        }

        public async Task<bool> UpdateAllocationStatusAsync(int id, string status, int schoolId, string userId)
        {
            var x = await _allocationRepo.GetByIdAsync(id);
            if (x == null || x.SchoolRegistrationId != schoolId) return false;

            x.Status = status;
            x.UpdatedBy = userId;
            x.UpdatedDate = DateTime.UtcNow;

            await _allocationRepo.UpdateAsync(x);
            return true;
        }
    }
}
