using AutoMapper;
using Microsoft.EntityFrameworkCore;
using School.Domain.School;
using School.Infrastructure.Repositories.IRepositories;
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
    public class SchoolSubscriptionService : ISchoolSubscriptionService
    {
        private readonly ISchoolSubscriptionRepository _repo;
        private readonly IMapper _mapper;

        public SchoolSubscriptionService(ISchoolSubscriptionRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<PagedResponse<SchoolSubscriptionDto>> GetAllsAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, bool? isActive = null)
        {
            try
            {
                var query = _repo.GetAllQueryable();

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    query = query.Where(x => x.TransactionId != null && x.TransactionId.ToLower().Contains(searchTerm));
                }

                if (isActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == isActive.Value);
                }

                var totalCount = await query.CountAsync();
                var items = await query.OrderByDescending(x => x.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
                
                return new PagedResponse<SchoolSubscriptionDto>
                {
                    Success = true,
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK,
                    Data = _mapper.Map<IEnumerable<SchoolSubscriptionDto>>(items),
                    TotalRecords = totalCount,
                    CurrentPage = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                return new PagedResponse<SchoolSubscriptionDto>
                {
                    Success = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = new List<SchoolSubscriptionDto>()
                };
            }
        }

        public async Task<APIResponse<SchoolSubscriptionDto>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null) return new APIResponse<SchoolSubscriptionDto> { Success = false, StatusCode = HttpStatusCode.NotFound };
                return new APIResponse<SchoolSubscriptionDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = _mapper.Map<SchoolSubscriptionDto>(entity) };
            }
            catch (Exception ex)
            {
                return new APIResponse<SchoolSubscriptionDto> { Success = false, Message = ex.Message, StatusCode = HttpStatusCode.InternalServerError };
            }
        }

        public async Task<APIResponse<SchoolSubscriptionDto>> AddAsync(SchoolSubscriptionModel model)
        {
            try
            {
                var entity = _mapper.Map<SchoolSubscription>(model);
                var res = await _repo.AddAsync(entity);
                if (res > 0) return new APIResponse<SchoolSubscriptionDto> { Success = true, StatusCode = HttpStatusCode.Created, Data = _mapper.Map<SchoolSubscriptionDto>(entity) };
                return new APIResponse<SchoolSubscriptionDto> { Success = false, StatusCode = HttpStatusCode.BadRequest };
            }
            catch (Exception ex)
            {
                return new APIResponse<SchoolSubscriptionDto> { Success = false, Message = ex.Message, StatusCode = HttpStatusCode.InternalServerError };
            }
        }

        public async Task<APIResponse<SchoolSubscriptionDto>> EditAsync(SchoolSubscriptionModel model)
        {
            try
            {
                var entity = _mapper.Map<SchoolSubscription>(model);
                var res = await _repo.UpdateAsync(entity);
                if (res > 0) return new APIResponse<SchoolSubscriptionDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = _mapper.Map<SchoolSubscriptionDto>(entity) };
                return new APIResponse<SchoolSubscriptionDto> { Success = false, StatusCode = HttpStatusCode.BadRequest };
            }
            catch (Exception ex)
            {
                return new APIResponse<SchoolSubscriptionDto> { Success = false, Message = ex.Message, StatusCode = HttpStatusCode.InternalServerError };
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



