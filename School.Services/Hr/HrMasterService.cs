using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using School.Infrastructure.UnitOfWork.Interfaces;
using School_DTOs;
using School_DTOs.Common;

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
                Data = _mapper.Map<IEnumerable<global::School_DTOs.Hr.HrMasterDto>>(entities),
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

            return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.OK, Data = _mapper.Map<global::School_DTOs.Hr.HrMasterDto>(entity) };
        }

        public async Task<APIResponse<object>> CreateAsync(object dto, string username)
        {
            var entity = _mapper.Map<TEntity>(dto);
            entity.CreatedBy = username;

            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();

            return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.Created, Data = _mapper.Map<global::School_DTOs.Hr.HrMasterDto>(entity) };
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

            return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.OK, Data = _mapper.Map<global::School_DTOs.Hr.HrMasterDto>(existingEntity) };
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

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetLookupAsync()
        {
            var entities = await _repository.List().ToListAsync();
            var result = entities.Select(e => new DropdownDto
            {
                Id = (int)(e.GetType().GetProperty("Id")?.GetValue(e) ?? 0),
                Name = e.GetType().GetProperty("Name")?.GetValue(e)?.ToString() ?? string.Empty,
                Code = e.GetType().GetProperty("Code")?.GetValue(e)?.ToString()
            });
            return new APIResponse<IEnumerable<DropdownDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = result };
        }

        public async Task<APIResponse<bool>> BulkDeleteAsync(IEnumerable<int> ids, string username)
        {
            foreach (var id in ids)
            {
                var entity = await _repository.FindAsync(e => EF.Property<int>(e, "Id") == id);
                if (entity != null)
                {
                    entity.IsDeleted = true;
                    entity.UpdatedBy = username;
                    entity.UpdatedDate = DateTime.Now;
                    _repository.Update(entity);
                }
            }
            await _unitOfWork.CommitAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Data = true, Message = "Records deleted successfully." };
        }

        public async Task<APIResponse<bool>> BulkStatusChangeAsync(IEnumerable<int> ids, string status, string username)
        {
            foreach (var id in ids)
            {
                var entity = await _repository.FindAsync(e => EF.Property<int>(e, "Id") == id);
                if (entity != null)
                {
                    entity.GetType().GetProperty("Status")?.SetValue(entity, status);
                    entity.UpdatedBy = username;
                    entity.UpdatedDate = DateTime.Now;
                    _repository.Update(entity);
                }
            }
            await _unitOfWork.CommitAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Data = true, Message = "Status updated successfully." };
        }
    }
}

