using System.Net;
using Microsoft.EntityFrameworkCore;
using School.Domain.Location;
using School.Infrastructure.UnitOfWork.Interfaces;
using School_DTOs;
using School_DTOs.Common;
using School_DTOs.Location;

namespace School.Services.Location
{
    public class StateLocationService : IStateLocationService
    {
        private readonly IRepository<State> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public StateLocationService(IRepository<State> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<PagedResponse<StateLocationDto>>> GetAllAsync(PaginationFilterDto filter)
        {
            var query = _repository.List().Include(x => x.Country).AsNoTracking();

            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                var search = filter.SearchText.ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(search) ||
                                         (x.StateCode != null && x.StateCode.ToLower().Contains(search)));
            }

            var totalRecords = await query.CountAsync();
            var data = await query
                .OrderBy(x => x.Name)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new StateLocationDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    StateCode = x.StateCode,
                    Description = x.Description,
                    CountryId = x.CountryId,
                    CountryName = x.Country.Name,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return new APIResponse<PagedResponse<StateLocationDto>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = new PagedResponse<StateLocationDto>
                {
                    Data = data,
                    TotalRecords = totalRecords,
                    CurrentPage = filter.PageNumber,
                    PageSize = filter.PageSize
                }
            };
        }

        public async Task<APIResponse<StateLocationDto>> GetByIdAsync(int id)
        {
            var entity = await _repository.List().Include(x => x.Country).Where(x => x.Id == id)
                .Select(x => new StateLocationDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    StateCode = x.StateCode,
                    Description = x.Description,
                    CountryId = x.CountryId,
                    CountryName = x.Country.Name,
                    IsActive = x.IsActive
                }).FirstOrDefaultAsync();

            if (entity == null)
                return new APIResponse<StateLocationDto> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };

            return new APIResponse<StateLocationDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = entity };
        }

        public async Task<APIResponse<object>> CreateAsync(CreateStateLocationDto dto, string username)
        {
            var entity = new State
            {
                Name = dto.Name,
                StateCode = dto.StateCode,
                Description = dto.Description,
                CountryId = dto.CountryId,
                IsActive = dto.IsActive,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.Created, Message = "Created successfully" };
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateStateLocationDto dto, string username)
        {
            if (id != dto.Id) return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Id mismatch" };
            var entity = await _repository.FindAsync(x => x.Id == id);
            if (entity == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };

            entity.Name = dto.Name;
            entity.StateCode = dto.StateCode;
            entity.Description = dto.Description;
            entity.CountryId = dto.CountryId;
            entity.IsActive = dto.IsActive;
            entity.UpdatedBy = username;
            entity.UpdatedDate = DateTime.UtcNow;

            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Updated successfully" };
        }

        public async Task<APIResponse<object>> DeleteAsync(int id, string username)
        {
            var entity = await _repository.FindAsync(x => x.Id == id);
            if (entity == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            _repository.Delete(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Deleted successfully" };
        }

        public async Task<APIResponse<object>> ToggleStatusAsync(int id, string username)
        {
            var entity = await _repository.FindAsync(x => x.Id == id);
            if (entity == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            entity.IsActive = !entity.IsActive;
            entity.UpdatedBy = username;
            entity.UpdatedDate = DateTime.UtcNow;
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Status toggled successfully" };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetDropdownAsync(int? countryId = null)
        {
            var query = _repository.List().Where(x => x.IsActive);
            if (countryId.HasValue)
                query = query.Where(x => x.CountryId == countryId.Value);

            var data = await query.OrderBy(x => x.Name)
                .Select(x => new DropdownDto { Id = x.Id, Name = x.Name, Code = x.StateCode })
                .ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = data };
        }
    }
}
