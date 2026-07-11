using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using School.Domain.Hostel;
using School.Infrastructure;
using School_DTOs.Hostel;

namespace School.Services.Hostel
{
    #nullable disable

    public class HostelService : IHostelService
    {
        private readonly SchoolDbContext _db;
        private readonly IMapper _mapper;

        public HostelService(SchoolDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        // ════════════════════════════════════════════════════════════════════
        // HOSTELS
        // ════════════════════════════════════════════════════════════════════
        public async Task<PagedResultDto<HostelDto>> GetHostelsAsync(int schoolId, string search, int page, int pageSize, string status)
        {
            var q = _db.Hostels.Where(h => h.SchoolRegistrationId == schoolId && !h.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower().Trim();
                q = q.Where(h => h.Name.ToLower().Contains(s) || h.Code.ToLower().Contains(s) || h.Address.ToLower().Contains(s));
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                q = q.Where(h => h.Status == status);
            }

            var total = await q.CountAsync();
            var items = await q.OrderByDescending(h => h.Id)
                               .Skip((page - 1) * pageSize).Take(pageSize)
                               .ToListAsync();

            return new PagedResultDto<HostelDto>
            {
                Items = _mapper.Map<List<HostelDto>>(items),
                TotalItems = total,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<HostelDto> GetHostelByIdAsync(int id, int schoolId)
        {
            var h = await _db.Hostels.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            return h == null ? null : _mapper.Map<HostelDto>(h);
        }

        public async Task<HostelDto> CreateHostelAsync(CreateHostelDto dto, int schoolId, string userId)
        {
            var h = _mapper.Map<Domain.Hostel.Hostel>(dto);
            h.SchoolRegistrationId = schoolId;
            h.CreatedBy = userId;
            h.CreatedDate = DateTime.UtcNow;

            _db.Hostels.Add(h);
            await _db.SaveChangesAsync();
            return _mapper.Map<HostelDto>(h);
        }

        public async Task<bool> UpdateHostelAsync(int id, CreateHostelDto dto, int schoolId, string userId)
        {
            var h = await _db.Hostels.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (h == null) return false;

            _mapper.Map(dto, h);
            h.UpdatedBy = userId;
            h.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteHostelAsync(int id, int schoolId)
        {
            var h = await _db.Hostels.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (h == null) return false;

            h.IsDeleted = true;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreHostelAsync(int id, int schoolId)
        {
            var h = await _db.Hostels.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && x.IsDeleted);
            if (h == null) return false;

            h.IsDeleted = false;
            await _db.SaveChangesAsync();
            return true;
        }

        // ════════════════════════════════════════════════════════════════════
        // BUILDINGS
        // ════════════════════════════════════════════════════════════════════
        public async Task<PagedResultDto<BuildingDto>> GetBuildingsAsync(int schoolId, string search, int page, int pageSize, string status, int? hostelId)
        {
            var q = _db.Buildings.Include(b => b.Hostel).Where(b => b.SchoolRegistrationId == schoolId && !b.IsDeleted);

            if (hostelId.HasValue && hostelId > 0)
                q = q.Where(b => b.HostelId == hostelId.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower().Trim();
                q = q.Where(b => b.Name.ToLower().Contains(s) || b.Code.ToLower().Contains(s));
            }
            if (!string.IsNullOrWhiteSpace(status))
                q = q.Where(b => b.Status == status);

            var total = await q.CountAsync();
            var items = await q.OrderByDescending(b => b.Id)
                               .Skip((page - 1) * pageSize).Take(pageSize)
                               .ToListAsync();

            return new PagedResultDto<BuildingDto>
            {
                Items = _mapper.Map<List<BuildingDto>>(items),
                TotalItems = total,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<BuildingDto> GetBuildingByIdAsync(int id, int schoolId)
        {
            var b = await _db.Buildings.Include(x => x.Hostel).FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            return b == null ? null : _mapper.Map<BuildingDto>(b);
        }

        public async Task<BuildingDto> CreateBuildingAsync(CreateBuildingDto dto, int schoolId, string userId)
        {
            var b = _mapper.Map<Building>(dto);
            b.SchoolRegistrationId = schoolId;
            b.CreatedBy = userId;
            b.CreatedDate = DateTime.UtcNow;

            _db.Buildings.Add(b);

            // Update Hostel building count
            var h = await _db.Hostels.FindAsync(dto.HostelId);
            if (h != null) h.BuildingCount++;

            await _db.SaveChangesAsync();
            return _mapper.Map<BuildingDto>(b);
        }

        public async Task<bool> UpdateBuildingAsync(int id, CreateBuildingDto dto, int schoolId, string userId)
        {
            var b = await _db.Buildings.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (b == null) return false;

            _mapper.Map(dto, b);
            b.UpdatedBy = userId;
            b.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBuildingAsync(int id, int schoolId)
        {
            var b = await _db.Buildings.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (b == null) return false;

            b.IsDeleted = true;

            var h = await _db.Hostels.FindAsync(b.HostelId);
            if (h != null && h.BuildingCount > 0) h.BuildingCount--;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreBuildingAsync(int id, int schoolId)
        {
            var b = await _db.Buildings.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && x.IsDeleted);
            if (b == null) return false;

            b.IsDeleted = false;

            var h = await _db.Hostels.FindAsync(b.HostelId);
            if (h != null) h.BuildingCount++;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<BuildingDto>> GetAllBuildingsDropdownAsync(int schoolId, int? hostelId)
        {
            var q = _db.Buildings.Where(b => b.SchoolRegistrationId == schoolId && b.Status == "active" && !b.IsDeleted);
            if (hostelId.HasValue && hostelId > 0)
                q = q.Where(b => b.HostelId == hostelId.Value);

            var items = await q.ToListAsync();
            return _mapper.Map<List<BuildingDto>>(items);
        }

        // ════════════════════════════════════════════════════════════════════
        // FLOORS
        // ════════════════════════════════════════════════════════════════════
        public async Task<PagedResultDto<FloorDto>> GetFloorsAsync(int schoolId, int page, int pageSize, int? buildingId)
        {
            var q = _db.Floors.Include(f => f.Building).ThenInclude(b => b.Hostel).Where(f => f.SchoolRegistrationId == schoolId && !f.IsDeleted);

            if (buildingId.HasValue && buildingId > 0)
                q = q.Where(f => f.BuildingId == buildingId.Value);

            var total = await q.CountAsync();
            var items = await q.OrderBy(f => f.FloorNumber)
                               .Skip((page - 1) * pageSize).Take(pageSize)
                               .ToListAsync();

            return new PagedResultDto<FloorDto>
            {
                Items = _mapper.Map<List<FloorDto>>(items),
                TotalItems = total,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<FloorDto> GetFloorByIdAsync(int id, int schoolId)
        {
            var f = await _db.Floors.Include(x => x.Building).FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            return f == null ? null : _mapper.Map<FloorDto>(f);
        }

        public async Task<FloorDto> CreateFloorAsync(CreateFloorDto dto, int schoolId, string userId)
        {
            var f = _mapper.Map<Floor>(dto);
            f.SchoolRegistrationId = schoolId;
            f.CreatedBy = userId;
            f.CreatedDate = DateTime.UtcNow;

            _db.Floors.Add(f);
            await _db.SaveChangesAsync();
            return _mapper.Map<FloorDto>(f);
        }

        public async Task<bool> UpdateFloorAsync(int id, CreateFloorDto dto, int schoolId, string userId)
        {
            var f = await _db.Floors.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (f == null) return false;

            _mapper.Map(dto, f);
            f.UpdatedBy = userId;
            f.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteFloorAsync(int id, int schoolId)
        {
            var f = await _db.Floors.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (f == null) return false;

            f.IsDeleted = true;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<FloorDto>> GetFloorsDropdownAsync(int schoolId, int buildingId)
        {
            var items = await _db.Floors.Where(f => f.BuildingId == buildingId && f.SchoolRegistrationId == schoolId && !f.IsDeleted)
                                       .OrderBy(f => f.FloorNumber)
                                       .ToListAsync();
            return _mapper.Map<List<FloorDto>>(items);
        }

        // ════════════════════════════════════════════════════════════════════
        // ROOM CATEGORIES
        // ════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<RoomCategoryDto>> GetRoomCategoriesAsync(int schoolId)
        {
            var items = await _db.RoomCategories.Where(c => c.SchoolRegistrationId == schoolId && !c.IsDeleted).ToListAsync();
            return _mapper.Map<List<RoomCategoryDto>>(items);
        }

        public async Task<RoomCategoryDto> GetRoomCategoryByIdAsync(int id, int schoolId)
        {
            var c = await _db.RoomCategories.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            return c == null ? null : _mapper.Map<RoomCategoryDto>(c);
        }

        public async Task<RoomCategoryDto> CreateRoomCategoryAsync(CreateRoomCategoryDto dto, int schoolId, string userId)
        {
            var c = _mapper.Map<RoomCategory>(dto);
            c.SchoolRegistrationId = schoolId;
            c.CreatedBy = userId;
            c.CreatedDate = DateTime.UtcNow;

            _db.RoomCategories.Add(c);
            await _db.SaveChangesAsync();
            return _mapper.Map<RoomCategoryDto>(c);
        }

        public async Task<bool> UpdateRoomCategoryAsync(int id, CreateRoomCategoryDto dto, int schoolId, string userId)
        {
            var c = await _db.RoomCategories.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (c == null) return false;

            _mapper.Map(dto, c);
            c.UpdatedBy = userId;
            c.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRoomCategoryAsync(int id, int schoolId)
        {
            var c = await _db.RoomCategories.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (c == null) return false;

            c.IsDeleted = true;
            await _db.SaveChangesAsync();
            return true;
        }

        // ════════════════════════════════════════════════════════════════════
        // ROOMS
        // ════════════════════════════════════════════════════════════════════
        public async Task<PagedResultDto<RoomDto>> GetRoomsAsync(int schoolId, string search, int page, int pageSize, string status, int? hostelId, int? buildingId, int? floorId, int? categoryId)
        {
            var q = _db.Rooms
                .Include(r => r.Hostel)
                .Include(r => r.Building)
                .Include(r => r.Floor)
                .Include(r => r.RoomCategory)
                .Where(r => r.SchoolRegistrationId == schoolId && !r.IsDeleted);

            if (hostelId.HasValue && hostelId > 0) q = q.Where(r => r.HostelId == hostelId.Value);
            if (buildingId.HasValue && buildingId > 0) q = q.Where(r => r.BuildingId == buildingId.Value);
            if (floorId.HasValue && floorId > 0) q = q.Where(r => r.FloorId == floorId.Value);
            if (categoryId.HasValue && categoryId > 0) q = q.Where(r => r.RoomCategoryId == categoryId.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower().Trim();
                q = q.Where(r => r.RoomNumber.ToLower().Contains(s));
            }
            if (!string.IsNullOrWhiteSpace(status))
                q = q.Where(r => r.Status == status);

            var total = await q.CountAsync();
            var items = await q.OrderByDescending(r => r.Id)
                               .Skip((page - 1) * pageSize).Take(pageSize)
                               .ToListAsync();

            return new PagedResultDto<RoomDto>
            {
                Items = _mapper.Map<List<RoomDto>>(items),
                TotalItems = total,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<RoomDto> GetRoomByIdAsync(int id, int schoolId)
        {
            var r = await _db.Rooms
                .Include(x => x.Hostel)
                .Include(x => x.Building)
                .Include(x => x.Floor)
                .Include(x => x.RoomCategory)
                .FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            return r == null ? null : _mapper.Map<RoomDto>(r);
        }

        public async Task<RoomDto> CreateRoomAsync(CreateRoomDto dto, int schoolId, string userId)
        {
            var r = _mapper.Map<Room>(dto);
            r.SchoolRegistrationId = schoolId;
            r.CreatedBy = userId;
            r.CreatedDate = DateTime.UtcNow;
            r.OccupiedBeds = 0;
            r.AvailableBeds = dto.Capacity;

            _db.Rooms.Add(r);
            await _db.SaveChangesAsync();
            return _mapper.Map<RoomDto>(r);
        }

        public async Task<bool> UpdateRoomAsync(int id, CreateRoomDto dto, int schoolId, string userId)
        {
            var r = await _db.Rooms.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (r == null) return false;

            var oldCapacity = r.Capacity;
            _mapper.Map(dto, r);

            // Recalculate beds availability
            r.AvailableBeds = r.Capacity - r.OccupiedBeds;

            r.UpdatedBy = userId;
            r.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRoomAsync(int id, int schoolId)
        {
            var r = await _db.Rooms.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (r == null) return false;

            r.IsDeleted = true;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreRoomAsync(int id, int schoolId)
        {
            var r = await _db.Rooms.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && x.IsDeleted);
            if (r == null) return false;

            r.IsDeleted = false;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RoomDto>> GetRoomsDropdownAsync(int schoolId, int floorId)
        {
            var items = await _db.Rooms.Where(r => r.FloorId == floorId && r.SchoolRegistrationId == schoolId && r.Status == "active" && !r.IsDeleted)
                                       .OrderBy(r => r.RoomNumber)
                                       .ToListAsync();
            return _mapper.Map<List<RoomDto>>(items);
        }

        // ════════════════════════════════════════════════════════════════════
        // BEDS
        // ════════════════════════════════════════════════════════════════════
        public async Task<PagedResultDto<BedDto>> GetBedsAsync(int schoolId, string search, int page, int pageSize, string status, int? roomId)
        {
            var q = _db.Beds.Include(b => b.Room).ThenInclude(r => r.Hostel).Where(b => b.SchoolRegistrationId == schoolId && !b.IsDeleted);

            if (roomId.HasValue && roomId > 0) q = q.Where(b => b.RoomId == roomId.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower().Trim();
                q = q.Where(b => b.BedNumber.ToLower().Contains(s));
            }
            if (!string.IsNullOrWhiteSpace(status))
                q = q.Where(b => b.Status == status);

            var total = await q.CountAsync();
            var items = await q.OrderBy(b => b.BedNumber)
                               .Skip((page - 1) * pageSize).Take(pageSize)
                               .ToListAsync();

            return new PagedResultDto<BedDto>
            {
                Items = _mapper.Map<List<BedDto>>(items),
                TotalItems = total,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<BedDto> GetBedByIdAsync(int id, int schoolId)
        {
            var b = await _db.Beds.Include(x => x.Room).FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            return b == null ? null : _mapper.Map<BedDto>(b);
        }

        public async Task<BedDto> CreateBedAsync(CreateBedDto dto, int schoolId, string userId)
        {
            var b = _mapper.Map<Bed>(dto);
            b.SchoolRegistrationId = schoolId;
            b.CreatedBy = userId;
            b.CreatedDate = DateTime.UtcNow;
            b.QrCode = $"BED-{b.RoomId}-{b.BedNumber}-{Guid.NewGuid().ToString("N")[..6]}";
            b.Barcode = Guid.NewGuid().ToString("N")[..12].ToUpper();
            b.CleaningStatus = "clean";

            _db.Beds.Add(b);
            await _db.SaveChangesAsync();
            return _mapper.Map<BedDto>(b);
        }

        public async Task<bool> UpdateBedAsync(int id, CreateBedDto dto, int schoolId, string userId)
        {
            var b = await _db.Beds.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (b == null) return false;

            _mapper.Map(dto, b);
            b.UpdatedBy = userId;
            b.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBedAsync(int id, int schoolId)
        {
            var b = await _db.Beds.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (b == null) return false;

            b.IsDeleted = true;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreBedAsync(int id, int schoolId)
        {
            var b = await _db.Beds.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && x.IsDeleted);
            if (b == null) return false;

            b.IsDeleted = false;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<BedDto>> GetBedsDropdownAsync(int schoolId, int roomId, string status)
        {
            var q = _db.Beds.Where(b => b.RoomId == roomId && b.SchoolRegistrationId == schoolId && !b.IsDeleted);
            if (!string.IsNullOrWhiteSpace(status))
                q = q.Where(b => b.Status == status);

            var items = await q.OrderBy(b => b.BedNumber).ToListAsync();
            return _mapper.Map<List<BedDto>>(items);
        }

        // ════════════════════════════════════════════════════════════════════
        // WARDENS
        // ════════════════════════════════════════════════════════════════════
        public async Task<PagedResultDto<HostelWardenDto>> GetWardensAsync(int schoolId, string search, int page, int pageSize, string status, int? hostelId)
        {
            var q = _db.HostelWardens
                .Include(w => w.Employee)
                .Include(w => w.Hostel)
                .Where(w => w.SchoolRegistrationId == schoolId && !w.IsDeleted);

            if (hostelId.HasValue && hostelId > 0) q = q.Where(w => w.HostelId == hostelId.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower().Trim();
                q = q.Where(w => w.Employee.FirstName.ToLower().Contains(s) || w.Employee.LastName.ToLower().Contains(s) || w.Employee.EmployeeCode.ToLower().Contains(s));
            }
            if (!string.IsNullOrWhiteSpace(status))
                q = q.Where(w => w.Status == status);

            var total = await q.CountAsync();
            var items = await q.OrderByDescending(w => w.Id)
                               .Skip((page - 1) * pageSize).Take(pageSize)
                               .ToListAsync();

            return new PagedResultDto<HostelWardenDto>
            {
                Items = _mapper.Map<List<HostelWardenDto>>(items),
                TotalItems = total,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<HostelWardenDto> GetWardenByIdAsync(int id, int schoolId)
        {
            var w = await _db.HostelWardens
                .Include(x => x.Employee)
                .Include(x => x.Hostel)
                .FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            return w == null ? null : _mapper.Map<HostelWardenDto>(w);
        }

        public async Task<HostelWardenDto> CreateWardenAsync(CreateHostelWardenDto dto, int schoolId, string userId)
        {
            var w = _mapper.Map<HostelWarden>(dto);
            w.SchoolRegistrationId = schoolId;
            w.CreatedBy = userId;
            w.CreatedDate = DateTime.UtcNow;

            _db.HostelWardens.Add(w);
            await _db.SaveChangesAsync();
            return _mapper.Map<HostelWardenDto>(w);
        }

        public async Task<bool> UpdateWardenAsync(int id, CreateHostelWardenDto dto, int schoolId, string userId)
        {
            var w = await _db.HostelWardens.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (w == null) return false;

            _mapper.Map(dto, w);
            w.UpdatedBy = userId;
            w.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteWardenAsync(int id, int schoolId)
        {
            var w = await _db.HostelWardens.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (w == null) return false;

            w.IsDeleted = true;
            await _db.SaveChangesAsync();
            return true;
        }

        // ════════════════════════════════════════════════════════════════════
        // ADMISSIONS / ALLOCATION
        // ════════════════════════════════════════════════════════════════════
        public async Task<PagedResultDto<HostelAdmissionDto>> GetAdmissionsAsync(int schoolId, string search, int page, int pageSize, string status, int? hostelId, int? roomId)
        {
            var q = _db.HostelAdmissions
                .Include(a => a.Student)
                .Include(a => a.Hostel)
                .Include(a => a.Room)
                .Include(a => a.Bed)
                .Include(a => a.AcademicYear)
                .Where(a => a.SchoolRegistrationId == schoolId && !a.IsDeleted);

            if (hostelId.HasValue && hostelId > 0) q = q.Where(a => a.HostelId == hostelId.Value);
            if (roomId.HasValue && roomId > 0) q = q.Where(a => a.RoomId == roomId.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower().Trim();
                q = q.Where(a => a.Student.Name.ToLower().Contains(s) || a.Student.StudentId.ToLower().Contains(s) || a.AdmissionNumber.ToLower().Contains(s));
            }
            if (!string.IsNullOrWhiteSpace(status))
                q = q.Where(a => a.Status == status);

            var total = await q.CountAsync();
            var items = await q.OrderByDescending(a => a.Id)
                               .Skip((page - 1) * pageSize).Take(pageSize)
                               .ToListAsync();

            return new PagedResultDto<HostelAdmissionDto>
            {
                Items = _mapper.Map<List<HostelAdmissionDto>>(items),
                TotalItems = total,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<HostelAdmissionDto> GetAdmissionByIdAsync(int id, int schoolId)
        {
            var a = await _db.HostelAdmissions
                .Include(x => x.Student)
                .Include(x => x.Hostel)
                .Include(x => x.Room)
                .Include(x => x.Bed)
                .Include(x => x.AcademicYear)
                .FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            return a == null ? null : _mapper.Map<HostelAdmissionDto>(a);
        }

        public async Task<HostelAdmissionDto> CreateAdmissionAsync(CreateHostelAdmissionDto dto, int schoolId, string userId)
        {
            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                // Verify Bed is Vacant
                var bed = await _db.Beds.FirstOrDefaultAsync(b => b.Id == dto.BedId && b.SchoolRegistrationId == schoolId && !b.IsDeleted);
                if (bed == null || bed.Status != "vacant")
                    throw new Exception("Selected Bed is not available!");

                // Check Gender validation
                var student = await _db.Students.FirstOrDefaultAsync(s => s.Id == dto.StudentId && s.SchoolRegistrationId == schoolId && !s.IsDeleted);
                if (student == null) throw new Exception("Student not found!");

                var hostel = await _db.Hostels.FirstOrDefaultAsync(h => h.Id == dto.HostelId && h.SchoolRegistrationId == schoolId && !h.IsDeleted);
                if (hostel == null) throw new Exception("Hostel not found!");

                // Gender validation logic
                var isBoysHostel = hostel.HostelType.Equals("Boys", StringComparison.OrdinalIgnoreCase);
                var isGirlsHostel = hostel.HostelType.Equals("Girls", StringComparison.OrdinalIgnoreCase);
                var isMaleStudent = student.Gender != null && student.Gender.Equals("Male", StringComparison.OrdinalIgnoreCase);
                var isFemaleStudent = student.Gender != null && student.Gender.Equals("Female", StringComparison.OrdinalIgnoreCase);

                if (isBoysHostel && isFemaleStudent)
                    throw new Exception("Female student cannot be admitted to Boys Hostel!");
                if (isGirlsHostel && isMaleStudent)
                    throw new Exception("Male student cannot be admitted to Girls Hostel!");

                // Verify room occupancy capacity limits
                var room = await _db.Rooms.FirstOrDefaultAsync(r => r.Id == dto.RoomId && r.SchoolRegistrationId == schoolId && !r.IsDeleted);
                if (room == null || room.AvailableBeds <= 0)
                    throw new Exception("Room is already full!");

                // Verify student duplicate active admissions
                var activeAdmissions = await _db.HostelAdmissions.AnyAsync(a => a.StudentId == dto.StudentId && (a.Status == "admitted" || a.Status == "checkedin") && !a.IsDeleted);
                if (activeAdmissions)
                    throw new Exception("Student is already admitted in a hostel!");

                var admission = _mapper.Map<HostelAdmission>(dto);
                admission.AdmissionNumber = $"HST-{DateTime.UtcNow.Year}-{Guid.NewGuid().ToString("N")[..5].ToUpper()}";
                admission.SchoolRegistrationId = schoolId;
                admission.Status = "admitted";
                admission.CreatedBy = userId;
                admission.CreatedDate = DateTime.UtcNow;

                _db.HostelAdmissions.Add(admission);

                // Update Bed status
                bed.Status = "occupied";

                // Update Room allocation numbers
                room.OccupiedBeds++;
                room.AvailableBeds--;

                await _db.SaveChangesAsync();
                await tx.CommitAsync();

                return _mapper.Map<HostelAdmissionDto>(admission);
            }
            catch (Exception)
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> CheckInStudentAsync(int admissionId, string biometricId, string rfidTag, int schoolId, string userId)
        {
            var admission = await _db.HostelAdmissions.FirstOrDefaultAsync(a => a.Id == admissionId && a.SchoolRegistrationId == schoolId && !a.IsDeleted);
            if (admission == null) return false;

            admission.Status = "checkedin";
            admission.CheckInDate = DateTime.UtcNow;
            if (!string.IsNullOrEmpty(biometricId)) admission.BiometricId = biometricId;
            if (!string.IsNullOrEmpty(rfidTag)) admission.RfidTag = rfidTag;

            admission.UpdatedBy = userId;
            admission.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckOutStudentAsync(int admissionId, int schoolId, string userId)
        {
            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                var a = await _db.HostelAdmissions.FirstOrDefaultAsync(x => x.Id == admissionId && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
                if (a == null || a.Status == "checkedout") return false;

                a.Status = "checkedout";
                a.ActualCheckOutDate = DateTime.UtcNow;
                a.UpdatedBy = userId;
                a.UpdatedDate = DateTime.UtcNow;

                // Release bed state
                var bed = await _db.Beds.FindAsync(a.BedId);
                if (bed != null) bed.Status = "vacant";

                // Free up room allocation numbers
                var room = await _db.Rooms.FindAsync(a.RoomId);
                if (room != null)
                {
                    room.OccupiedBeds = Math.Max(0, room.OccupiedBeds - 1);
                    room.AvailableBeds = room.Capacity - room.OccupiedBeds;
                }

                await _db.SaveChangesAsync();
                await tx.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> TransferRoomAsync(RoomTransferDto dto, int schoolId, string userId)
        {
            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                var a = await _db.HostelAdmissions.FirstOrDefaultAsync(x => x.StudentId == dto.StudentId && (x.Status == "admitted" || x.Status == "checkedin") && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
                if (a == null) throw new Exception("Active admission not found for student!");

                // Check availability of target Room and Bed
                var targetRoom = await _db.Rooms.FirstOrDefaultAsync(r => r.Id == dto.ToRoomId && r.SchoolRegistrationId == schoolId && !r.IsDeleted);
                if (targetRoom == null || targetRoom.AvailableBeds <= 0)
                    throw new Exception("Target Room is full or unavailable!");

                var targetBed = await _db.Beds.FirstOrDefaultAsync(b => b.Id == dto.ToBedId && b.SchoolRegistrationId == schoolId && !b.IsDeleted);
                if (targetBed == null || targetBed.Status != "vacant")
                    throw new Exception("Target Bed is occupied or unavailable!");

                // Record transfer history log
                var hist = new RoomTransferHistory
                {
                    StudentId = dto.StudentId,
                    FromRoomId = a.RoomId,
                    ToRoomId = dto.ToRoomId,
                    FromBedId = a.BedId,
                    ToBedId = dto.ToBedId,
                    TransferDate = DateTime.UtcNow,
                    Reason = dto.Reason,
                    ApprovedBy = dto.ApprovedBy,
                    SchoolRegistrationId = schoolId,
                    CreatedBy = userId,
                    CreatedDate = DateTime.UtcNow
                };
                _db.RoomTransferHistories.Add(hist);

                // Release old bed state
                var oldBed = await _db.Beds.FindAsync(a.BedId);
                if (oldBed != null) oldBed.Status = "vacant";

                var oldRoom = await _db.Rooms.FindAsync(a.RoomId);
                if (oldRoom != null)
                {
                    oldRoom.OccupiedBeds = Math.Max(0, oldRoom.OccupiedBeds - 1);
                    oldRoom.AvailableBeds = oldRoom.Capacity - oldRoom.OccupiedBeds;
                }

                // Update admission mapping values
                a.RoomId = dto.ToRoomId;
                a.BedId = dto.ToBedId;
                a.UpdatedBy = userId;
                a.UpdatedDate = DateTime.UtcNow;

                // Occupy target bed state
                targetBed.Status = "occupied";
                targetRoom.OccupiedBeds++;
                targetRoom.AvailableBeds--;

                await _db.SaveChangesAsync();
                await tx.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> ExchangeRoomAsync(RoomExchangeDto dto, int schoolId, string userId)
        {
            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                var aA = await _db.HostelAdmissions.FirstOrDefaultAsync(x => x.StudentId == dto.StudentAId && (x.Status == "admitted" || x.Status == "checkedin") && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
                var aB = await _db.HostelAdmissions.FirstOrDefaultAsync(x => x.StudentId == dto.StudentBId && (x.Status == "admitted" || x.Status == "checkedin") && x.SchoolRegistrationId == schoolId && !x.IsDeleted);

                if (aA == null || aB == null)
                    throw new Exception("Both students must have an active admission assignment!");

                // Swap details
                var rA = aA.RoomId;
                var bA = aA.BedId;

                var rB = aB.RoomId;
                var bB = aB.BedId;

                // Update admissions
                aA.RoomId = rB;
                aA.BedId = bB;
                aA.UpdatedBy = userId;
                aA.UpdatedDate = DateTime.UtcNow;

                aB.RoomId = rA;
                aB.BedId = bA;
                aB.UpdatedBy = userId;
                aB.UpdatedDate = DateTime.UtcNow;

                // Swap logs
                _db.RoomTransferHistories.Add(new RoomTransferHistory
                {
                    StudentId = dto.StudentAId,
                    FromRoomId = rA,
                    ToRoomId = rB,
                    FromBedId = bA,
                    ToBedId = bB,
                    TransferDate = DateTime.UtcNow,
                    Reason = $"Exchange with Student B: {dto.Reason}",
                    ApprovedBy = dto.ApprovedBy,
                    SchoolRegistrationId = schoolId,
                    CreatedBy = userId,
                    CreatedDate = DateTime.UtcNow
                });

                _db.RoomTransferHistories.Add(new RoomTransferHistory
                {
                    StudentId = dto.StudentBId,
                    FromRoomId = rB,
                    ToRoomId = rA,
                    FromBedId = bB,
                    ToBedId = bA,
                    TransferDate = DateTime.UtcNow,
                    Reason = $"Exchange with Student A: {dto.Reason}",
                    ApprovedBy = dto.ApprovedBy,
                    SchoolRegistrationId = schoolId,
                    CreatedBy = userId,
                    CreatedDate = DateTime.UtcNow
                });

                await _db.SaveChangesAsync();
                await tx.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // ════════════════════════════════════════════════════════════════════
        // RESERVATIONS
        // ════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<BedReservationDto>> GetReservationsAsync(int schoolId)
        {
            var items = await _db.BedReservations
                .Include(r => r.Student)
                .Include(r => r.Bed)
                .ThenInclude(b => b.Room)
                .Where(r => r.SchoolRegistrationId == schoolId && !r.IsDeleted)
                .ToListAsync();
            return _mapper.Map<List<BedReservationDto>>(items);
        }

        public async Task<BedReservationDto> ReserveBedAsync(CreateBedReservationDto dto, int schoolId, string userId)
        {
            var bed = await _db.Beds.FirstOrDefaultAsync(b => b.Id == dto.BedId && b.SchoolRegistrationId == schoolId && !b.IsDeleted);
            if (bed == null || bed.Status != "vacant")
                throw new Exception("Bed is not vacant!");

            var res = _mapper.Map<BedReservation>(dto);
            res.ReservationDate = DateTime.UtcNow;
            res.Status = "active";
            res.SchoolRegistrationId = schoolId;
            res.CreatedBy = userId;
            res.CreatedDate = DateTime.UtcNow;

            bed.Status = "reserved";

            _db.BedReservations.Add(res);
            await _db.SaveChangesAsync();
            return _mapper.Map<BedReservationDto>(res);
        }

        public async Task<bool> CancelReservationAsync(int id, int schoolId, string userId)
        {
            var res = await _db.BedReservations.FirstOrDefaultAsync(r => r.Id == id && r.SchoolRegistrationId == schoolId && !r.IsDeleted);
            if (res == null) return false;

            res.Status = "cancelled";
            res.UpdatedBy = userId;
            res.UpdatedDate = DateTime.UtcNow;

            var bed = await _db.Beds.FindAsync(res.BedId);
            if (bed != null && bed.Status == "reserved")
                bed.Status = "vacant";

            await _db.SaveChangesAsync();
            return true;
        }

        // ════════════════════════════════════════════════════════════════════
        // MESS MENU
        // ════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<MessMenuDto>> GetMessMenusAsync(int schoolId, int hostelId)
        {
            var items = await _db.MessMenus
                .Include(m => m.Hostel)
                .Where(m => m.HostelId == hostelId && m.SchoolRegistrationId == schoolId && !m.IsDeleted)
                .ToListAsync();
            return _mapper.Map<List<MessMenuDto>>(items);
        }

        public async Task<MessMenuDto> CreateMessMenuAsync(CreateMessMenuDto dto, int schoolId, string userId)
        {
            var m = _mapper.Map<MessMenu>(dto);
            m.SchoolRegistrationId = schoolId;
            m.CreatedBy = userId;
            m.CreatedDate = DateTime.UtcNow;

            _db.MessMenus.Add(m);
            await _db.SaveChangesAsync();
            return _mapper.Map<MessMenuDto>(m);
        }

        public async Task<bool> UpdateMessMenuAsync(int id, CreateMessMenuDto dto, int schoolId, string userId)
        {
            var m = await _db.MessMenus.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (m == null) return false;

            _mapper.Map(dto, m);
            m.UpdatedBy = userId;
            m.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMessMenuAsync(int id, int schoolId)
        {
            var m = await _db.MessMenus.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (m == null) return false;

            m.IsDeleted = true;
            await _db.SaveChangesAsync();
            return true;
        }

        // ════════════════════════════════════════════════════════════════════
        // MEAL ATTENDANCE
        // ════════════════════════════════════════════════════════════════════
        public async Task<PagedResultDto<MealAttendanceDto>> GetMealAttendanceAsync(int schoolId, int page, int pageSize, string mealType, DateTime? date)
        {
            var q = _db.MealAttendances.Include(a => a.Student).Where(a => a.SchoolRegistrationId == schoolId && !a.IsDeleted);

            if (!string.IsNullOrWhiteSpace(mealType))
                q = q.Where(a => a.MealType == mealType);

            if (date.HasValue)
            {
                var targetDate = date.Value.Date;
                q = q.Where(a => a.Date.Date == targetDate);
            }

            var total = await q.CountAsync();
            var items = await q.OrderByDescending(a => a.Date)
                               .Skip((page - 1) * pageSize).Take(pageSize)
                               .ToListAsync();

            return new PagedResultDto<MealAttendanceDto>
            {
                Items = _mapper.Map<List<MealAttendanceDto>>(items),
                TotalItems = total,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<MealAttendanceDto> MarkMealAttendanceAsync(CreateMealAttendanceDto dto, int schoolId, string userId)
        {
            var record = _mapper.Map<MealAttendance>(dto);
            record.Date = DateTime.UtcNow;
            record.SchoolRegistrationId = schoolId;
            record.CreatedBy = userId;
            record.CreatedDate = DateTime.UtcNow;

            _db.MealAttendances.Add(record);
            await _db.SaveChangesAsync();
            return _mapper.Map<MealAttendanceDto>(record);
        }

        // ════════════════════════════════════════════════════════════════════
        // VISITOR MANAGEMENT
        // ════════════════════════════════════════════════════════════════════
        public async Task<PagedResultDto<HostelVisitorDto>> GetVisitorsAsync(int schoolId, string search, int page, int pageSize, string status)
        {
            var q = _db.HostelVisitors.Include(v => v.Student).Where(v => v.SchoolRegistrationId == schoolId && !v.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower().Trim();
                q = q.Where(v => v.VisitorName.ToLower().Contains(s) || v.Student.Name.ToLower().Contains(s));
            }
            if (!string.IsNullOrWhiteSpace(status))
                q = q.Where(v => v.Status == status);

            var total = await q.CountAsync();
            var items = await q.OrderByDescending(v => v.EntryTime)
                               .Skip((page - 1) * pageSize).Take(pageSize)
                               .ToListAsync();

            return new PagedResultDto<HostelVisitorDto>
            {
                Items = _mapper.Map<List<HostelVisitorDto>>(items),
                TotalItems = total,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<HostelVisitorDto> RegisterVisitorAsync(CreateHostelVisitorDto dto, int schoolId, string userId)
        {
            var visitor = _mapper.Map<HostelVisitor>(dto);
            visitor.EntryTime = DateTime.UtcNow;
            visitor.Status = "approved";
            visitor.SchoolRegistrationId = schoolId;
            visitor.CreatedBy = userId;
            visitor.CreatedDate = DateTime.UtcNow;

            _db.HostelVisitors.Add(visitor);
            await _db.SaveChangesAsync();
            return _mapper.Map<HostelVisitorDto>(visitor);
        }

        public async Task<bool> CheckoutVisitorAsync(int visitorId, int schoolId, string userId)
        {
            var visitor = await _db.HostelVisitors.FirstOrDefaultAsync(v => v.Id == visitorId && v.SchoolRegistrationId == schoolId && !v.IsDeleted);
            if (visitor == null) return false;

            visitor.ExitTime = DateTime.UtcNow;
            visitor.Status = "out";
            visitor.UpdatedBy = userId;
            visitor.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        // ════════════════════════════════════════════════════════════════════
        // GATE PASS / LEAVES
        // ════════════════════════════════════════════════════════════════════
        public async Task<PagedResultDto<HostelGatePassDto>> GetGatePassesAsync(int schoolId, string search, int page, int pageSize, string status)
        {
            var q = _db.HostelGatePasses
                .Include(g => g.Student)
                .Include(g => g.Admission)
                .ThenInclude(a => a.Room)
                .Where(g => g.SchoolRegistrationId == schoolId && !g.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower().Trim();
                q = q.Where(g => g.Student.Name.ToLower().Contains(s) || g.Reason.ToLower().Contains(s));
            }
            if (!string.IsNullOrWhiteSpace(status))
                q = q.Where(g => g.Status == status);

            var total = await q.CountAsync();
            var items = await q.OrderByDescending(g => g.OutTime)
                               .Skip((page - 1) * pageSize).Take(pageSize)
                               .ToListAsync();

            return new PagedResultDto<HostelGatePassDto>
            {
                Items = _mapper.Map<List<HostelGatePassDto>>(items),
                TotalItems = total,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<HostelGatePassDto> IssueGatePassAsync(CreateHostelGatePassDto dto, int schoolId, string userId)
        {
            var pass = _mapper.Map<HostelGatePass>(dto);
            pass.Status = "pending";
            pass.QrCode = $"PASS-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
            pass.SchoolRegistrationId = schoolId;
            pass.CreatedBy = userId;
            pass.CreatedDate = DateTime.UtcNow;

            _db.HostelGatePasses.Add(pass);
            await _db.SaveChangesAsync();
            return _mapper.Map<HostelGatePassDto>(pass);
        }

        public async Task<bool> ApproveGatePassAsync(int gatePassId, string approverRole, string approvalStatus, int schoolId, string userId)
        {
            var pass = await _db.HostelGatePasses.FirstOrDefaultAsync(p => p.Id == gatePassId && p.SchoolRegistrationId == schoolId && !p.IsDeleted);
            if (pass == null) return false;

            if (approverRole.Equals("warden", StringComparison.OrdinalIgnoreCase))
                pass.WardenApproval = approvalStatus;
            else if (approverRole.Equals("parent", StringComparison.OrdinalIgnoreCase))
                pass.ParentApproval = approvalStatus;

            // Auto-approve overall gatepass if both approvals are done or if warden alone approves for non-leave cases
            if (pass.WardenApproval == "Approved" && (pass.ParentApproval == "Approved" || string.IsNullOrEmpty(pass.ParentApproval)))
                pass.Status = "approved";
            else if (pass.WardenApproval == "Rejected" || pass.ParentApproval == "Rejected")
                pass.Status = "rejected";

            pass.UpdatedBy = userId;
            pass.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ScanGatePassOutAsync(int gatePassId, int schoolId, string userId)
        {
            var pass = await _db.HostelGatePasses.FirstOrDefaultAsync(p => p.Id == gatePassId && p.SchoolRegistrationId == schoolId && !p.IsDeleted);
            if (pass == null || pass.Status != "approved") return false;

            pass.OutTime = DateTime.UtcNow;
            pass.Status = "out";
            pass.UpdatedBy = userId;
            pass.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ScanGatePassInAsync(int gatePassId, int schoolId, string userId)
        {
            var pass = await _db.HostelGatePasses.FirstOrDefaultAsync(p => p.Id == gatePassId && p.SchoolRegistrationId == schoolId && !p.IsDeleted);
            if (pass == null || pass.Status != "out") return false;

            pass.InTime = DateTime.UtcNow;
            pass.Status = "completed";
            pass.UpdatedBy = userId;
            pass.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        // ════════════════════════════════════════════════════════════════════
        // ATTENDANCE
        // ════════════════════════════════════════════════════════════════════
        public async Task<PagedResultDto<HostelAttendanceDto>> GetAttendanceAsync(int schoolId, string search, int page, int pageSize, string type, DateTime? date)
        {
            var q = _db.HostelAttendances.Include(a => a.Student).Where(a => a.SchoolRegistrationId == schoolId && !a.IsDeleted);

            if (!string.IsNullOrWhiteSpace(type))
                q = q.Where(a => a.AttendanceType == type);

            if (date.HasValue)
            {
                var target = date.Value.Date;
                q = q.Where(a => a.Date.Date == target);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower().Trim();
                q = q.Where(a => a.Student.Name.ToLower().Contains(s));
            }

            var total = await q.CountAsync();
            var items = await q.OrderByDescending(a => a.Date)
                               .Skip((page - 1) * pageSize).Take(pageSize)
                               .ToListAsync();

            var dtos = _mapper.Map<List<HostelAttendanceDto>>(items);
            if (dtos.Count > 0)
            {
                var studentIds = dtos.Select(d => d.StudentId).ToList();
                var admissions = await _db.HostelAdmissions
                    .Include(a => a.Room)
                    .Where(a => studentIds.Contains(a.StudentId) && (a.Status == "admitted" || a.Status == "checkedin") && !a.IsDeleted)
                    .ToDictionaryAsync(a => a.StudentId, a => a.Room.RoomNumber);

                foreach (var d in dtos)
                {
                    if (admissions.TryGetValue(d.StudentId, out var roomNo))
                        d.RoomNumber = roomNo;
                }
            }

            return new PagedResultDto<HostelAttendanceDto>
            {
                Items = dtos,
                TotalItems = total,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<bool> MarkAttendanceAsync(List<CreateHostelAttendanceDto> list, int schoolId, string userId)
        {
            foreach (var item in list)
            {
                // Check if already exists for same student, type and date
                var existing = await _db.HostelAttendances.FirstOrDefaultAsync(a => a.StudentId == item.StudentId && a.Date.Date == item.Date.Date && a.AttendanceType == item.AttendanceType && a.SchoolRegistrationId == schoolId && !a.IsDeleted);
                if (existing != null)
                {
                    existing.Status = item.Status;
                    existing.Remarks = item.Remarks;
                    existing.UpdatedBy = userId;
                    existing.UpdatedDate = DateTime.UtcNow;
                }
                else
                {
                    var att = _mapper.Map<HostelAttendance>(item);
                    att.SchoolRegistrationId = schoolId;
                    att.CreatedBy = userId;
                    att.CreatedDate = DateTime.UtcNow;
                    _db.HostelAttendances.Add(att);
                }
            }

            await _db.SaveChangesAsync();
            return true;
        }

        // ════════════════════════════════════════════════════════════════════
        // COMPLAINTS
        // ════════════════════════════════════════════════════════════════════
        public async Task<PagedResultDto<HostelComplaintDto>> GetComplaintsAsync(int schoolId, string search, int page, int pageSize, string status, string priority)
        {
            var q = _db.HostelComplaints
                .Include(c => c.Student)
                .Include(c => c.AssignedStaff)
                .Where(c => c.SchoolRegistrationId == schoolId && !c.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower().Trim();
                q = q.Where(c => c.Description.ToLower().Contains(s) || c.Student.Name.ToLower().Contains(s));
            }
            if (!string.IsNullOrWhiteSpace(status))
                q = q.Where(c => c.Status == status);
            if (!string.IsNullOrWhiteSpace(priority))
                q = q.Where(c => c.Priority == priority);

            var total = await q.CountAsync();
            var items = await q.OrderByDescending(c => c.Id)
                               .Skip((page - 1) * pageSize).Take(pageSize)
                               .ToListAsync();

            var dtos = _mapper.Map<List<HostelComplaintDto>>(items);
            if (dtos.Count > 0)
            {
                var studentIds = dtos.Select(d => d.StudentId).ToList();
                var admissions = await _db.HostelAdmissions
                    .Include(a => a.Room)
                    .Where(a => studentIds.Contains(a.StudentId) && (a.Status == "admitted" || a.Status == "checkedin") && !a.IsDeleted)
                    .ToDictionaryAsync(a => a.StudentId, a => a.Room.RoomNumber);

                foreach (var d in dtos)
                {
                    if (admissions.TryGetValue(d.StudentId, out var roomNo))
                        d.RoomNumber = roomNo;
                }
            }

            return new PagedResultDto<HostelComplaintDto>
            {
                Items = dtos,
                TotalItems = total,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<HostelComplaintDto> RaiseComplaintAsync(CreateHostelComplaintDto dto, int schoolId, string userId)
        {
            var comp = _mapper.Map<HostelComplaint>(dto);
            comp.Status = "Open";
            comp.SchoolRegistrationId = schoolId;
            comp.CreatedBy = userId;
            comp.CreatedDate = DateTime.UtcNow;

            _db.HostelComplaints.Add(comp);
            await _db.SaveChangesAsync();
            return _mapper.Map<HostelComplaintDto>(comp);
        }

        public async Task<bool> AssignComplaintAsync(int id, int staffId, int schoolId, string userId)
        {
            var comp = await _db.HostelComplaints.FirstOrDefaultAsync(c => c.Id == id && c.SchoolRegistrationId == schoolId && !c.IsDeleted);
            if (comp == null) return false;

            comp.AssignedStaffId = staffId;
            comp.Status = "Assigned";
            comp.UpdatedBy = userId;
            comp.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ResolveComplaintAsync(int id, string details, int schoolId, string userId)
        {
            var comp = await _db.HostelComplaints.FirstOrDefaultAsync(c => c.Id == id && c.SchoolRegistrationId == schoolId && !c.IsDeleted);
            if (comp == null) return false;

            comp.Status = "Resolved";
            comp.ResolutionDetails = details;
            comp.UpdatedBy = userId;
            comp.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        // ════════════════════════════════════════════════════════════════════
        // MAINTENANCE
        // ════════════════════════════════════════════════════════════════════
        public async Task<PagedResultDto<HostelMaintenanceDto>> GetMaintenanceRequestsAsync(int schoolId, int page, int pageSize, string status)
        {
            var q = _db.HostelMaintenances.Include(m => m.Complaint).Where(m => m.SchoolRegistrationId == schoolId && !m.IsDeleted);

            if (!string.IsNullOrWhiteSpace(status))
                q = q.Where(m => m.Status == status);

            var total = await q.CountAsync();
            var items = await q.OrderByDescending(m => m.Id)
                               .Skip((page - 1) * pageSize).Take(pageSize)
                               .ToListAsync();

            return new PagedResultDto<HostelMaintenanceDto>
            {
                Items = _mapper.Map<List<HostelMaintenanceDto>>(items),
                TotalItems = total,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<HostelMaintenanceDto> CreateMaintenanceRequestAsync(CreateHostelMaintenanceDto dto, int schoolId, string userId)
        {
            var req = _mapper.Map<HostelMaintenance>(dto);
            req.SchoolRegistrationId = schoolId;
            req.CreatedBy = userId;
            req.CreatedDate = DateTime.UtcNow;

            _db.HostelMaintenances.Add(req);
            await _db.SaveChangesAsync();
            return _mapper.Map<HostelMaintenanceDto>(req);
        }

        public async Task<bool> CompleteMaintenanceRequestAsync(int id, decimal cost, string details, int schoolId, string userId)
        {
            var req = await _db.HostelMaintenances.FirstOrDefaultAsync(m => m.Id == id && m.SchoolRegistrationId == schoolId && !m.IsDeleted);
            if (req == null) return false;

            req.Status = "Completed";
            req.Cost = cost;
            req.CompletionDate = DateTime.UtcNow;
            req.UpdatedBy = userId;
            req.UpdatedDate = DateTime.UtcNow;

            // Auto-resolve associated complaint if any
            if (req.ComplaintId.HasValue)
            {
                var comp = await _db.HostelComplaints.FindAsync(req.ComplaintId.Value);
                if (comp != null)
                {
                    comp.Status = "Resolved";
                    comp.ResolutionDetails = $"Maintenance Completed: {details}";
                }
            }

            await _db.SaveChangesAsync();
            return true;
        }

        // ════════════════════════════════════════════════════════════════════
        // LAUNDRY
        // ════════════════════════════════════════════════════════════════════
        public async Task<PagedResultDto<LaundryTransactionDto>> GetLaundryTransactionsAsync(int schoolId, int page, int pageSize, string status)
        {
            var q = _db.LaundryTransactions.Include(l => l.Student).Where(l => l.SchoolRegistrationId == schoolId && !l.IsDeleted);

            if (!string.IsNullOrWhiteSpace(status))
                q = q.Where(l => l.Status == status);

            var total = await q.CountAsync();
            var items = await q.OrderByDescending(l => l.PickupDate)
                               .Skip((page - 1) * pageSize).Take(pageSize)
                               .ToListAsync();

            return new PagedResultDto<LaundryTransactionDto>
            {
                Items = _mapper.Map<List<LaundryTransactionDto>>(items),
                TotalItems = total,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<LaundryTransactionDto> CreateLaundryTransactionAsync(CreateLaundryTransactionDto dto, int schoolId, string userId)
        {
            var l = _mapper.Map<LaundryTransaction>(dto);
            l.Status = "Collected";
            l.SchoolRegistrationId = schoolId;
            l.CreatedBy = userId;
            l.CreatedDate = DateTime.UtcNow;

            _db.LaundryTransactions.Add(l);
            await _db.SaveChangesAsync();
            return _mapper.Map<LaundryTransactionDto>(l);
        }

        public async Task<bool> UpdateLaundryStatusAsync(int id, string status, int schoolId, string userId)
        {
            var l = await _db.LaundryTransactions.FirstOrDefaultAsync(x => x.Id == id && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            if (l == null) return false;

            l.Status = status;
            if (status.Equals("Delivered", StringComparison.OrdinalIgnoreCase))
                l.ActualDelivery = DateTime.UtcNow;

            l.UpdatedBy = userId;
            l.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        // ════════════════════════════════════════════════════════════════════
        // INVENTORY
        // ════════════════════════════════════════════════════════════════════
        public async Task<PagedResultDto<HostelInventoryDto>> GetInventoryAsync(int schoolId, string search, int page, int pageSize, string status, int? roomId)
        {
            var q = _db.HostelInventories.Include(i => i.Room).ThenInclude(r => r.Hostel).Where(i => i.SchoolRegistrationId == schoolId && !i.IsDeleted);

            if (roomId.HasValue && roomId > 0) q = q.Where(i => i.RoomId == roomId.Value);
            if (!string.IsNullOrWhiteSpace(status)) q = q.Where(i => i.Status == status);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower().Trim();
                q = q.Where(i => i.AssetName.ToLower().Contains(s) || i.AssetTag.ToLower().Contains(s));
            }

            var total = await q.CountAsync();
            var items = await q.OrderBy(i => i.AssetName)
                               .Skip((page - 1) * pageSize).Take(pageSize)
                               .ToListAsync();

            return new PagedResultDto<HostelInventoryDto>
            {
                Items = _mapper.Map<List<HostelInventoryDto>>(items),
                TotalItems = total,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<HostelInventoryDto> CreateInventoryAsync(CreateHostelInventoryDto dto, int schoolId, string userId)
        {
            var item = _mapper.Map<HostelInventory>(dto);
            item.SchoolRegistrationId = schoolId;
            item.CreatedBy = userId;
            item.CreatedDate = DateTime.UtcNow;

            _db.HostelInventories.Add(item);
            await _db.SaveChangesAsync();
            return _mapper.Map<HostelInventoryDto>(item);
        }

        public async Task<bool> AuditInventoryAsync(int id, string status, int schoolId, string userId)
        {
            var item = await _db.HostelInventories.FirstOrDefaultAsync(i => i.Id == id && i.SchoolRegistrationId == schoolId && !i.IsDeleted);
            if (item == null) return false;

            item.Status = status;
            item.AuditDate = DateTime.UtcNow;
            item.UpdatedBy = userId;
            item.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        // ════════════════════════════════════════════════════════════════════
        // MEDICAL
        // ════════════════════════════════════════════════════════════════════
        public async Task<PagedResultDto<HostelMedicalLogDto>> GetMedicalLogsAsync(int schoolId, int page, int pageSize, string status)
        {
            var q = _db.HostelMedicalLogs.Include(m => m.Student).Include(m => m.IsolationRoom).Where(m => m.SchoolRegistrationId == schoolId && !m.IsDeleted);

            if (!string.IsNullOrWhiteSpace(status))
                q = q.Where(m => m.Status == status);

            var total = await q.CountAsync();
            var items = await q.OrderByDescending(m => m.Id)
                               .Skip((page - 1) * pageSize).Take(pageSize)
                               .ToListAsync();

            return new PagedResultDto<HostelMedicalLogDto>
            {
                Items = _mapper.Map<List<HostelMedicalLogDto>>(items),
                TotalItems = total,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<HostelMedicalLogDto> CreateMedicalLogAsync(CreateHostelMedicalLogDto dto, int schoolId, string userId)
        {
            var log = _mapper.Map<HostelMedicalLog>(dto);
            log.SchoolRegistrationId = schoolId;
            log.CreatedBy = userId;
            log.CreatedDate = DateTime.UtcNow;

            _db.HostelMedicalLogs.Add(log);
            await _db.SaveChangesAsync();
            return _mapper.Map<HostelMedicalLogDto>(log);
        }

        public async Task<bool> UpdateMedicalStatusAsync(int id, string status, int schoolId, string userId)
        {
            var log = await _db.HostelMedicalLogs.FirstOrDefaultAsync(m => m.Id == id && m.SchoolRegistrationId == schoolId && !m.IsDeleted);
            if (log == null) return false;

            log.Status = status;
            log.UpdatedBy = userId;
            log.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        // ════════════════════════════════════════════════════════════════════
        // DISCIPLINE
        // ════════════════════════════════════════════════════════════════════
        public async Task<PagedResultDto<HostelDisciplineDto>> GetDisciplinesAsync(int schoolId, int page, int pageSize, string severity)
        {
            var q = _db.HostelDisciplines.Include(d => d.Student).Where(d => d.SchoolRegistrationId == schoolId && !d.IsDeleted);

            if (!string.IsNullOrWhiteSpace(severity))
                q = q.Where(d => d.Severity == severity);

            var total = await q.CountAsync();
            var items = await q.OrderByDescending(d => d.IncidentDate)
                               .Skip((page - 1) * pageSize).Take(pageSize)
                               .ToListAsync();

            return new PagedResultDto<HostelDisciplineDto>
            {
                Items = _mapper.Map<List<HostelDisciplineDto>>(items),
                TotalItems = total,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<HostelDisciplineDto> RecordDisciplineAsync(CreateHostelDisciplineDto dto, int schoolId, string userId)
        {
            var d = _mapper.Map<HostelDiscipline>(dto);
            d.SchoolRegistrationId = schoolId;
            d.CreatedBy = userId;
            d.CreatedDate = DateTime.UtcNow;

            _db.HostelDisciplines.Add(d);
            await _db.SaveChangesAsync();
            return _mapper.Map<HostelDisciplineDto>(d);
        }

        // ════════════════════════════════════════════════════════════════════
        // DASHBOARD & REPORTS
        // ════════════════════════════════════════════════════════════════════
        public async Task<HostelDashboardDto> GetDashboardAsync(int schoolId)
        {
            var totalHostels = await _db.Hostels.CountAsync(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            var boysHostels = await _db.Hostels.CountAsync(x => x.HostelType == "Boys" && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            var girlsHostels = await _db.Hostels.CountAsync(x => x.HostelType == "Girls" && x.SchoolRegistrationId == schoolId && !x.IsDeleted);

            var buildings = await _db.Buildings.CountAsync(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            var floors = await _db.Floors.CountAsync(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            var rooms = await _db.Rooms.CountAsync(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted);

            var bedsList = await _db.Beds.Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted).ToListAsync();
            var totalBeds = bedsList.Count;
            var occupiedBeds = bedsList.Count(x => x.Status == "occupied");
            var availableBeds = bedsList.Count(x => x.Status == "vacant");
            var reservedBeds = bedsList.Count(x => x.Status == "reserved");
            var repairBeds = bedsList.Count(x => x.Status == "repair");

            var students = await _db.HostelAdmissions.CountAsync(x => (x.Status == "admitted" || x.Status == "checkedin") && x.SchoolRegistrationId == schoolId && !x.IsDeleted);

            var today = DateTime.UtcNow.Date;
            var messAttendance = await _db.MealAttendances.CountAsync(x => x.Date.Date == today && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            var visitors = await _db.HostelVisitors.CountAsync(x => x.EntryTime.Date == today && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            var complaints = await _db.HostelComplaints.CountAsync(x => x.Status == "Open" && x.SchoolRegistrationId == schoolId && !x.IsDeleted);
            var activePasses = await _db.HostelGatePasses.CountAsync(x => x.Status == "out" && x.SchoolRegistrationId == schoolId && !x.IsDeleted);

            var dash = new HostelDashboardDto
            {
                TotalHostels = totalHostels,
                BoysHostels = boysHostels,
                GirlsHostels = girlsHostels,
                TotalBuildings = buildings,
                TotalFloors = floors,
                TotalRooms = rooms,
                TotalBeds = totalBeds,
                OccupiedBeds = occupiedBeds,
                AvailableBeds = availableBeds,
                ReservedBeds = reservedBeds,
                RepairBeds = repairBeds,
                TotalStudents = students,
                MessAttendanceToday = messAttendance,
                VisitorsToday = visitors,
                PendingComplaints = complaints,
                ActiveGatePasses = activePasses
            };

            // Aggregates for Occupancy
            var hostelsList = await _db.Hostels.Where(h => h.SchoolRegistrationId == schoolId && !h.IsDeleted).ToListAsync();
            foreach (var h in hostelsList)
            {
                var rList = await _db.Rooms.Where(r => r.HostelId == h.Id && !r.IsDeleted).ToListAsync();
                var capacity = rList.Sum(r => r.Capacity);
                var occupied = rList.Sum(r => r.OccupiedBeds);
                var percentage = capacity > 0 ? ((double)occupied / capacity) * 100 : 0.0;

                dash.HostelOccupancyStats.Add(new HostelOccupancyStatDto
                {
                    HostelName = h.Name,
                    OccupancyPercentage = Math.Round(percentage, 2)
                });
            }

            // Aggregates for Complaint categories
            var complaintGroups = await _db.HostelComplaints
                .Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .GroupBy(x => x.Category)
                .Select(g => new ComplaintCategoryStatDto { CategoryName = g.Key, Count = g.Count() })
                .ToListAsync();
            dash.ComplaintCategoryStats = complaintGroups;

            // Aggregate Monthly Admissions trend (past 6 months)
            var sixMonthsAgo = DateTime.UtcNow.AddMonths(-6);
            var admissionsTrend = await _db.HostelAdmissions
                .Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted && x.AdmissionDate >= sixMonthsAgo)
                .ToListAsync();

            var trendGroup = admissionsTrend
                .GroupBy(x => x.AdmissionDate.ToString("MMMM yyyy"))
                .Select(g => new MonthlyAdmissionsStatDto { MonthName = g.Key, Count = g.Count() })
                .ToList();
            dash.MonthlyAdmissionsStats = trendGroup;

            return dash;
        }

        public async Task<IEnumerable<HostelAdmissionDto>> GetOccupancyReportAsync(int schoolId, int? hostelId, int? buildingId)
        {
            var q = _db.HostelAdmissions
                .Include(a => a.Student)
                .Include(a => a.Hostel)
                .Include(a => a.Room)
                .Include(a => a.Bed)
                .Where(a => a.SchoolRegistrationId == schoolId && (a.Status == "admitted" || a.Status == "checkedin") && !a.IsDeleted);

            if (hostelId.HasValue && hostelId > 0) q = q.Where(a => a.HostelId == hostelId.Value);
            if (buildingId.HasValue && buildingId > 0) q = q.Where(a => a.Room.BuildingId == buildingId.Value);

            var items = await q.ToListAsync();
            return _mapper.Map<List<HostelAdmissionDto>>(items);
        }
    }
}
