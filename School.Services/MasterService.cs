using AutoMapper;
using School.Infrastructure;
using School.Infrastructure.Seeds;
using School.Services.Interfaces;
using School.Utilities.Enums;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Net;
using System.Text.Json;

namespace School.Services
{
    public class MasterService : IMasterService
    {
        private readonly IMapper _mapper;
        private readonly SchoolDbContext _dbContext;
        private readonly IDistributedCache _cache;
        public MasterService(IMapper mapper, SchoolDbContext dbContext, IDistributedCache cache)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _cache = cache;
        }
        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetCitiesAsync(int id)
        {
            var entity = await _dbContext.Cities.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.CityCode }).ToListAsync();
            if (entity != null)
                return new APIResponse<IEnumerable<DropdownDto>>
                {
                    Data = entity,
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            else
                return new APIResponse<IEnumerable<DropdownDto>>
                {
                    Message = CommonResource.RecordNotFound,
                    Success = false,
                    StatusCode = HttpStatusCode.OK
                };
        }
        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetStatesAsync(int id)
        {
            string key = "States";

            if (!TryGetFromCache<IEnumerable<DropdownDto>>(key, out var result) || result == null)
            {
                var entity = await _dbContext.States.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.StateCode }).ToListAsync();
                if (entity == null || !entity.Any())
                {
                    return new APIResponse<IEnumerable<DropdownDto>>
                    {
                        Message = CommonResource.RecordNotFound,
                        Success = false,
                        StatusCode = HttpStatusCode.OK
                    };
                }

                _cache.SetString(key, JsonSerializer.Serialize(entity));
            }

            return new APIResponse<IEnumerable<DropdownDto>>
            {
                Data = result,
                Message = CommonResource.FetchSuccess,
                Success = true,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetStatusAsync()
        {
            var enity = await _dbContext.Statuses.Select(x => new DropdownDto { Name = x.Name, Id = x.Id }).ToListAsync();
            if (enity != null)
                return new APIResponse<IEnumerable<DropdownDto>>
                {
                    Data = _mapper.Map<List<DropdownDto>>(enity),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            else
                return new APIResponse<IEnumerable<DropdownDto>>
                {
                    Message = CommonResource.RecordNotFound,
                    Success = false,
                    StatusCode = HttpStatusCode.OK
                };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetCoursesAsync(int courseTypeId)
        {
            string key = "Courses";

            if (!TryGetFromCache<IEnumerable<DropdownDto>>(key, out var result) || result == null)
            {
                var entity = await _dbContext.Courses.Where(x => !x.IsDeleted && (int)x.CourseType! == courseTypeId).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.CourseCode }).ToListAsync();
                if (entity == null || !entity.Any())
                {
                    return new APIResponse<IEnumerable<DropdownDto>>
                    {
                        Message = CommonResource.RecordNotFound,
                        Success = false,
                        StatusCode = HttpStatusCode.OK
                    };
                }

                _cache.SetString(key, JsonSerializer.Serialize(entity));
            }

            return new APIResponse<IEnumerable<DropdownDto>>
            {
                Data = result,
                Message = CommonResource.FetchSuccess,
                Success = true,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetAcademicYearAsync()
        {
            string key = "AcademicYear";

            if (!TryGetFromCache<IEnumerable<DropdownDto>>(key, out var result) || result == null)
            {
                var entity = await _dbContext.AcademicYears.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.YearName, Id = x.Id, Code = x.YearName }).ToListAsync();
                if (entity == null || !entity.Any())
                {
                    return new APIResponse<IEnumerable<DropdownDto>>
                    {
                        Message = CommonResource.RecordNotFound,
                        Success = false,
                        StatusCode = HttpStatusCode.OK
                    };
                }

                _cache.SetString(key, JsonSerializer.Serialize(entity));
            }

            return new APIResponse<IEnumerable<DropdownDto>>
            {
                Data = result,
                Message = CommonResource.FetchSuccess,
                Success = true,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetAffiliationBoardsAsync()
        {
            var data = await _dbContext.AffiliationBoards
                .Where(x => x.IsActive && !x.IsDeleted)
                .Select(x => new DropdownDto { Id = x.Id, Name = x.Name })
                .ToListAsync();

            return new APIResponse<IEnumerable<DropdownDto>>
            {
                Data = data,
                Success = true,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetSchoolTypesAsync()
        {
            var data = await _dbContext.SchoolTypes
                .Where(x => x.IsActive && !x.IsDeleted)
                .Select(x => new DropdownDto { Id = x.Id, Name = x.Name })
                .ToListAsync();

            return new APIResponse<IEnumerable<DropdownDto>>
            {
                Data = data,
                Success = true,
                StatusCode = HttpStatusCode.OK
            };
        }
        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetSchoolMediumsAsync()
        {
            var data = await _dbContext.SchoolMediums
                .Where(x => x.IsActive && !x.IsDeleted)
                .Select(x => new DropdownDto { Id = x.Id, Name = x.Name })
                .ToListAsync();

            return new APIResponse<IEnumerable<DropdownDto>>
            {
                Data = data,
                Success = true,
                StatusCode = HttpStatusCode.OK
            };
        }

        private bool TryGetFromCache<T>(string key, out T? value)
        {
            var cachedData = _cache.GetString(key);
            if (!string.IsNullOrEmpty(cachedData))
            {
                value = JsonSerializer.Deserialize<T>(cachedData);
                return true;
            }

            value = default;
            return false;
        }

        public async Task<APIResponse<IEnumerable<RoleDto>>> GetRolesAsync()
        {
            var roles = await _dbContext.Roles.Where(x => x.Name != Roles.SuperAdmin.ToString()).Select(x => new RoleDto
            {
                Id = x.Id,
                Name = x.Name!,
                NormalizedName = x.NormalizedName!

            }).ToListAsync();
            if (roles != null)
            {
                return new APIResponse<IEnumerable<RoleDto>>
                {
                    Data = roles,
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<IEnumerable<RoleDto>>
                {
                    Message = CommonResource.RecordNotFound,
                    Success = false,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }
    }
}
