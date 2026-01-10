using AutoMapper;
using School.Domain.Website;
using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Models.Website;
using School.Services.Interfaces;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.Website;
using Microsoft.EntityFrameworkCore;
using System.Net;
using School.Infrastructure;

namespace School.Services
{
    public class EnquiryService : IEnquiryService
    {
        private readonly IEnquiryRepository _enquiryRepository;
        private readonly IMapper _mapper;
        private readonly SchoolDbContext _context;

        public EnquiryService(IEnquiryRepository enquiryRepository, IMapper mapper, SchoolDbContext context)
        {
            _enquiryRepository = enquiryRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<APIResponse<EnquiryDto>> AddEnquiryAsync(EnquiryModel model, string? ipAddress = null, string? userAgent = null)
        {
            // Get course name if CourseId is provided
            string? courseName = null;
            if (model.CourseId.HasValue && model.CourseId.Value > 0)
            {
                var course = await _context.Courses
                    .FirstOrDefaultAsync(c => c.Id == model.CourseId.Value && !c.IsDeleted);
                courseName = course?.Name;
            }

            // Get or set StatusId - if not provided or invalid, find "New" status by name
            int statusId = model.StatusId;
            if (statusId <= 0)
            {
                var newStatus = await _context.Statuses
                    .FirstOrDefaultAsync(s => s.Name.ToLower() == "new");
                if (newStatus != null)
                {
                    statusId = newStatus.Id;
                }
                else
                {
                    // Fallback: use first status if "New" doesn't exist
                    var firstStatus = await _context.Statuses.FirstOrDefaultAsync();
                    statusId = firstStatus?.Id ?? 14; // DefaultStatus.New = 14
                }
            }
            else
            {
                // Validate that the provided StatusId exists
                var statusExists = await _context.Statuses
                    .AnyAsync(s => s.Id == statusId);
                if (!statusExists)
                {
                    // If provided StatusId doesn't exist, find "New" status
                    var newStatus = await _context.Statuses
                        .FirstOrDefaultAsync(s => s.Name.ToLower() == "new");
                    statusId = newStatus?.Id ?? 14;
                }
            }

            // Generate enquiry number
            var enquiryNo = await _enquiryRepository.GenerateEnquiryNoAsync();

            // Map Model to Entity
            var entity = _mapper.Map<Enquiry>(model);
            entity.EnquiryFromNo = enquiryNo;
            entity.CourseName = courseName ?? model.CourseName;
            entity.StatusId = statusId;
            entity.IpAddress = ipAddress;
            entity.UserAgent = userAgent;
            entity.CreatedBy = model.CreatedBy ?? "Public";
            entity.CreatedDate = DateTime.UtcNow;

            // Trim string fields
            entity.Name = entity.Name?.Trim() ?? "";
            entity.Email = entity.Email?.Trim().ToLower() ?? "";
            entity.Mobile = entity.Mobile?.Trim() ?? "";
            entity.Subject = entity.Subject?.Trim();
            entity.Message = entity.Message?.Trim() ?? "";
            entity.Address = entity.Address?.Trim();
            entity.City = entity.City?.Trim();
            entity.PinCode = entity.PinCode?.Trim();

            entity = await _enquiryRepository.AddEnquiryAsync(entity);

            if (entity != null && entity.Id == 0)
            {
                return new APIResponse<EnquiryDto>
                {
                    Success = false,
                    Data = MapToDto(entity),
                    Message = string.Format(CommonResource.AlreadyExists, typeof(Enquiry).Name, model.Name),
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
            else if (entity != null && entity.Id > 0)
            {
                // Reload with Status navigation property
                var savedEntity = await _enquiryRepository.GetEnquiryByIdAsync(entity.Id);
                return new APIResponse<EnquiryDto>
                {
                    Success = true,
                    Data = MapToDto(savedEntity),
                    Message = "Enquiry submitted successfully. We will get back to you soon.",
                    StatusCode = HttpStatusCode.Created
                };
            }
            else
            {
                return new APIResponse<EnquiryDto>
                {
                    Success = false,
                    Message = CommonResource.AddFailed,
                    StatusCode = HttpStatusCode.Forbidden
                };
            }
        }

        public async Task<APIResponse<EnquiryDto>> GetEnquiryByIdAsync(int id)
        {
            var result = await _enquiryRepository.GetEnquiryByIdAsync(id);

            if (result != null && result.Id > 0)
            {
                return new APIResponse<EnquiryDto>
                {
                    Data = MapToDto(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<EnquiryDto>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.NotFound
                };
            }
        }

        public async Task<APIResponse<IEnumerable<EnquiryDto>>> GetAllEnquiriesAsync(int? statusId = null, int? pageNumber = null, int? pageSize = null)
        {
            var result = await _enquiryRepository.GetAllAsync(statusId, pageNumber, pageSize);

            if (result != null && result.Any())
            {
                var dtos = result.Select(MapToDto);
                return new APIResponse<IEnumerable<EnquiryDto>>
                {
                    Data = dtos,
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                };
            }
            else
            {
                return new APIResponse<IEnumerable<EnquiryDto>>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK,
                };
            }
        }

        public async Task<PagedResponse<EnquiryDto>> GetAllPagedAsync(int pageNumber = 1, int pageSize = 10, int? statusId = null)
        {
            var result = await _enquiryRepository.GetAllAsync(statusId, pageNumber, pageSize);
            var totalCount = await _enquiryRepository.GetTotalCountAsync(statusId);

            if (result != null && result.Any())
            {
                return new PagedResponse<EnquiryDto>
                {
                    Data = result.Select(MapToDto),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    TotalRecords = totalCount,
                    PageSize = pageSize,
                    CurrentPage = pageNumber,
                };
            }
            else
            {
                return new PagedResponse<EnquiryDto>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK,
                };
            }
        }

        public async Task<APIResponse> UpdateEnquiryAsync(EnquiryModel model)
        {
            if (!model.Id.HasValue || model.Id.Value <= 0)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Invalid enquiry ID",
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }

            var existingEntity = await _enquiryRepository.GetEnquiryByIdAsync(model.Id.Value);
            if (existingEntity == null || existingEntity.Id == 0)
            {
                return new APIResponse
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            // Get course name if CourseId is provided
            string? courseName = null;
            if (model.CourseId.HasValue && model.CourseId.Value > 0)
            {
                var course = await _context.Courses
                    .FirstOrDefaultAsync(c => c.Id == model.CourseId.Value && !c.IsDeleted);
                courseName = course?.Name;
            }

            // Update properties from model
            existingEntity.Name = model.Name.Trim();
            existingEntity.Email = model.Email.Trim().ToLower();
            existingEntity.Mobile = model.Mobile.Trim();
            existingEntity.Subject = model.Subject?.Trim();
            existingEntity.Message = model.Message.Trim();
            existingEntity.Address = model.Address?.Trim();
            existingEntity.City = model.City?.Trim();
            existingEntity.PinCode = model.PinCode?.Trim();
            existingEntity.CourseId = model.CourseId;
            existingEntity.CourseName = courseName ?? model.CourseName;
            existingEntity.UpdatedBy = model.UpdatedBy;
            existingEntity.UpdatedDate = DateTime.UtcNow;

            // Update StatusId if provided and valid
            if (model.StatusId > 0)
            {
                var statusExists = await _context.Statuses.AnyAsync(s => s.Id == model.StatusId);
                if (statusExists)
                {
                    existingEntity.StatusId = model.StatusId;
                }
            }

            var result = await _enquiryRepository.UpdateEnquiryAsync(existingEntity);
            if (result > 0)
            {
                return new APIResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Message = CommonResource.UpdateSuccess
                };
            }
            else
            {
                return new APIResponse
                {
                    Message = CommonResource.UpdateFailed,
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
        }

        public async Task<APIResponse> UpdateEnquiryStatusAsync(int id, int statusId, string? adminReply = null, string? repliedBy = null)
        {
            var result = await _enquiryRepository.UpdateEnquiryStatusAsync(id, statusId, adminReply, repliedBy);
            if (result > 0)
            {
                return new APIResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Message = CommonResource.UpdateSuccess
                };
            }
            else
            {
                return new APIResponse
                {
                    Message = CommonResource.UpdateFailed,
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
        }

        public async Task<APIResponse> DeleteEnquiryAsync(int id)
        {
            int changes = await _enquiryRepository.DeleteEnquiryAsync(id);
            if (changes > 0)
                return new APIResponse
                {
                    Success = true,
                    Message = CommonResource.DeleteSuccess,
                    StatusCode = HttpStatusCode.OK,
                };
            else
                return new APIResponse
                {
                    Message = CommonResource.DeleteFailed,
                    StatusCode = HttpStatusCode.BadRequest,
                };
        }

        public async Task<APIResponse<int>> GetEnquiryCountAsync(int? statusId = null)
        {
            try
            {
                var count = await _enquiryRepository.GetTotalCountAsync(statusId);
                return new APIResponse<int>
                {
                    Success = true,
                    Message = CommonResource.FetchSuccess,
                    StatusCode = HttpStatusCode.OK,
                    Data = count
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<int>
                {
                    Success = false,
                    Message = $"Failed to get count: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        private EnquiryDto MapToDto(Enquiry entity)
        {
            var dto = _mapper.Map<EnquiryDto>(entity);

            // Map StatusName from navigation property
            dto.StatusName = entity.Status?.Name;

            // Map CourseName from navigation property (if not already set)
            if (string.IsNullOrEmpty(dto.CourseName) && entity.Course != null)
            {
                dto.CourseName = entity.Course.Name;
            }

            return dto;
        }
    }
}
