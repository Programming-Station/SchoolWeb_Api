using School.Domain.Website;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Models.Website;
using School_DTOs;
using School_DTOs.Website;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace School.Infrastructure.Repositories
{
    public class EnquiryRepository : Repository<Enquiry>, IEnquiryRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public EnquiryRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
        private async Task<string> GenerateEnquiryNoAsync()
        {
            var now = DateTime.UtcNow;

            string year = now.Year.ToString();       // 2025
            string month = now.Month.ToString("D2"); // 09

            string prefix = $"ENQ-{year}-{month}-";

            var lastNo = await _context.Enquiries
                .Where(x => x.EnquiryFromNo.StartsWith(prefix))
                .OrderByDescending(x => x.Id)
                .Select(x => x.EnquiryFromNo)
                .FirstOrDefaultAsync();

            int next = 1;

            if (!string.IsNullOrEmpty(lastNo))
            {
                var lastSeq = lastNo.Split('-').Last(); // 00001
                next = int.Parse(lastSeq) + 1;
            }

            return $"{prefix}{next:D5}";
        }

        public async Task<APIResponse<EnquiryDto>> AddEnquiryAsync(EnquiryModel model, string? ipAddress = null, string? userAgent = null)
        {
            try
            {
                string? courseName = null;
                if (model.CourseId.HasValue && model.CourseId.Value > 0)
                {
                    var course = await _context.Courses
                        .FirstOrDefaultAsync(c => c.Id == model.CourseId.Value && !c.IsDeleted);
                    courseName = course?.Name;
                }

                int statusId = model.StatusId;
                if (statusId <= 0)
                {
                    var newStatus = await _context.Statuses
                        .FirstOrDefaultAsync(s => s.Name.ToLower() == "New");
                    if (newStatus != null)
                    {
                        statusId = newStatus.Id;
                    }
                    else
                    {
                        var firstStatus = await _context.Statuses.FirstOrDefaultAsync();
                        statusId = firstStatus?.Id ?? 14;
                    }
                }
                else
                {
                    var statusExists = await _context.Statuses
                        .AnyAsync(s => s.Id == statusId);
                    if (!statusExists)
                    {
                        var newStatus = await _context.Statuses
                            .FirstOrDefaultAsync(s => s.Name.ToLower() == "new");
                        statusId = newStatus?.Id ?? 1;
                    }
                }

                var enquiryNo = await GenerateEnquiryNoAsync();
                var enquiry = new Enquiry
                {
                    EnquiryFromNo = enquiryNo,
                    Name = model.Name,
                    Email = model.Email,
                    Mobile = model.Mobile,
                    Subject = model.Subject,
                    Message = model.Message,
                    Address = model.Address,
                    City = model.City,
                    PinCode = model.PinCode,
                    CourseId = model.CourseId,
                    CourseName = courseName ?? model.CourseName,
                    StatusId = statusId,
                    IpAddress = ipAddress,
                    UserAgent = userAgent,
                    CreatedBy = model.CreatedBy ?? "Public",
                    CreatedDate = DateTime.UtcNow
                };

                Add(enquiry);
                await _unitOfWork.CommitAsync();

                var savedEnquiry = await _context.Enquiries
                    .Include(e => e.Status)
                    .FirstOrDefaultAsync(e => e.Id == enquiry.Id);

                var dto = MapToDto(savedEnquiry ?? enquiry);
                return new APIResponse<EnquiryDto>
                {
                    Success = true,
                    Message = "Enquiry submitted successfully. We will get back to you soon.",
                    StatusCode = HttpStatusCode.Created,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<EnquiryDto>
                {
                    Success = false,
                    Message = $"Failed to submit enquiry: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<EnquiryDto>> GetEnquiryByIdAsync(int id)
        {
            try
            {
                var enquiry = await _context.Enquiries
                    .Include(e => e.Status)
                    .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

                if (enquiry == null)
                {
                    return new APIResponse<EnquiryDto>
                    {
                        Success = false,
                        Message = "Enquiry not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                return new APIResponse<EnquiryDto>
                {
                    Success = true,
                    Message = "Enquiry fetched successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = MapToDto(enquiry)
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<EnquiryDto>
                {
                    Success = false,
                    Message = $"Failed to get enquiry: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<IEnumerable<EnquiryDto>>> GetAllEnquiriesAsync(int? statusId = null, int? pageNumber = null, int? pageSize = null)
        {
            try
            {
                var query = _context.Enquiries
                    .Include(e => e.Status)
                    .Where(e => !e.IsDeleted);

                if (statusId.HasValue && statusId.Value > 0)
                {
                    query = query.Where(e => e.StatusId == statusId.Value);
                }

                if (pageNumber.HasValue && pageSize.HasValue && pageNumber.Value > 0 && pageSize.Value > 0)
                {
                    var skip = (pageNumber.Value - 1) * pageSize.Value;
                    query = query.Skip(skip).Take(pageSize.Value);
                }

                var enquiries = await query
                    .OrderByDescending(e => e.CreatedDate)
                    .ToListAsync();

                var dtos = enquiries.Select(MapToDto);

                return new APIResponse<IEnumerable<EnquiryDto>>
                {
                    Success = true,
                    Message = "Enquiries fetched successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = dtos
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<IEnumerable<EnquiryDto>>
                {
                    Success = false,
                    Message = $"Failed to get enquiries: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse> UpdateEnquiryStatusAsync(int id, int statusId, string? adminReply = null, string? repliedBy = null)
        {
            try
            {
                var enquiry = await _context.Enquiries
                    .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

                if (enquiry == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Enquiry not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                enquiry.StatusId = statusId;
                enquiry.UpdatedDate = DateTime.UtcNow;
                enquiry.UpdatedBy = repliedBy;

                if (!string.IsNullOrEmpty(adminReply))
                {
                    enquiry.AdminReply = adminReply;
                    enquiry.RepliedDate = DateTime.UtcNow;
                    enquiry.RepliedBy = repliedBy;
                }

                Update(enquiry);
                await _unitOfWork.CommitAsync();

                return new APIResponse
                {
                    Success = true,
                    Message = "Enquiry status updated successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"Failed to update enquiry status: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse> DeleteEnquiryAsync(int id)
        {
            try
            {
                var enquiry = await _context.Enquiries
                    .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

                if (enquiry == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Enquiry not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                enquiry.IsDeleted = true;
                enquiry.UpdatedDate = DateTime.UtcNow;
                Delete(enquiry);
                await _unitOfWork.CommitAsync();

                return new APIResponse
                {
                    Success = true,
                    Message = "Enquiry deleted successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"Failed to delete enquiry: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<int>> GetEnquiryCountAsync(int? statusId = null)
        {
            try
            {
                var query = _context.Enquiries
                    .Where(e => !e.IsDeleted);

                if (statusId.HasValue && statusId.Value > 0)
                {
                    query = query.Where(e => e.StatusId == statusId.Value);
                }

                var count = await query.CountAsync();

                return new APIResponse<int>
                {
                    Success = true,
                    Message = "Enquiry count fetched successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = count
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<int>
                {
                    Success = false,
                    Message = $"Failed to get enquiry count: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        private EnquiryDto MapToDto(Enquiry enquiry)
        {
            return new EnquiryDto
            {
                Id = enquiry.Id,
                EnquiryFromNo = enquiry.EnquiryFromNo,
                Name = enquiry.Name,
                Email = enquiry.Email,
                Mobile = enquiry.Mobile,
                Subject = enquiry.Subject,
                Message = enquiry.Message,
                Address = enquiry.Address,
                City = enquiry.City,
                PinCode = enquiry.PinCode,
                CourseId = enquiry.CourseId,
                CourseName = enquiry.CourseName,
                StatusId = enquiry.StatusId,
                StatusName = enquiry.Status?.Name,
                AdminReply = enquiry.AdminReply,
                RepliedDate = enquiry.RepliedDate,
                RepliedBy = enquiry.RepliedBy,
                IpAddress = enquiry.IpAddress,
                UserAgent = enquiry.UserAgent,
                CreatedDate = enquiry.CreatedDate,
                CreatedBy = enquiry.CreatedBy,
                UpdatedDate = enquiry.UpdatedDate,
                UpdatedBy = enquiry.UpdatedBy
            };
        }
    }
}

