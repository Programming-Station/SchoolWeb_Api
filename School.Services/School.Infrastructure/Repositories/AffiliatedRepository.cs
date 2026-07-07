using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Models.AffiliationCollege;
using School_DTOs;
using School_DTOs.AffiliationCollege;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace School.Infrastructure.Repositories
{
    public class AffiliatedRepository : Repository<Affiliated>, IAffiliatedRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public AffiliatedRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<AffiliatedDto>> AddAffiliationCollegeAsync(AffiliatedModel model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.CollegeCode))
                {
                    var exists = await _context.Affiliateds
                        .AnyAsync(x => x.CollegeCode == model.CollegeCode && !x.IsDeleted);

                    if (exists)
                    {
                        return new APIResponse<AffiliatedDto>
                        {
                            Success = false,
                            Message = "College code already exists",
                            StatusCode = HttpStatusCode.BadRequest
                        };
                    }
                }

                if (model.StateId <= 0)
                {
                    return new APIResponse<AffiliatedDto>
                    {
                        Success = false,
                        Message = "StateId is required and must be greater than 0",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                var state = await _context.States
                    .FirstOrDefaultAsync(s => s.Id == model.StateId && !s.IsDeleted);

                if (state == null)
                {
                    return new APIResponse<AffiliatedDto>
                    {
                        Success = false,
                        Message = $"State with ID {model.StateId} not found or has been deleted",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                if (!state.IsActive)
                {
                    return new APIResponse<AffiliatedDto>
                    {
                        Success = false,
                        Message = $"State '{state.Name}' is inactive. Please select an active state.",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                if (model.CityId <= 0)
                {
                    return new APIResponse<AffiliatedDto>
                    {
                        Success = false,
                        Message = "CityId is required and must be greater than 0",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                var city = await _context.Cities
                    .FirstOrDefaultAsync(c => c.Id == model.CityId && !c.IsDeleted);

                if (city == null)
                {
                    return new APIResponse<AffiliatedDto>
                    {
                        Success = false,
                        Message = $"City with ID {model.CityId} not found or has been deleted",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                if (city.StateId != model.StateId)
                {
                    return new APIResponse<AffiliatedDto>
                    {
                        Success = false,
                        Message = $"City '{city.Name}' does not belong to selected State '{state.Name}'",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                if (!city.IsActive)
                {
                    return new APIResponse<AffiliatedDto>
                    {
                        Success = false,
                        Message = $"City '{city.Name}' is inactive. Please select an active city.",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                var affiliated = new Affiliated
                {
                    CollegeName = model.CollegeName,
                    CollegeCode = model.CollegeCode,
                    UniversityName = model.UniversityName,
                    UniversityCode = model.UniversityCode,
                    StateId = model.StateId,
                    CityId = model.CityId,
                    Address = model.Address,
                    Pincode = model.Pincode,
                    ContactPerson = model.ContactPerson,
                    MobileNo = model.MobileNo,
                    Email = model.Email,
                    ImagePath = model.ImagePath,
                    IsActive = model.IsActive,
                    CreatedBy = model.CreatedBy,
                    CreatedDate = DateTime.UtcNow
                };

                Add(affiliated);
                await _unitOfWork.CommitAsync();

                var savedEntity = await _context.Affiliateds
                    .Include(x => x.State)
                    .FirstAsync(x => x.Id == affiliated.Id);

                return new APIResponse<AffiliatedDto>
                {
                    Success = true,
                    Message = "Affiliation college added successfully",
                    StatusCode = HttpStatusCode.Created,
                    Data = MapToDto(savedEntity)
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<AffiliatedDto>
                {
                    Success = false,
                    Message = $"Failed to add affiliation college: {ex.InnerException?.Message ?? ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }




        public async Task<APIResponse<AffiliatedDto>> GetAffiliationCollegeByIdAsync(int id)
        {
            try
            {
                var college = await _context.Affiliateds
                    .Include(x => x.State)
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

                if (college == null)
                {
                    return new APIResponse<AffiliatedDto>
                    {
                        Success = false,
                        Message = "Affiliation college not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                return new APIResponse<AffiliatedDto>
                {
                    Success = true,
                    Message = "Affiliation college fetched successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = MapToDto(college)
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<AffiliatedDto>
                {
                    Success = false,
                    Message = $"Failed to get affiliation college: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<IEnumerable<AffiliatedDto>>> GetAllAffiliationCollegesAsync(
            int? stateId = null,
            int? cityId = null,
            bool? isActive = null)
        {
            try
            {
                var query = _context.Affiliateds
                    .Include(x => x.State).ThenInclude(x => x.Cities)
                    .Where(x => !x.IsDeleted);

                if (stateId.HasValue)
                    query = query.Where(x => x.StateId == stateId.Value);

                if (cityId.HasValue)
                    query = query.Where(x => x.CityId == cityId.Value);

                if (isActive.HasValue)
                    query = query.Where(x => x.IsActive == isActive.Value);

                var list = await query
                    .OrderBy(x => x.CollegeName)
                    .ToListAsync();

                return new APIResponse<IEnumerable<AffiliatedDto>>
                {
                    Success = true,
                    Message = "Affiliation colleges fetched successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = list.Select(MapToDto)
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<IEnumerable<AffiliatedDto>>
                {
                    Success = false,
                    Message = $"Failed to get affiliation colleges: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse> UpdateAffiliationCollegeAsync(AffiliatedModel model)
        {
            try
            {
                if (model.Id <= 0)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Affiliation college ID is required and must be greater than 0",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                var college = await _context.Affiliateds
                    .FirstOrDefaultAsync(x => x.Id == model.Id && !x.IsDeleted);

                if (college == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Affiliation college not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                if (!string.IsNullOrEmpty(model.CollegeCode) && college.CollegeCode != model.CollegeCode)
                {
                    var exists = await _context.Affiliateds
                        .AnyAsync(x => x.CollegeCode == model.CollegeCode &&
                                       x.Id != model.Id &&
                                       !x.IsDeleted);

                    if (exists)
                    {
                        return new APIResponse
                        {
                            Success = false,
                            Message = "College code already exists",
                            StatusCode = HttpStatusCode.BadRequest
                        };
                    }
                }

                if (model.StateId <= 0)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "StateId is required and must be greater than 0",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                var state = await _context.States
                    .FirstOrDefaultAsync(s => s.Id == model.StateId && !s.IsDeleted);

                if (state == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = $"State with ID {model.StateId} not found or has been deleted",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                if (!state.IsActive)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = $"State '{state.Name}' is inactive. Please select an active state.",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                if (model.CityId <= 0)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "CityId is required and must be greater than 0",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                var city = await _context.Cities
                    .FirstOrDefaultAsync(c => c.Id == model.CityId && !c.IsDeleted);

                if (city == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = $"City with ID {model.CityId} not found or has been deleted",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                if (city.StateId != model.StateId)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = $"City '{city.Name}' does not belong to selected State '{state.Name}'",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                if (!city.IsActive)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = $"City '{city.Name}' is inactive. Please select an active city.",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                college.CollegeName = model.CollegeName;
                college.CollegeCode = model.CollegeCode;
                college.UniversityName = model.UniversityName;
                college.UniversityCode = model.UniversityCode;
                college.StateId = model.StateId;
                college.CityId = model.CityId;
                college.Address = model.Address;
                college.Pincode = model.Pincode;
                college.ContactPerson = model.ContactPerson;
                college.MobileNo = model.MobileNo;
                college.Email = model.Email;
                college.ImagePath = model.ImagePath;
                college.IsActive = model.IsActive;
                college.UpdatedBy = model.UpdatedBy;
                college.UpdatedDate = DateTime.UtcNow;

                Update(college);
                await _unitOfWork.CommitAsync();

                return new APIResponse
                {
                    Success = true,
                    Message = "Affiliation college updated successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"Failed to update affiliation college: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse> DeleteAffiliationCollegeAsync(int id)
        {
            try
            {
                var college = await _context.Affiliateds
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

                if (college == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Affiliation college not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                college.IsDeleted = true;
                college.UpdatedDate = DateTime.UtcNow;

                Delete(college);
                await _unitOfWork.CommitAsync();

                return new APIResponse
                {
                    Success = true,
                    Message = "Affiliation college deleted successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"Failed to delete affiliation college: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse> ToggleAffiliationCollegeStatusAsync(int id)
        {
            try
            {
                var college = await _context.Affiliateds
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

                if (college == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Affiliation college not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                college.IsActive = !college.IsActive;
                college.UpdatedDate = DateTime.UtcNow;

                Update(college);
                await _unitOfWork.CommitAsync();

                return new APIResponse
                {
                    Success = true,
                    Message = "Affiliation college status updated successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"Failed to toggle status: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        private AffiliatedDto MapToDto(Affiliated college)
        {
            return new AffiliatedDto
            {
                Id = college.Id,
                CollegeName = college.CollegeName,
                CollegeCode = college.CollegeCode ?? string.Empty,
                UniversityName = college.UniversityName,
                UniversityCode = college.UniversityCode,
                StateId = college.StateId,
                StateName = college.State?.Name ?? string.Empty,
                CityId = college.CityId,
                Address = college.Address,
                Pincode = college.Pincode,
                ContactPerson = college.ContactPerson,
                MobileNo = college.MobileNo,
                Email = college.Email,
                ImagePath = college.ImagePath,
                IsActive = college.IsActive,
                CreatedDate = college.CreatedDate,
                CreatedBy = college.CreatedBy,
                UpdatedDate = college.UpdatedDate,
                UpdatedBy = college.UpdatedBy
            };
        }
    }
}
