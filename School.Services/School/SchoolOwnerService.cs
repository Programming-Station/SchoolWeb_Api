using AutoMapper;
using Microsoft.EntityFrameworkCore;
using School.Domain.School;
using School.Infrastructure.Repositories.School;
using School.Services.School.ISchoolServices;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.School;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace School.Services.School
{
    public class SchoolOwnerService : ISchoolOwnerService
    {
        private readonly ISchoolOwnerRepository _repo;
        private readonly IMapper _mapper;

        public SchoolOwnerService(ISchoolOwnerRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<PagedResponse<SchoolOwnerDto>> GetAllsAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, bool? isActive = null)
        {
            try
            {
                IQueryable<SchoolOwner> query = _repo.GetAllQueryable()
                    .Include(x => x.SchoolRegistration)
                    .Include(x => x.ApplicationUser);

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    query = query.Where(x => x.SchoolRegistration.SchoolName.ToLower().Contains(searchTerm) 
                                          || x.ApplicationUser.UserName.ToLower().Contains(searchTerm));
                }

                if (isActive.HasValue)
                {
                    query = query.Where(x => x.IsLocked == !isActive.Value);
                }

                var totalCount = await query.CountAsync();
                var items = await query.OrderByDescending(x => x.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
                
                return new PagedResponse<SchoolOwnerDto>
                {
                    Success = true,
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK,
                    Data = _mapper.Map<IEnumerable<SchoolOwnerDto>>(items),
                    TotalRecords = totalCount,
                    CurrentPage = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                return new PagedResponse<SchoolOwnerDto>
                {
                    Success = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = new List<SchoolOwnerDto>()
                };
            }
        }

        public async Task<APIResponse<SchoolOwnerDto>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _repo.GetAllQueryable()
                    .Include(x => x.SchoolRegistration)
                    .Include(x => x.ApplicationUser)
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (entity == null) return new APIResponse<SchoolOwnerDto> { Success = false, StatusCode = HttpStatusCode.NotFound };
                return new APIResponse<SchoolOwnerDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = _mapper.Map<SchoolOwnerDto>(entity) };
            }
            catch (Exception ex)
            {
                return new APIResponse<SchoolOwnerDto> { Success = false, Message = ex.Message, StatusCode = HttpStatusCode.InternalServerError };
            }
        }

        public async Task<APIResponse<SchoolOwnerDto>> AddAsync(SchoolOwnerModel model)
        {
            try
            {
                var entity = _mapper.Map<SchoolOwner>(model);
                var res = await _repo.AddAsync(entity);
                if (res > 0) return new APIResponse<SchoolOwnerDto> { Success = true, StatusCode = HttpStatusCode.Created, Data = _mapper.Map<SchoolOwnerDto>(entity) };
                return new APIResponse<SchoolOwnerDto> { Success = false, StatusCode = HttpStatusCode.BadRequest };
            }
            catch (Exception ex)
            {
                return new APIResponse<SchoolOwnerDto> { Success = false, Message = ex.Message, StatusCode = HttpStatusCode.InternalServerError };
            }
        }

        public async Task<APIResponse<SchoolOwnerDto>> EditAsync(SchoolOwnerModel model)
        {
            try
            {
                var entity = _mapper.Map<SchoolOwner>(model);
                var res = await _repo.UpdateAsync(entity);
                if (res > 0) return new APIResponse<SchoolOwnerDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = _mapper.Map<SchoolOwnerDto>(entity) };
                return new APIResponse<SchoolOwnerDto> { Success = false, StatusCode = HttpStatusCode.BadRequest };
            }
            catch (Exception ex)
            {
                return new APIResponse<SchoolOwnerDto> { Success = false, Message = ex.Message, StatusCode = HttpStatusCode.InternalServerError };
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



