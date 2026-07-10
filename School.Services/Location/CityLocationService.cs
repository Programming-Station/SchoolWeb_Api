using Microsoft.EntityFrameworkCore;
using School.Domain.Location;
using School.Infrastructure.UnitOfWork.Interfaces;
using School_DTOs;
using School_DTOs.Common;
using School_DTOs.Location;
using System.Net;

namespace School.Services.Location
{
    public class CityLocationService : ICityLocationService
    {
        private readonly IRepository<City> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CityLocationService(IRepository<City> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<PagedResponse<CityLocationDto>>> GetAllAsync(PaginationFilterDto filter)
        {
            var query = _repository.List().Include(x => x.State).AsNoTracking();

            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                var search = filter.SearchText.ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(search) ||
                                         (x.CityCode != null && x.CityCode.ToLower().Contains(search)));
            }

            var totalRecords = await query.CountAsync();
            var data = await query
                .OrderBy(x => x.Name)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new CityLocationDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    CityCode = x.CityCode,
                    Description = x.Description,
                    StateId = x.StateId,
                    StateName = x.State.Name,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return new APIResponse<PagedResponse<CityLocationDto>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = new PagedResponse<CityLocationDto>
                {
                    Data = data,
                    TotalRecords = totalRecords,
                    CurrentPage = filter.PageNumber,
                    PageSize = filter.PageSize
                }
            };
        }

        public async Task<APIResponse<CityLocationDto>> GetByIdAsync(int id)
        {
            var entity = await _repository.List().Include(x => x.State).Where(x => x.Id == id)
                .Select(x => new CityLocationDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    CityCode = x.CityCode,
                    Description = x.Description,
                    StateId = x.StateId,
                    StateName = x.State.Name,
                    IsActive = x.IsActive
                }).FirstOrDefaultAsync();

            if (entity == null)
                return new APIResponse<CityLocationDto> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };

            return new APIResponse<CityLocationDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = entity };
        }

        public async Task<APIResponse<object>> CreateAsync(CreateCityLocationDto dto, string username)
        {
            var entity = new City
            {
                Name = dto.Name,
                CityCode = dto.CityCode,
                Description = dto.Description,
                StateId = dto.StateId,
                IsActive = dto.IsActive,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.Created, Message = "Created successfully" };
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateCityLocationDto dto, string username)
        {
            if (id != dto.Id) return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Id mismatch" };
            var entity = await _repository.FindAsync(x => x.Id == id);
            if (entity == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };

            entity.Name = dto.Name;
            entity.CityCode = dto.CityCode;
            entity.Description = dto.Description;
            entity.StateId = dto.StateId;
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

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetDropdownAsync(int? stateId = null)
        {
            var query = _repository.List().Where(x => x.IsActive);
            if (stateId.HasValue)
                query = query.Where(x => x.StateId == stateId.Value);

            var data = await query.OrderBy(x => x.Name)
                .Select(x => new DropdownDto { Id = x.Id, Name = x.Name, Code = x.CityCode })
                .ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = data };
        }
    }
}
