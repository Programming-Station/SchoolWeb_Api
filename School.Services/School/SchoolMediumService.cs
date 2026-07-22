using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using School.Domain.School;
using School.Infrastructure.Repositories.School;
using School.Services.School.ISchoolServices;
using School_DTOs;
using School_DTOs.School;

namespace School.Services.School
{
    public class SchoolMediumService : ISchoolMediumService
    {
        private readonly ISchoolMediumRepository _repo;
        private readonly IMapper _mapper;

        public SchoolMediumService(ISchoolMediumRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<PagedResponse<SchoolMediumDto>> GetAllsAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, bool? isActive = null)
        {
            try
            {
                var query = _repo.GetAllQueryable();

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    query = query.Where(x => x.Name.ToLower().Contains(searchTerm));
                }

                if (isActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == isActive.Value);
                }

                var totalCount = await query.CountAsync();
                var items = await query.OrderByDescending(x => x.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

                return new PagedResponse<SchoolMediumDto>
                {
                    Success = true,
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK,
                    Data = _mapper.Map<IEnumerable<SchoolMediumDto>>(items),
                    TotalRecords = totalCount,
                    CurrentPage = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                return new PagedResponse<SchoolMediumDto>
                {
                    Success = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = new List<SchoolMediumDto>()
                };
            }
        }

        public async Task<APIResponse<SchoolMediumDto>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null) return new APIResponse<SchoolMediumDto> { Success = false, StatusCode = HttpStatusCode.NotFound };
                return new APIResponse<SchoolMediumDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = _mapper.Map<SchoolMediumDto>(entity) };
            }
            catch (Exception ex)
            {
                return new APIResponse<SchoolMediumDto> { Success = false, Message = ex.Message, StatusCode = HttpStatusCode.InternalServerError };
            }
        }

        public async Task<APIResponse<SchoolMediumDto>> AddAsync(SchoolMediumModel model)
        {
            try
            {
                var entity = _mapper.Map<SchoolMedium>(model);
                var res = await _repo.AddAsync(entity);
                if (res > 0) return new APIResponse<SchoolMediumDto> { Success = true, StatusCode = HttpStatusCode.Created, Data = _mapper.Map<SchoolMediumDto>(entity) };
                return new APIResponse<SchoolMediumDto> { Success = false, StatusCode = HttpStatusCode.BadRequest };
            }
            catch (Exception ex)
            {
                return new APIResponse<SchoolMediumDto> { Success = false, Message = ex.Message, StatusCode = HttpStatusCode.InternalServerError };
            }
        }

        public async Task<APIResponse<SchoolMediumDto>> EditAsync(SchoolMediumModel model)
        {
            try
            {
                var entity = _mapper.Map<SchoolMedium>(model);
                var res = await _repo.UpdateAsync(entity);
                if (res > 0) return new APIResponse<SchoolMediumDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = _mapper.Map<SchoolMediumDto>(entity) };
                return new APIResponse<SchoolMediumDto> { Success = false, StatusCode = HttpStatusCode.BadRequest };
            }
            catch (Exception ex)
            {
                return new APIResponse<SchoolMediumDto> { Success = false, Message = ex.Message, StatusCode = HttpStatusCode.InternalServerError };
            }
        }

        public async Task<APIResponse> DeleteAsync(int id)
        {
            try
            {
                var res = await _repo.DeleteAsync(id);
                if (res > 0) return new APIResponse { Success = true, StatusCode = HttpStatusCode.OK };
                return new APIResponse { Success = false, StatusCode = HttpStatusCode.BadRequest };
            }
            catch (Exception ex)
            {
                return new APIResponse { Success = false, Message = ex.Message, StatusCode = HttpStatusCode.InternalServerError };
            }
        }
    }
}




