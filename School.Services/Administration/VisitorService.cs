using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using School.Domain.Administration;
using School.Infrastructure;
using School.Services.Interfaces;
using School_DTOs;
using School_DTOs.Administration;

namespace School.Services.Administration
{
    public class VisitorService : IVisitorService
    {
        private readonly SchoolDbContext _context;
        private readonly IMapper _mapper;

        public VisitorService(SchoolDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<APIResponse<List<VisitorDto>>> GetVisitorsAsync(VisitorFilterDto filter, int schoolId)
        {
            var query = _context.Visitors
                .Where(v => v.SchoolRegistrationId == schoolId && !v.IsDeleted);

            if (!string.IsNullOrEmpty(filter.Status))
                query = query.Where(v => v.Status == filter.Status);
            if (!string.IsNullOrEmpty(filter.Purpose))
                query = query.Where(v => v.Purpose == filter.Purpose);
            if (filter.FromDate.HasValue)
                query = query.Where(v => v.CheckInTime >= filter.FromDate);
            if (filter.ToDate.HasValue)
                query = query.Where(v => v.CheckInTime <= filter.ToDate);

            var items = await query.OrderByDescending(v => v.CheckInTime).ToListAsync();
            return new APIResponse<List<VisitorDto>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = _mapper.Map<List<VisitorDto>>(items)
            };
        }

        public async Task<APIResponse<VisitorDto>> GetVisitorByIdAsync(int id, int schoolId)
        {
            var entity = await _context.Visitors
                .FirstOrDefaultAsync(v => v.Id == id && v.SchoolRegistrationId == schoolId && !v.IsDeleted);

            if (entity == null)
                return new APIResponse<VisitorDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Visitor not found" };

            return new APIResponse<VisitorDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = _mapper.Map<VisitorDto>(entity) };
        }

        public async Task<APIResponse<VisitorDto>> CheckInVisitorAsync(CreateVisitorDto dto, string userId, int schoolId)
        {
            var entity = _mapper.Map<Visitor>(dto);
            entity.SchoolRegistrationId = schoolId;
            entity.VisitorNumber = await GenerateVisitorNumber(schoolId);
            entity.CheckInTime = DateTime.UtcNow;
            entity.Status = "CheckedIn";
            entity.ApprovedByUserId = userId;
            entity.CreatedBy = userId;
            entity.CreatedDate = DateTime.UtcNow;

            _context.Visitors.Add(entity);
            await _context.SaveChangesAsync();

            return new APIResponse<VisitorDto> { Success = true, StatusCode = HttpStatusCode.Created, Data = _mapper.Map<VisitorDto>(entity), Message = "Visitor checked in" };
        }

        public async Task<APIResponse<bool>> CheckOutVisitorAsync(int id, int schoolId)
        {
            var entity = await _context.Visitors.FirstOrDefaultAsync(v => v.Id == id && v.SchoolRegistrationId == schoolId);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Visitor not found" };

            entity.CheckOutTime = DateTime.UtcNow;
            entity.Status = "CheckedOut";
            entity.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Data = true, Message = "Visitor checked out" };
        }

        public async Task<APIResponse<bool>> DeleteVisitorAsync(int id, int schoolId)
        {
            var entity = await _context.Visitors.FirstOrDefaultAsync(v => v.Id == id && v.SchoolRegistrationId == schoolId);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Visitor not found" };

            entity.IsDeleted = true;
            entity.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Data = true, Message = "Visitor record deleted" };
        }

        private async Task<string> GenerateVisitorNumber(int schoolId)
        {
            var today = DateTime.UtcNow.Date;
            var count = await _context.Visitors.CountAsync(v => v.SchoolRegistrationId == schoolId && v.CheckInTime >= today);
            return $"VIS-{DateTime.UtcNow:yyyyMMdd}-{(count + 1):D4}";
        }
    }
}
