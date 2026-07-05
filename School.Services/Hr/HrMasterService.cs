using AutoMapper;
using Microsoft.EntityFrameworkCore;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork.Interfaces;
using School_DTOs;
using School_DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace School.Services.Hr
{
    public class HrMasterService<TEntity> : IHrMasterService<TEntity> where TEntity : class, global::School.Domain.BaseEntity.IAuditEntity<int>, global::School.Domain.BaseEntity.ITenantEntity
    {
        private readonly IRepository<TEntity> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HrMasterService(IRepository<TEntity> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponse<PagedResponse<object>>> GetAllAsync(PaginationFilterDto filter)
        {
            var query = _repository.List();

            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                var lowerSearch = filter.SearchText.ToLower();
                query = query.Where(e => EF.Property<string>(e, "Name").ToLower().Contains(lowerSearch) || 
                                         EF.Property<string>(e, "Code").ToLower().Contains(lowerSearch));
            }

            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                bool isDesc = filter.SortDirection?.ToLower() == "desc";
                query = isDesc 
                    ? query.OrderByDescending(e => EF.Property<object>(e, filter.SortBy))
                    : query.OrderBy(e => EF.Property<object>(e, filter.SortBy));
            }
            else
            {
                query = query.OrderByDescending(e => EF.Property<int>(e, "Id"));
            }

            var totalRecords = await query.CountAsync();
            var entities = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var pagedResponse = new PagedResponse<object>
            {
                Data = _mapper.Map<IEnumerable<object>>(entities),
                CurrentPage = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalRecords = totalRecords,
                
            };

            return new APIResponse<PagedResponse<object>> { Success = true, StatusCode = HttpStatusCode.OK, Data = pagedResponse };
        }

        public async Task<APIResponse<object>> GetByIdAsync(int id)
        {
            var entity = await _repository.FindAsync(e => EF.Property<int>(e, "Id") == id);
            if (entity == null)
                return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Record not found." };

            return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.OK, Data = _mapper.Map<object>(entity) };
        }

        public async Task<APIResponse<object>> CreateAsync(object dto, string username)
        {
            var entity = _mapper.Map<TEntity>(dto);
            entity.CreatedBy = username;
            
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();

            return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.Created, Data = _mapper.Map<object>(entity) };
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, object dto, string username)
        {
            var existingEntity = await _repository.FindAsync(e => EF.Property<int>(e, "Id") == id);
            if (existingEntity == null)
                return new APIResponse<object> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Record not found." };

            _mapper.Map(dto, existingEntity);
            existingEntity.UpdatedBy = username;
            existingEntity.UpdatedDate = DateTime.Now;

            _repository.Update(existingEntity);
            await _unitOfWork.CommitAsync();

            return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.OK, Data = _mapper.Map<object>(existingEntity) };
        }

        public async Task<APIResponse<bool>> DeleteAsync(int id, string username)
        {
            var entity = await _repository.FindAsync(e => EF.Property<int>(e, "Id") == id);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Record not found." };

            entity.IsDeleted = true;
            entity.UpdatedBy = username;
            entity.UpdatedDate = DateTime.Now;

            _repository.Update(entity);
            await _unitOfWork.CommitAsync();

            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Data = true, Message = "Deleted successfully." };
        }

        public async Task<APIResponse<bool>> ToggleStatusAsync(int id, string username)
        {
            var entity = await _repository.FindAsync(e => EF.Property<int>(e, "Id") == id);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Record not found." };

            var currentStatus = EF.Property<string>(entity, "Status");
            var newStatus = currentStatus == "active" ? "inactive" : "active";
            
            // Set property value using reflection if possible or EF Core tracking
            entity.GetType().GetProperty("Status")?.SetValue(entity, newStatus);
            
            entity.UpdatedBy = username;
            entity.UpdatedDate = DateTime.Now;

            _repository.Update(entity);
            await _unitOfWork.CommitAsync();

            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Data = true };
        }
    }
}

