using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using School.Domain.Administration;
using School.Infrastructure;
using School.Services.Interfaces;
using School_DTOs;
using School_DTOs.Administration;

namespace School.Services.Administration
{
    public class ComplaintService : IComplaintService
    {
        private readonly SchoolDbContext _context;
        private readonly IMapper _mapper;

        public ComplaintService(SchoolDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<APIResponse<List<ComplaintDto>>> GetComplaintsAsync(ComplaintFilterDto filter, int schoolId)
        {
            var query = _context.Complaints
                .Where(c => c.SchoolRegistrationId == schoolId && !c.IsDeleted);

            if (!string.IsNullOrEmpty(filter.Status))
                query = query.Where(c => c.Status == filter.Status);
            if (!string.IsNullOrEmpty(filter.Category))
                query = query.Where(c => c.Category == filter.Category);
            if (!string.IsNullOrEmpty(filter.Priority))
                query = query.Where(c => c.Priority == filter.Priority);
            if (filter.FromDate.HasValue)
                query = query.Where(c => c.CreatedDate >= filter.FromDate);
            if (filter.ToDate.HasValue)
                query = query.Where(c => c.CreatedDate <= filter.ToDate);

            var items = await query.OrderByDescending(c => c.CreatedDate).ToListAsync();
            return new APIResponse<List<ComplaintDto>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = _mapper.Map<List<ComplaintDto>>(items)
            };
        }

        public async Task<APIResponse<ComplaintDto>> GetComplaintByIdAsync(int id, int schoolId)
        {
            var entity = await _context.Complaints
                .FirstOrDefaultAsync(c => c.Id == id && c.SchoolRegistrationId == schoolId && !c.IsDeleted);

            if (entity == null)
                return new APIResponse<ComplaintDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Complaint not found" };

            return new APIResponse<ComplaintDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = _mapper.Map<ComplaintDto>(entity) };
        }

        public async Task<APIResponse<ComplaintDto>> CreateComplaintAsync(CreateComplaintDto dto, string userId, int schoolId)
        {
            var entity = _mapper.Map<Complaint>(dto);
            entity.SchoolRegistrationId = schoolId;
            entity.RaisedByUserId = userId;
            entity.Status = "Open";
            entity.ComplaintNumber = await GenerateComplaintNumber(schoolId);
            entity.CreatedBy = userId;
            entity.CreatedDate = DateTime.UtcNow;

            _context.Complaints.Add(entity);
            await _context.SaveChangesAsync();

            return new APIResponse<ComplaintDto> { Success = true, StatusCode = HttpStatusCode.Created, Data = _mapper.Map<ComplaintDto>(entity), Message = "Complaint registered successfully" };
        }

        public async Task<APIResponse<ComplaintDto>> UpdateComplaintAsync(int id, ComplaintDto dto, int schoolId)
        {
            var entity = await _context.Complaints
                .FirstOrDefaultAsync(c => c.Id == id && c.SchoolRegistrationId == schoolId && !c.IsDeleted);

            if (entity == null)
                return new APIResponse<ComplaintDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Complaint not found" };

            entity.Subject = dto.Subject;
            entity.Description = dto.Description;
            entity.Category = dto.Category;
            entity.Priority = dto.Priority;
            entity.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return new APIResponse<ComplaintDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = _mapper.Map<ComplaintDto>(entity) };
        }

        public async Task<APIResponse<bool>> AssignComplaintAsync(int id, string assignedToUserId, string assignedToName, int schoolId)
        {
            var entity = await _context.Complaints.FirstOrDefaultAsync(c => c.Id == id && c.SchoolRegistrationId == schoolId);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Complaint not found" };

            entity.AssignedToUserId = assignedToUserId;
            entity.AssignedToName = assignedToName;
            entity.Status = "InProgress";
            entity.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Data = true, Message = "Complaint assigned" };
        }

        public async Task<APIResponse<bool>> ResolveComplaintAsync(int id, string resolutionDetails, string resolvedBy, int schoolId)
        {
            var entity = await _context.Complaints.FirstOrDefaultAsync(c => c.Id == id && c.SchoolRegistrationId == schoolId);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Complaint not found" };

            entity.ResolutionDetails = resolutionDetails;
            entity.Status = "Resolved";
            entity.ResolvedDate = DateTime.UtcNow;
            entity.UpdatedBy = resolvedBy;
            entity.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Data = true, Message = "Complaint resolved" };
        }

        public async Task<APIResponse<bool>> EscalateComplaintAsync(int id, string notes, int schoolId)
        {
            var entity = await _context.Complaints.FirstOrDefaultAsync(c => c.Id == id && c.SchoolRegistrationId == schoolId);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Complaint not found" };

            entity.Status = "Escalated";
            entity.EscalationNotes = notes;
            entity.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Data = true, Message = "Complaint escalated" };
        }

        public async Task<APIResponse<bool>> DeleteComplaintAsync(int id, int schoolId)
        {
            var entity = await _context.Complaints.FirstOrDefaultAsync(c => c.Id == id && c.SchoolRegistrationId == schoolId);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Complaint not found" };

            entity.IsDeleted = true;
            entity.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Data = true, Message = "Complaint deleted" };
        }

        private async Task<string> GenerateComplaintNumber(int schoolId)
        {
            var count = await _context.Complaints.CountAsync(c => c.SchoolRegistrationId == schoolId);
            return $"CMP-{DateTime.UtcNow.Year}-{(count + 1):D4}";
        }
    }
}
