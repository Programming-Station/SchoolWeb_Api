using Microsoft.EntityFrameworkCore;
using School.Domain.Location;
using School.Infrastructure.UnitOfWork.Interfaces;
using School_DTOs;
using School_DTOs.Common;
using School_DTOs.Location;
using System.Net;

namespace School.Services.Location
{
    public class CountryService : ICountryService
    {
        private readonly IRepository<Country> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CountryService(IRepository<Country> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<PagedResponse<CountryDto>>> GetAllAsync(PaginationFilterDto filter)
        {
            var query = _repository.List().AsNoTracking();

            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                var search = filter.SearchText.ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(search) ||
                                         (x.CountryCode != null && x.CountryCode.ToLower().Contains(search)));
            }

            var totalRecords = await query.CountAsync();
            var data = await query
                .OrderBy(x => x.Name)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new CountryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    CountryCode = x.CountryCode,
                    Currency = x.Currency,
                    CurrencySymbol = x.CurrencySymbol,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return new APIResponse<PagedResponse<CountryDto>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = new PagedResponse<CountryDto>
                {
                    Data = data,
                    TotalRecords = totalRecords,
                    CurrentPage = filter.PageNumber,
                    PageSize = filter.PageSize
                }
            };
        }

        public async Task<APIResponse<CountryDto>> GetByIdAsync(int id)
        {
            var entity = await _repository.List().Where(x => x.Id == id)
                .Select(x => new CountryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    CountryCode = x.CountryCode,
                    Currency = x.Currency,
                    CurrencySymbol = x.CurrencySymbol,
                    IsActive = x.IsActive
                }).FirstOrDefaultAsync();

            if (entity == null)
                return new APIResponse<CountryDto> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };

            return new APIResponse<CountryDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = entity };
        }

        public async Task<APIResponse<object>> CreateAsync(CreateCountryDto dto, string username)
        {
            var entity = new Country
            {
                Name = dto.Name,
                CountryCode = dto.CountryCode,
                Currency = dto.Currency,
                CurrencySymbol = dto.CurrencySymbol,
                IsActive = dto.IsActive,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.Created, Message = "Created successfully" };
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateCountryDto dto, string username)
        {
            if (id != dto.Id) return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Id mismatch" };
            var entity = await _repository.FindAsync(x => x.Id == id);
            if (entity == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };

            entity.Name = dto.Name;
            entity.CountryCode = dto.CountryCode;
            entity.Currency = dto.Currency;
            entity.CurrencySymbol = dto.CurrencySymbol;
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

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetDropdownAsync()
        {
            var data = await _repository.List().Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .Select(x => new DropdownDto { Id = x.Id, Name = x.Name, Code = x.CountryCode })
                .ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = data };
        }

        public async Task<APIResponse<bool>> BulkDeleteAsync(IEnumerable<int> ids, string username)
        {
            foreach (var id in ids)
            {
                var entity = await _repository.FindAsync(x => x.Id == id);
                if (entity != null)
                {
                    _repository.Delete(entity);
                }
            }
            await _unitOfWork.CommitAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Data = true, Message = "Bulk delete successful" };
        }

        public async Task<APIResponse<bool>> BulkStatusChangeAsync(IEnumerable<int> ids, bool isActive, string username)
        {
            foreach (var id in ids)
            {
                var entity = await _repository.FindAsync(x => x.Id == id);
                if (entity != null)
                {
                    entity.IsActive = isActive;
                    entity.UpdatedBy = username;
                    entity.UpdatedDate = DateTime.UtcNow;
                    _repository.Update(entity);
                }
            }
            await _unitOfWork.CommitAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Data = true, Message = "Bulk status change successful" };
        }
    }
}
