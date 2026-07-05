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
    public class AffiliationBoardService : IAffiliationBoardService
    {
        private readonly IAffiliationBoardRepository _repo;
        private readonly IMapper _mapper;

        public AffiliationBoardService(IAffiliationBoardRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<PagedResponse<AffiliationBoardDto>> GetAllsAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, bool? isActive = null)
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
                
                return new PagedResponse<AffiliationBoardDto>
                {
                    Success = true,
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK,
                    Data = _mapper.Map<IEnumerable<AffiliationBoardDto>>(items),
                    TotalRecords = totalCount,
                    CurrentPage = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                return new PagedResponse<AffiliationBoardDto>
                {
                    Success = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = new List<AffiliationBoardDto>()
                };
            }
        }

        public async Task<APIResponse<AffiliationBoardDto>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null) return new APIResponse<AffiliationBoardDto> { Success = false, StatusCode = HttpStatusCode.NotFound };
                return new APIResponse<AffiliationBoardDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = _mapper.Map<AffiliationBoardDto>(entity) };
            }
            catch (Exception ex)
            {
                return new APIResponse<AffiliationBoardDto> { Success = false, Message = ex.Message, StatusCode = HttpStatusCode.InternalServerError };
            }
        }

        public async Task<APIResponse<AffiliationBoardDto>> AddAsync(AffiliationBoardModel model)
        {
            try
            {
                var entity = _mapper.Map<AffiliationBoard>(model);
                var res = await _repo.AddAsync(entity);
                if (res > 0) return new APIResponse<AffiliationBoardDto> { Success = true, StatusCode = HttpStatusCode.Created, Data = _mapper.Map<AffiliationBoardDto>(entity) };
                return new APIResponse<AffiliationBoardDto> { Success = false, StatusCode = HttpStatusCode.BadRequest };
            }
            catch (Exception ex)
            {
                return new APIResponse<AffiliationBoardDto> { Success = false, Message = ex.Message, StatusCode = HttpStatusCode.InternalServerError };
            }
        }

        public async Task<APIResponse<AffiliationBoardDto>> EditAsync(AffiliationBoardModel model)
        {
            try
            {
                var entity = _mapper.Map<AffiliationBoard>(model);
                var res = await _repo.UpdateAsync(entity);
                if (res > 0) return new APIResponse<AffiliationBoardDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = _mapper.Map<AffiliationBoardDto>(entity) };
                return new APIResponse<AffiliationBoardDto> { Success = false, StatusCode = HttpStatusCode.BadRequest };
            }
            catch (Exception ex)
            {
                return new APIResponse<AffiliationBoardDto> { Success = false, Message = ex.Message, StatusCode = HttpStatusCode.InternalServerError };
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



